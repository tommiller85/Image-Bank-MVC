using System.Web;

namespace ImageBank.Services.Virtual
{
    public class VirtualPathFinder : IVirtualPathFinder
    {
        public string ResolvePath(string virtualPath)
        {
            return HttpContext.Current.Server.MapPath(virtualPath);
        }
    }
}