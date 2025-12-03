namespace HRManagementSystem.Models
{
    public class Payroll
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Bonuses { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetSalary { get; set; }
        public DateTime PayrollMonth { get; set; }
        public DateTime GeneratedAt { get; set; }
        public string? CreatedById { get; set; } 
        public ApplicationUser? CreatedBy { get; set; }
    }
}
