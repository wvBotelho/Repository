using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WVB.Framework.EntityFrameworkRepository.UnitTest.Migrations
{
    public partial class VersaoInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "auditoria_wvb_rep",
                columns: table => new
                {
                    id_auditoria = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    id_entidade = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    nome_entidade = table.Column<string>(type: "varchar(100)", nullable: false),
                    alterado_por = table.Column<string>(type: "varchar(50)", nullable: true),
                    data_alteracao = table.Column<DateTime>(type: "datetime", nullable: false),
                    ultima_acao = table.Column<string>(type: "varchar(50)", nullable: false),
                    campos_alterados = table.Column<string>(type: "nvarchar(4000)", nullable: true),
                    valores_adicionados = table.Column<string>(type: "nvarchar(4000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_auditoria_id", x => x.id_auditoria);
                });

            migrationBuilder.CreateTable(
                name: "customer_wvb_rep",
                columns: table => new
                {
                    customer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    project_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "varchar(100)", nullable: false),
                    email = table.Column<string>(type: "varchar(100)", nullable: false),
                    phone = table.Column<string>(type: "varchar(14)", nullable: false),
                    deletado = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customer_id", x => x.customer_id);
                });

            migrationBuilder.CreateTable(
                name: "project_wvb_rep",
                columns: table => new
                {
                    project_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    customer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    name = table.Column<string>(type: "varchar(100)", nullable: false),
                    description = table.Column<string>(type: "NVARCHAR(1000)", nullable: false),
                    start = table.Column<DateTime>(type: "datetime", nullable: false),
                    end = table.Column<DateTime>(type: "datetime", nullable: true),
                    budget = table.Column<decimal>(type: "money", nullable: false),
                    critical = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    deletado = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_id", x => x.project_id);
                    table.ForeignKey(
                        name: "FK_project_wvb_rep_customer_wvb_rep_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customer_wvb_rep",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "project_resource_wvb_rep",
                columns: table => new
                {
                    project_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    resource_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_resource_id", x => new { x.project_id, x.resource_id });
                    table.ForeignKey(
                        name: "FK_project_resource_wvb_rep_project_wvb_rep_project_id",
                        column: x => x.project_id,
                        principalTable: "project_wvb_rep",
                        principalColumn: "project_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "technology_wvb_rep",
                columns: table => new
                {
                    technology_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    resource_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    name = table.Column<string>(type: "varchar(100)", nullable: false),
                    deletado = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_technology_id", x => x.technology_id);
                });

            migrationBuilder.CreateTable(
                name: "resource_wvb_rep",
                columns: table => new
                {
                    resource_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    technology_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    name = table.Column<string>(type: "varchar(100)", nullable: false),
                    email = table.Column<string>(type: "varchar(100)", nullable: false),
                    phone = table.Column<string>(type: "varchar(14)", nullable: false),
                    deletado = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_resource_id", x => x.resource_id);
                    table.ForeignKey(
                        name: "FK_resource_wvb_rep_technology_wvb_rep_technology_id",
                        column: x => x.technology_id,
                        principalTable: "technology_wvb_rep",
                        principalColumn: "technology_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_entidade",
                table: "auditoria_wvb_rep",
                columns: new[] { "id_entidade", "nome_entidade" });

            migrationBuilder.CreateIndex(
                name: "IX_project_resource_wvb_rep_resource_id",
                table: "project_resource_wvb_rep",
                column: "resource_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_wvb_rep_customer_id",
                table: "project_wvb_rep",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_resource_wvb_rep_technology_id",
                table: "resource_wvb_rep",
                column: "technology_id");

            migrationBuilder.CreateIndex(
                name: "IX_technology_wvb_rep_resource_id",
                table: "technology_wvb_rep",
                column: "resource_id");

            migrationBuilder.AddForeignKey(
                name: "FK_project_resource_wvb_rep_resource_wvb_rep_resource_id",
                table: "project_resource_wvb_rep",
                column: "resource_id",
                principalTable: "resource_wvb_rep",
                principalColumn: "resource_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_technology_wvb_rep_resource_wvb_rep_resource_id",
                table: "technology_wvb_rep",
                column: "resource_id",
                principalTable: "resource_wvb_rep",
                principalColumn: "resource_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_technology_wvb_rep_resource_wvb_rep_resource_id",
                table: "technology_wvb_rep");

            migrationBuilder.DropTable(
                name: "auditoria_wvb_rep");

            migrationBuilder.DropTable(
                name: "project_resource_wvb_rep");

            migrationBuilder.DropTable(
                name: "project_wvb_rep");

            migrationBuilder.DropTable(
                name: "customer_wvb_rep");

            migrationBuilder.DropTable(
                name: "resource_wvb_rep");

            migrationBuilder.DropTable(
                name: "technology_wvb_rep");
        }
    }
}
