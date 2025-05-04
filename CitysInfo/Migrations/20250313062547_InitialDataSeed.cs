using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CitysInfo.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cities",
                columns: ["Id", "Description", "Name"],
                values: new object[,]
                {
                    { 1, "The one with that with park.", "Tehran" },
                    { 2, "The one with the ....", "Ahwaz" },
                    { 3, "The one with that with park.", "Shiraz" }
                });

            migrationBuilder.InsertData(
                table: "PointsOfInterest",
                columns: ["Id", "CityId", "Description", "Name"],
                values: new object[,]
                {
                    { 1, 1, "The most visit unban park in the Iran.", "Central Park" },
                    { 2, 1, "A 102-story skyscraper located in Midtown Tehran", "Empire State Building" },
                    { 3, 2, "A Gothic style cathedral, conceived by architects Jan.", "Cathedral" },
                    { 4, 2, "The the finest example of railway architecture in Belgi", "Antwerp Central Station" },
                    { 5, 3, "A wrought iron lattice tower on the Champ de Mars, name", "Eiffel Tower" },
                    { 6, 3, "The world's largest museum.", "The Louvre" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
