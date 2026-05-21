using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class ListEnrollmentsHandler(IUnitOfWork unitOfWork)
        : IMessageHandler<ListEnrollmentsCommand, AthenaApiResponse<List<EnrollmentResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<EnrollmentResponseDto>>> Handle(ListEnrollmentsCommand request, CancellationToken cancellationToken)
        {
            var enrollments = await unitOfWork.EnrollmentRepository.GetByEditionAsync(request.ProgramEditionId);
            return AthenaApiResponse<List<EnrollmentResponseDto>>.Ok(enrollments.Select(e => new EnrollmentResponseDto(e)).ToList());
        }
    }
}
