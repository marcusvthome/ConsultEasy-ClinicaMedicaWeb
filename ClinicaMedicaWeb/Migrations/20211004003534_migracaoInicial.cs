using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClinicaMedicaWeb.Migrations
{
    public partial class migracaoInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBAdministrador",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBAdministrador", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TBMedico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CPF = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CRM = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBMedico", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TBSecretaria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CPF = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBSecretaria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TBLogin",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdministradorID = table.Column<int>(type: "int", nullable: true),
                    SecretariaID = table.Column<int>(type: "int", nullable: true),
                    MedicoID = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBLogin", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TBLogin_TBAdministrador_AdministradorID",
                        column: x => x.AdministradorID,
                        principalTable: "TBAdministrador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TBLogin_TBMedico_MedicoID",
                        column: x => x.MedicoID,
                        principalTable: "TBMedico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TBLogin_TBSecretaria_SecretariaID",
                        column: x => x.SecretariaID,
                        principalTable: "TBSecretaria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TBLogin_AdministradorID",
                table: "TBLogin",
                column: "AdministradorID");

            migrationBuilder.CreateIndex(
                name: "IX_TBLogin_MedicoID",
                table: "TBLogin",
                column: "MedicoID");

            migrationBuilder.CreateIndex(
                name: "IX_TBLogin_SecretariaID",
                table: "TBLogin",
                column: "SecretariaID");

            migrationBuilder.CreateIndex(
                name: "IX_TBMedico_CPF",
                table: "TBMedico",
                column: "CPF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TBMedico_CRM",
                table: "TBMedico",
                column: "CRM",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TBSecretaria_CPF",
                table: "TBSecretaria",
                column: "CPF",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBLogin");

            migrationBuilder.DropTable(
                name: "TBAdministrador");

            migrationBuilder.DropTable(
                name: "TBMedico");

            migrationBuilder.DropTable(
                name: "TBSecretaria");
        }
    }
}
