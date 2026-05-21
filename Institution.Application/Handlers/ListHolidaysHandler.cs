using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class ListHolidaysHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<ListHolidaysCommand, AthenaApiResponse<List<HolidayResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<HolidayResponseDto>>> Handle(ListHolidaysCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<List<HolidayResponseDto>>.NotFound("Institution not found.");

            var holidays = await unitOfWork.HolidayRepository.GetByYearAsync(institution.Id, request.Year);
            return AthenaApiResponse<List<HolidayResponseDto>>.Ok(holidays.Select(h => new HolidayResponseDto(h)).ToList());
        }
    }
}
