using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlpaTabApi.Migrations
{
    public partial class removedBalanceBeforeTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BalanceBeforeTransaction",
                table: "AlpaTabTransaction");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BalanceBeforeTransaction",
                table: "AlpaTabTransaction",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
