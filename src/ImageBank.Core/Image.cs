using System;
using System.ComponentModel.DataAnnotations;

namespace ImageBank.Core
{
    public class Image
    {
        [Key]
        public virtual int Id { get; set; }

        [Required]
        public virtual string Filename { get; set; }

        [Required]
        public virtual string SystemFilename { get; set; }

        [Required]
        public virtual DateTime UploadDate { get; set; }
    }
}