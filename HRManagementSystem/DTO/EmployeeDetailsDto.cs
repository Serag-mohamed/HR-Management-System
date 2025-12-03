namespace HRManagementSystem.DTO
{
    public class EmployeeDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public decimal BasicSalary { get; set; }
        public DateOnly HireDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ManagerName { get; set; }
        public string PositionName { get; set; }
        public string PositionDescription { get; set; }
        public string DepartmentName { get; set; }

    }
}
