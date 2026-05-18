using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace patinhasemdia.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TUTOR",
                columns: table => new
                {
                    ID_TUTOR = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    TELEFONE = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TUTOR", x => x.ID_TUTOR);
                });

            migrationBuilder.CreateTable(
                name: "PET",
                columns: table => new
                {
                    ID_PET = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    ESPECIE = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    RACA = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    IDADE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ID_TUTOR = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PET", x => x.ID_PET);
                    table.ForeignKey(
                        name: "FK_PET_TUTOR_ID_TUTOR",
                        column: x => x.ID_TUTOR,
                        principalTable: "TUTOR",
                        principalColumn: "ID_TUTOR",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EVENTO_CUIDADO",
                columns: table => new
                {
                    ID_EVENTO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_PET = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TIPO_CUIDADO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    DATA_PREVISTA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    STATUS = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    PRIORIDADE = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                    OBSERVACAO = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENTO_CUIDADO", x => x.ID_EVENTO);
                    table.ForeignKey(
                        name: "FK_EVENTO_CUIDADO_PET_ID_PET",
                        column: x => x.ID_PET,
                        principalTable: "PET",
                        principalColumn: "ID_PET",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EVENTO_CUIDADO_ID_PET",
                table: "EVENTO_CUIDADO",
                column: "ID_PET");

            migrationBuilder.CreateIndex(
                name: "IX_PET_ID_TUTOR",
                table: "PET",
                column: "ID_TUTOR");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EVENTO_CUIDADO");

            migrationBuilder.DropTable(
                name: "PET");

            migrationBuilder.DropTable(
                name: "TUTOR");
        }
    }
}
