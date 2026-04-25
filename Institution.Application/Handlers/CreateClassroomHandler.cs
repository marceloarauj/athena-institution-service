using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Auth;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Institution.Domain.Enums;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class CreateClassroomHandler
    (
        IUnitOfWork unitOfWork,
        IInstitutionContext institutionContext,
        IUser user
    ) : IMessageHandler<CreateClassroomCommand, AthenaApiResponse<ClassroomResponseDto>>
    {
        public async Task<AthenaApiResponse<ClassroomResponseDto>> Handle(CreateClassroomCommand request, CancellationToken cancellationToken)
        {
            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(institutionContext.Alias!);

            if (institution == null)
                return AthenaApiResponse<ClassroomResponseDto>.NotFound("Institution not found.");

            var discipline = await unitOfWork.DisciplineRepository.FindByIdAsync(request.Dto.DisciplineId);

            if (discipline == null || discipline.InstitutionId != institution.Id)
                return AthenaApiResponse<ClassroomResponseDto>.NotFound("Discipline not found.");

            var classroom = request.Dto.ToEntity(discipline, user.UserId);

            await unitOfWork.ClassroomRepository.AddAsync(classroom);

            if (request.Dto.GenerateDayLessons)
            {
                var config = await unitOfWork.DayLessonScheduleConfigRepository
                    .FindActiveByDisciplineAsync(discipline.Id)
                    ?? await unitOfWork.DayLessonScheduleConfigRepository
                    .FindActiveByInstitutionAsync(institution.Id);

                if (config == null)
                    return AthenaApiResponse<ClassroomResponseDto>.BadRequest(
                        "No active schedule configuration found for the discipline or institution.");

                var dayLessons = GenerateDayLessons(classroom, config);

                if (dayLessons.Count > 0)
                    await unitOfWork.DayLessonRepository.AddRangeAsync(dayLessons);
            }

            await unitOfWork.CommitAsync();

            return AthenaApiResponse<ClassroomResponseDto>.Created(new ClassroomResponseDto(classroom));
        }

        private static List<DayLessonEntity> GenerateDayLessons(
            ClassroomEntity classroom,
            DayLessonScheduleConfigEntity config)
        {
            var lessons = new List<DayLessonEntity>();
            var current = config.StartDate.Date;
            var endDate = config.EndDate?.Date ?? DateTime.MaxValue.Date;
            var maxLessons = config.LessonCount;

            while (current <= endDate)
            {
                var dayFlag = (WeekDays)(1 << (int)current.DayOfWeek);

                if ((config.DaysOfWeek & dayFlag) != 0)
                {
                    var lessonStart = current.Add(config.LessonStartTime.ToTimeSpan());
                    var lessonEnd = current.Add(config.LessonEndTime.ToTimeSpan());

                    lessons.Add(new DayLessonEntity
                    {
                        ClassroomId = classroom.Id,
                        Classroom = classroom,
                        Location = classroom.Location,
                        TeacherId = classroom.TeacherId,
                        StartDate = lessonStart,
                        EndDate = lessonEnd
                    });

                    if (maxLessons.HasValue && lessons.Count >= maxLessons.Value)
                        break;
                }

                current = current.AddDays(1);
            }

            return lessons;
        }
    }
}
