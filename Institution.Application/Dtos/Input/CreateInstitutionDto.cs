using Institution.Domain.Entities;
using Institution.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Institution.Application.Dtos.Input
{
    public class CreateInstitutionDto
    {
        public required string Alias { get; set; }
        public required bool IsPrivate { get; set; }
        public required bool ChargePayment { get; set; }
        public required PaymentFormat PaymentFormat { get; set; }
        public bool SaveUpdateHistory { get; set; }
        public string? TaxDocument { get; set; }
        public required string DisplayName { get; set; }
        public IFormFile? Logo { get; set; }
        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }
        public string? DangerColor { get; set; }
                
        public InstitutionEntity ToEntity()
        {
            return new InstitutionEntity
            {
                Alias = Alias,
                IsPrivate = IsPrivate,
                ChargePayment = ChargePayment,
                PaymentFormat = PaymentFormat,
                SaveUpdateHistory = SaveUpdateHistory,
                TaxDocument = TaxDocument,
                DisplayName = DisplayName,
                IsActive = true,
                PrimaryColor = PrimaryColor,
                SecondaryColor = SecondaryColor,
                DangerColor = DangerColor
            };
        }
    }
}
