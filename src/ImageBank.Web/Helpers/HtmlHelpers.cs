using System.Web;
using System.Web.Mvc;

namespace ImageBank.Web.Helpers
{
    public static class HtmlHelpers
    {
        public static IHtmlString MediumSizeImage(this HtmlHelper helper, string filename, string systemFilename)
        {
            return
                new MvcHtmlString(string.Format("<img src=\"../../Images/Upload/640x427/{0}\" alt=\"{1}\" />", systemFilename,
                                                filename));
        }
    }
}