namespace HRManagementSystem.DTO
{
    public class PositionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int EmployeeCount { get; set; }
    }
}
