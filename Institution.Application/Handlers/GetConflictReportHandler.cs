using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class GetConflictReportHandler(IUnitOfWork unitOfWork)
        : IMessageHandler<GetConflictReportCommand, AthenaApiResponse<ConflictReportResponseDto>>
    {
        public async Task<AthenaApiResponse<ConflictReportResponseDto>> Handle(GetConflictReportCommand request, CancellationToken cancellationToken)
        {
            var report = await unitOfWork.ConflictReportRepository.GetLatestByEditionAsync(request.ProgramEditionId);
            if (report == null)
                return AthenaApiResponse<ConflictReportResponseDto>.NotFound("No conflict report found. Run conflict detection first.");

            return AthenaApiResponse<ConflictReportResponseDto>.Ok(new ConflictReportResponseDto(report));
        }
    }
}
