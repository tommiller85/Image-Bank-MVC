using ImageBank.Core;

namespace ImageBank.Persistence
{
    public class ImageRepository : GenericRepository<ImageBankContext, Image>, IImageRepository
    {
        public ImageRepository(ImageBankContext ctx)
            : base(ctx)
        {
        }
    }
}