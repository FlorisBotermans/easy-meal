using System.ComponentModel.DataAnnotations;

namespace EasyMeal_WebGUI.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }
        [Required]
        [UIHint("password")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; } = "/";
    }
}
