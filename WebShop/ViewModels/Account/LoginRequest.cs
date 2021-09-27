using System.ComponentModel.DataAnnotations;

namespace WebShop.ViewModels.Account
{
    public class LoginRequest
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "RememberMe?")]
        public bool RememberMe { get; set; }
    }
}
