using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace ImageBank.Services.ImageProcessing
{
    public class ImageResizer : IImageResizer, IDisposable
    {
        private Image _image;
        private Image _output;

        public void GenerateMipMap(string imagePathToResize, MipMap map, string uploadDir, string filename)
        {
            LoadImage(imagePathToResize);

            if (_image == null)
            {
                throw new InvalidOperationException("No Image has been loaded.");
            }

            if (_image.Width <= map.Width || _image.Height <= map.Height)
            {
                Resize(_image.Width, _image.Height, false, map.HighQuality);
            }
            else
            {
                Resize(map.Width, map.Height, map.PreserveAspect, map.HighQuality);
            }

            Save(uploadDir, filename, map.Codec, map.SaveBitDepth, map.SaveQuality);
        }

        private void LoadImage(string filePath)
        {
            _image = Image.FromFile(filePath, true);
        }

        private void LoadImage(Stream stream)
        {
            _image = Image.FromStream(stream, true);
        }

        private void Resize(int width, int height, bool preserveAspect, bool highQuality)
        {
            if (_image == null)
            {
                throw new InvalidOperationException("No Image has been loaded.");
            }

            int newHeight = height;

            if (preserveAspect)
            {
                newHeight = _image.Height*width/_image.Width;
                if (newHeight > height)
                {
                    width = _image.Width*height/_image.Height;
                    newHeight = height;
                }
            }

            Size newSize = new Size(width, newHeight);
            _output = new Bitmap(newSize.Width, newSize.Height);
            Graphics graphics = Graphics.FromImage(_output);

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

            graphics.DrawImage(_image, rect, 0, 0, _image.Width, _image.Height, GraphicsUnit.Pixel);
        }

        private void Save(string uploadDir, string filename, ImageCodecInfo codec, long bitDepth, long quality)
        {
            if (codec == null)
            {
                throw new ArgumentNullException("codec", "you must specify a codec");
            }

            Encoder encoderInstance = Encoder.Quality;
            EncoderParameters encoderParametersInstance = new EncoderParameters(2);
            EncoderParameter encoderParameterInstance = new EncoderParameter(encoderInstance, quality);
            encoderParametersInstance.Param[0] = encoderParameterInstance;
            encoderInstance = Encoder.ColorDepth;
            encoderParameterInstance = new EncoderParameter(encoderInstance, bitDepth);
            encoderParametersInstance.Param[1] = encoderParameterInstance;

            _output.Save(Path.Combine(uploadDir, filename), codec, encoderParametersInstance);
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

        public void Dispose()
        {
            if(_image != null)
                _image.Dispose();
            if(_output != null)
                _output.Dispose();
        }
    }
}