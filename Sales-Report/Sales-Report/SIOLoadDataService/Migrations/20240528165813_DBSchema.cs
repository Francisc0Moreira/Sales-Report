using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIOLoadDataService.Migrations
{
    /// <inheritdoc />
    public partial class DBSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                table: "SalesLines");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "Locals");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "Clients");

            migrationBuilder.CreateTable(
                name: "ClientDimension",
                columns: table => new
                {
                    ClientKey = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientDimension", x => x.ClientKey);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    CompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nif = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FiscalYear = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "LocalDimension",
                columns: table => new
                {
                    LocalKey = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalDimension", x => x.LocalKey);
                });

            migrationBuilder.CreateTable(
                name: "ProductDimension",
                columns: table => new
                {
                    ProductKey = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Family = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDimension", x => x.ProductKey);
                });

            migrationBuilder.CreateTable(
                name: "SalesFactTable",
                columns: table => new
                {
                    S_Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Sales_Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    QuantitySold = table.Column<int>(type: "int", nullable: true),
                    Sales_Profit = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesFactTable", x => x.S_Key);
                });

            migrationBuilder.CreateTable(
                name: "TimeDimension",
                columns: table => new
                {
                    DateKey = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: true),
                    Quarter = table.Column<int>(type: "int", nullable: true),
                    Semester = table.Column<int>(type: "int", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeDimension", x => x.DateKey);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientDimension");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "LocalDimension");

            migrationBuilder.DropTable(
                name: "ProductDimension");

            migrationBuilder.DropTable(
                name: "SalesFactTable");

            migrationBuilder.DropTable(
                name: "TimeDimension");

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "SalesLines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "Locals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
