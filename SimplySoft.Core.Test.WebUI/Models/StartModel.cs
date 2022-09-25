using System.ComponentModel.DataAnnotations;

namespace SimplySoft.Core.Test.WebUI.Models
{
    public class StartModel
    {
        public int NextStage { get; set; } = 1;
        public ResponseMessage ResponseMessage { get; set; }
        public UsernameModel UsernamePrompt { get; set; }
        public ActivationModel ActivationPrompt { get; set; }
        public ResetPasswordModel ResetPasswordPrompt { get; set; }
    }

    public class UsernameModel
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }
    }

    public class ActivationModel 
    {
        [Display(Name = "Verification PIN")]
        [Required(ErrorMessage = "PIN is required.")]
        public string Pin { get; set; }

        public string Email { get; set; }
    }

    public class ResetPasswordModel
    {
        [Display(Name = "New Password")]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Password confirmation is required.")]
        [Compare("Password", ErrorMessage = "Password does not match.")]
        public string ConfirmPassword { get; set; }
    }
}
