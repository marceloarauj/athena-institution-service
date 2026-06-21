using Institution.Application.Dtos.Input;
using Institution.Application.Interfaces.Services;
using Institution.Application.Models;
using Institution.Domain.Entities;
using Institution.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Text.Json;

namespace Institution.Infrastructure.Services
{
    public class ReportCardService(AppDbContext dbContext, IGradeService gradeService) : IReportCardService
    {
        public async Task<byte[]> GeneratePdfAsync(
            InstitutionEntity institution,
            Guid studentId,
            string layoutJson,
            CancellationToken cancellationToken = default)
        {
            var variables = await dbContext.EvaluationVariables
                .Where(v => v.InstitutionId == institution.Id)
                .OrderBy(v => v.Key)
                .ToListAsync(cancellationToken);

            var registrations = await dbContext.StudentClassroomRegistrations
                .Include(r => r.Classroom)
                    .ThenInclude(c => c.Discipline)
                .Where(r => r.StudentId == studentId && r.IsActive)
                .Where(r => r.Classroom.Discipline!.InstitutionId == institution.Id)
                .ToListAsync(cancellationToken);

            if (registrations.Count == 0)
                return [];

            var studentName = registrations[0].StudentName;
            var classroomIds = registrations.Select(r => r.ClassroomId).ToList();

            var allNotes = await dbContext.StudentClassroomNotes
                .Where(n => n.StudentId == studentId && classroomIds.Contains(n.ClassroomId))
                .ToListAsync(cancellationToken);

            var disciplineIds = registrations.Select(r => r.Classroom.DisciplineId).Distinct().ToList();
            var evalSystems = await dbContext.EvaluationSystems
                .Where(s => s.InstitutionId == institution.Id && s.Active && disciplineIds.Contains(s.DisciplineId))
                .ToListAsync(cancellationToken);

            var gradeRows = new List<ClassroomGradeData>();
            foreach (var reg in registrations)
            {
                var notes = allNotes
                    .Where(n => n.ClassroomId == reg.ClassroomId)
                    .ToDictionary(n => n.Key, n => n.Value);

                var formula = evalSystems
                    .FirstOrDefault(s => s.DisciplineId == reg.Classroom.DisciplineId)?.Formula;

                bool? isApproved = null;
                if (formula != null && notes.Count > 0)
                {
                    try
                    {
                        var testGrades = notes.Select(kv => new TestGradeDto { Key = kv.Key, Value = (double)kv.Value }).ToList();
                        isApproved = await gradeService.IsApproved(testGrades, formula);
                    }
                    catch { }
                }

                gradeRows.Add(new ClassroomGradeData
                {
                    DisciplineName = reg.Classroom.Discipline!.Name,
                    Notes = notes,
                    IsApproved = isApproved,
                });
            }

            var layout = JsonSerializer.Deserialize<LayoutConfig>(layoutJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new LayoutConfig();

            return BuildPdf(institution, studentName, variables, gradeRows, layout);
        }

        private static byte[] BuildPdf(
            InstitutionEntity institution,
            string studentName,
            List<EvaluationVariableEntity> variables,
            List<ClassroomGradeData> grades,
            LayoutConfig layout)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var primaryColor = institution.PrimaryColor ?? "#2277DD";
            var showInstitutionName = layout.IsVisible("institution_name");
            var showStudentName = layout.IsVisible("student_name");
            var customTexts = layout.GetCustomTexts();
            var responsibleName = layout.GetContent("responsible_name");
            var phoneNumber = layout.GetContent("phone_number");

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1.5f, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(inner =>
                            {
                                if (showInstitutionName)
                                    inner.Item().Text(institution.DisplayName)
                                        .FontSize(18).Bold().FontColor(primaryColor);
                                inner.Item().Text("Boletim Escolar")
                                    .FontSize(10).FontColor(Colors.Grey.Medium);
                            });
                        });
                        col.Item().PaddingTop(4).LineHorizontal(1).LineColor(primaryColor);
                    });

                    page.Content().PaddingTop(12).Column(col =>
                    {
                        if (showStudentName)
                            col.Item().Text($"Aluno: {studentName}").FontSize(12).Bold();

                        col.Item().PaddingTop(12).Table(table =>
                        {
                            table.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn(3);
                                foreach (var _ in variables)
                                    cols.RelativeColumn();
                                cols.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                void HeaderCell(string text) =>
                                    header.Cell()
                                        .Background(primaryColor)
                                        .PaddingVertical(5).PaddingHorizontal(4)
                                        .Text(text).FontColor(Colors.White).Bold().FontSize(9);

                                HeaderCell("Disciplina");
                                foreach (var v in variables) HeaderCell(v.Key);
                                HeaderCell("Status");
                            });

                            var useAlt = false;
                            foreach (var grade in grades)
                            {
                                var bg = useAlt ? Colors.Grey.Lighten4 : Colors.White;
                                useAlt = !useAlt;

                                table.Cell().Background(bg)
                                    .PaddingVertical(5).PaddingHorizontal(4)
                                    .Text(grade.DisciplineName).FontSize(9);

                                foreach (var v in variables)
                                {
                                    var value = grade.Notes.TryGetValue(v.Key, out var n)
                                        ? n.ToString("F1") : "-";
                                    table.Cell().Background(bg)
                                        .PaddingVertical(5).PaddingHorizontal(4)
                                        .AlignCenter().Text(value).FontSize(9);
                                }

                                var status = grade.IsApproved == null ? "-"
                                    : grade.IsApproved.Value ? "Aprovado" : "Reprovado";
                                var statusColor = grade.IsApproved == null ? "#888888"
                                    : grade.IsApproved.Value ? "#16a34a" : "#dc2626";
                                table.Cell().Background(bg)
                                    .PaddingVertical(5).PaddingHorizontal(4)
                                    .AlignCenter().Text(status).FontSize(9).FontColor(statusColor).Bold();
                            }
                        });

                        if (responsibleName != null || phoneNumber != null)
                        {
                            col.Item().PaddingTop(16).Row(row =>
                            {
                                if (responsibleName != null)
                                    row.RelativeItem().Text($"Responsável: {responsibleName}").FontSize(9);
                                if (phoneNumber != null)
                                    row.RelativeItem().AlignRight().Text($"Tel: {phoneNumber}").FontSize(9);
                            });
                        }

                        foreach (var text in customTexts)
                            col.Item().PaddingTop(6).Text(text).FontSize(9).FontColor(Colors.Grey.Medium);
                    });

                    page.Footer().AlignRight()
                        .Text($"Gerado em {DateTime.UtcNow:dd/MM/yyyy HH:mm} UTC")
                        .FontSize(8).FontColor(Colors.Grey.Lighten1);
                });
            }).GeneratePdf();
        }
    }
}
