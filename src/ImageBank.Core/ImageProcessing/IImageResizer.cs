using System.Collections.Generic;

namespace ImageBank.Core.ImageProcessing
{
    public interface IImageResizer
    {
        void GenerateMipMap(string imagePathToResize, MipMap map, string uploadDir, string filename);
    }
}