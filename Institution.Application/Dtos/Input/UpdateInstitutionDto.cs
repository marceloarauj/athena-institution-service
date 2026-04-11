using Institution.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Institution.Application.Dtos.Input
{
    public class UpdateInstitutionDto
    {
        public bool? IsPrivate { get; set; }
        public bool? ChargePayment { get; set; }
        public PaymentFormat? PaymentFormat { get; set; }
        public bool? SaveUpdateHistory { get; set; }
        public string? TaxDocument { get; set; }
        public string? DisplayName { get; set; }
        public bool? IsActive { get; set; }
        public IFormFile? Logo { get; set; }
        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }
        public string? DangerColor { get; set; }
    }
}