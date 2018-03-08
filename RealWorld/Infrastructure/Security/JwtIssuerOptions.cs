using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWorld.Infrastructure.Security
{
    public class JwtIssuerOptions
    {
        public const string Schemes = "Token,Bearer";

        public string Issuer { get; set; }

        public string Subject { get; set; }

        public string Audience { get; set; }

        public DateTime NotBefore
        {
            get
            {
                return DateTime.UtcNow;
            }
        }

        public DateTime IssuedAt => DateTime.UtcNow;

        public TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(5);

        public DateTime Expiration => IssuedAt.Add(ValidFor);

        public Func<Task<string>> JtiGenerator => () => Task.FromResult(Guid.NewGuid().ToString());


        public SigningCredentials SigningCredentials { get; set; }
    }
}
