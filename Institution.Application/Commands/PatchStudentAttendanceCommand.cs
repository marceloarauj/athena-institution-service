using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Dtos.Input;
using Institution.Application.Dtos.Output;
using Mediator.Mediator;

namespace Institution.Application.Commands
{
    public record PatchStudentAttendanceCommand(Guid DayLessonId, Guid StudentId, PatchAttendanceDto Dto)
        : IRequestMessage<AthenaApiResponse<AttendanceResponseDto>>;
}
