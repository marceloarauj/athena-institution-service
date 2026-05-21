using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class ListRecessesHandler(IUnitOfWork unitOfWork)
        : IMessageHandler<ListRecessesCommand, AthenaApiResponse<List<RecessResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<RecessResponseDto>>> Handle(ListRecessesCommand request, CancellationToken cancellationToken)
        {
            var recesses = await unitOfWork.HolidayRepository.GetRecessesByEditionAsync(request.ProgramEditionId);
            return AthenaApiResponse<List<RecessResponseDto>>.Ok(recesses.Select(r => new RecessResponseDto(r)).ToList());
        }
    }
}
