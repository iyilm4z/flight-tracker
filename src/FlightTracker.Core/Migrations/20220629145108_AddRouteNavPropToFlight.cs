using Microsoft.EntityFrameworkCore.Migrations;

namespace FlightTracker.Migrations
{
    public partial class AddRouteNavPropToFlight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Flight_RouteId",
                table: "Flight",
                column: "RouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flight_Route_RouteId",
                table: "Flight",
                column: "RouteId",
                principalTable: "Route",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flight_Route_RouteId",
                table: "Flight");

            migrationBuilder.DropIndex(
                name: "IX_Flight_RouteId",
                table: "Flight");
        }
    }
}
