namespace HRManagementSystem.DTO
{
    public class DashboardDto
    {
        public int TotalEmployees { get; set; }
        public int DepartmentsCount { get; set; }
        public int PositionsCount { get; set; }
        public int PendingLeaves { get; set; }
        public decimal AverageSalary { get; set; }

        public List<LatestEmployeeDto> LatestEmployees { get; set; } = new();
        public List<DepartmentEmployeeCountDto> DepartmentChart { get; set; } = new();
    }
    public class LatestEmployeeDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PositionName { get; set; }
        public DateOnly HireDate { get; set; }
    }
    public class DepartmentEmployeeCountDto
    {
        public string DepartmentName { get; set; }
        public int EmployeeCount { get; set; }
    }

}
