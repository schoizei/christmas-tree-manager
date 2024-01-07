using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChristmasTreeManager.Infrastructure.Sqlite.Migrations.Application
{
    /// <inheritdoc />
    public partial class AddTeamLeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TeamLeader",
                table: "CollectionTours",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamLeader",
                table: "CollectionTours");
        }
    }
}
