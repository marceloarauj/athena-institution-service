using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class EnrollStudentHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<EnrollStudentCommand, AthenaApiResponse<EnrollmentResponseDto>>
    {
        public async Task<AthenaApiResponse<EnrollmentResponseDto>> Handle(EnrollStudentCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<EnrollmentResponseDto>.NotFound("Institution not found.");

            var edition = await unitOfWork.ProgramEditionRepository.FindByIdAsync(request.Dto.ProgramEditionId);
            if (edition == null || edition.AcademicProgram.InstitutionId != institution.Id)
                return AthenaApiResponse<EnrollmentResponseDto>.NotFound("Program edition not found.");

            var exists = await unitOfWork.EnrollmentRepository.ExistsAsync(request.Dto.StudentId, edition.Id);
            if (exists)
                return AthenaApiResponse<EnrollmentResponseDto>.BadRequest("Student is already enrolled in this edition.");

            var entity = new EnrollmentEntity
            {
                StudentId = request.Dto.StudentId,
                GradeOrYear = request.Dto.GradeOrYear,
                EnrolledAt = DateTime.UtcNow,
                PurchaseReference = request.Dto.PurchaseReference,
                ExpiresAt = request.Dto.ExpiresAt,
                ProgramEditionId = edition.Id,
                ProgramEdition = edition
            };

            await unitOfWork.EnrollmentRepository.AddAsync(entity);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<EnrollmentResponseDto>.Created(new EnrollmentResponseDto(entity));
        }
    }
}
