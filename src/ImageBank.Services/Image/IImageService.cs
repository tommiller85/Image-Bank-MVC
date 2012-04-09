using System.Collections.Generic;

namespace ImageBank.Services.Image
{
    public interface IImageService
    {
        IEnumerable<Core.Image> GetImagesByUser(string uploadedByUsername);
        void EditImages(IEnumerable<Core.Image> images);
        Core.Image GetImageById(int id);
    }
}