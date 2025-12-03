namespace HRManagementSystem.DTO
{
    public class PayrollManagementDto
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string PositionName { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Bonuses { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetSalary { get; set; }
    }
}
