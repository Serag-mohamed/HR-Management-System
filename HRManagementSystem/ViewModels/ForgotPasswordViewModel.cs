using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "*")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
    }
}

