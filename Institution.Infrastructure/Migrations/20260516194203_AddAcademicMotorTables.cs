using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Institution.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAcademicMotorTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "academic");

            migrationBuilder.EnsureSchema(
                name: "enrollment");

            migrationBuilder.EnsureSchema(
                name: "scheduling");

            migrationBuilder.CreateTable(
                name: "academic_program",
                schema: "academic",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    period_type = table.Column<int>(type: "integer", nullable: false),
                    has_weekly_schedule = table.Column<bool>(type: "boolean", nullable: false),
                    duration_years = table.Column<int>(type: "integer", nullable: false),
                    min_completion_percent = table.Column<decimal>(type: "numeric", nullable: true),
                    min_school_days = table.Column<int>(type: "integer", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_academic_program", x => x.id);
                    table.ForeignKey(
                        name: "FK_academic_program_institution_institution_id",
                        column: x => x.institution_id,
                        principalSchema: "institution",
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "holiday",
                schema: "academic",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    is_recurring = table.Column<bool>(type: "boolean", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_holiday", x => x.id);
                    table.ForeignKey(
                        name: "FK_holiday_institution_institution_id",
                        column: x => x.institution_id,
                        principalSchema: "institution",
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "room",
                schema: "scheduling",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false),
                    has_lab = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room", x => x.id);
                    table.ForeignKey(
                        name: "FK_room_institution_institution_id",
                        column: x => x.institution_id,
                        principalSchema: "institution",
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shift",
                schema: "scheduling",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    start_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    end_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shift", x => x.id);
                    table.ForeignKey(
                        name: "FK_shift_institution_institution_id",
                        column: x => x.institution_id,
                        principalSchema: "institution",
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teacher",
                schema: "scheduling",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    email = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacher", x => x.id);
                    table.ForeignKey(
                        name: "FK_teacher_institution_institution_id",
                        column: x => x.institution_id,
                        principalSchema: "institution",
                        principalTable: "institution",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "program_edition",
                schema: "academic",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    published_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    academic_program_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_program_edition", x => x.id);
                    table.ForeignKey(
                        name: "FK_program_edition_academic_program_academic_program_id",
                        column: x => x.academic_program_id,
                        principalSchema: "academic",
                        principalTable: "academic_program",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subject",
                schema: "academic",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    code = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    academic_program_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subject", x => x.id);
                    table.ForeignKey(
                        name: "FK_subject_academic_program_academic_program_id",
                        column: x => x.academic_program_id,
                        principalSchema: "academic",
                        principalTable: "academic_program",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "schedule_slot",
                schema: "scheduling",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    start_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    end_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    shift_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schedule_slot", x => x.id);
                    table.ForeignKey(
                        name: "FK_schedule_slot_shift_shift_id",
                        column: x => x.shift_id,
                        principalSchema: "scheduling",
                        principalTable: "shift",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teacher_availability",
                schema: "scheduling",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    teacher_id = table.Column<Guid>(type: "uuid", nullable: false),
                    day_of_week = table.Column<int>(type: "integer", nullable: false),
                    shift_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacher_availability", x => x.id);
                    table.ForeignKey(
                        name: "FK_teacher_availability_shift_shift_id",
                        column: x => x.shift_id,
                        principalSchema: "scheduling",
                        principalTable: "shift",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_teacher_availability_teacher_teacher_id",
                        column: x => x.teacher_id,
                        principalSchema: "scheduling",
                        principalTable: "teacher",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "class_group",
                schema: "enrollment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    grade_or_year = table.Column<int>(type: "integer", nullable: true),
                    max_students = table.Column<int>(type: "integer", nullable: false),
                    program_edition_id = table.Column<Guid>(type: "uuid", nullable: false),
                    room_id = table.Column<Guid>(type: "uuid", nullable: true),
                    shift_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_class_group", x => x.id);
                    table.ForeignKey(
                        name: "FK_class_group_program_edition_program_edition_id",
                        column: x => x.program_edition_id,
                        principalSchema: "academic",
                        principalTable: "program_edition",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_class_group_room_room_id",
                        column: x => x.room_id,
                        principalSchema: "scheduling",
                        principalTable: "room",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_class_group_shift_shift_id",
                        column: x => x.shift_id,
                        principalSchema: "scheduling",
                        principalTable: "shift",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "conflict_report",
                schema: "enrollment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    generated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    total_critical = table.Column<int>(type: "integer", nullable: false),
                    total_high = table.Column<int>(type: "integer", nullable: false),
                    total_medium = table.Column<int>(type: "integer", nullable: false),
                    program_edition_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conflict_report", x => x.id);
                    table.ForeignKey(
                        name: "FK_conflict_report_program_edition_program_edition_id",
                        column: x => x.program_edition_id,
                        principalSchema: "academic",
                        principalTable: "program_edition",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "enrollment",
                schema: "enrollment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    grade_or_year = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    enrolled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    purchase_reference = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    program_edition_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enrollment", x => x.id);
                    table.ForeignKey(
                        name: "FK_enrollment_program_edition_program_edition_id",
                        column: x => x.program_edition_id,
                        principalSchema: "academic",
                        principalTable: "program_edition",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "program_period",
                schema: "academic",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    number = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    school_days = table.Column<int>(type: "integer", nullable: true),
                    program_edition_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_program_period", x => x.id);
                    table.ForeignKey(
                        name: "FK_program_period_program_edition_program_edition_id",
                        column: x => x.program_edition_id,
                        principalSchema: "academic",
                        principalTable: "program_edition",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "recess",
                schema: "academic",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    program_edition_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recess", x => x.id);
                    table.ForeignKey(
                        name: "FK_recess_program_edition_program_edition_id",
                        column: x => x.program_edition_id,
                        principalSchema: "academic",
                        principalTable: "program_edition",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "schedule_generation_log",
                schema: "scheduling",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    generated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    total_assigned = table.Column<int>(type: "integer", nullable: false),
                    total_unresolved = table.Column<int>(type: "integer", nullable: false),
                    program_edition_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schedule_generation_log", x => x.id);
                    table.ForeignKey(
                        name: "FK_schedule_generation_log_program_edition_program_edition_id",
                        column: x => x.program_edition_id,
                        principalSchema: "academic",
                        principalTable: "program_edition",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "curriculum_entry",
                schema: "academic",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    grade_or_year = table.Column<int>(type: "integer", nullable: true),
                    period_number = table.Column<int>(type: "integer", nullable: true),
                    weekly_hours = table.Column<int>(type: "integer", nullable: true),
                    total_hours = table.Column<int>(type: "integer", nullable: true),
                    program_edition_id = table.Column<Guid>(type: "uuid", nullable: false),
                    subject_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_curriculum_entry", x => x.id);
                    table.ForeignKey(
                        name: "FK_curriculum_entry_program_edition_program_edition_id",
                        column: x => x.program_edition_id,
                        principalSchema: "academic",
                        principalTable: "program_edition",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_curriculum_entry_subject_subject_id",
                        column: x => x.subject_id,
                        principalSchema: "academic",
                        principalTable: "subject",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teacher_subject",
                schema: "scheduling",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    teacher_id = table.Column<Guid>(type: "uuid", nullable: false),
                    subject_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacher_subject", x => x.id);
                    table.ForeignKey(
                        name: "FK_teacher_subject_subject_subject_id",
                        column: x => x.subject_id,
                        principalSchema: "academic",
                        principalTable: "subject",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_teacher_subject_teacher_teacher_id",
                        column: x => x.teacher_id,
                        principalSchema: "scheduling",
                        principalTable: "teacher",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "conflict_item",
                schema: "enrollment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    severity = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    class_group_id = table.Column<Guid>(type: "uuid", nullable: true),
                    teacher_id = table.Column<Guid>(type: "uuid", nullable: true),
                    subject_id = table.Column<Guid>(type: "uuid", nullable: true),
                    day_of_week = table.Column<int>(type: "integer", nullable: true),
                    schedule_slot_id = table.Column<Guid>(type: "uuid", nullable: true),
                    conflict_report_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conflict_item", x => x.id);
                    table.ForeignKey(
                        name: "FK_conflict_item_conflict_report_conflict_report_id",
                        column: x => x.conflict_report_id,
                        principalSchema: "enrollment",
                        principalTable: "conflict_report",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "class_group_student",
                schema: "enrollment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    assigned_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    class_group_id = table.Column<Guid>(type: "uuid", nullable: false),
                    enrollment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_class_group_student", x => x.id);
                    table.ForeignKey(
                        name: "FK_class_group_student_class_group_class_group_id",
                        column: x => x.class_group_id,
                        principalSchema: "enrollment",
                        principalTable: "class_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_class_group_student_enrollment_enrollment_id",
                        column: x => x.enrollment_id,
                        principalSchema: "enrollment",
                        principalTable: "enrollment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "calendar_day",
                schema: "academic",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    holiday_name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    program_edition_id = table.Column<Guid>(type: "uuid", nullable: false),
                    program_period_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_calendar_day", x => x.id);
                    table.ForeignKey(
                        name: "FK_calendar_day_program_edition_program_edition_id",
                        column: x => x.program_edition_id,
                        principalSchema: "academic",
                        principalTable: "program_edition",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_calendar_day_program_period_program_period_id",
                        column: x => x.program_period_id,
                        principalSchema: "academic",
                        principalTable: "program_period",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "class_schedule",
                schema: "scheduling",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    day_of_week = table.Column<int>(type: "integer", nullable: false),
                    class_group_id = table.Column<Guid>(type: "uuid", nullable: false),
                    subject_id = table.Column<Guid>(type: "uuid", nullable: false),
                    teacher_id = table.Column<Guid>(type: "uuid", nullable: false),
                    schedule_slot_id = table.Column<Guid>(type: "uuid", nullable: false),
                    program_period_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_class_schedule", x => x.id);
                    table.ForeignKey(
                        name: "FK_class_schedule_class_group_class_group_id",
                        column: x => x.class_group_id,
                        principalSchema: "enrollment",
                        principalTable: "class_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_class_schedule_program_period_program_period_id",
                        column: x => x.program_period_id,
                        principalSchema: "academic",
                        principalTable: "program_period",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_class_schedule_schedule_slot_schedule_slot_id",
                        column: x => x.schedule_slot_id,
                        principalSchema: "scheduling",
                        principalTable: "schedule_slot",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_class_schedule_subject_subject_id",
                        column: x => x.subject_id,
                        principalSchema: "academic",
                        principalTable: "subject",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_class_schedule_teacher_teacher_id",
                        column: x => x.teacher_id,
                        principalSchema: "scheduling",
                        principalTable: "teacher",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "progress_record",
                schema: "enrollment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    final_grade = table.Column<decimal>(type: "numeric", nullable: true),
                    completion_percent = table.Column<decimal>(type: "numeric", nullable: true),
                    enrollment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    program_period_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_progress_record", x => x.id);
                    table.ForeignKey(
                        name: "FK_progress_record_enrollment_enrollment_id",
                        column: x => x.enrollment_id,
                        principalSchema: "enrollment",
                        principalTable: "enrollment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_progress_record_program_period_program_period_id",
                        column: x => x.program_period_id,
                        principalSchema: "academic",
                        principalTable: "program_period",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_academic_program_institution_id",
                schema: "academic",
                table: "academic_program",
                column: "institution_id");

            migrationBuilder.CreateIndex(
                name: "IX_calendar_day_program_edition_id",
                schema: "academic",
                table: "calendar_day",
                column: "program_edition_id");

            migrationBuilder.CreateIndex(
                name: "IX_calendar_day_program_period_id",
                schema: "academic",
                table: "calendar_day",
                column: "program_period_id");

            migrationBuilder.CreateIndex(
                name: "IX_class_group_program_edition_id",
                schema: "enrollment",
                table: "class_group",
                column: "program_edition_id");

            migrationBuilder.CreateIndex(
                name: "IX_class_group_room_id",
                schema: "enrollment",
                table: "class_group",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_class_group_shift_id",
                schema: "enrollment",
                table: "class_group",
                column: "shift_id");

            migrationBuilder.CreateIndex(
                name: "IX_class_group_student_class_group_id",
                schema: "enrollment",
                table: "class_group_student",
                column: "class_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_class_group_student_enrollment_id",
                schema: "enrollment",
                table: "class_group_student",
                column: "enrollment_id");

            migrationBuilder.CreateIndex(
                name: "IX_class_schedule_class_group_id",
                schema: "scheduling",
                table: "class_schedule",
                column: "class_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_class_schedule_program_period_id",
                schema: "scheduling",
                table: "class_schedule",
                column: "program_period_id");

            migrationBuilder.CreateIndex(
                name: "IX_class_schedule_schedule_slot_id",
                schema: "scheduling",
                table: "class_schedule",
                column: "schedule_slot_id");

            migrationBuilder.CreateIndex(
                name: "IX_class_schedule_subject_id",
                schema: "scheduling",
                table: "class_schedule",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_class_schedule_teacher_id",
                schema: "scheduling",
                table: "class_schedule",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_conflict_item_conflict_report_id",
                schema: "enrollment",
                table: "conflict_item",
                column: "conflict_report_id");

            migrationBuilder.CreateIndex(
                name: "IX_conflict_report_program_edition_id",
                schema: "enrollment",
                table: "conflict_report",
                column: "program_edition_id");

            migrationBuilder.CreateIndex(
                name: "IX_curriculum_entry_program_edition_id",
                schema: "academic",
                table: "curriculum_entry",
                column: "program_edition_id");

            migrationBuilder.CreateIndex(
                name: "IX_curriculum_entry_subject_id",
                schema: "academic",
                table: "curriculum_entry",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_enrollment_program_edition_id",
                schema: "enrollment",
                table: "enrollment",
                column: "program_edition_id");

            migrationBuilder.CreateIndex(
                name: "IX_holiday_institution_id",
                schema: "academic",
                table: "holiday",
                column: "institution_id");

            migrationBuilder.CreateIndex(
                name: "IX_program_edition_academic_program_id",
                schema: "academic",
                table: "program_edition",
                column: "academic_program_id");

            migrationBuilder.CreateIndex(
                name: "IX_program_period_program_edition_id",
                schema: "academic",
                table: "program_period",
                column: "program_edition_id");

            migrationBuilder.CreateIndex(
                name: "IX_progress_record_enrollment_id",
                schema: "enrollment",
                table: "progress_record",
                column: "enrollment_id");

            migrationBuilder.CreateIndex(
                name: "IX_progress_record_program_period_id",
                schema: "enrollment",
                table: "progress_record",
                column: "program_period_id");

            migrationBuilder.CreateIndex(
                name: "IX_recess_program_edition_id",
                schema: "academic",
                table: "recess",
                column: "program_edition_id");

            migrationBuilder.CreateIndex(
                name: "IX_room_institution_id",
                schema: "scheduling",
                table: "room",
                column: "institution_id");

            migrationBuilder.CreateIndex(
                name: "IX_schedule_generation_log_program_edition_id",
                schema: "scheduling",
                table: "schedule_generation_log",
                column: "program_edition_id");

            migrationBuilder.CreateIndex(
                name: "IX_schedule_slot_shift_id",
                schema: "scheduling",
                table: "schedule_slot",
                column: "shift_id");

            migrationBuilder.CreateIndex(
                name: "IX_shift_institution_id",
                schema: "scheduling",
                table: "shift",
                column: "institution_id");

            migrationBuilder.CreateIndex(
                name: "IX_subject_academic_program_id",
                schema: "academic",
                table: "subject",
                column: "academic_program_id");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_institution_id",
                schema: "scheduling",
                table: "teacher",
                column: "institution_id");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_availability_shift_id",
                schema: "scheduling",
                table: "teacher_availability",
                column: "shift_id");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_availability_teacher_id",
                schema: "scheduling",
                table: "teacher_availability",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_subject_subject_id",
                schema: "scheduling",
                table: "teacher_subject",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_subject_teacher_id",
                schema: "scheduling",
                table: "teacher_subject",
                column: "teacher_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "calendar_day",
                schema: "academic");

            migrationBuilder.DropTable(
                name: "class_group_student",
                schema: "enrollment");

            migrationBuilder.DropTable(
                name: "class_schedule",
                schema: "scheduling");

            migrationBuilder.DropTable(
                name: "conflict_item",
                schema: "enrollment");

            migrationBuilder.DropTable(
                name: "curriculum_entry",
                schema: "academic");

            migrationBuilder.DropTable(
                name: "holiday",
                schema: "academic");

            migrationBuilder.DropTable(
                name: "progress_record",
                schema: "enrollment");

            migrationBuilder.DropTable(
                name: "recess",
                schema: "academic");

            migrationBuilder.DropTable(
                name: "schedule_generation_log",
                schema: "scheduling");

            migrationBuilder.DropTable(
                name: "teacher_availability",
                schema: "scheduling");

            migrationBuilder.DropTable(
                name: "teacher_subject",
                schema: "scheduling");

            migrationBuilder.DropTable(
                name: "class_group",
                schema: "enrollment");

            migrationBuilder.DropTable(
                name: "schedule_slot",
                schema: "scheduling");

            migrationBuilder.DropTable(
                name: "conflict_report",
                schema: "enrollment");

            migrationBuilder.DropTable(
                name: "enrollment",
                schema: "enrollment");

            migrationBuilder.DropTable(
                name: "program_period",
                schema: "academic");

            migrationBuilder.DropTable(
                name: "subject",
                schema: "academic");

            migrationBuilder.DropTable(
                name: "teacher",
                schema: "scheduling");

            migrationBuilder.DropTable(
                name: "room",
                schema: "scheduling");

            migrationBuilder.DropTable(
                name: "shift",
                schema: "scheduling");

            migrationBuilder.DropTable(
                name: "program_edition",
                schema: "academic");

            migrationBuilder.DropTable(
                name: "academic_program",
                schema: "academic");
        }
    }
}
