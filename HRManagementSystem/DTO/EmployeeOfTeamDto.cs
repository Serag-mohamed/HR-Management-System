namespace HRManagementSystem.DTO
{
    public class EmployeeOfTeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PositionName { get; set; }
        public DateOnly HireDate { get; set; }
    }
}
