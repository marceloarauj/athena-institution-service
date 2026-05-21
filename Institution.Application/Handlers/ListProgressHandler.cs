using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class ListProgressHandler(IUnitOfWork unitOfWork)
        : IMessageHandler<ListProgressCommand, AthenaApiResponse<List<ProgressRecordResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<ProgressRecordResponseDto>>> Handle(ListProgressCommand request, CancellationToken cancellationToken)
        {
            var records = await unitOfWork.ProgressRecordRepository.GetByEditionAsync(request.ProgramEditionId);
            return AthenaApiResponse<List<ProgressRecordResponseDto>>.Ok(
                records.Select(r => new ProgressRecordResponseDto(r)).ToList());
        }
    }
}
