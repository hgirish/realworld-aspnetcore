using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealWorld.Domain;
using RealWorld.Features.Articles;
using RealWorld.Infrastructure;
using RealWorld.Infrastructure.Errors;

namespace RealWorld.Features.Favorites
{
    public  class Delete
    {
        public  class Command : IRequest<ArticleEnvelope>
        {
            public Command(string slug)
            {
                Slug = slug;
            }

            public string Slug { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Slug).NotNull().NotEmpty();
            }
        }

        public class QueryHandler : IRequestHandler<Command, ArticleEnvelope>
        {
            private readonly AppDbContext _context;
            private readonly ICurrentUserAccessor _currentUserAccessor;

            public QueryHandler(AppDbContext context, ICurrentUserAccessor currentUserAccessor)
            {
                _context = context;
                _currentUserAccessor = currentUserAccessor;
            }

            public async Task<ArticleEnvelope> Handle(Command request, CancellationToken cancellationToken)
            {
                var article = await _context.Articles.FirstOrDefaultAsync(
                    x => x.Slug == request.Slug, cancellationToken);

                if (article == null)
                {
                    throw new RestException(System.Net.HttpStatusCode.NotFound);
                }

                var person = await _context.Persons.FirstOrDefaultAsync(
                    x => x.Username == _currentUserAccessor.GetCurrentUsername(), cancellationToken);

                var favorite = await _context.ArticleFavorites.FirstOrDefaultAsync(
                    x => x.ArticleId == article.ArticleId &&
                    x.PersonId == person.PersonId,
                    cancellationToken
                    );

                if (favorite != null)
                {
                    _context.ArticleFavorites.Remove(favorite);
                    await _context.SaveChangesAsync(cancellationToken);
                }


                return new ArticleEnvelope(await _context.Articles.GetAllData()
                    .FirstOrDefaultAsync(
                    x => x.ArticleId == article.ArticleId,
                    cancellationToken));
            }
        }

    }
}
