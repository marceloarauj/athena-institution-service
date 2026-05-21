using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Dtos.Input;
using Institution.Application.Dtos.Output;
using Mediator.Mediator;

namespace Institution.Application.Commands
{
    public record GenerateScheduleCommand(GenerateScheduleDto Dto) : IRequestMessage<AthenaApiResponse<ScheduleGenerationResultDto>>;
    public record GetClassGroupScheduleCommand(Guid ClassGroupId) : IRequestMessage<AthenaApiResponse<List<ClassScheduleResponseDto>>>;
    public record GetTeacherScheduleCommand(Guid TeacherId, Guid ProgramEditionId) : IRequestMessage<AthenaApiResponse<List<ClassScheduleResponseDto>>>;
}
