using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealWorld.Infrastructure;
using RealWorld.Infrastructure.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RealWorld.Features.Articles
{
  public class Details
  {

    public class Query : IRequest<ArticleEnvelope>
    {
      public Query(string slug)
      {
        Slug = slug;
      }

      public string Slug { get; }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
      public QueryValidator()
      {
        RuleFor(x => x.Slug).NotNull().NotEmpty();
      }
    }

    public class QueryHandler: IRequestHandler<Query, ArticleEnvelope>
    {
      private readonly AppDbContext _context;

      public QueryHandler(AppDbContext context)
      {
        _context = context;
      }

      public async Task<ArticleEnvelope> Handle(Query message, CancellationToken cancellationToken)
      {
        var article = await _context.Articles.GetAllData()
          .FirstOrDefaultAsync(x => x.Slug == message.Slug, cancellationToken);
        if (article == null)
        {
          throw new RestException(System.Net.HttpStatusCode.NotFound);
        }
        return new ArticleEnvelope(article);
      }
    }
  }
}
