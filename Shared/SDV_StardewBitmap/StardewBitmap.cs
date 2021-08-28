


#if Classic
using System.Drawing;
using System.Drawing.Imaging;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System.Runtime.InteropServices;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
#if SKIA
using SkiaSharp;
#endif


namespace StardewModHelpers
{
#if SKIA
    public class StardewBitmap
    {
        private SKBitmap SourceImage = null;

    #region "Constructors"
        public StardewBitmap()
        {

        }
        public StardewBitmap(Texture2D texture)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                texture.SaveAsPng(ms, texture.Width, texture.Height);
                //Go To the  beginning of the stream.
                ms.Seek(0, SeekOrigin.Begin);
                //Create the image based on the stream.
                SourceImage = SKBitmap.Decode(ms);
            }
        }
        public StardewBitmap(int width, int height)
        {
            SourceImage = new SKBitmap(width, height);
        }
        public StardewBitmap(MemoryStream ms)
        {
            SourceImage = SKBitmap.Decode(ms);

        }
    #endregion

    #region "public properties"
        public int Height { get { return SourceImage.Height; } }
        public int Width { get { return SourceImage.Width; } }
    #endregion

    #region "public methods"
        public Texture2D Texture()
        {
            return Texture2D.FromStream(Game1.graphics.GraphicsDevice, SourceStream());
        }

        public void DrawImage(StardewBitmap image, Rectangle source, Rectangle destination)
        {
            var canvas = new SKCanvas(SourceImage);

            canvas.DrawBitmap(StardewToSK(image), ConvertxRect(source), ConvertxRect(destination));
            canvas.Flush();

        }
        public void DrawString(string text, int x, int y)
        {
            var canvas = new SKCanvas(SourceImage);
            var font = SKTypeface.FromFamilyName("Arial");
            var brush = new SKPaint
            {
                Typeface = font,
                TextSize = 64.0f,
                IsAntialias = false,
                Color = new SKColor(255, 255, 255, 255)
            };
            canvas.DrawText(text, x, y, brush);
        }
        public MemoryStream SourceStream()
        {
            SKImage image = SKImage.FromBitmap(SourceImage);
            SKData encoded = image.Encode();
            var memoryStream = new MemoryStream();
            encoded.AsStream().CopyTo(memoryStream);
            return memoryStream;
        }

    #endregion

    #region "private methods"

        private SKRect ConvertxRect(Rectangle rect)
        {
            return new SKRect(rect.X, rect.Y, rect.Width, rect.Height);
        }

        private SKBitmap StardewToNative(StardewBitmap source)
        {
            return SKBitmap.Decode(source.SourceStream());
        }
    #endregion
    }

#endif

#if Classic || Common
    public class StardewBitmap
    {
        public Bitmap SourceImage = null;
        private Texture2D txOutput = null;

        #region "Constructors"
        public StardewBitmap()
        {

        }
        public StardewBitmap(Image source)
        {
            SourceImage = new Bitmap(source);
        }
        public StardewBitmap(StardewBitmap source)
        {
            SourceImage = new Bitmap(source.SourceImage);
        }
        public StardewBitmap(string pngFileName)
        {
            SourceImage = (Bitmap)Bitmap.FromFile(pngFileName);
        }
        public StardewBitmap(Texture2D texture)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                texture.SaveAsPng(ms, texture.Width, texture.Height);
                //Go To the  beginning of the stream.
                ms.Seek(0, SeekOrigin.Begin);
                //Create the image based on the stream.
                SourceImage = new Bitmap(ms);
            }
        }
        public StardewBitmap(int width, int height)
        {
            SourceImage = new Bitmap(width, height);
        }
        public StardewBitmap(MemoryStream ms)
        {
            SourceImage = new Bitmap(ms);
        }
        #endregion

        #region "public properties"
        public int Height { get { return SourceImage.Height; } }
        public int Width { get { return SourceImage.Width; } }
        #endregion

        #region "public methods"
        public Texture2D Texture()
        {
            if (txOutput == null)
            {
                txOutput= Texture2D.FromStream(Game1.graphics.GraphicsDevice, SourceStream());
            }
            return txOutput;
        }

        public void DrawImage(StardewBitmap image, Rectangle source, Rectangle destination)
        {
            //Image iTileSheet = new Bitmap(ImageHelpers.GetTextureStream(Game1.bigCraftableSpriteSheet));
            ImageAttributes iAttr = new ImageAttributes();
            using (Graphics gr = Graphics.FromImage(SourceImage))
            {
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                gr.DrawImage(image.SourceImage, GetSysRectangle(destination), GetSysRectangle(source), GraphicsUnit.Pixel);
            }
            //txSign = ImageHelpers.LoadTextureFromImage(imSign);
        }
        public void DrawString(string text, int x, int y, int width, int height)
        {
            Font fText = new Font("Verdana", 16, FontStyle.Bold);
            using (Graphics gr = Graphics.FromImage(SourceImage))
            {
                SizeF szStrring = gr.MeasureString(text, fText);
                StringFormat sf = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
                gr.DrawString(text, fText, Brushes.Black, new System.Drawing.Rectangle(x, y, width, height), sf);
            }
        }
        public MemoryStream SourceStream()
        {
            MemoryStream memoryStream = new MemoryStream();
            SourceImage.Save(memoryStream, ImageFormat.Png);
            return memoryStream;
        }
        public void ResizeImage(int width, int height)
        {
            Image imNew = new Bitmap(SourceImage, width, height);
            SourceImage = new Bitmap(imNew);
        }
        #endregion

        #region "private methods"
        private  MemoryStream GetTextureStream(Texture2D tTexture)
        {
            MemoryStream stream = new MemoryStream();

            using (Bitmap bitmap = new Bitmap(tTexture.Width, tTexture.Height, PixelFormat.Format32bppArgb))
            {
                byte blue;
                IntPtr safePtr;
                BitmapData bitmapData;
                Rectangle rect = new Rectangle(0, 0, tTexture.Width, tTexture.Height);
                byte[] textureData = new byte[4 * tTexture.Width * tTexture.Height];

                tTexture.GetData<byte>(textureData);
                for (int i = 0; i < textureData.Length; i += 4)
                {
                    blue = textureData[i];
                    textureData[i] = textureData[i + 2];
                    textureData[i + 2] = blue;
                }
                bitmapData = bitmap.LockBits(GetSysRectangle(rect), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                safePtr = bitmapData.Scan0;
                Marshal.Copy(textureData, 0, safePtr, textureData.Length);
                bitmap.UnlockBits(bitmapData);

                bitmap.Save(stream, ImageFormat.Png);
            }

            return stream;
        }

        private System.Drawing.Rectangle GetSysRectangle(Rectangle rXNA)
        {
            return new System.Drawing.Rectangle(rXNA.X, rXNA.Y, rXNA.Width, rXNA.Height);
        }

        private Bitmap StardewToNative(StardewBitmap source)
        {
            return new Bitmap(source.SourceStream());
        }
        private static MemoryStream ReadImage(string sImage)
        {
            MemoryStream ms = new MemoryStream();

            using (FileStream fs = new FileStream(sImage, FileMode.Open, FileAccess.Read))
            {
                fs.CopyTo(ms);
            }

            return ms;
        }

        private static Image GetImage(string sFilename)
        {
            Bitmap originalBmp = new Bitmap(Image.FromFile(sFilename));

            // Create a blank bitmap with the same dimensions
            Bitmap tempBitmap = new Bitmap(originalBmp.Width, originalBmp.Height);

            // From this bitmap, the graphics can be obtained, because it has the right PixelFormat
            using (Graphics g = Graphics.FromImage(tempBitmap))
            {
                // Draw the original bitmap onto the graphics of the new bitmap
                g.DrawImage(originalBmp, 0, 0);
            }

            // Use tempBitmap as you would have used originalBmp
            return tempBitmap;
        }


        #endregion
    }


#endif
}
