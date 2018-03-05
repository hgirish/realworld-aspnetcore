using RealWorld.Domain;
using System.Collections.Generic;

namespace RealWorld.Infrastructure.Articles
{
  public class ArticlesEnvelope
  {
    public List<Article> Articles { get; set; }

    public int ArticlesCount => Articles?.Count ?? 0;
  }
}
