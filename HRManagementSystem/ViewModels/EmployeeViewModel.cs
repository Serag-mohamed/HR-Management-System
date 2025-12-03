using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "*")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Required(ErrorMessage = "*")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*")]
        [Length(11,11,ErrorMessage = "Phone consist of 11 Number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "*")]
        public string Address { get; set; }

        [Required(ErrorMessage = "*")]
        public decimal BasicSalary { get; set; }

        [Required(ErrorMessage = "*")]
        public DateOnly HireDate { get; set; }

        [Required(ErrorMessage = "*")]
        public int PositionId { get; set; }

        public int? ManagerId { get; set; }

    }
}