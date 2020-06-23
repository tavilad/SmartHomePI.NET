using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartHomePI.NET.API.Migrations
{
    public partial class updates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "TemperatureAndHumidities",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "Rooms",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "TemperatureAndHumidities");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "Rooms");
        }
    }
}
