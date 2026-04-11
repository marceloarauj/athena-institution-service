namespace Institution.Application.Dtos.Input
{
    public class UpdateDisciplineDto
    {
        public string? Name { get; set; }
        public int? StudyHours { get; set; }
        public int? Credits { get; set; }
        public bool? Available { get; set; }
        public bool? ChargePayment { get; set; }
    }
}
