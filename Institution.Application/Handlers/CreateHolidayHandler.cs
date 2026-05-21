using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class CreateHolidayHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<CreateHolidayCommand, AthenaApiResponse<HolidayResponseDto>>
    {
        public async Task<AthenaApiResponse<HolidayResponseDto>> Handle(CreateHolidayCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<HolidayResponseDto>.NotFound("Institution not found.");

            var entity = new HolidayEntity
            {
                Name = request.Dto.Name,
                Date = request.Dto.Date,
                Type = request.Dto.Type,
                IsRecurring = request.Dto.IsRecurring,
                InstitutionId = institution.Id,
                Institution = institution
            };

            await unitOfWork.HolidayRepository.AddAsync(entity);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<HolidayResponseDto>.Created(new HolidayResponseDto(entity));
        }
    }
}
