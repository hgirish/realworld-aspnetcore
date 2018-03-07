using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RealWorld.Infrastructure
{
  public class CurrentUserAccessor : ICurrentUserAccessor
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }
    public string GetCurrentUsername()
    {
      string useName = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x=>x.Type == ClaimTypes.NameIdentifier)?.Value;

      if (string.IsNullOrEmpty(useName))
      {
        useName = "anonymous";
      }
      return useName;
    }
  }
}
