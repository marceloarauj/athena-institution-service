using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Institution.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDayLessonScheduleConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "lesson_replacement_id",
                schema: "classroom",
                table: "day_lesson",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "classroom_id",
                schema: "classroom",
                table: "day_lesson",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "day_lesson_schedule_config",
                schema: "institution",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lesson_start_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    lesson_end_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    days_of_week = table.Column<int>(type: "integer", nullable: false),
                    lesson_count = table.Column<int>(type: "integer", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    discipline_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_day_lesson_schedule_config", x => x.id);
                    table.ForeignKey(
                        name: "FK_day_lesson_schedule_config_discipline_discipline_id",
                        column: x => x.discipline_id,
                        principalSchema: "institution",
                        principalTable: "discipline",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_day_lesson_schedule_config_institution_institution_id",
                        column: x => x.institution_id,
                        principalSchema: "institution",
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "day_lesson_schedule_config_history",
                schema: "institution",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by_user = table.Column<Guid>(type: "uuid", nullable: false),
                    field = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    old_value = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    new_value = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    config_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_day_lesson_schedule_config_history", x => x.id);
                    table.ForeignKey(
                        name: "FK_day_lesson_schedule_config_history_day_lesson_schedule_conf~",
                        column: x => x.config_id,
                        principalSchema: "institution",
                        principalTable: "day_lesson_schedule_config",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_day_lesson_classroom_id",
                schema: "classroom",
                table: "day_lesson",
                column: "classroom_id");

            migrationBuilder.CreateIndex(
                name: "IX_day_lesson_schedule_config_discipline_id",
                schema: "institution",
                table: "day_lesson_schedule_config",
                column: "discipline_id");

            migrationBuilder.CreateIndex(
                name: "IX_day_lesson_schedule_config_institution_id",
                schema: "institution",
                table: "day_lesson_schedule_config",
                column: "institution_id");

            migrationBuilder.CreateIndex(
                name: "IX_day_lesson_schedule_config_history_config_id",
                schema: "institution",
                table: "day_lesson_schedule_config_history",
                column: "config_id");

            migrationBuilder.AddForeignKey(
                name: "FK_day_lesson_classroom_classroom_id",
                schema: "classroom",
                table: "day_lesson",
                column: "classroom_id",
                principalSchema: "classroom",
                principalTable: "classroom",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_day_lesson_classroom_classroom_id",
                schema: "classroom",
                table: "day_lesson");

            migrationBuilder.DropTable(
                name: "day_lesson_schedule_config_history",
                schema: "institution");

            migrationBuilder.DropTable(
                name: "day_lesson_schedule_config",
                schema: "institution");

            migrationBuilder.DropIndex(
                name: "IX_day_lesson_classroom_id",
                schema: "classroom",
                table: "day_lesson");

            migrationBuilder.DropColumn(
                name: "classroom_id",
                schema: "classroom",
                table: "day_lesson");

            migrationBuilder.AlterColumn<Guid>(
                name: "lesson_replacement_id",
                schema: "classroom",
                table: "day_lesson",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
