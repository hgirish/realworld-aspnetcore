using MediatR;
using Microsoft.EntityFrameworkCore;
using RealWorld.Domain;
using RealWorld.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RealWorld.Features.Articles
{
    public class List
    {
    public class Query : IRequest<ArticlesEnvelope>
    {
      public Query(string tag, string author, string favorited, int? limit, int? offset)
      {
        Tag = tag;
        Author = author;
        FavoritedUsername = favorited;
        Limit = limit;
        Offset = offset;

      }

      public string Tag { get; }
      public string Author { get; }
      public string FavoritedUsername { get; }
      public int? Limit { get; }
      public int? Offset { get; }
      public bool IsFeed { get; set; }
    }
        public class QueryHandler : IRequestHandler<Query, ArticlesEnvelope>
        {
            private readonly AppDbContext _context;

            public QueryHandler(AppDbContext context)
            {
                _context = context;
            }
            public async Task<ArticlesEnvelope> Handle(Query message, CancellationToken cancellationToken)
            {
                IQueryable<Article> queryable = _context.Articles.GetAllData();

                var articles = await queryable
                    .OrderByDescending(x => x.CreatedAt)
                    .Skip(message.Offset ?? 0)
                    .Take(message.Limit ?? 20)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                return new ArticlesEnvelope
                {
                    Articles = articles
                };
                    

            }
        }
    }
}
