using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using FluentValidation;
using System.Threading;
using RealWorld.Infrastructure;
using Microsoft.EntityFrameworkCore;
using RealWorld.Infrastructure.Errors;
using RealWorld.Domain;

namespace RealWorld.Features.Comments
{
    public class Create
    {
        public class CommentData
        {
            public string Body { get; set; }
        }
        public class CommentDataValidator : AbstractValidator<CommentData>
        {
            public CommentDataValidator()
            {
                RuleFor(x => x.Body).NotNull().NotEmpty();
            }
        }
        public class Command : IRequest<CommentEnvelope>
        {
            public CommentData Comment { get; set; }
            public string Slug { get; set; }
        }

        public class CommandValidator: AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Comment).NotNull().SetValidator(new CommentDataValidator());
            }
        }

        public class Handler : IRequestHandler<Command, CommentEnvelope>
        {
            private readonly AppDbContext _context;
            private readonly ICurrentUserAccessor _currentUserAccessor;

            public Handler(AppDbContext context, ICurrentUserAccessor currentUserAccessor)
            {
                _context = context;
                _currentUserAccessor = currentUserAccessor;
            }
            public async  Task<CommentEnvelope> Handle(Command message, CancellationToken cancellationToken)
            {
                var article = await _context.Articles
                    .Include(x => x.Comments)
                    .FirstOrDefaultAsync(x => x.Slug == message.Slug, cancellationToken);

                if (article == null)
                {
                    throw new RestException(System.Net.HttpStatusCode.NotFound);
                }

                var author = await _context.Persons.FirstAsync(x => x.Username == 
                _currentUserAccessor.GetCurrentUsername(),cancellationToken);

                var comment = new Comment
                {
                    Author = author,
                    Body = message.Comment.Body,
                    CreatedAt = DateTime.UtcNow,
                    UpdateAt = DateTime.UtcNow
                };

                await _context.Comments.AddAsync(comment, cancellationToken);

                article.Comments.Add(comment);

                await _context.SaveChangesAsync(cancellationToken);

                return new CommentEnvelope(comment);
            }
        }
    }
}
