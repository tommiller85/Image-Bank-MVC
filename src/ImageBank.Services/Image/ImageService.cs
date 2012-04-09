using System.Linq;
using System.Collections.Generic;
using ImageBank.Core;
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

        public PagedList<Core.Image> GetImagesByUser(string uploadedByUsername, int pageIndex, int pageSize)
        {
            return
                new PagedList<Core.Image>(
                    _imageRepository.GetAll().OrderByDescending(x => x.UploadDate).Where(
                        x => x.UploadedByUsername == uploadedByUsername && x.Deleted == false), pageIndex,
                    pageSize);
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