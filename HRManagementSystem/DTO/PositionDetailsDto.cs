namespace HRManagementSystem.DTO
{
    public class PositionDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<EmployeeDto> Employees { get; set; }
    }
}
