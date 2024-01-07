using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChristmasTreeManager.Infrastructure.Sqlite.Migrations.Application
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DistributionTourFormCount",
                table: "Streets",
                newName: "DistributionTourOrderNumber");
        }
    }
}
