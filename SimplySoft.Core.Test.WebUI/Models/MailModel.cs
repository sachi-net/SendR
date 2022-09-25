using System.ComponentModel.DataAnnotations;

namespace SimplySoft.Core.Test.WebUI.Models
{
    public class MailModel
    {
        public ResponseMessage ResponseMessage { get; set; }

        [Display(Name = "Subject")]
        [Required(ErrorMessage = "Subject is required.")]
        public string Subject { get; set; }

        [Display(Name = "Subject")]
        [Required(ErrorMessage = "Subject is required.")]
        [StringLength(256, MinimumLength = 16, ErrorMessage = "Length should be between 16 and 256")]
        public string Message { get; set; }
    }
}
