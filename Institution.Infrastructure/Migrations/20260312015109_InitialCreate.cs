using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Institution.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "classroom");

            migrationBuilder.EnsureSchema(
                name: "institution");

            migrationBuilder.CreateTable(
                name: "day_lesson",
                schema: "classroom",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    started = table.Column<bool>(type: "boolean", nullable: false),
                    location = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    canceled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    teacher_id = table.Column<Guid>(type: "uuid", nullable: false),
                    lesson_replacement_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_day_lesson", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "institution",
                schema: "institution",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    alias = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    tax_document = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    display_name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_private = table.Column<bool>(type: "boolean", nullable: false),
                    charge_payment = table.Column<bool>(type: "boolean", nullable: false),
                    payment_format = table.Column<int>(type: "integer", nullable: false),
                    save_update_history = table.Column<bool>(type: "boolean", nullable: false),
                    logo_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    primary_color = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    secondary_color = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    danger_color = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_institution", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "student_day_lesson",
                schema: "classroom",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_present = table.Column<bool>(type: "boolean", nullable: true),
                    observation = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    day_lesson_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_day_lesson", x => x.id);
                    table.ForeignKey(
                        name: "FK_student_day_lesson_day_lesson_day_lesson_id",
                        column: x => x.day_lesson_id,
                        principalSchema: "classroom",
                        principalTable: "day_lesson",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "discipline",
                schema: "institution",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    study_hours = table.Column<int>(type: "integer", nullable: false),
                    credits = table.Column<int>(type: "integer", nullable: false),
                    available = table.Column<bool>(type: "boolean", nullable: false),
                    charge_payment = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discipline", x => x.id);
                    table.ForeignKey(
                        name: "FK_discipline_institution_institution_id",
                        column: x => x.institution_id,
                        principalSchema: "institution",
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "evaluation_variable",
                schema: "institution",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    key = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_evaluation_variable", x => x.id);
                    table.ForeignKey(
                        name: "FK_evaluation_variable_institution_institution_id",
                        column: x => x.institution_id,
                        principalSchema: "institution",
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "event",
                schema: "institution",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: true),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event", x => x.id);
                    table.ForeignKey(
                        name: "FK_event_institution_institution_id",
                        column: x => x.institution_id,
                        principalSchema: "institution",
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "institution_update_history",
                schema: "institution",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by_user = table.Column<Guid>(type: "uuid", nullable: false),
                    field = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    old_value = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    new_value = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_institution_update_history", x => x.id);
                    table.ForeignKey(
                        name: "FK_institution_update_history_institution_institution_id",
                        column: x => x.institution_id,
                        principalSchema: "institution",
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "classroom",
                schema: "classroom",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    max_students = table.Column<int>(type: "integer", nullable: true),
                    class_name = table.Column<Guid>(type: "uuid", nullable: false),
                    location = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    teacher_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by_user = table.Column<Guid>(type: "uuid", nullable: false),
                    discipline_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classroom", x => x.id);
                    table.ForeignKey(
                        name: "FK_classroom_discipline_discipline_id",
                        column: x => x.discipline_id,
                        principalSchema: "institution",
                        principalTable: "discipline",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "discipline_topic",
                schema: "institution",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    lesson_number = table.Column<int>(type: "integer", nullable: false),
                    discipline_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discipline_topic", x => x.id);
                    table.ForeignKey(
                        name: "FK_discipline_topic_discipline_discipline_id",
                        column: x => x.discipline_id,
                        principalSchema: "institution",
                        principalTable: "discipline",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "discipline_update_history",
                schema: "institution",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by_user = table.Column<Guid>(type: "uuid", nullable: false),
                    field = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    old_value = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    new_value = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    discipline_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discipline_update_history", x => x.id);
                    table.ForeignKey(
                        name: "FK_discipline_update_history_discipline_discipline_id",
                        column: x => x.discipline_id,
                        principalSchema: "institution",
                        principalTable: "discipline",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "evaluation_system",
                schema: "institution",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    formula = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    discipline_id = table.Column<Guid>(type: "uuid", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_evaluation_system", x => x.id);
                    table.ForeignKey(
                        name: "FK_evaluation_system_discipline_discipline_id",
                        column: x => x.discipline_id,
                        principalSchema: "institution",
                        principalTable: "discipline",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_evaluation_system_institution_institution_id",
                        column: x => x.institution_id,
                        principalSchema: "institution",
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "student_classroom_note",
                schema: "classroom",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    key = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    value = table.Column<decimal>(type: "numeric", nullable: false),
                    classroom_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_classroom_note", x => x.id);
                    table.ForeignKey(
                        name: "FK_student_classroom_note_classroom_classroom_id",
                        column: x => x.classroom_id,
                        principalSchema: "classroom",
                        principalTable: "classroom",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "student_classroom_registration",
                schema: "classroom",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_name = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    classroom_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_classroom_registration", x => x.id);
                    table.ForeignKey(
                        name: "FK_student_classroom_registration_classroom_classroom_id",
                        column: x => x.classroom_id,
                        principalSchema: "classroom",
                        principalTable: "classroom",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "day_lesson_discipline_topic",
                schema: "classroom",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    day_lesson_id = table.Column<Guid>(type: "uuid", nullable: false),
                    discipline_topic_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_day_lesson_discipline_topic", x => x.id);
                    table.ForeignKey(
                        name: "FK_day_lesson_discipline_topic_day_lesson_day_lesson_id",
                        column: x => x.day_lesson_id,
                        principalSchema: "classroom",
                        principalTable: "day_lesson",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_day_lesson_discipline_topic_discipline_topic_discipline_top~",
                        column: x => x.discipline_topic_id,
                        principalSchema: "institution",
                        principalTable: "discipline_topic",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_classroom_class_name_discipline_id",
                schema: "classroom",
                table: "classroom",
                columns: new[] { "class_name", "discipline_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_classroom_discipline_id",
                schema: "classroom",
                table: "classroom",
                column: "discipline_id");

            migrationBuilder.CreateIndex(
                name: "IX_day_lesson_discipline_topic_day_lesson_id",
                schema: "classroom",
                table: "day_lesson_discipline_topic",
                column: "day_lesson_id");

            migrationBuilder.CreateIndex(
                name: "IX_day_lesson_discipline_topic_discipline_topic_id",
                schema: "classroom",
                table: "day_lesson_discipline_topic",
                column: "discipline_topic_id");

            migrationBuilder.CreateIndex(
                name: "IX_discipline_institution_id",
                schema: "institution",
                table: "discipline",
                column: "institution_id");

            migrationBuilder.CreateIndex(
                name: "IX_discipline_topic_discipline_id",
                schema: "institution",
                table: "discipline_topic",
                column: "discipline_id");

            migrationBuilder.CreateIndex(
                name: "IX_discipline_update_history_discipline_id",
                schema: "institution",
                table: "discipline_update_history",
                column: "discipline_id");

            migrationBuilder.CreateIndex(
                name: "IX_evaluation_system_discipline_id",
                schema: "institution",
                table: "evaluation_system",
                column: "discipline_id");

            migrationBuilder.CreateIndex(
                name: "IX_evaluation_system_institution_id",
                schema: "institution",
                table: "evaluation_system",
                column: "institution_id");

            migrationBuilder.CreateIndex(
                name: "IX_evaluation_variable_institution_id_key",
                schema: "institution",
                table: "evaluation_variable",
                columns: new[] { "institution_id", "key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_event_institution_id",
                schema: "institution",
                table: "event",
                column: "institution_id");

            migrationBuilder.CreateIndex(
                name: "IX_institution_alias",
                schema: "institution",
                table: "institution",
                column: "alias",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_institution_update_history_institution_id",
                schema: "institution",
                table: "institution_update_history",
                column: "institution_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_classroom_note_classroom_id",
                schema: "classroom",
                table: "student_classroom_note",
                column: "classroom_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_classroom_note_student_id_classroom_id_key",
                schema: "classroom",
                table: "student_classroom_note",
                columns: new[] { "student_id", "classroom_id", "key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_classroom_registration_classroom_id",
                schema: "classroom",
                table: "student_classroom_registration",
                column: "classroom_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_day_lesson_day_lesson_id",
                schema: "classroom",
                table: "student_day_lesson",
                column: "day_lesson_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "day_lesson_discipline_topic",
                schema: "classroom");

            migrationBuilder.DropTable(
                name: "discipline_update_history",
                schema: "institution");

            migrationBuilder.DropTable(
                name: "evaluation_system",
                schema: "institution");

            migrationBuilder.DropTable(
                name: "evaluation_variable",
                schema: "institution");

            migrationBuilder.DropTable(
                name: "event",
                schema: "institution");

            migrationBuilder.DropTable(
                name: "institution_update_history",
                schema: "institution");

            migrationBuilder.DropTable(
                name: "student_classroom_note",
                schema: "classroom");

            migrationBuilder.DropTable(
                name: "student_classroom_registration",
                schema: "classroom");

            migrationBuilder.DropTable(
                name: "student_day_lesson",
                schema: "classroom");

            migrationBuilder.DropTable(
                name: "discipline_topic",
                schema: "institution");

            migrationBuilder.DropTable(
                name: "classroom",
                schema: "classroom");

            migrationBuilder.DropTable(
                name: "day_lesson",
                schema: "classroom");

            migrationBuilder.DropTable(
                name: "discipline",
                schema: "institution");

            migrationBuilder.DropTable(
                name: "institution",
                schema: "institution");
        }
    }
}
