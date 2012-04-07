using System.IO;

namespace ImageBank.Core.ImageProcessing
{
    public class ImageChunk
    {
        public int? Chunk { get; set; }
        public int? Chunks { get; set; }
        public string Filename { get; set; }
        public Stream InputStream { get; set; }
    }
}