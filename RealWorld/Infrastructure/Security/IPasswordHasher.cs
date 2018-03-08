using System;
using System.Collections.Generic;
using System.Linq;

namespace RealWorld.Infrastructure.Security
{
    public  interface IPasswordHasher
    {
        byte[] Hash(string password, byte[] salt);
    }
}
