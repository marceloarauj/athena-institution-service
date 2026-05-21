using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class ListShiftsHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<ListShiftsCommand, AthenaApiResponse<List<ShiftResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<ShiftResponseDto>>> Handle(ListShiftsCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<List<ShiftResponseDto>>.NotFound("Institution not found.");

            var shifts = await unitOfWork.ShiftRepository.GetByInstitutionAsync(institution.Id);
            return AthenaApiResponse<List<ShiftResponseDto>>.Ok(shifts.Select(s => new ShiftResponseDto(s)).ToList());
        }
    }
}
