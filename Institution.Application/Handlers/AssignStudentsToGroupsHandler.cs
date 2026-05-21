using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Institution.Domain.Enums;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class AssignStudentsToGroupsHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<AssignStudentsToGroupsCommand, AthenaApiResponse<List<ClassGroupStudentResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<ClassGroupStudentResponseDto>>> Handle(AssignStudentsToGroupsCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<List<ClassGroupStudentResponseDto>>.NotFound("Institution not found.");

            var edition = await unitOfWork.ProgramEditionRepository.FindByIdAsync(request.Dto.ProgramEditionId);
            if (edition == null || edition.AcademicProgram.InstitutionId != institution.Id)
                return AthenaApiResponse<List<ClassGroupStudentResponseDto>>.NotFound("Program edition not found.");

            if (edition.Status == EditionStatus.Published || edition.Status == EditionStatus.Closed)
                return AthenaApiResponse<List<ClassGroupStudentResponseDto>>.UnprocessableEntity("Cannot modify a published or closed edition.");

            var enrollments = await unitOfWork.EnrollmentRepository.GetByEditionAsync(edition.Id);
            var groups = await unitOfWork.ClassGroupRepository.GetByEditionAsync(edition.Id);

            var assignments = new List<ClassGroupStudentEntity>();
            var byGrade = enrollments.GroupBy(e => e.GradeOrYear);

            foreach (var gradeGroup in byGrade)
            {
                var gradeGroups = groups.Where(g => g.GradeOrYear == gradeGroup.Key).ToList();
                if (gradeGroups.Count == 0) continue;

                var students = gradeGroup.ToList();
                int groupIndex = 0;
                var groupCounts = gradeGroups.ToDictionary(g => g.Id, _ => 0);

                foreach (var enrollment in students)
                {
                    var target = gradeGroups[groupIndex];
                    if (groupCounts[target.Id] >= target.MaxStudents)
                    {
                        groupIndex = (groupIndex + 1) % gradeGroups.Count;
                        target = gradeGroups[groupIndex];
                    }

                    assignments.Add(new ClassGroupStudentEntity
                    {
                        ClassGroupId = target.Id,
                        ClassGroup = target,
                        EnrollmentId = enrollment.Id,
                        Enrollment = enrollment,
                        AssignedAt = DateTime.UtcNow
                    });

                    groupCounts[target.Id]++;
                    groupIndex = (groupIndex + 1) % gradeGroups.Count;
                }
            }

            await unitOfWork.ClassGroupRepository.AddStudentsAsync(assignments);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<List<ClassGroupStudentResponseDto>>.Created(
                assignments.Select(a => new ClassGroupStudentResponseDto(a)).ToList());
        }
    }
}
