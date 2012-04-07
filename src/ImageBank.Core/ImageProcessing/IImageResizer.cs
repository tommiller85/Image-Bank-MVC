using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace ImageBank.Core.ImageProcessing
{
    public interface IImageResizer
    {
        List<MemoryStream> GenerateMipMaps(List<MipMap> maps);
        void Resize(int width, int height, bool preserveAspect, bool highQuality);
        void Save(string pathToSave, ImageFormat imageFormat);
        MemoryStream Save(ImageFormat imageFormat);
        MemoryStream Save(ImageCodecInfo codec, long bitDepth, long quality);
        void LoadImage(string fileName);
        void LoadImage(Stream stream);
    }
}
