
namespace ImageBank.Web.Models
{
    public class PagerModel
    {
        public int PageIndex { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
        public string Href { get; set; }
    }
}