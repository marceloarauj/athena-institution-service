using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Dtos.Input;
using Institution.Application.Dtos.Output;
using Mediator.Mediator;

namespace Institution.Application.Commands
{
    public record RunConflictDetectionCommand(Guid ProgramEditionId) : IRequestMessage<AthenaApiResponse<ConflictReportResponseDto>>;
    public record GetConflictReportCommand(Guid ProgramEditionId) : IRequestMessage<AthenaApiResponse<ConflictReportResponseDto>>;
    public record OverrideScheduleSlotCommand(Guid ScheduleId, OverrideScheduleSlotDto Dto) : IRequestMessage<AthenaApiResponse<ClassScheduleResponseDto>>;
}
