using System.ComponentModel.DataAnnotations;

namespace GigHub.Core.Models
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}