using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class GetTeacherAvailabilityHandler(IUnitOfWork unitOfWork)
        : IMessageHandler<GetTeacherAvailabilityCommand, AthenaApiResponse<List<TeacherAvailabilityResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<TeacherAvailabilityResponseDto>>> Handle(GetTeacherAvailabilityCommand request, CancellationToken cancellationToken)
        {
            var availabilities = await unitOfWork.TeacherRepository.GetAvailabilityAsync(request.TeacherId);
            return AthenaApiResponse<List<TeacherAvailabilityResponseDto>>.Ok(availabilities.Select(a => new TeacherAvailabilityResponseDto(a)).ToList());
        }
    }
}
