using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWorld.Infrastructure
{
    public class ValidationActionFilter : IActionFilter
    {
        private readonly ILogger<ValidationActionFilter> _logger;

        public ValidationActionFilter(ILogger<ValidationActionFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var result = new ContentResult();
                var errors = new Dictionary<string, string[]>();

                foreach (var valuePair  in context.ModelState)
                {
                    errors.Add(valuePair.Key, valuePair.Value.Errors.Select(x =>
                     x.ErrorMessage).ToArray());

                }

                string content = JsonConvert.SerializeObject(new { errors });
                result.Content = content;

                result.ContentType = "application/json";

                context.HttpContext.Response.StatusCode = 422;
                context.Result = result;

            }
        }
    }
}
