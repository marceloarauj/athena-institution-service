using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class GetClassGroupScheduleHandler(IUnitOfWork unitOfWork)
        : IMessageHandler<GetClassGroupScheduleCommand, AthenaApiResponse<List<ClassScheduleResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<ClassScheduleResponseDto>>> Handle(GetClassGroupScheduleCommand request, CancellationToken cancellationToken)
        {
            var schedules = await unitOfWork.ClassScheduleRepository.GetByGroupAsync(request.ClassGroupId);
            return AthenaApiResponse<List<ClassScheduleResponseDto>>.Ok(
                schedules.Select(s => new ClassScheduleResponseDto(s)).ToList());
        }
    }
}
