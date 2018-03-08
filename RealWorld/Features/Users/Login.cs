using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealWorld.Infrastructure;
using RealWorld.Infrastructure.Errors;
using RealWorld.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RealWorld.Features.Users
{
    public class Login
    {
        public class UserData
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class UserDataValidator : AbstractValidator<UserData>
        {
            public UserDataValidator()
            {
                RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
                RuleFor(x => x.Password).NotNull().NotEmpty();
            }
        }

        public class Command: IRequest<UserEnvelope>
        {
            public UserData User { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.User).NotNull().SetValidator(new UserDataValidator());
            }
        }

        public class Handler : IRequestHandler<Command, UserEnvelope>
        {
            private readonly AppDbContext _context;
            private readonly IPasswordHasher _passwordHasher;
            private readonly IMapper _mapper;
            private readonly IJwtTokenGenerator _jwtTokenGenerator;

            public Handler(AppDbContext context, IPasswordHasher passwordHasher, 
                IMapper mapper, IJwtTokenGenerator jwtTokenGenerator)
            {
                _context = context;
                _passwordHasher = passwordHasher;
                _mapper = mapper;
                _jwtTokenGenerator = jwtTokenGenerator;
            }

            public async  Task<UserEnvelope> Handle(Command message, CancellationToken cancellationToken)
            {
                var person = await _context.Persons.Where(x => x.Email == message.User.Email).SingleOrDefaultAsync(cancellationToken);
                if (person == null)
                {
                    throw new RestException(System.Net.HttpStatusCode.Unauthorized);
                }
                if (!person.Hash.SequenceEqual(_passwordHasher.Hash(message.User.Password, person.Salt)))
                {
                    throw new RestException(System.Net.HttpStatusCode.Unauthorized);
                }
                var user = _mapper.Map<Domain.Person, User>(person);

                user.Token = await _jwtTokenGenerator.CreateToken(person.Username);

                return new UserEnvelope(user);


            }
        }
    }
}
