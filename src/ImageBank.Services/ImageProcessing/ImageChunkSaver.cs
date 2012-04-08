using System.IO;

namespace ImageBank.Services.ImageProcessing
{
    public class ImageChunkSaver : IImageChunkSaver
    {
        private static readonly object _syncRoot = new object();

        public void SaveImageChunk(ImageChunk imageChunk, FileMode fileMode, string saveDir)
        {
            CreateDirIfNotExists(saveDir);

            using (
                var fs = new FileStream(Path.Combine(saveDir, imageChunk.SystemFilename),
                                        fileMode))
            {
                var buffer = new byte[imageChunk.InputStream.Length];
                imageChunk.InputStream.Read(buffer, 0, buffer.Length);
                fs.Write(buffer, 0, buffer.Length);
            }
        }

        //TODO: Test this to ensure it's working as programming renegade at the mo.
        private void CreateDirIfNotExists(string dir)
        {
            if (!Directory.Exists(dir))
            {
                lock (_syncRoot)
                {
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                }
            }
        }
    }
}