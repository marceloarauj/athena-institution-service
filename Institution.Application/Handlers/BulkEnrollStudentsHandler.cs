using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class BulkEnrollStudentsHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<BulkEnrollStudentsCommand, AthenaApiResponse<List<EnrollmentResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<EnrollmentResponseDto>>> Handle(BulkEnrollStudentsCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<List<EnrollmentResponseDto>>.NotFound("Institution not found.");

            var edition = await unitOfWork.ProgramEditionRepository.FindByIdAsync(request.Dto.ProgramEditionId);
            if (edition == null || edition.AcademicProgram.InstitutionId != institution.Id)
                return AthenaApiResponse<List<EnrollmentResponseDto>>.NotFound("Program edition not found.");

            var enrollments = new List<EnrollmentEntity>();
            foreach (var student in request.Dto.Students)
            {
                var exists = await unitOfWork.EnrollmentRepository.ExistsAsync(student.StudentId, edition.Id);
                if (exists) continue;

                enrollments.Add(new EnrollmentEntity
                {
                    StudentId = student.StudentId,
                    GradeOrYear = student.GradeOrYear,
                    EnrolledAt = DateTime.UtcNow,
                    ProgramEditionId = edition.Id,
                    ProgramEdition = edition
                });
            }

            if (enrollments.Count > 0)
                await unitOfWork.EnrollmentRepository.AddRangeAsync(enrollments);

            await unitOfWork.CommitAsync();

            return AthenaApiResponse<List<EnrollmentResponseDto>>.Created(enrollments.Select(e => new EnrollmentResponseDto(e)).ToList());
        }
    }
}
