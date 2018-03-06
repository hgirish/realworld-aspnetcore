﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWorld.Features.Tags
{
    [Route("tags")]
    public class TagsController : Controller
    {
        private readonly IMediator _mediator;

        public TagsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<TagsEnvelope> Get()
        {
            return await _mediator.Send(new List.Query());
        }
    }
}
