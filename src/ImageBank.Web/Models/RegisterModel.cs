using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ImageBank.Web.Models
{
    public class RegisterModel
    {
        [Required]
        [Display(Name = "Choose a username")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Choose a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm your password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords don't match.")]
        public string ConfirmPassword { get; set; }
    }
}