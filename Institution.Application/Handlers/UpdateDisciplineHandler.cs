using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Auth;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Domain.Entities;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class UpdateDisciplineHandler
    (
        IUnitOfWork unitOfWork,
        IUser user
    ) : IMessageHandler<UpdateDisciplineCommand, AthenaApiResponse<UpdateDisciplineResponseDto>>
    {
        public async Task<AthenaApiResponse<UpdateDisciplineResponseDto>> Handle(UpdateDisciplineCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var userId = user.UserId;

            await unitOfWork.BeginTransactionAsync();

            var discipline = await unitOfWork.DisciplineRepository.FindByIdAsync(request.Id);

            if (discipline == null)
                return AthenaApiResponse<UpdateDisciplineResponseDto>.NotFound("Discipline not found.");

            var updates = new List<DisciplineUpdateHistoryEntity>();

            UpdateStringIfChanged(dto.Name, discipline.Name, v => discipline.Name = v, nameof(DisciplineEntity.Name), discipline, userId, updates);
            UpdateIfChanged(dto.StudyHours, discipline.StudyHours, v => discipline.StudyHours = v, nameof(DisciplineEntity.StudyHours), discipline, userId, updates);
            UpdateIfChanged(dto.Credits, discipline.Credits, v => discipline.Credits = v, nameof(DisciplineEntity.Credits), discipline, userId, updates);
            UpdateIfChanged(dto.Available, discipline.Available, v => discipline.Available = v, nameof(DisciplineEntity.Available), discipline, userId, updates);
            UpdateIfChanged(dto.ChargePayment, discipline.ChargePayment, v => discipline.ChargePayment = v, nameof(DisciplineEntity.ChargePayment), discipline, userId, updates);

            if (updates.Count > 0)
                await unitOfWork.DisciplineUpdateHistoryRepository.AddRangeAsync(updates);

            await unitOfWork.CommitAsync();

            return AthenaApiResponse<UpdateDisciplineResponseDto>.Ok(new UpdateDisciplineResponseDto(discipline));
        }

        private static void UpdateIfChanged<T>
        (
            T? newValue,
            T oldValue,
            Action<T> setter,
            string propertyName,
            DisciplineEntity entity,
            Guid userId,
            List<DisciplineUpdateHistoryEntity> updates
        ) where T : struct
        {
            if (newValue.HasValue && !EqualityComparer<T>.Default.Equals(oldValue, newValue.Value))
            {
                updates.Add(LogUpdateHistory(entity, userId, propertyName, oldValue.ToString(), newValue.Value.ToString()));
                setter(newValue.Value);
            }
        }

        private static void UpdateStringIfChanged
        (
            string? newValue,
            string oldValue,
            Action<string> setter,
            string propertyName,
            DisciplineEntity entity,
            Guid userId,
            List<DisciplineUpdateHistoryEntity> updates
        )
        {
            if (newValue is not null && oldValue != newValue)
            {
                updates.Add(LogUpdateHistory(entity, userId, propertyName, oldValue, newValue));
                setter(newValue);
            }
        }

        private static DisciplineUpdateHistoryEntity LogUpdateHistory
        (
            DisciplineEntity discipline,
            Guid userId,
            string field,
            string? oldValue,
            string? newValue
        )
        {
            return new DisciplineUpdateHistoryEntity
            {
                Discipline = discipline,
                UpdatedByUser = userId,
                Field = field,
                OldValue = oldValue ?? string.Empty,
                NewValue = newValue ?? string.Empty
            };
        }
    }
}
