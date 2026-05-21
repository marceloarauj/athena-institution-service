using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Dtos.Input;
using Institution.Application.Dtos.Output;
using Mediator.Mediator;

namespace Institution.Application.Commands
{
    public record RecordProgressCommand(RecordProgressDto Dto) : IRequestMessage<AthenaApiResponse<ProgressRecordResponseDto>>;
    public record BulkRecordProgressCommand(BulkRecordProgressDto Dto) : IRequestMessage<AthenaApiResponse<List<ProgressRecordResponseDto>>>;
    public record ListProgressCommand(Guid ProgramEditionId) : IRequestMessage<AthenaApiResponse<List<ProgressRecordResponseDto>>>;
    public record GetPromotionPreviewCommand(RunPromotionDto Dto) : IRequestMessage<AthenaApiResponse<PromotionResultDto>>;
    public record RunPromotionCommand(RunPromotionDto Dto) : IRequestMessage<AthenaApiResponse<PromotionResultDto>>;
}
