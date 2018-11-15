using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace GenerateBitmapTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var bmap = GenerateBitmap("TEST");
            bmap.Save("image.jpg");

            Console.WriteLine("Press any key, then find and open 'image.jpg'...");
            Console.ReadKey();
        }

        static Bitmap GenerateBitmap(string text)
        {
            const int Width = 300;
            const int Height = 75;
            var bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var rectangle = new Rectangle(0, 0, Width, Height);
                SizeF size;
                float fontSize = rectangle.Height + 1;
                Font font;
                do
                {
                    fontSize--;
                    font = new Font(FontFamily.GenericSansSerif, fontSize, FontStyle.Bold);
                    size = graphics.MeasureString(text, font);
                }
                while (size.Width > rectangle.Width);
                var format = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                using (var path = new GraphicsPath())
                {
                    using (font)
                    {
                        path.AddString(text, font.FontFamily, (int)font.Style, font.Size, rectangle, format);
                    }
                    const float V = 4F;
                    var random = new Random(DateTime.UtcNow.Millisecond);
                    PointF[] points =
                    {
                        new PointF(random.Next(rectangle.Width) / V, random.Next(rectangle.Height) / V),
                        new PointF(rectangle.Width - random.Next(rectangle.Width) / V, random.Next(rectangle.Height) / V),
                        new PointF(random.Next(rectangle.Width) / V, rectangle.Height - random.Next(rectangle.Height) / V),
                        new PointF(rectangle.Width - random.Next(rectangle.Width) / V, rectangle.Height - random.Next(rectangle.Height) / V)
                    };
                    path.Warp(points, rectangle, null, WarpMode.Perspective, 0F);
                    using (var brush = new SolidBrush(Color.Black))
                    {
                        graphics.FillPath(brush, path);
                    }
                }
            }
            return bitmap;
        }
    }
}
