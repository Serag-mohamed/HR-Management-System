namespace HRManagementSystem.DTO
{
    public class PayrollDetailsDto
    {
        public decimal BasicSalary { get; set; }
        public decimal Bonuses { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetSalary { get; set; }
        public DateTime PayrollMonth { get; set; }
    }
}
