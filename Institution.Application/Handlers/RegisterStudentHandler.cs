using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class RegisterStudentHandler
    (
        IUnitOfWork unitOfWork
    ) : IMessageHandler<RegisterStudentCommand, AthenaApiResponse<StudentRegistrationResponseDto>>
    {
        public async Task<AthenaApiResponse<StudentRegistrationResponseDto>> Handle(RegisterStudentCommand request, CancellationToken cancellationToken)
        {
            var classroom = await unitOfWork.ClassroomRepository.FindByIdAsync(request.Dto.ClassroomId);

            if (classroom == null)
                return AthenaApiResponse<StudentRegistrationResponseDto>.NotFound("Classroom not found.");

            var alreadyRegistered = await unitOfWork.StudentClassroomRegistrationRepository.ExistsAsync(request.Dto.StudentId, classroom.Id);

            if (alreadyRegistered)
                return AthenaApiResponse<StudentRegistrationResponseDto>.BadRequest("Student is already registered in this classroom.");

            var registration = new StudentClassroomRegistrationEntity
            {
                StudentId = request.Dto.StudentId,
                StudentName = "TODO: fetch from API",
                IsActive = true,
                ClassroomId = classroom.Id,
                Classroom = classroom
            };

            await unitOfWork.StudentClassroomRegistrationRepository.AddAsync(registration);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<StudentRegistrationResponseDto>.Created(new StudentRegistrationResponseDto(registration));
        }
    }
}
