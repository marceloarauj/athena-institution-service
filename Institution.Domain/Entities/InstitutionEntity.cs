using Institution.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("institution", Schema = Schemes.INSTITUTION)]
    [Index(nameof(Alias), IsUnique = true)]
    public class InstitutionEntity : BaseEntity
    {
        [Column("alias"), MaxLength(50)]
        public required string Alias { get; set; }

        [Column("tax_document"), MaxLength(20)]
        public string? TaxDocument { get; set; }

        [Column("display_name"), MaxLength(300)]
        public required string DisplayName { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("is_private")]
        public bool IsPrivate { get; set; } = false;

        [Column("charge_payment")]
        public required bool ChargePayment { get; set; }

        [Column("payment_format")]
        public required PaymentFormat PaymentFormat { get; set; }

        [Column("save_update_history")]
        public bool SaveUpdateHistory { get; set; } = true;

        [Column("logo_url"), MaxLength(500)]
        public string LogoUrl { get; set; } = string.Empty;

        [Column("primary_color"), MaxLength(10)]
        public string? PrimaryColor { get; set; }

        [Column("secondary_color"), MaxLength(10)]
        public string? SecondaryColor { get; set; }

        [Column("danger_color"), MaxLength(10)]
        public string? DangerColor { get; set; }

        [Column("created_by")]
        public Guid CreatedBy { get; set; }
    }
}
