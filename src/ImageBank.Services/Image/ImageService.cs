using System.Linq;
using System.Collections.Generic;
using ImageBank.Persistence;

namespace ImageBank.Services.Image
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;

        public ImageService(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public IEnumerable<Core.Image> GetImagesByUser(string uploadedByUsername)
        {
            return _imageRepository.GetAll().Where(
                x => x.UploadedByUsername == uploadedByUsername
                ).ToList();
        }

        public void EditImages(IEnumerable<Core.Image> images)
        {
            foreach (var image in images)
            {
                _imageRepository.Edit(image);
            }
        }

        public Core.Image GetImageById(int id)
        {
            return _imageRepository.Get(id);
        }
    }
}