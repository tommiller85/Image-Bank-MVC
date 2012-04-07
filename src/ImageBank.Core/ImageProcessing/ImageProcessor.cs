using System;
using System.IO;

namespace ImageBank.Core.ImageProcessing
{
    public class ImageProcessor : IImageProcessor
    {
        public string ProcessImage(FileMode fileMode, Stream stream, string filename, string savePath)
        {
            string systemFilename = GenerateSystemFilename(Path.GetExtension(filename));
            using (
                var fs = new FileStream(Path.Combine(savePath, systemFilename),
                                        fileMode))
            {
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                fs.Write(buffer, 0, buffer.Length);
            }
            return systemFilename;
        }

        private string GenerateSystemFilename(string extension)
        {
            return Guid.NewGuid().ToString() + extension;
        }
    }
}