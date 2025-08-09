using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignUpCompany.Data.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseSignUpCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OtpCode",
                table: "Companies",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtpCode",
                table: "Companies");
        }
    }
}
