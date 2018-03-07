using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RealWorld.Migrations
{
    public partial class Person : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorPersonId",
                table: "Articles",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Bio = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Hash = table.Column<byte[]>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Salt = table.Column<byte[]>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_AuthorPersonId",
                table: "Articles",
                column: "AuthorPersonId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Articles_Persons_AuthorPersonId",
            //    table: "Articles",
            //    column: "AuthorPersonId",
            //    principalTable: "Persons",
            //    principalColumn: "PersonId",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Persons_AuthorPersonId",
                table: "Articles");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Articles_AuthorPersonId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "AuthorPersonId",
                table: "Articles");
        }
    }
}
