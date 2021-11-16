using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClinicaMedicaWeb.Migrations
{
    public partial class cancelaConsulta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataCancelamento",
                table: "TBConsulta",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "MotivoCancelamento",
                table: "TBConsulta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecretariaIdCancelamento",
                table: "TBConsulta",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataCancelamento",
                table: "TBConsulta");

            migrationBuilder.DropColumn(
                name: "MotivoCancelamento",
                table: "TBConsulta");

            migrationBuilder.DropColumn(
                name: "SecretariaIdCancelamento",
                table: "TBConsulta");
        }
    }
}
