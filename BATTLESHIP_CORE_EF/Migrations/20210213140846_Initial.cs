using Microsoft.EntityFrameworkCore.Migrations;

namespace BATTLESHIP_CORE_EF.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Board",
                columns: table => new
                {
                    GAMES_ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    STATUS = table.Column<string>(type: "TEXT", nullable: true),
                    SIZE_X = table.Column<int>(type: "INTEGER", nullable: false),
                    SIZE_Y = table.Column<int>(type: "INTEGER", nullable: false),
                    RESULT_WIN = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Board", x => x.GAMES_ID);
                });

            migrationBuilder.CreateTable(
                name: "Ship",
                columns: table => new
                {
                    SHIP_ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SHIP_NAME = table.Column<string>(type: "TEXT", nullable: true),
                    SIZE_X = table.Column<int>(type: "INTEGER", nullable: false),
                    SIZE_Y = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ship", x => x.SHIP_ID);
                });

            migrationBuilder.CreateTable(
                name: "ActionMove",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GAME_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    PLAYER = table.Column<int>(type: "INTEGER", nullable: false),
                    X = table.Column<int>(type: "INTEGER", nullable: false),
                    Y = table.Column<int>(type: "INTEGER", nullable: false),
                    RESULT = table.Column<string>(type: "TEXT", nullable: true),
                    BoardGAMES_ID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionMove", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ActionMove_Board_BoardGAMES_ID",
                        column: x => x.BoardGAMES_ID,
                        principalTable: "Board",
                        principalColumn: "GAMES_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActionShipInstall",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PLAYER = table.Column<int>(type: "INTEGER", nullable: false),
                    GAME_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    SHIP_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    X = table.Column<int>(type: "INTEGER", nullable: false),
                    Y = table.Column<int>(type: "INTEGER", nullable: false),
                    IS_HIT = table.Column<bool>(type: "INTEGER", nullable: true),
                    BoardGAMES_ID = table.Column<int>(type: "INTEGER", nullable: true),
                    SHIP_ID1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionShipInstall", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ActionShipInstall_Board_BoardGAMES_ID",
                        column: x => x.BoardGAMES_ID,
                        principalTable: "Board",
                        principalColumn: "GAMES_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionShipInstall_Ship_SHIP_ID1",
                        column: x => x.SHIP_ID1,
                        principalTable: "Ship",
                        principalColumn: "SHIP_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionMove_BoardGAMES_ID",
                table: "ActionMove",
                column: "BoardGAMES_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ActionShipInstall_BoardGAMES_ID",
                table: "ActionShipInstall",
                column: "BoardGAMES_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ActionShipInstall_SHIP_ID1",
                table: "ActionShipInstall",
                column: "SHIP_ID1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionMove");

            migrationBuilder.DropTable(
                name: "ActionShipInstall");

            migrationBuilder.DropTable(
                name: "Board");

            migrationBuilder.DropTable(
                name: "Ship");
        }
    }
}
