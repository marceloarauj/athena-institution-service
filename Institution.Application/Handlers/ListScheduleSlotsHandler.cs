using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class ListScheduleSlotsHandler(IUnitOfWork unitOfWork)
        : IMessageHandler<ListScheduleSlotsCommand, AthenaApiResponse<List<ScheduleSlotResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<ScheduleSlotResponseDto>>> Handle(ListScheduleSlotsCommand request, CancellationToken cancellationToken)
        {
            var slots = await unitOfWork.ShiftRepository.GetSlotsByShiftAsync(request.ShiftId);
            return AthenaApiResponse<List<ScheduleSlotResponseDto>>.Ok(slots.Select(s => new ScheduleSlotResponseDto(s)).ToList());
        }
    }
}
