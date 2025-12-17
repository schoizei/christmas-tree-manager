using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChristmasTreeManager.Infrastructure.Sqlite.Migrations.Application
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CollectionTours",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Vehicle = table.Column<string>(type: "TEXT", nullable: false),
                    Driver = table.Column<string>(type: "TEXT", nullable: false),
                    Staff = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionTours", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DistributionTours",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Staff = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributionTours", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationPoints",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationPoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Streets",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ZipCode = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    District = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LowestHouseNumber = table.Column<uint>(type: "INTEGER", nullable: false),
                    HighestHouseNumber = table.Column<uint>(type: "INTEGER", nullable: false),
                    DistributionTourId = table.Column<string>(type: "TEXT", nullable: true),
                    DistributionTourOrderNumber = table.Column<uint>(type: "INTEGER", nullable: false),
                    CollectionTourId = table.Column<string>(type: "TEXT", nullable: true),
                    CollectionTourOrderNumber = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Streets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Streets_CollectionTours_CollectionTourId",
                        column: x => x.CollectionTourId,
                        principalTable: "CollectionTours",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Streets_DistributionTours_DistributionTourId",
                        column: x => x.DistributionTourId,
                        principalTable: "DistributionTours",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    RegistrationPointId = table.Column<string>(type: "TEXT", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Customer = table.Column<string>(type: "TEXT", nullable: false),
                    StreetId = table.Column<string>(type: "TEXT", nullable: false),
                    Housenumber = table.Column<uint>(type: "INTEGER", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    Mail = table.Column<string>(type: "TEXT", nullable: false),
                    TreeCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    Donation = table.Column<double>(type: "REAL", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Registrations_RegistrationPoints_RegistrationPointId",
                        column: x => x.RegistrationPointId,
                        principalTable: "RegistrationPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Registrations_Streets_StreetId",
                        column: x => x.StreetId,
                        principalTable: "Streets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_RegistrationPointId",
                table: "Registrations",
                column: "RegistrationPointId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_StreetId",
                table: "Registrations",
                column: "StreetId");

            migrationBuilder.CreateIndex(
                name: "IX_Streets_CollectionTourId",
                table: "Streets",
                column: "CollectionTourId");

            migrationBuilder.CreateIndex(
                name: "IX_Streets_DistributionTourId",
                table: "Streets",
                column: "DistributionTourId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Registrations");

            migrationBuilder.DropTable(
                name: "RegistrationPoints");

            migrationBuilder.DropTable(
                name: "Streets");

            migrationBuilder.DropTable(
                name: "CollectionTours");

            migrationBuilder.DropTable(
                name: "DistributionTours");
        }
    }
}
