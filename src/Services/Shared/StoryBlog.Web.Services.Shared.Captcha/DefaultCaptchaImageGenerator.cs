using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace StoryBlog.Web.Services.Shared.Captcha
{
    public class DefaultCaptchaImageGenerator : ICaptchaImageGenerator
    {
        private readonly CaptchaOptions options;
        private readonly Random random;

        public DefaultCaptchaImageGenerator(IOptions<CaptchaOptions> options)
        {
            this.options = options.Value;
            random = new Random();
        }

        public Task<Image> GenerateImageAsync(char[] captcha)
        {
            var imageSize = options.Image.Size;
            var size = new Size(imageSize.Width - 1, imageSize.Height - 1);
            var bounds = new Rectangle(Point.Empty, size);
            var image = new Bitmap(imageSize.Width, imageSize.Height);

            using (var graph = Graphics.FromImage(image))
            {
                graph.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
                graph.CompositingMode = CompositingMode.SourceOver;
                graph.CompositingQuality = CompositingQuality.HighSpeed;
                graph.InterpolationMode = InterpolationMode.Default;
                graph.SmoothingMode = SmoothingMode.HighSpeed;

                graph.Clear(GetBackgroundColor());
                DrawRandomLines(graph, bounds);
                DrawCaptchaText(graph, bounds, captcha);
            }

            return Task.FromResult<Image>(image);
        }

        private void DrawCaptchaText(Graphics graph, Rectangle bounds, char[] captcha)
        {
            var fontSize = GetFontSize(bounds.Width, captcha.Length);
            var font = new Font(FontFamily.GenericSerif, fontSize, FontStyle.Bold, GraphicsUnit.Pixel);

            for (var index = 0; index < captcha.Length; index++)
            {
                var fontBrush = new SolidBrush(GetForegroundColor());

                var shiftPx = fontSize / 6;
                var maxY = Math.Max(bounds.Height - fontSize, 0);

                var position = new PointF(
                    index * fontSize + random.Next(-shiftPx, shiftPx) + random.Next(-shiftPx, shiftPx),
                    random.Next(0, maxY)
                );
                var transform = new Matrix();

                transform.RotateAt(random.Next(-40, 40), position);

                graph.Transform = transform;

                graph.DrawString(captcha[index].ToString(), font, fontBrush, position);
            }
        }

        private void DrawRandomLines(Graphics graph, Rectangle bounds)
        {
            for (var count = random.Next(3, 5); 0 < count; count--)
            {
                var pen = new Pen(new SolidBrush(GetBackgroundColor()), random.Next(3, 5));
                var start = new Point(
                    random.Next(bounds.Left, bounds.Right),
                    random.Next(bounds.Top, bounds.Bottom)
                );
                var end = new Point(
                    random.Next(bounds.Left, bounds.Right),
                    random.Next(bounds.Top, bounds.Bottom)
                );

                if (random.NextDouble() < 0.5d)
                {
                    graph.DrawLine(pen, start, end);
                    continue;
                }

                graph.DrawBezier(
                    pen,
                    start,
                    new Point(
                        random.Next(bounds.Left, bounds.Right),
                        random.Next(bounds.Top, bounds.Bottom)
                    ),
                    new Point(
                        random.Next(bounds.Left, bounds.Right),
                        random.Next(bounds.Top, bounds.Bottom)
                    ),
                    end
                );
            }
        }

        /*private void AdjustRippleEffect(Bitmap bitmap, Rectangle bounds)
        {
            short nWave = 6;

            var nWidth = bitmap.Width;
            var nHeight = bitmap.Height;
            var pt = new Point[nWidth, nHeight];

            for (var x = 0; x < nWidth; ++x)
            {
                for (var y = 0; y < nHeight; ++y)
                {
                    var xo = ((double) nWave * Math.Sin(2.0 * 3.1415 * (float) y / 128.0));
                    var yo = ((double) nWave * Math.Cos(2.0 * 3.1415 * (float) x / 128.0));

                    var newX = (x + xo);
                    var newY = (y + yo);

                    pt[x, y].X = newX > 0 && newX < nWidth ? (int) newX : 0;
                    pt[x, y].Y = newY > 0 && newY < nHeight ? (int) newY : 0;
                }
            }

            var bSrc = (Bitmap) bitmap.Clone();
            var bitmapData = bitmap.LockBits(bounds, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            var bmSrc = bSrc.LockBits(bounds, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            var scanline = bitmapData.Stride;
            var Scan0 = bitmapData.Scan0;
            var SrcScan0 = bmSrc.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* pSrc = (byte*)(void*)SrcScan0;

                var nOffset = bitmapData.Stride - bitmap.Width * 3;

                int xOffset, yOffset;

                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        xOffset = pt[x, y].X;
                        yOffset = pt[x, y].Y;

                        if (yOffset >= 0 && yOffset < nHeight && xOffset >= 0 && xOffset < nWidth)
                        {
                            p[0] = pSrc[(yOffset * scanline) + (xOffset * 3)];
                            p[1] = pSrc[(yOffset * scanline) + (xOffset * 3) + 1];
                            p[2] = pSrc[(yOffset * scanline) + (xOffset * 3) + 2];
                        }

                        p += 3;
                    }
                    p += nOffset;
                }
            }

            bitmap.UnlockBits(bitmapData);

            bSrc.UnlockBits(bmSrc);
            bSrc.Dispose();
        }*/

        private static int GetFontSize(int imageWidth, int captchaLength)
        {
            var averageSize = imageWidth / captchaLength;
            return Convert.ToInt32(averageSize);
        }

        private Color GetForegroundColor()
        {
            const int red = 160;
            const int green = 100;
            const int blue = 160;

            return Color.FromArgb(
                random.Next(red),
                random.Next(green),
                random.Next(blue)
            );
        }

        private Color GetBackgroundColor()
        {
            const int low = 180;
            const int high = 255;
            const int factor = (high - low) + low;

            return Color.FromArgb(
                random.Next(high) % factor,   // red
                random.Next(high) % factor,   // green
                random.Next(high) % factor    // blue
            );
        }
    }
}
