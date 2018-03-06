using Microsoft.EntityFrameworkCore;
using RealWorld.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWorld.Features.Articles
{
    public static  class ArticleExtensions
    {
        public static IQueryable<Article> GetAllData(this DbSet<Article> articles)
        {
            return articles
                .Include(x=>x.ArticleTags)
                .AsNoTracking();
        }
    }
}
