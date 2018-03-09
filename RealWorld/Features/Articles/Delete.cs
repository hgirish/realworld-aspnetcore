using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using FluentValidation;
using RealWorld.Infrastructure;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using RealWorld.Infrastructure.Errors;

namespace RealWorld.Features.Articles
{
    public class Delete
    {
        public class Command : IRequest
        {

            public Command(string slug)
            {
                Slug = slug;
            }

            public string Slug { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Slug).NotNull().NotEmpty();
            }
        }

        public class QueryHandler: IRequestHandler<Command>
        {
            private readonly AppDbContext _context;

            public QueryHandler(AppDbContext context)
            {
                _context = context;
            }

            public async  Task Handle(Command message, CancellationToken cancellationToken)
            {
                var article = await _context.Articles
                    .FirstOrDefaultAsync(x => x.Slug == message.Slug, cancellationToken);

                if (article == null)
                {
                    throw new RestException(System.Net.HttpStatusCode.NotFound);
                }

                _context.Articles.Remove(article);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
