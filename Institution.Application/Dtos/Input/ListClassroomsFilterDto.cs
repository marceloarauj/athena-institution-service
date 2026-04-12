namespace Institution.Application.Dtos.Input
{
    public class ListClassroomsFilterDto
    {
        public Guid? DisciplineId { get; set; }
        public Guid? TeacherId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
