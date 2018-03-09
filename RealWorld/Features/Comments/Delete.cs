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

namespace RealWorld.Features.Comments
{
    public class Delete
    {
        public class Command: IRequest
        {
            public Command(string slug, int id)
            {
                Slug = slug;
                Id = id;
            }

            public string Slug { get; }
            public int Id { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Slug).NotNull().NotEmpty();
            }
        }

        public class QueryHandler : IRequestHandler<Command>
        {
            private readonly AppDbContext _context;

            public QueryHandler(AppDbContext context)
            {
                _context = context;
            }
            public async  Task Handle(Command message, CancellationToken cancellationToken)
            {
                var article = await _context.Articles
                    .Include(x => x.Comments)
                    .FirstOrDefaultAsync(x => x.Slug == message.Slug, cancellationToken);

                var comment = article.Comments.FirstOrDefault(x => x.CommentId == message.Id);

                if (comment == null)
                {
                    throw new RestException(System.Net.HttpStatusCode.NotFound);
                }

                _context.Comments.Remove(comment);
               await  _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
