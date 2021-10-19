using Microsoft.EntityFrameworkCore.Migrations;

namespace ClinicaMedicaWeb.Migrations
{
    public partial class atualizacaoModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TBLogin_TBAdministrador_AdministradorID",
                table: "TBLogin");

            migrationBuilder.DropForeignKey(
                name: "FK_TBLogin_TBMedico_MedicoID",
                table: "TBLogin");

            migrationBuilder.DropForeignKey(
                name: "FK_TBLogin_TBSecretaria_SecretariaID",
                table: "TBLogin");

            migrationBuilder.DropIndex(
                name: "IX_TBLogin_AdministradorID",
                table: "TBLogin");

            migrationBuilder.DropIndex(
                name: "IX_TBLogin_MedicoID",
                table: "TBLogin");

            migrationBuilder.DropIndex(
                name: "IX_TBLogin_SecretariaID",
                table: "TBLogin");

            migrationBuilder.DropColumn(
                name: "AdministradorID",
                table: "TBLogin");

            migrationBuilder.DropColumn(
                name: "MedicoID",
                table: "TBLogin");

            migrationBuilder.DropColumn(
                name: "SecretariaID",
                table: "TBLogin");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "TBSecretaria",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LoginID",
                table: "TBSecretaria",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "TBMedico",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LoginID",
                table: "TBMedico",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "TBLogin",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "TipoUsuario",
                table: "TBLogin",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "TBAdministrador",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LoginID",
                table: "TBAdministrador",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TBSecretaria_LoginID",
                table: "TBSecretaria",
                column: "LoginID");

            migrationBuilder.CreateIndex(
                name: "IX_TBMedico_LoginID",
                table: "TBMedico",
                column: "LoginID");

            migrationBuilder.CreateIndex(
                name: "IX_TBLogin_Email",
                table: "TBLogin",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TBAdministrador_LoginID",
                table: "TBAdministrador",
                column: "LoginID");

            migrationBuilder.AddForeignKey(
                name: "FK_TBAdministrador_TBLogin_LoginID",
                table: "TBAdministrador",
                column: "LoginID",
                principalTable: "TBLogin",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TBMedico_TBLogin_LoginID",
                table: "TBMedico",
                column: "LoginID",
                principalTable: "TBLogin",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TBSecretaria_TBLogin_LoginID",
                table: "TBSecretaria",
                column: "LoginID",
                principalTable: "TBLogin",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TBAdministrador_TBLogin_LoginID",
                table: "TBAdministrador");

            migrationBuilder.DropForeignKey(
                name: "FK_TBMedico_TBLogin_LoginID",
                table: "TBMedico");

            migrationBuilder.DropForeignKey(
                name: "FK_TBSecretaria_TBLogin_LoginID",
                table: "TBSecretaria");

            migrationBuilder.DropIndex(
                name: "IX_TBSecretaria_LoginID",
                table: "TBSecretaria");

            migrationBuilder.DropIndex(
                name: "IX_TBMedico_LoginID",
                table: "TBMedico");

            migrationBuilder.DropIndex(
                name: "IX_TBLogin_Email",
                table: "TBLogin");

            migrationBuilder.DropIndex(
                name: "IX_TBAdministrador_LoginID",
                table: "TBAdministrador");

            migrationBuilder.DropColumn(
                name: "LoginID",
                table: "TBSecretaria");

            migrationBuilder.DropColumn(
                name: "LoginID",
                table: "TBMedico");

            migrationBuilder.DropColumn(
                name: "TipoUsuario",
                table: "TBLogin");

            migrationBuilder.DropColumn(
                name: "LoginID",
                table: "TBAdministrador");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "TBSecretaria",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "TBMedico",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "TBLogin",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "AdministradorID",
                table: "TBLogin",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MedicoID",
                table: "TBLogin",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecretariaID",
                table: "TBLogin",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "TBAdministrador",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.AddForeignKey(
                name: "FK_TBLogin_TBAdministrador_AdministradorID",
                table: "TBLogin",
                column: "AdministradorID",
                principalTable: "TBAdministrador",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TBLogin_TBMedico_MedicoID",
                table: "TBLogin",
                column: "MedicoID",
                principalTable: "TBMedico",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TBLogin_TBSecretaria_SecretariaID",
                table: "TBLogin",
                column: "SecretariaID",
                principalTable: "TBSecretaria",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
