using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Alturos.Yolo
{
    public class ImageResizer
    {
        public byte[] Resize(byte[] imageData, int width, int height)
        {
            using (var image = this.Byte2Image(imageData))
            using (var resizedImage = this.ResizeImage(image, width, height))
            {
                return this.Image2Byte(resizedImage);
            }
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        private Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.SmoothingMode = SmoothingMode.None;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private Image Byte2Image(byte[] imageData)
        {
            using (var memoryStream = new MemoryStream(imageData))
            {
                return Image.FromStream(memoryStream);
            }
        }

        private byte[] Image2Byte(Image image)
        {
            using (var memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, ImageFormat.Bmp);
                return memoryStream.ToArray();
            }
        }
    }
}
