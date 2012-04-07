using System.IO;

namespace ImageBank.Core.ImageProcessing
{
    public interface IImageProcessor
    {
        string ProcessImage(FileMode fileMode, Stream stream, string filename, string savePath);
    }
}