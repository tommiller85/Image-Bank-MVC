using System.IO;

namespace ImageBank.Core.ImageProcessing
{
    public class ImageProcessor : IImageProcessor
    {
        private readonly IImageResizer _imageResizer;

        public ImageProcessor(IImageResizer imageResizer)
        {
            
        }

        public void ProcessChunkedImage(ImageChunk imageChunk, FileMode fileMode, string uploadDir)
        {
            using (
                var fs = new FileStream(Path.Combine(uploadDir, imageChunk.Filename),
                                        fileMode))
            {
                var buffer = new byte[imageChunk.InputStream.Length];
                imageChunk.InputStream.Read(buffer, 0, buffer.Length);
                fs.Write(buffer, 0, buffer.Length);
            }
        }

        public void GenerateMipMaps(string filePath)
        {
            ////todo: fix with di aswell

            //using (ImageResizer imgLibrary = new ImageResizer(filePath))
            //{
            //    ImageCodecInfo info = ImageResizer.ProcessCodecs("image/jpeg"); // fix this

            //    List<MipMap> maps = new List<MipMap>
            //                            {
            //                                new MipMap {Codec = info, Width = 640, Height = 427}
            //                            };

            //    imgLibrary.GenerateMipMaps(maps);
            //}
        }
    }
}