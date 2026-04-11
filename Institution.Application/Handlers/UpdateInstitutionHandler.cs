using AthenaUnionLibrary.ApiResponse;
using Institution.Application.Commands;
using Institution.Application.Dtos.Output;
using Institution.Application.Interfaces;
using Institution.Application.Interfaces.Services;
using Institution.Domain.Entities;
using Mediator.Mediator;

namespace Institution.Application.Handlers
{
    public class UpdateInstitutionHandler
    (
        IUnitOfWork unitOfWork,
        IInstitutionService institutionService
    ) : IMessageHandler<UpdateInstitutionCommand, AthenaApiResponse<UpdateInstitutionResponseDto>>
    {
        public async Task<AthenaApiResponse<UpdateInstitutionResponseDto>> Handle(UpdateInstitutionCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var userId = Guid.NewGuid();

            await unitOfWork.BeginTransactionAsync();

            var institution = await unitOfWork.InstitutionRepository.FindByAliasAsync(request.Alias);

            var updateLogoUrl = "logo-url";

            if (institution == null)
                return AthenaApiResponse<UpdateInstitutionResponseDto>.NotFound("Institution not found.");

            var updates = new List<InstitutionUpdateHistoryEntity>();

            if (dto.Logo != null)
                await institutionService.UpdateInstitutionLogo(dto.Logo);

            UpdateStringIfChanged(dto.DisplayName, institution.DisplayName, v => institution.DisplayName = v, nameof(InstitutionEntity.DisplayName), institution, userId, updates);
            UpdateIfChanged(dto.IsPrivate, institution.IsPrivate, v => institution.IsPrivate = v, nameof(InstitutionEntity.IsPrivate), institution, userId, updates);
            UpdateIfChanged(dto.ChargePayment, institution.ChargePayment, v => institution.ChargePayment = v, nameof(InstitutionEntity.ChargePayment), institution, userId, updates);
            UpdateIfChanged(dto.PaymentFormat, institution.PaymentFormat, v => institution.PaymentFormat = v, nameof(InstitutionEntity.PaymentFormat), institution, userId, updates);
            UpdateIfChanged(dto.IsActive, institution.IsActive, v => institution.IsActive = v, nameof(InstitutionEntity.IsActive), institution, userId, updates);
            UpdateIfChanged(dto.SaveUpdateHistory, institution.SaveUpdateHistory, v => institution.SaveUpdateHistory = v, nameof(InstitutionEntity.SaveUpdateHistory), institution, userId, updates);
            UpdateStringIfChanged(dto.TaxDocument, institution.TaxDocument, v => institution.TaxDocument = v, nameof(InstitutionEntity.TaxDocument), institution, userId, updates);

            UpdateStringIfChanged(updateLogoUrl, institution.LogoUrl, v => institution.LogoUrl = v, nameof(InstitutionEntity.LogoUrl), institution, userId, updates);
            UpdateStringIfChanged(dto.PrimaryColor, institution.PrimaryColor, v => institution.PrimaryColor = v, nameof(InstitutionEntity.PrimaryColor), institution, userId, updates);
            UpdateStringIfChanged(dto.SecondaryColor, institution.SecondaryColor, v => institution.SecondaryColor = v, nameof(InstitutionEntity.SecondaryColor), institution, userId, updates);
            UpdateStringIfChanged(dto.DangerColor, institution.DangerColor, v => institution.DangerColor = v, nameof(InstitutionEntity.DangerColor), institution, userId, updates);

            if (institution.SaveUpdateHistory)
                await unitOfWork.InstitutionUpdateHistoryRepository.AddRangeAsync(updates);

            await unitOfWork.CommitAsync();

            return AthenaApiResponse<UpdateInstitutionResponseDto>.Ok(new UpdateInstitutionResponseDto(institution));
        }

        private static void UpdateIfChanged<T>
        (
            T? newValue, 
            T? oldValue, 
            Action<T> setter, 
            string propertyName, 
            InstitutionEntity entity, 
            Guid userId, 
            List<InstitutionUpdateHistoryEntity> updates
        ) where T : struct
        {
            if (newValue.HasValue && !EqualityComparer<T>.Default.Equals(oldValue.GetValueOrDefault(), newValue.Value))
            {
                updates.Add(LogUpdateHistory(entity, userId, propertyName, oldValue?.ToString(), newValue.ToString()));
                setter(newValue.Value);
            }
        }

        private static void UpdateStringIfChanged
        (
            string? newValue, 
            string? oldValue, 
            Action<string> setter, 
            string propertyName, 
            InstitutionEntity entity, 
            Guid userId, 
            List<InstitutionUpdateHistoryEntity> updates
        )
        {
            if (newValue is not null && oldValue != newValue)
            {
                updates.Add(LogUpdateHistory(entity, userId, propertyName, oldValue, newValue));
                setter(newValue);
            }
        }

        private static InstitutionUpdateHistoryEntity LogUpdateHistory
        (
            InstitutionEntity institution,
            Guid userId, 
            string field, 
            string? oldValue, 
            string? newValue
        )
        {
            return new InstitutionUpdateHistoryEntity()
            {
                Institution = institution,
                UpdatedByUser = userId,
                Field = field,
                OldValue = oldValue,
                NewValue = newValue
            };
        }
    }
}