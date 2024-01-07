using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChristmasTreeManager.Infrastructure.Postgres.Migrations.Application
{
    /// <inheritdoc />
    public partial class AddFormCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DistributionTourOrderNumber",
                table: "Streets",
                newName: "DistributionTourFormCount");

            migrationBuilder.AddColumn<string>(
                name: "HousenumberPostfix",
                table: "Registrations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TeamLeader",
                table: "CollectionTours",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HousenumberPostfix",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "TeamLeader",
                table: "CollectionTours");

            migrationBuilder.RenameColumn(
                name: "DistributionTourFormCount",
                table: "Streets",
                newName: "DistributionTourOrderNumber");
        }
    }
}
