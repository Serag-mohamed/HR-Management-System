using System.ComponentModel.DataAnnotations;

public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "*")]
        public string Token { get; set; } 

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } 

        [Required(ErrorMessage = "*")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }  

        [Required(ErrorMessage = "*")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

