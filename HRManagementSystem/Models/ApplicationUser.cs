using Microsoft.AspNetCore.Identity;

namespace HRManagementSystem.Models
{
    public class ApplicationUser: IdentityUser
    {
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public string DisplayName { get; set; } = "";
        public ICollection<Payroll> CreatedPayrolls { get; set; } = new List<Payroll>();
        public ICollection<Leave> ApprovedLeaves { get; set; } = new List<Leave>();
    }
}
