using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignUpCompany.Data.Migrations
{
    /// <inheritdoc />
    public partial class CompanySystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EnglishName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ArabicName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    WebsiteUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    LogoFileName = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    PasswordHash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
