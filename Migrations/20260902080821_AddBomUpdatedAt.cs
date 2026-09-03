using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training.ProductApi1.Migrations
{
    /// <inheritdoc />
    public partial class AddBomUpdatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Boms",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Boms");
        }
    }
}
