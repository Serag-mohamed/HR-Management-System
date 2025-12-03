using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.ViewModels
{
    public class PayrollViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "*")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "*")]
        public decimal BasicSalary { get; set; }

        [Required(ErrorMessage = "*")]
        public decimal Bonuses { get; set; }

        [Required(ErrorMessage = "*")]
        public decimal Deductions { get; set; }

        [Required(ErrorMessage = "*")]
        public DateTime PayrollMonth { get; set; }
    }
}
