using System.IO;
using ImageBank.Core;

namespace ImageBank.Services.ImageProcessing
{
    public interface IImageProcessor
    {
        //void SaveImageMetadata(Image image);
        //void ProcessChunkedImage(ImageChunk imageChunk, FileMode fileMode, string uploadDir);
        //void GenerateMipMaps(string filePath);

        void ProcessImageChunk(ImageChunk imageChunk);
    }
}