
namespace ImageBank.Services.ImageProcessing
{
    public interface IImageProcessor
    {
        void ProcessImageChunk(ImageChunk imageChunk, string uploadedByUsername);
    }
}