using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Bogus;
using EFWebRazor.models;

#nullable disable

namespace EFWebRazor.Migrations
{
    /// <inheritdoc />
    public partial class Db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Content = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articles", x => x.Id);
                });

                Randomizer.Seed = new Random(8675309);
                var fakeData = new Faker<Article>();
                fakeData.RuleFor(a=> a.Title, y=> y.Lorem.Sentence(5, 5));
                fakeData.RuleFor(a=> a.Created, y=>y.Date.Between(new DateTime(2022, 09, 09), new DateTime(2023, 02, 22)));
                fakeData.RuleFor(a=>a.Content, y=>y.Lorem.Paragraphs(2, 5));

                for (int i = 0; i < 150; i++)
                {
                    Article articles = fakeData.Generate();

                migrationBuilder.InsertData(
                    table : "articles",
                    columns : new[]{"Title","Created","Content"},
                    values : new object[]
                    {
                        articles.Title,
                        articles.Created,
                        articles.Content
                        
                    }
                );
                    
                }
                Article article = fakeData.Generate();

                migrationBuilder.InsertData(
                    table : "articles",
                    columns : new[]{"Title","Created","Content"},
                    values : new object[]
                    {
                        "bài viết 1",
                        new DateTime(2023, 02,22),
                        "Nội dung 1"
                    }
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "articles");
        }
    }
}
