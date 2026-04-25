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
    public class UpdateDayLessonScheduleConfigHandler
    (
        IUnitOfWork unitOfWork,
        IUser user
    ) : IMessageHandler<UpdateDayLessonScheduleConfigCommand, AthenaApiResponse<DayLessonScheduleConfigResponseDto>>
    {
        public async Task<AthenaApiResponse<DayLessonScheduleConfigResponseDto>> Handle(
            UpdateDayLessonScheduleConfigCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var userId = user.UserId;

            await unitOfWork.BeginTransactionAsync();

            var config = await unitOfWork.DayLessonScheduleConfigRepository.FindByIdAsync(request.Id);

            if (config == null)
                return AthenaApiResponse<DayLessonScheduleConfigResponseDto>.NotFound(
                    "Schedule config not found.");

            var histories = new List<DayLessonScheduleConfigHistoryEntity>();

            UpdateIfChanged(dto.StartDate, config.StartDate, v => config.StartDate = v,
                nameof(DayLessonScheduleConfigEntity.StartDate), config, userId, histories);

            UpdateNullableIfChanged(dto.EndDate, config.EndDate, v => config.EndDate = v,
                nameof(DayLessonScheduleConfigEntity.EndDate), config, userId, histories);

            UpdateIfChanged(dto.LessonStartTime, config.LessonStartTime, v => config.LessonStartTime = v,
                nameof(DayLessonScheduleConfigEntity.LessonStartTime), config, userId, histories);

            UpdateIfChanged(dto.LessonEndTime, config.LessonEndTime, v => config.LessonEndTime = v,
                nameof(DayLessonScheduleConfigEntity.LessonEndTime), config, userId, histories);

            UpdateIfChanged(dto.DaysOfWeek, config.DaysOfWeek, v => config.DaysOfWeek = v,
                nameof(DayLessonScheduleConfigEntity.DaysOfWeek), config, userId, histories);

            UpdateNullableIfChanged(dto.LessonCount, config.LessonCount, v => config.LessonCount = v,
                nameof(DayLessonScheduleConfigEntity.LessonCount), config, userId, histories);

            UpdateIfChanged(dto.IsActive, config.IsActive, v => config.IsActive = v,
                nameof(DayLessonScheduleConfigEntity.IsActive), config, userId, histories);

            if (histories.Count > 0)
                await unitOfWork.DayLessonScheduleConfigHistoryRepository.AddRangeAsync(histories);

            await unitOfWork.CommitAsync();

            return AthenaApiResponse<DayLessonScheduleConfigResponseDto>.Ok(
                new DayLessonScheduleConfigResponseDto(config));
        }

        private static void UpdateIfChanged<T>(
            T? newValue,
            T oldValue,
            Action<T> setter,
            string propertyName,
            DayLessonScheduleConfigEntity entity,
            Guid userId,
            List<DayLessonScheduleConfigHistoryEntity> histories) where T : struct
        {
            if (newValue.HasValue && !EqualityComparer<T>.Default.Equals(oldValue, newValue.Value))
            {
                histories.Add(LogHistory(entity, userId, propertyName, oldValue.ToString(), newValue.Value.ToString()));
                setter(newValue.Value);
            }
        }

        private static void UpdateNullableIfChanged<T>(
            T? newValue,
            T? oldValue,
            Action<T?> setter,
            string propertyName,
            DayLessonScheduleConfigEntity entity,
            Guid userId,
            List<DayLessonScheduleConfigHistoryEntity> histories) where T : struct
        {
            if (newValue.HasValue && !EqualityComparer<T?>.Default.Equals(oldValue, newValue))
            {
                histories.Add(LogHistory(entity, userId, propertyName, oldValue?.ToString(), newValue.Value.ToString()));
                setter(newValue.Value);
            }
        }

        private static DayLessonScheduleConfigHistoryEntity LogHistory(
            DayLessonScheduleConfigEntity config,
            Guid userId,
            string field,
            string? oldValue,
            string? newValue)
        {
            return new DayLessonScheduleConfigHistoryEntity
            {
                Config = config,
                ConfigId = config.Id,
                UpdatedByUser = userId,
                Field = field,
                OldValue = oldValue,
                NewValue = newValue
            };
        }
    }
}
