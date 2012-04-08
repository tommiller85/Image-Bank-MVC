
using System.IO;

namespace ImageBank.Services.ImageProcessing
{
    public interface IImageChunkSaver
    {
        void SaveImageChunk(ImageChunk imageChunk, FileMode fileMode, string saveDir);
    }
}
