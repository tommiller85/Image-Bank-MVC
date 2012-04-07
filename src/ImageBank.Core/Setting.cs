using System.ComponentModel.DataAnnotations;

namespace ImageBank.Core
{
    public class Setting
    {
        [Key]
        [Required]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }
    }
}