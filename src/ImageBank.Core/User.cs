using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ImageBank.Core
{
    public class User
    {
        [Key]
        [StringLength(255)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        public ICollection<Image> Images { get; set; }
    }
}