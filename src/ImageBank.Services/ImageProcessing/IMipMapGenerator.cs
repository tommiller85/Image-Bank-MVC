namespace ImageBank.Services.ImageProcessing
{
    public interface IMipMapGenerator
    {
        void GenerateMipMap(string imagePathToResize, MipMap map, string uploadDir, string filename);
    }
}