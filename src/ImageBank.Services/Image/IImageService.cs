using System.Collections.Generic;
using ImageBank.Core;

namespace ImageBank.Services.Image
{
    public interface IImageService
    {
        PagedList<Core.Image> GetImagesByUser(string uploadedByUsername, int pageIndex, int pageSize);
        void EditImages(IEnumerable<Core.Image> images);
        Core.Image GetImageById(int id);
        IEnumerable<Core.Image> GetHomepageImages();
    }
}