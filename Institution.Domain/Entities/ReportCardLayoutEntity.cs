using System.ComponentModel.DataAnnotations.Schema;

namespace Institution.Domain.Entities
{
    [Table("report_card_layout", Schema = Schemes.INSTITUTION)]
    public class ReportCardLayoutEntity : BaseEntity
    {
        [Column("institution_id")]
        public Guid InstitutionId { get; set; }

        [Column("layout_json", TypeName = "text")]
        public string LayoutJson { get; set; } = "{}";

        [ForeignKey(nameof(InstitutionId))]
        public InstitutionEntity? Institution { get; set; }
    }
}
