using System;
using System.IO;

namespace ImageBank.Core.ImageProcessing
{
    public class ImageProcessor : IImageProcessor
    {
        public void ProcessChunkedImage(ImageChunk imageChunk, FileMode fileMode, string savePath)
        {
            using (
                var fs = new FileStream(Path.Combine(savePath, imageChunk.Filename),
                                        fileMode))
            {
                var buffer = new byte[imageChunk.InputStream.Length];
                imageChunk.InputStream.Read(buffer, 0, buffer.Length);
                fs.Write(buffer, 0, buffer.Length);
            }
        }

        public void GenerateMipMaps()
        {
            //todo:
            throw new NotImplementedException();
        }
    }
}