using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Dtos.Input;
using Institution.Application.Dtos.Output;
using Mediator.Mediator;

namespace Institution.Application.Commands
{
    public record EnrollStudentCommand(EnrollStudentDto Dto) : IRequestMessage<AthenaApiResponse<EnrollmentResponseDto>>;
    public record BulkEnrollStudentsCommand(BulkEnrollStudentsDto Dto) : IRequestMessage<AthenaApiResponse<List<EnrollmentResponseDto>>>;
    public record ListEnrollmentsCommand(Guid ProgramEditionId) : IRequestMessage<AthenaApiResponse<List<EnrollmentResponseDto>>>;
    public record UpdateEnrollmentStatusCommand(Guid EnrollmentId, UpdateEnrollmentStatusDto Dto) : IRequestMessage<AthenaApiResponse<EnrollmentResponseDto>>;
}
