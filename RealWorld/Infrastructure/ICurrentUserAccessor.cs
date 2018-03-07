using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWorld.Infrastructure
{
   public  interface ICurrentUserAccessor
    {
    string GetCurrentUsername();
  }
}
