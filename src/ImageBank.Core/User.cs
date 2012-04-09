using System.ComponentModel.DataAnnotations;

namespace ImageBank.Core
{
    public class User
    {
        [Key]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
