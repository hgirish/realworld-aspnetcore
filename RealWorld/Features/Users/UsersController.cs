using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace RealWorld.Features.Users
{
    [Route("users")]
    public class UsersController :Controller
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<UserEnvelope> Create([FromBody] Create.Command command)
        {
            return await _mediator.Send(command);
        }
    }
}
