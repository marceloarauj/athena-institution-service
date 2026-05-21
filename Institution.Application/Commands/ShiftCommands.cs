using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Dtos.Input;
using Institution.Application.Dtos.Output;
using Mediator.Mediator;

namespace Institution.Application.Commands
{
    public record CreateShiftCommand(CreateShiftDto Dto) : IRequestMessage<AthenaApiResponse<ShiftResponseDto>>;
    public record ListShiftsCommand() : IRequestMessage<AthenaApiResponse<List<ShiftResponseDto>>>;
    public record AddScheduleSlotCommand(AddScheduleSlotDto Dto) : IRequestMessage<AthenaApiResponse<ScheduleSlotResponseDto>>;
    public record ListScheduleSlotsCommand(Guid ShiftId) : IRequestMessage<AthenaApiResponse<List<ScheduleSlotResponseDto>>>;
}
