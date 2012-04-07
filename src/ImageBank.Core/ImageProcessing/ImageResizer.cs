using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace ImageBank.Core.ImageProcessing
{
    public class ImageResizer : IImageResizer, IDisposable
    {
        private System.Drawing.Image Image;
        private System.Drawing.Image Output;

        public ImageResizer(string fileName)
        {
            LoadImage(fileName);
        }

        public ImageResizer(Stream stream)
        {
            LoadImage(stream);
        }

        public List<MemoryStream> GenerateMipMaps(List<MipMap> maps)
        {
            if (Image == null)
            {
                throw new InvalidOperationException("No Image has been loaded.");
            }

            List<MemoryStream> mipmaps = new List<MemoryStream>();
            foreach (MipMap map in maps)
            {
                MemoryStream stream = new MemoryStream();

                if (Image.Width <= map.Width || Image.Height <= map.Height)
                {
                    Resize(Image.Width, Image.Height, false, map.HighQuality);
                }
                else
                {
                    Resize(map.Width, map.Height, map.PreserveAspect, map.HighQuality);
                }

                stream = Save(map.Codec, map.SaveBitDepth, map.SaveQuality);

                mipmaps.Add(stream);
            }

            return mipmaps;
        }

        public void Resize(int width, int height, bool preserveAspect, bool highQuality)
        {
            if (Image == null)
            {
                throw new InvalidOperationException("No Image has been loaded.");
            }

            int newHeight = height;

            if (preserveAspect)
            {
                newHeight = Image.Height*width/Image.Width;
                if (newHeight > height)
                {
                    width = Image.Width*height/Image.Height;
                    newHeight = height;
                }
            }

            Size newSize = new Size(width, newHeight);
            Output = new Bitmap(newSize.Width, newSize.Height);
            Graphics graphics = Graphics.FromImage(Output);

            if (highQuality)
            {
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            }
            else
            {
                graphics.SmoothingMode = SmoothingMode.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            }

            Rectangle rect = new Rectangle(0, 0, newSize.Width, newSize.Height);

            graphics.DrawImage(Image, rect, 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel);
        }

        public void Save(string pathToSave, ImageFormat imageFormat)
        {
            if (Output == null)
            {
                throw new InvalidOperationException("There is no output image.");
            }

            Output.Save(pathToSave, imageFormat);
        }

        public MemoryStream Save(ImageFormat imageFormat)
        {
            if (Output == null)
            {
                throw new InvalidOperationException("There is no output image.");
            }

            MemoryStream stream = new MemoryStream();
            Output.Save(stream, imageFormat);

            return stream;
        }

        public MemoryStream Save(ImageCodecInfo codec, long bitDepth, long quality)
        {
            if (codec == null)
            {
                throw new ArgumentNullException("codec", "you must specify a codec");
            }

            MemoryStream stream = new MemoryStream();

            Encoder encoderInstance = Encoder.Quality;
            EncoderParameters encoderParametersInstance = new EncoderParameters(2);
            EncoderParameter encoderParameterInstance = new EncoderParameter(encoderInstance, quality);
            encoderParametersInstance.Param[0] = encoderParameterInstance;
            encoderInstance = Encoder.ColorDepth;
            encoderParameterInstance = new EncoderParameter(encoderInstance, bitDepth);
            encoderParametersInstance.Param[1] = encoderParameterInstance;

            Output.Save(stream, codec, encoderParametersInstance);

            return stream;
        }

        public static ImageCodecInfo ProcessCodecs(string mimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            for (int i = 0; i < codecs.Length; i++)
            {
                if (codecs[i].MimeType.Equals(mimeType))
                    return codecs[i];
            }

            return null;
        }

        public void LoadImage(string fileName)
        {
            Image = System.Drawing.Image.FromFile(fileName, true);
        }

        public void LoadImage(Stream stream)
        {
            Image = System.Drawing.Image.FromStream(stream, true);
        }

        public void Dispose()
        {
            Output.Dispose();
            Image.Dispose();
        }
    }
}