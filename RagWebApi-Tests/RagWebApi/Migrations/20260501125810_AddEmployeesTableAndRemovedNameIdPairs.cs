using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RagWebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeesTableAndRemovedNameIdPairs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NameIdPairs");

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Attrition = table.Column<bool>(type: "bit", nullable: false),
                    BusinessTravel = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DailyRate = table.Column<int>(type: "int", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DistanceFromHome = table.Column<int>(type: "int", nullable: false),
                    Education = table.Column<int>(type: "int", nullable: false),
                    EducationField = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    EmployeeCount = table.Column<int>(type: "int", nullable: false),
                    EnvironmentSatisfaction = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    HourlyRate = table.Column<int>(type: "int", nullable: false),
                    JobInvolvement = table.Column<int>(type: "int", nullable: false),
                    JobLevel = table.Column<int>(type: "int", nullable: false),
                    JobRole = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    JobSatisfaction = table.Column<int>(type: "int", nullable: false),
                    MaritalStatus = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    MonthlyIncome = table.Column<double>(type: "float", nullable: false),
                    MonthlyRate = table.Column<double>(type: "float", nullable: false),
                    NumCompaniesWorked = table.Column<int>(type: "int", nullable: false),
                    Over18 = table.Column<bool>(type: "bit", nullable: false),
                    OverTime = table.Column<bool>(type: "bit", nullable: false),
                    PercentSalaryHike = table.Column<double>(type: "float", nullable: false),
                    PerformanceRating = table.Column<int>(type: "int", nullable: false),
                    RelationshipSatisfaction = table.Column<int>(type: "int", nullable: false),
                    StandardHours = table.Column<int>(type: "int", nullable: false),
                    StockOptionLevel = table.Column<int>(type: "int", nullable: false),
                    TotalWorkingYears = table.Column<int>(type: "int", nullable: false),
                    TrainingTimesLastYear = table.Column<int>(type: "int", nullable: false),
                    WorkLifeBalance = table.Column<int>(type: "int", nullable: false),
                    YearsAtCompany = table.Column<int>(type: "int", nullable: false),
                    YearsInCurrentRole = table.Column<int>(type: "int", nullable: false),
                    YearsSinceLastPromotion = table.Column<int>(type: "int", nullable: false),
                    YearsWithCurrManager = table.Column<int>(type: "int", nullable: false),
                    DescriptionEmbedding = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeNumber);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.CreateTable(
                name: "NameIdPairs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NameIdPairs", x => x.Id);
                });
        }
    }
}
