namespace ImageBank.Services.ImageProcessing
{
    public interface IImageResizer
    {
        void GenerateMipMap(string imagePathToResize, MipMap map, string uploadDir, string filename);
    }
}