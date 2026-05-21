using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class ListPeriodsHandler(IUnitOfWork unitOfWork)
        : IMessageHandler<ListPeriodsCommand, AthenaApiResponse<List<ProgramPeriodResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<ProgramPeriodResponseDto>>> Handle(ListPeriodsCommand request, CancellationToken cancellationToken)
        {
            var periods = await unitOfWork.ProgramPeriodRepository.GetByEditionAsync(request.ProgramEditionId);
            return AthenaApiResponse<List<ProgramPeriodResponseDto>>.Ok(periods.Select(p => new ProgramPeriodResponseDto(p)).ToList());
        }
    }
}
