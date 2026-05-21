using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class DeleteHolidayHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<DeleteHolidayCommand, AthenaApiResponse<bool>>
    {
        public async Task<AthenaApiResponse<bool>> Handle(DeleteHolidayCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<bool>.NotFound("Institution not found.");

            var holiday = await unitOfWork.HolidayRepository.FindByIdAsync(request.Id);
            if (holiday == null || holiday.InstitutionId != institution.Id)
                return AthenaApiResponse<bool>.NotFound("Holiday not found.");

            await unitOfWork.HolidayRepository.DeleteAsync(holiday);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<bool>.Ok(true);
        }
    }
}
