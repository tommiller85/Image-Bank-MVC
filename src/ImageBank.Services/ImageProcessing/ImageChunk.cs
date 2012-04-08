using System.IO;

namespace ImageBank.Services.ImageProcessing
{
    public class ImageChunk
    {
        public int? Chunk { get; set; }
        public int? Chunks { get; set; }
        public string SystemFilename { get; set; }
        public Stream InputStream { get; set; }
    }
}