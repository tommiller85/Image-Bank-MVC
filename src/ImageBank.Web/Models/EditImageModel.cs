using System.ComponentModel.DataAnnotations;

namespace ImageBank.Web.Models
{
    public class EditImageModel
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string Filename { get; set; }

        public string SystemFilename { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Display(Name = "Is Public")]
        public bool IsPublic { get; set; }

        [Display(Name = "Show On Homepage")]
        public bool ShowOnHomepage { get; set; }

        [Display(Name = "Delete Image")]
        public bool Delete { get; set; }
    }
}