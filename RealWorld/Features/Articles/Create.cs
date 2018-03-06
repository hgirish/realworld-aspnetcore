using MediatR;
using RealWorld.Domain;
using RealWorld.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RealWorld.Features.Articles
{
    public class Create
    {
        public class ArticleData
        {
            public string Title { get; set; }

            public string Description { get; set; }

            public string Body { get; set; }

            public string[] TagList { get; set; }
        }

        public class Command : IRequest<ArticleEnvelope>
        {
            public ArticleData Article { get; set; }
        }

        public class Handler: IRequestHandler<Command, ArticleEnvelope>
        {
            private readonly AppDbContext _context;

            public Handler(AppDbContext context)
            {
                _context = context;
            }

            public async Task<ArticleEnvelope> Handle(Command message, CancellationToken cancellationToken)
            {
                var tags = new List<Tag>();

                foreach (var tag in message.Article.TagList ?? Enumerable.Empty<string>())
                {
                    var t = await _context.Tags.FindAsync(tag);

                    if (t == null)
                    {
                        t = new Tag
                        {
                            TagId = tag
                        };
                        await _context.Tags.AddAsync(t, cancellationToken);
                        await _context.SaveChangesAsync(cancellationToken);
                    }
                    tags.Add(t);
                }
                var article = new Article
                {
                    Body = message.Article.Body,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Description = message.Article.Description,
                    Title = message.Article.Title,
                    Slug = message.Article.Title.GenerateSlug()
                };

                await _context.Articles.AddAsync(article, cancellationToken);

                await _context.ArticleTags.AddRangeAsync(tags.Select(x => new ArticleTag()
                {
                    Article = article,
                    Tag = x
                }), cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
                return new ArticleEnvelope(article);


            }


        }
    }
}
