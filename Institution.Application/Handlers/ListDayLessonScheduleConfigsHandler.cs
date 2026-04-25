using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class ListDayLessonScheduleConfigsHandler
    (
        IUnitOfWork unitOfWork,
        IInstitutionContext institutionContext
    ) : IMessageHandler<ListDayLessonScheduleConfigsCommand, AthenaApiResponse<List<DayLessonScheduleConfigResponseDto>>>
    {
        public async Task<AthenaApiResponse<List<DayLessonScheduleConfigResponseDto>>> Handle(
            ListDayLessonScheduleConfigsCommand request,
            CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);

            if (institution == null)
                return AthenaApiResponse<List<DayLessonScheduleConfigResponseDto>>.NotFound("Institution not found.");

            var configs = await unitOfWork.DayLessonScheduleConfigRepository.GetByInstitutionAsync(institution.Id);

            return AthenaApiResponse<List<DayLessonScheduleConfigResponseDto>>.Ok(
                configs.Select(c => new DayLessonScheduleConfigResponseDto(c)).ToList());
        }
    }
}
