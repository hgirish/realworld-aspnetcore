using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWorld.Domain
{
    public class Tag
    {
        public string TagId { get; set; }
        public List<ArticleTag> ArticleTags { get; set; }
    }
}
