using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class InactivateStudentHandler(IUnitOfWork unitOfWork)
        : IMessageHandler<InactivateStudentCommand, AthenaApiResponse<StudentRegistrationResponseDto>>
    {
        public async Task<AthenaApiResponse<StudentRegistrationResponseDto>> Handle(InactivateStudentCommand request, CancellationToken cancellationToken)
        {
            var registration = await unitOfWork.StudentClassroomRegistrationRepository
                .FindByStudentAndClassroomAsync(request.Dto.StudentId, request.Dto.ClassroomId);

            if (registration == null)
                return AthenaApiResponse<StudentRegistrationResponseDto>.NotFound("Student registration not found.");

            if (!registration.IsActive)
                return AthenaApiResponse<StudentRegistrationResponseDto>.BadRequest("Student is already inactive in this classroom.");

            registration.IsActive = false;

            await unitOfWork.CommitAsync();

            return AthenaApiResponse<StudentRegistrationResponseDto>.Ok(new StudentRegistrationResponseDto(registration));
        }
    }
}
