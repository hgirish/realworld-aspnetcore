using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Net;
using System.Threading;
using RealWorld.Infrastructure;
using RealWorld.Infrastructure.Errors;

namespace RealWorld.Features.Users
{
    public class Details
    {
        public class Query : IRequest<UserEnvelope>
        {
            public string Username { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Username).NotNull().NotEmpty();
            }
        }

        public class QueryHandler : IRequestHandler<Query, UserEnvelope>
        {
            private readonly AppDbContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(AppDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async  Task<UserEnvelope> Handle(Query message, CancellationToken cancellationToken)
            {
                var person = await _context.Persons
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Username == message.Username, cancellationToken);

                if (person == null)
                {
                    throw new RestException(HttpStatusCode.NotFound);
                }

                return new UserEnvelope(_mapper.Map<Domain.Person, User>(person));
            }
        }

    }
}
