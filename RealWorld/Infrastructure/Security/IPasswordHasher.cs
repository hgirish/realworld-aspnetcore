using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWorld.Infrastructure.Security
{
   public  interface IPasswordHasher
    {
        byte[] Hash(string password, byte[] salt);
    }
}
