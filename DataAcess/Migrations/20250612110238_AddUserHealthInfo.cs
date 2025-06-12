using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAcess.Migrations
{
    /// <inheritdoc />
    public partial class AddUserHealthInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserHealthInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: true),
                    Height = table.Column<float>(type: "real", nullable: true),
                    IsSmoker = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MedicalConditions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StressLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhysicalTiredness = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MentalExhaustion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExerciseTiredness = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Exercise = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExerciseDays = table.Column<int>(type: "int", nullable: true),
                    EnergyLevel = table.Column<int>(type: "int", nullable: false),
                    Diabetes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BloodPressureMedications = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeartRate = table.Column<int>(type: "int", nullable: true),
                    GlucoseLevel = table.Column<int>(type: "int", nullable: true),
                    SystolicBP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cholesterol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Angina = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CardiovascularDisease = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoronaryHeartDisease = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeartAttack = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stroke = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hypertension = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHealthInfos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserHealthInfos");
        }
    }
}
