using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealWorld.Infrastructure;
using RealWorld.Infrastructure.Errors;

namespace RealWorld.Features.Articles
{
  public class Edit
  {
    public class ArticleData
    {
      public string Title { get; set; }

      public string Description { get; set; }

      public string Body { get; set; }
    }
    public class Command : IRequest<ArticleEnvelope>
    {
      public ArticleData Article { get; set; }
      public string Slug { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
      public CommandValidator()
      {
        RuleFor(x => x.Article).NotNull();
      }
    }

    public class Handler : IRequestHandler<Command, ArticleEnvelope>
    {
      private readonly AppDbContext _context;

      public Handler(AppDbContext context)
      {
        _context = context;
      }

      public async Task<ArticleEnvelope> Handle(Command message, CancellationToken cancellationToken)
      {
        var article = await _context.Articles
          .Where(x => x.Slug == message.Slug)
          .FirstOrDefaultAsync(cancellationToken);

        if (article == null)
        {
          throw new RestException(System.Net.HttpStatusCode.NotFound);
        }
        article.Description = message.Article.Description ?? article.Description;
        article.Body = message.Article.Body ?? article.Body;
        article.Title = message.Article.Title ?? article.Title;
        article.Slug = article.Title.GenerateSlug();

        if (_context.ChangeTracker.Entries().First(x => x.Entity == article).State == EntityState.Modified)
        {
          article.UpdatedAt = DateTime.UtcNow;
        }
        await _context.SaveChangesAsync(cancellationToken);

        return new ArticleEnvelope(await _context.Articles.GetAllData()
            .Where(x => x.Slug == article.Slug)
            .FirstOrDefaultAsync(cancellationToken));


      }
    }

  }


}
