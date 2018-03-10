using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWorld.Features.Profiles
{
    public class Profile
    {
        public string Username { get; set; }

        public string Bio { get; set; }

        public string Image { get; set; }
        [JsonProperty("following")]
        public bool IsFollowed { get; internal set; }
    }
}
