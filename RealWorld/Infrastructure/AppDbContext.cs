using Microsoft.EntityFrameworkCore;
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
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }
        public DbSet<ArticleFavorite> ArticleFavorites { get; set; }
        public DbSet<FollowedPeople> FollowedPeople { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"FileName={_databaseName}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticleTag>(b =>
            {
                b.HasKey(t => new { t.ArticleId, t.TagId });
                b.HasOne(pt => pt.Article)
                .WithMany(p => p.ArticleTags)
                .HasForeignKey(pt => pt.ArticleId);

                b.HasOne(pt => pt.Tag)
                .WithMany(t => t.ArticleTags)
                .HasForeignKey(pt => pt.TagId);


            });

            modelBuilder.Entity<ArticleFavorite>(b =>
            {
                b.HasKey(t => new { t.ArticleId, t.PersonId });

                b.HasOne(pt => pt.Article)
                .WithMany(p => p.ArticleFavorites)
                .HasForeignKey(pt => pt.ArticleId);

                b.HasOne(pt => pt.Person)
                .WithMany(t => t.ArticleFavorites)
                .HasForeignKey(pt => pt.PersonId);
            });

            modelBuilder.Entity<FollowedPeople>(b =>
            {
                b.HasKey(t => new { t.ObserverId, t.TargetId });

                b.HasOne(pt => pt.Observer)
                    .WithMany(p => p.Followers)
                    .HasForeignKey(pt => pt.ObserverId);

                b.HasOne(pt => pt.Target)
                    .WithMany(t => t.Following)
                    .HasForeignKey(pt => pt.TargetId);
            });

        }

    }
}
