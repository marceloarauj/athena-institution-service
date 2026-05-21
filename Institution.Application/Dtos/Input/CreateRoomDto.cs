namespace Institution.Application.Dtos.Input
{
    public class CreateRoomDto
    {
        public required string Name { get; set; }
        public required int Capacity { get; set; }
        public bool HasLab { get; set; }
    }
}
