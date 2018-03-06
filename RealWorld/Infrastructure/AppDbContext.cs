﻿using Microsoft.EntityFrameworkCore;
using RealWorld.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWorld.Infrastructure
{
    public class AppDbContext : DbContext
    {
        private readonly string _databaseName = Startup.DATABASE_FILE;

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public AppDbContext(string databaseName)
        {
            _databaseName = databaseName;
        }

        public DbSet<Article> Articles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"FileName={_databaseName}");
        }

    }
}
