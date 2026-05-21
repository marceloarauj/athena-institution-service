using Institution.Domain.Enums;

namespace Institution.Application.Dtos.Output
{
    public class ScheduleGenerationResultDto
    {
        public int TotalAssigned { get; set; }
        public int TotalUnresolved { get; set; }
        public GenerationStatus Status { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}
