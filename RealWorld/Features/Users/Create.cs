using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

using RealWorld.Domain;
using RealWorld.Infrastructure;
using RealWorld.Infrastructure.Errors;
using RealWorld.Infrastructure.Security;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace RealWorld.Features.Users
{
    public class Create
    {
        public class UserData
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class UserDataValidator : AbstractValidator<UserData>
        {
            public UserDataValidator()
            {
                RuleFor(x => x.Username).NotNull().NotEmpty();
                RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
                RuleFor(x => x.Password).NotNull().NotEmpty();
            }
        }
        public class Command: IRequest<UserEnvelope>
        {
            public UserData User { get; set; }
        }
        public class CommandValidator: AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.User).NotNull().SetValidator(new UserDataValidator());
            }
        }

   

        public class Handler: IRequestHandler<Command, UserEnvelope>
        {
            private readonly AppDbContext _context;
            private readonly IMapper _mapper;
            private readonly IPasswordHasher _passwordHasher;

            public Handler(AppDbContext context, IMapper mapper, IPasswordHasher passwordHasher)
            {
                _context = context;
                _mapper = mapper;
                _passwordHasher = passwordHasher;
            }

            public async  Task<UserEnvelope> Handle(Command message, CancellationToken cancellationToken)
            {
                if (await _context.Persons.Where(x=>x.Username == message.User.Username).AnyAsync(cancellationToken))
                {
                    throw new RestException(HttpStatusCode.BadRequest);
                }
                if (await _context.Persons.Where(x=>x.Email == message.User.Email).AnyAsync(cancellationToken))
                {
                    throw new RestException(HttpStatusCode.BadRequest);
                }
                var salt = Guid.NewGuid().ToByteArray();

                var person = new Person
                {
                    Username = message.User.Username,
                    Email = message.User.Email,
                    Salt = salt,
                    Hash = _passwordHasher.Hash(message.User.Password, salt),
                };
                _context.Persons.Add(person);
                await _context.SaveChangesAsync(cancellationToken);
                var user = _mapper.Map<Domain.Person, User>(person);
                return new UserEnvelope(user);
            }
        }

    }
}
