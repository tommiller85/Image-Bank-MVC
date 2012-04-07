using System.Drawing.Imaging;

namespace ImageBank.Core.ImageProcessing
{
    public class MipMap
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool PreserveAspect { get; set; }
        public bool HighQuality { get; set; }
        public long SaveQuality { get; set; }
        public long SaveBitDepth { get; set; }
        public ImageCodecInfo Codec { get; set; }

        public MipMap()
        {
            PreserveAspect = true;
            HighQuality = true;
            SaveQuality = 80;
            SaveBitDepth = 24;
        }
    }
}