using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ImageBank.Core
{
    public class Category
    {
        [Key]
        public virtual int Id { get; set; }

        [Required]
        [StringLength(255)]
        public virtual string Name { get; set; }

        public virtual ICollection<Image> Images { get; set; }
    }
}