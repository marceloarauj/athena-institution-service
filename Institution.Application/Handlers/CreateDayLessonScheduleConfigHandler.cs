using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class CreateDayLessonScheduleConfigHandler
    (
        IUnitOfWork unitOfWork,
        IInstitutionContext institutionContext
    ) : IMessageHandler<CreateDayLessonScheduleConfigCommand, AthenaApiResponse<DayLessonScheduleConfigResponseDto>>
    {
        public async Task<AthenaApiResponse<DayLessonScheduleConfigResponseDto>> Handle(
            CreateDayLessonScheduleConfigCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            if (dto.EndDate == null && dto.LessonCount == null)
                return AthenaApiResponse<DayLessonScheduleConfigResponseDto>.BadRequest(
                    "Either EndDate or LessonCount must be provided.");

            if (dto.DaysOfWeek == Domain.Enums.WeekDays.None)
                return AthenaApiResponse<DayLessonScheduleConfigResponseDto>.BadRequest(
                    "At least one day of the week must be selected.");

            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);

            if (institution == null)
                return AthenaApiResponse<DayLessonScheduleConfigResponseDto>.NotFound("Institution not found.");

            Domain.Entities.DisciplineEntity? discipline = null;

            if (dto.DisciplineId.HasValue)
            {
                discipline = await unitOfWork.DisciplineRepository.FindByIdAsync(dto.DisciplineId.Value);

                if (discipline == null || discipline.InstitutionId != institution.Id)
                    return AthenaApiResponse<DayLessonScheduleConfigResponseDto>.NotFound("Discipline not found.");
            }

            var config = dto.ToEntity(institution, discipline);

            await unitOfWork.DayLessonScheduleConfigRepository.AddAsync(config);
            await unitOfWork.CommitAsync();

            return AthenaApiResponse<DayLessonScheduleConfigResponseDto>.Created(
                new DayLessonScheduleConfigResponseDto(config));
        }
    }
}
