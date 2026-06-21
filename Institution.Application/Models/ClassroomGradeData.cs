namespace Institution.Application.Models
{
    public class ClassroomGradeData
    {
        public string DisciplineName { get; set; } = "";
        public Dictionary<string, decimal> Notes { get; set; } = [];
        public bool? IsApproved { get; set; }
    }
}
