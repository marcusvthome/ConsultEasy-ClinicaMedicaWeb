using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClinicaMedicaWeb.Migrations
{
    public partial class migracaoInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBLogin",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoUsuario = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBLogin", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TBPaciente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CPF = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Endereco = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Profissao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sexo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBPaciente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TBAdministrador",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginID = table.Column<int>(type: "int", nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBAdministrador", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBAdministrador_TBLogin_LoginID",
                        column: x => x.LoginID,
                        principalTable: "TBLogin",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TBMedico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginID = table.Column<int>(type: "int", nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CPF = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CRM = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBMedico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBMedico_TBLogin_LoginID",
                        column: x => x.LoginID,
                        principalTable: "TBLogin",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TBSecretaria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginID = table.Column<int>(type: "int", nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CPF = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBSecretaria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBSecretaria_TBLogin_LoginID",
                        column: x => x.LoginID,
                        principalTable: "TBLogin",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TBConsulta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PacienteId = table.Column<int>(type: "int", nullable: true),
                    MedicoId = table.Column<int>(type: "int", nullable: true),
                    SecretariaId = table.Column<int>(type: "int", nullable: true),
                    DataConsulta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraConsulta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataCadastroConsulta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraFim = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Pagamento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Presenca = table.Column<bool>(type: "bit", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBConsulta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBConsulta_TBMedico_MedicoId",
                        column: x => x.MedicoId,
                        principalTable: "TBMedico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TBConsulta_TBPaciente_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "TBPaciente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TBConsulta_TBSecretaria_SecretariaId",
                        column: x => x.SecretariaId,
                        principalTable: "TBSecretaria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TBAdministrador_LoginID",
                table: "TBAdministrador",
                column: "LoginID");

            migrationBuilder.CreateIndex(
                name: "IX_TBConsulta_MedicoId",
                table: "TBConsulta",
                column: "MedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_TBConsulta_PacienteId",
                table: "TBConsulta",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_TBConsulta_SecretariaId",
                table: "TBConsulta",
                column: "SecretariaId");

            migrationBuilder.CreateIndex(
                name: "IX_TBLogin_Email",
                table: "TBLogin",
                column: "Email",
                unique: true);

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
                name: "IX_TBMedico_LoginID",
                table: "TBMedico",
                column: "LoginID");

            migrationBuilder.CreateIndex(
                name: "IX_TBSecretaria_CPF",
                table: "TBSecretaria",
                column: "CPF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TBSecretaria_LoginID",
                table: "TBSecretaria",
                column: "LoginID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBAdministrador");

            migrationBuilder.DropTable(
                name: "TBConsulta");

            migrationBuilder.DropTable(
                name: "TBMedico");

            migrationBuilder.DropTable(
                name: "TBPaciente");

            migrationBuilder.DropTable(
                name: "TBSecretaria");

            migrationBuilder.DropTable(
                name: "TBLogin");
        }
    }
}
