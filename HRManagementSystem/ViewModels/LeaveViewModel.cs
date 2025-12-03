using HRManagementSystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.ViewModels
{
    public class LeaveViewModel
    {
        [Required(ErrorMessage = "*")]
        public LeaveType LeaveType { get; set; }

        [Required(ErrorMessage = "*")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "*")]
        public DateTime EndDate { get; set; }
    }
}
