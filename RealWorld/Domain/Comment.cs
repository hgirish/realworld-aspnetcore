using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWorld.Domain
{
    public class Comment
    {
        [JsonProperty("id")]
        public int CommentId { get; set; }
        public string Body { get; set; }
        public Person Author { get; set; }
        [JsonIgnore]
        public Article Article { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
