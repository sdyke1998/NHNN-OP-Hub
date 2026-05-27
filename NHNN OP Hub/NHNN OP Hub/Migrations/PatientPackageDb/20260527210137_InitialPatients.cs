using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NHNN_OP_Hub.Migrations.PatientPackageDb
{
    /// <inheritdoc />
    public partial class InitialPatients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientPackages",
                columns: table => new
                {
                    WorkRequestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MRN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateDispensed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPaying = table.Column<bool>(type: "bit", nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    RxCost = table.Column<float>(type: "real", nullable: true),
                    ReciptNumber = table.Column<int>(type: "int", nullable: true),
                    PackageType = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    TicketNumber = table.Column<int>(type: "int", nullable: true),
                    CollectionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HasCollected = table.Column<bool>(type: "bit", nullable: true),
                    TrackingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasReturned = table.Column<bool>(type: "bit", nullable: true),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientPackages", x => x.WorkRequestID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientPackages");
        }
    }
}
