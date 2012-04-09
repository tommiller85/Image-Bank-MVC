using System.Text;
using System.Web;
using System.Web.Mvc;
using ImageBank.Core;

namespace ImageBank.Web.Helpers
{
    public static class HtmlHelpers
    {
        public static IHtmlString Pager<T>(this HtmlHelper htmlHelper, PagedList<T> pagedList)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div>");

            if(pagedList.HasPreviousPage)
            {
                sb.Append("<a href=\"\" alt=\"\">previous</a>");
            }
            if (pagedList.HasNextPage)
            {
                sb.Append("<a href=\"\" alt=\"\">next</a>");
            }

            sb.Append("</div>");

            return new MvcHtmlString(sb.ToString());
        }
    }
}