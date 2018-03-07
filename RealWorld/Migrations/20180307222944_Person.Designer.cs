﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using RealWorld.Infrastructure;
using System;

namespace RealWorld.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20180307222944_Person")]
    partial class Person
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("RealWorld.Domain.Article", b =>
                {
                    b.Property<int>("ArticleId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AuthorPersonId");

                    b.Property<string>("Body");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Description");

                    b.Property<string>("Slug");

                    b.Property<string>("Title");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("ArticleId");

                    b.HasIndex("AuthorPersonId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("RealWorld.Domain.ArticleTag", b =>
                {
                    b.Property<int>("ArticleId");

                    b.Property<string>("TagId");

                    b.HasKey("ArticleId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("ArticleTags");
                });

            modelBuilder.Entity("RealWorld.Domain.Person", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Bio");

                    b.Property<string>("Email");

                    b.Property<byte[]>("Hash");

                    b.Property<string>("Image");

                    b.Property<byte[]>("Salt");

                    b.Property<string>("Username");

                    b.HasKey("PersonId");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("RealWorld.Domain.Tag", b =>
                {
                    b.Property<string>("TagId")
                        .ValueGeneratedOnAdd();

                    b.HasKey("TagId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("RealWorld.Domain.Article", b =>
                {
                    b.HasOne("RealWorld.Domain.Person", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorPersonId");
                });

            modelBuilder.Entity("RealWorld.Domain.ArticleTag", b =>
                {
                    b.HasOne("RealWorld.Domain.Article", "Article")
                        .WithMany("ArticleTags")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RealWorld.Domain.Tag", "Tag")
                        .WithMany("ArticleTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
