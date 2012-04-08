using System.IO;

namespace ImageBank.Services.ImageProcessing
{
    public class ImageChunkSaver : IImageChunkSaver
    {
        public void SaveImageChunk(ImageChunk imageChunk, FileMode fileMode, string saveDir)
        {
            using (
                var fs = new FileStream(Path.Combine(saveDir, imageChunk.SystemFilename),
                                        fileMode))
            {
                var buffer = new byte[imageChunk.InputStream.Length];
                imageChunk.InputStream.Read(buffer, 0, buffer.Length);
                fs.Write(buffer, 0, buffer.Length);
            }
        }
    }
}