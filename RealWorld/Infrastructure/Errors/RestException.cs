using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RealWorld.Infrastructure.Errors
{
    public class RestException : Exception
    {
        public RestException(HttpStatusCode code, string message = null)
            :base(message)
        {
            Code = code;
        }

        public HttpStatusCode Code { get; }
    }
}
