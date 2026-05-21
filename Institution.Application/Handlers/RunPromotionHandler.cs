using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class RunPromotionHandler(IUnitOfWork unitOfWork)
        : IMessageHandler<RunPromotionCommand, AthenaApiResponse<PromotionResultDto>>
    {
        public async Task<AthenaApiResponse<PromotionResultDto>> Handle(RunPromotionCommand request, CancellationToken cancellationToken)
        {
            return await GetPromotionPreviewHandler.ComputePromotion(
                unitOfWork, request.Dto.SourceEditionId, request.Dto.TargetEditionId, commit: true);
        }
    }
}
