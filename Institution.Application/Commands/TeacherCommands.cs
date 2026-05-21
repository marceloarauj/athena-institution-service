using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Dtos.Input;
using Institution.Application.Dtos.Output;
using Mediator.Mediator;

namespace Institution.Application.Commands
{
    public record CreateTeacherCommand(CreateTeacherDto Dto) : IRequestMessage<AthenaApiResponse<TeacherResponseDto>>;
    public record ListTeachersCommand() : IRequestMessage<AthenaApiResponse<List<TeacherResponseDto>>>;
    public record SetTeacherSubjectsCommand(SetTeacherSubjectsDto Dto) : IRequestMessage<AthenaApiResponse<TeacherResponseDto>>;
    public record SetTeacherAvailabilityCommand(SetTeacherAvailabilityDto Dto) : IRequestMessage<AthenaApiResponse<List<TeacherAvailabilityResponseDto>>>;
    public record GetTeacherAvailabilityCommand(Guid TeacherId) : IRequestMessage<AthenaApiResponse<List<TeacherAvailabilityResponseDto>>>;
}
