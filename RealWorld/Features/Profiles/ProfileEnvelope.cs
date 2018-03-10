using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWorld.Features.Profiles
{
    public class ProfileEnvelope
    {

        public ProfileEnvelope(Profile profile)
        {
            Profile = profile;
        }

        public Profile Profile { get; }
    }
}
