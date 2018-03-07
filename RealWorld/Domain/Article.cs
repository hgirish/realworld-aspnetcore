using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWorld.Domain
{
    public class Article
    {
        [JsonIgnore]
        public int ArticleId { get; set; }

        public string Slug { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Body { get; set; }

        public DateTime CreatedAt { get; internal set; }
        public DateTime UpdatedAt { get; set; }
        [JsonIgnore]
        public List<ArticleTag> ArticleTags { get; set; }
    public Person Author { get;  set; }
  }
}
