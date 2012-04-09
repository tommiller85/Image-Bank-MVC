using System.ComponentModel.DataAnnotations;

namespace ImageBank.Core
{
    public class Setting
    {
        [Key]
        [StringLength(255)]
        public string Key { get; set; }

        [Required]
        [StringLength(255)]
        public string Value { get; set; }
    }
}