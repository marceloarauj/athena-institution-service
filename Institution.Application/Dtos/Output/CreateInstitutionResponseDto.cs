using Institution.Domain.Entities;
using Institution.Domain.Enums;

namespace Institution.Application.Dtos.Output
{
    public class CreateInstitutionResponseDto(InstitutionEntity institution)
    {
        public string Alias { get; set; } = institution.Alias;
        public string DisplayName { get; set; } = institution.DisplayName;
        public bool IsPrivate { get; set; } = institution.IsPrivate;
        public bool ChargePayment { get; set; } = institution.ChargePayment;
        public PaymentFormat PaymentFormat { get; set; } = institution.PaymentFormat;
        public bool SaveUpdateHistory { get; set; } = institution.SaveUpdateHistory;
        public string? TaxDocument { get; set; } = institution.TaxDocument;
    }
}
