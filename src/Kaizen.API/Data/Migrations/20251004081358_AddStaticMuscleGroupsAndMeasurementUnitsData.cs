using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Kaizen.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStaticMuscleGroupsAndMeasurementUnitsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MeasurementUnits",
                columns: new[] { "Code", "Name" },
                values: new object[,]
                {
                    { "kg", "Kilograms" },
                    { "lvl", "Level" }
                });

            migrationBuilder.InsertData(
                table: "MuscleGroups",
                columns: new[] { "Code", "Name" },
                values: new object[,]
                {
                    { "abductors", "Abductors" },
                    { "abs", "Abdominal" },
                    { "adductors", "Adductors" },
                    { "biceps", "Biceps" },
                    { "calves", "Calves" },
                    { "chest", "Chest" },
                    { "forearms", "Forearms" },
                    { "glutes", "Gluteus Maximus" },
                    { "hamstrings", "Hamstrings" },
                    { "hip_flexors", "Hip Flexors" },
                    { "it_band", "IT Band" },
                    { "lats", "Latissimus Dorsi" },
                    { "lower_back", "Lower Back" },
                    { "neck", "Neck" },
                    { "obliques", "Obliques" },
                    { "quads", "Quadriceps" },
                    { "shoulders", "Shoulders" },
                    { "traps", "Trapezius" },
                    { "triceps", "Triceps" },
                    { "upper_back", "Upper Back" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MeasurementUnits",
                keyColumn: "Code",
                keyValue: "kg");

            migrationBuilder.DeleteData(
                table: "MeasurementUnits",
                keyColumn: "Code",
                keyValue: "lvl");

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Code",
                keyValue: "abductors");

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Code",
                keyValue: "abs");

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Code",
                keyValue: "adductors");

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Code",
                keyValue: "biceps");

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Code",
                keyValue: "calves");

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Code",
                keyValue: "chest");

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Code",
                keyValue: "forearms");

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Code",
                keyValue: "glutes");

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Code",
                keyValue: "hamstrings");

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Code",
                keyValue: "hip_flexors");

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Code",
                keyValue: "it_band");

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Code",
                keyValue: "lats");

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Code",
                keyValue: "lower_back");

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Code",
                keyValue: "neck");

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Code",
                keyValue: "obliques");

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Code",
                keyValue: "quads");

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Code",
                keyValue: "shoulders");

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Code",
                keyValue: "traps");

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Code",
                keyValue: "triceps");

            migrationBuilder.DeleteData(
                table: "MuscleGroups",
                keyColumn: "Code",
                keyValue: "upper_back");
        }
    }
}
