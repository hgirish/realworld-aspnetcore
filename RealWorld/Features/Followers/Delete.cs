using MediatR;
using RealWorld.Features.Profiles;
using Microsoft.EntityFrameworkCore;
using RealWorld.Infrastructure.Errors;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;
using RealWorld.Infrastructure;

namespace RealWorld.Features.Followers
{
    public class Delete
    {
        public  class Command : IRequest<ProfileEnvelope>
        {

            public Command(string username)
            {
                Username = username;
            }

            public string Username { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Username).NotNull().NotEmpty();
            }
        }

        public class QueryHandler : IRequestHandler<Command, ProfileEnvelope>
        {
            private readonly AppDbContext _context;
            private readonly ICurrentUserAccessor _currentUserAccessor;
            private readonly IProfileReader _profileReader;

            public QueryHandler(AppDbContext context, ICurrentUserAccessor currentUserAccessor, IProfileReader profileReader)
            {
                _context = context;
                _currentUserAccessor = currentUserAccessor;
                _profileReader = profileReader;
            }

            public async  Task<ProfileEnvelope> Handle(Command message, CancellationToken cancellationToken)
            {
                var target = await _context.Persons.FirstOrDefaultAsync(x =>
                x.Username == message.Username, cancellationToken);

                if (target == null)
                {
                    throw new RestException(System.Net.HttpStatusCode.NotFound);
                }

                var observer = await _context.Persons.FirstOrDefaultAsync(
                    x => x.Username == _currentUserAccessor.GetCurrentUsername(), 
                    cancellationToken);

                var followedPeople = await _context.FollowedPeople.FirstOrDefaultAsync(
                    x => x.ObserverId == observer.PersonId &&
                    x.TargetId == target.PersonId,
                    cancellationToken);

                if (followedPeople != null)
                {
                    _context.FollowedPeople.Remove(followedPeople);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                return await _profileReader.ReadProfile(message.Username);



            }
        }


    }
}
