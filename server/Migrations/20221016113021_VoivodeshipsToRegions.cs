using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class VoivodeshipsToRegions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Voivodeships_VoivodeshipId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Voivodeships");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "VoivodeshipId",
                table: "Users",
                newName: "RegionId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_VoivodeshipId",
                table: "Users",
                newName: "IX_Users_RegionId");

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Regions_RegionId",
                table: "Users",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Regions_RegionId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.RenameColumn(
                name: "RegionId",
                table: "Users",
                newName: "VoivodeshipId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_RegionId",
                table: "Users",
                newName: "IX_Users_VoivodeshipId");

            migrationBuilder.AddColumn<float>(
                name: "Height",
                table: "Users",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Weight",
                table: "Users",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "Voivodeships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voivodeships", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Voivodeships_VoivodeshipId",
                table: "Users",
                column: "VoivodeshipId",
                principalTable: "Voivodeships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
