using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NHNN_OP_Hub.Migrations.PatientPackageDb
{
    /// <inheritdoc />
    public partial class UpdatePatientPackages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "RxCost",
                table: "PatientPackages",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "RxCost",
                table: "PatientPackages",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");
        }
    }
}
