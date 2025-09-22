using System.ComponentModel.DataAnnotations;

namespace MedLinkDashboard.ViewModels
{
    public class ResetPasswordVM
    {
        public string Email { get; set; }
        public string Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.{6,}$)(?=.*[A-Z])(?=.*[^A-Za-z0-9]).+$",
           ErrorMessage = "Password must be at least 6 chars, contain an uppercase letter and a special character.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

    }
}
