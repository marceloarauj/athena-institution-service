using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Dtos.Input;
using Institution.Application.Dtos.Output;
using Mediator.Mediator;

namespace Institution.Application.Commands
{
    public record UpsertCurriculumEntryCommand(UpsertCurriculumEntryDto Dto) : IRequestMessage<AthenaApiResponse<CurriculumEntryResponseDto>>;
    public record BulkUpsertCurriculumCommand(BulkUpsertCurriculumDto Dto) : IRequestMessage<AthenaApiResponse<List<CurriculumEntryResponseDto>>>;
    public record ListCurriculumCommand(Guid ProgramEditionId, int? GradeOrYear) : IRequestMessage<AthenaApiResponse<List<CurriculumEntryResponseDto>>>;
}
