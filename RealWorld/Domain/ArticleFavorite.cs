﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWorld.Domain
{
    public class ArticleFavorite
    {
        public int ArticleId { get; set; }
        public Article Article { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
