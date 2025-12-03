using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "*")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "*")]
        [MinLength(6)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "*")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}
