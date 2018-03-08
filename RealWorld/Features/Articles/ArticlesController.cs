using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealWorld.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWorld.Features.Articles
{
  [Route("articles")]
  public class ArticlesController : Controller
  {
    private readonly IMediator _mediator;

    public ArticlesController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpGet]
    public async Task<ArticlesEnvelope> Get([FromQuery] string tag,[FromQuery] string author, [FromQuery] string favorited, [FromQuery] int? limit,[FromQuery] int? offset)
    {
      return await _mediator.Send(new List.Query(tag, author, favorited, limit, offset));
    }

        [HttpPost]
        [Authorize(AuthenticationSchemes =JwtIssuerOptions.Schemes)]
        public async Task<ArticleEnvelope> Create([FromBody]Create.Command command)
        {
            return await _mediator.Send(command);
        }
  }
}
