using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Dtos.Output;
using Mediator.Mediator;

namespace Institution.Application.Commands
{
    public record ListDayLessonScheduleConfigsCommand
        : IRequestMessage<AthenaApiResponse<List<DayLessonScheduleConfigResponseDto>>>;
}
