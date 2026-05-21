using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class UpdateEnrollmentStatusHandler(IUnitOfWork unitOfWork)
        : IMessageHandler<UpdateEnrollmentStatusCommand, AthenaApiResponse<EnrollmentResponseDto>>
    {
        public async Task<AthenaApiResponse<EnrollmentResponseDto>> Handle(UpdateEnrollmentStatusCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await unitOfWork.EnrollmentRepository.FindByIdAsync(request.EnrollmentId);
            if (enrollment == null)
                return AthenaApiResponse<EnrollmentResponseDto>.NotFound("Enrollment not found.");

            enrollment.Status = request.Dto.Status;
            await unitOfWork.EnrollmentRepository.UpdateStatusAsync(enrollment);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<EnrollmentResponseDto>.Ok(new EnrollmentResponseDto(enrollment));
        }
    }
}
