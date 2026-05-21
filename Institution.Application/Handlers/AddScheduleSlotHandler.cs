using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class AddScheduleSlotHandler(IUnitOfWork unitOfWork, IInstitutionContext institutionContext)
        : IMessageHandler<AddScheduleSlotCommand, AthenaApiResponse<ScheduleSlotResponseDto>>
    {
        public async Task<AthenaApiResponse<ScheduleSlotResponseDto>> Handle(AddScheduleSlotCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);
            if (institution == null)
                return AthenaApiResponse<ScheduleSlotResponseDto>.NotFound("Institution not found.");

            var shift = await unitOfWork.ShiftRepository.FindByIdAsync(request.Dto.ShiftId);
            if (shift == null || shift.InstitutionId != institution.Id)
                return AthenaApiResponse<ScheduleSlotResponseDto>.NotFound("Shift not found.");

            var slot = new ScheduleSlotEntity
            {
                Order = request.Dto.Order,
                StartTime = request.Dto.StartTime,
                EndTime = request.Dto.EndTime,
                ShiftId = shift.Id,
                Shift = shift
            };

            await unitOfWork.ShiftRepository.AddSlotAsync(slot);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<ScheduleSlotResponseDto>.Created(new ScheduleSlotResponseDto(slot));
        }
    }
}
