using System.ComponentModel.DataAnnotations;

namespace CBS_ASP.NET_Core_Course_Project.Models
{
    public class RegisterBindingModel
    {
        [Display(Name = "Login")]
        [Required]
        [EmailAddress]
        public string Login { get; set; }
        [Display(Name = "Password")]
        [UIHint("Password")]
        [MinLength(8)]
        public string Password { get; set; }
        [Display(Name = "Confirm password")]
        [UIHint("Password")]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
    }

    public class LoginBindingModel
    {
        [Display(Name = "Login")]
        [Required]
        [EmailAddress]
        public string Login { get; set; }
        [Display(Name = "Password")]
        [UIHint("Password")]
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
