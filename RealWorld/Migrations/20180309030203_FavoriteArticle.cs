using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RealWorld.Migrations
{
    public partial class FavoriteArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticleFavorites",
                columns: table => new
                {
                    ArticleId = table.Column<int>(nullable: false),
                    PersonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleFavorites", x => new { x.ArticleId, x.PersonId });
                    table.ForeignKey(
                        name: "FK_ArticleFavorites_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleFavorites_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FollowedPeople",
                columns: table => new
                {
                    ObserverId = table.Column<int>(nullable: false),
                    TargetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowedPeople", x => new { x.ObserverId, x.TargetId });
                    table.ForeignKey(
                        name: "FK_FollowedPeople_Persons_ObserverId",
                        column: x => x.ObserverId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FollowedPeople_Persons_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleFavorites_PersonId",
                table: "ArticleFavorites",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowedPeople_TargetId",
                table: "FollowedPeople",
                column: "TargetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleFavorites");

            migrationBuilder.DropTable(
                name: "FollowedPeople");
        }
    }
}
