using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FolderTree.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DirectoryNodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ParentDirectoryId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectoryNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DirectoryNodes_DirectoryNodes_ParentDirectoryId",
                        column: x => x.ParentDirectoryId,
                        principalTable: "DirectoryNodes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DirectoryNodes_ParentDirectoryId",
                table: "DirectoryNodes",
                column: "ParentDirectoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DirectoryNodes");
        }
    }
}
