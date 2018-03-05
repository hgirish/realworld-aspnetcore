using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWorld.Infrastructure.Articles
{
    public class List
    {
    public class Query : IRequest<ArticlesEnvelope>
    {
      public Query(string tag, string author, string favorited, int? limit, int? offset)
      {
        Tag = tag;
        Author = author;
        FavoritedUsername = favorited;
        Limit = limit;
        Offset = offset;

      }

      public string Tag { get; }
      public string Author { get; }
      public string FavoritedUsername { get; }
      public int? Limit { get; }
      public int? Offset { get; }
      public bool IsFeed { get; set; }
    }
    }
}
