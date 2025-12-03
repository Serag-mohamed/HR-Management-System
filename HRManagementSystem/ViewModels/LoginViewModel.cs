using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.ViewModels
{
    public class LoginViewModel
    {
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Required(ErrorMessage = "*")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DefaultValue(false)]
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }

}
