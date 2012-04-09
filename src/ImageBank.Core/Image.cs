using System;
using System.ComponentModel.DataAnnotations;

namespace ImageBank.Core
{
    public class Image
    {
        [Key]
        public virtual int Id { get; set; }

        [Required]
        [StringLength(500)]
        public virtual string Filename { get; set; }

        [Required]
        [StringLength(500)]
        public virtual string SystemFilename { get; set; }

        [Required]
        public virtual DateTime UploadDate { get; set; }

        [StringLength(1000)]
        public virtual string Description { get; set; }

        [Required]
        public virtual bool IsPublic { get; set; }

        [Required]
        public virtual bool ShowOnHomePage { get; set; }

        public virtual User UploadedByUser { get; set; }

        public virtual string UploadedByUsername { get; set; }
    }
}