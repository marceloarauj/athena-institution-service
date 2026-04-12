using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Institution.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveClassNameFromClassroom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_classroom_class_name_discipline_id",
                schema: "classroom",
                table: "classroom");

            migrationBuilder.DropColumn(
                name: "class_name",
                schema: "classroom",
                table: "classroom");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "class_name",
                schema: "classroom",
                table: "classroom",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_classroom_class_name_discipline_id",
                schema: "classroom",
                table: "classroom",
                columns: new[] { "class_name", "discipline_id" },
                unique: true);
        }
    }
}
