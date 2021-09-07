using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System.Drawing;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using System.Drawing.Imaging;
//using StardewWeb.Utilities;

namespace SDV_Speaker.Speaker
{
    public class SpeechBubble : Sign
    {
        private Image imSign;
        private Texture2D txSign;
        private Vector2 vPlayerLoc;

        public SpeechBubble() { }

        public SpeechBubble(Vector2 signpos, string sText) : base(signpos, 39)
        {
            Image iTileSheet = new Bitmap(Game1.bigCraftableSpriteSheet);
            Rectangle? sourceRectangle = getSourceRectForBigCraftable(39);
            imSign = new Bitmap(sourceRectangle.Value.Width * 4, sourceRectangle.Value.Height * 4);
            ImageAttributes iAttr = new ImageAttributes();
            using (Graphics gr = Graphics.FromImage(imSign))
            {
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                gr.DrawImage(iTileSheet, GetSysRectangle(new Rectangle(0, 0, imSign.Width, imSign.Height)), GetSysRectangle(sourceRectangle.Value), GraphicsUnit.Pixel);
                Font fText = new Font("Verdana", 12, FontStyle.Bold);
                SizeF szStrring = gr.MeasureString(sText, fText);
                StringFormat sf = new StringFormat { Alignment = StringAlignment.Center };
                gr.DrawString(sText, fText, Brushes.Black, new System.Drawing.Rectangle(10, 30, 100, 100), sf);
            }
            txSign = StardewWebImageUtils.LoadTextureFromImage(imSign);
            vPlayerLoc = signpos;
            //Rectangle rBaseBox = base.boundingBox.Value;
            //base.boundingBox.Value = new Rectangle(rBaseBox.X, rBaseBox.Y, rBaseBox.Width * 2, rBaseBox.Height);
        }
        public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
        {
            int farmerX = Game1.player.getTileX();
            int farmerY = Game1.player.getTileY();
            if(vPlayerLoc.X!=farmerX || vPlayerLoc.Y != farmerY-1)
            {
                if (Game1.currentLocation.objects[vPlayerLoc] is SpeechBubble)
                {
                    Game1.currentLocation.objects.Remove(vPlayerLoc);
                    Game1.currentLocation.objects.Add(new Vector2(farmerX, farmerY), this);
                }
            }
            // base.draw(spriteBatch, x, y, alpha);
            Vector2 vPos = new Vector2((float)(farmerX * 64), (float)((farmerY - 1) * 64));
            Texture2D objectSpriteSheet = Game1.bigCraftableSpriteSheet;
            Vector2 position = Game1.GlobalToLocal(Game1.viewport, vPos);
            Rectangle sourceRectangle = new Rectangle(0, 0, txSign.Width, txSign.Height);
            Color color = Color.White * alpha;
            Vector2 origin = new Vector2(8f, 8f);
            Vector2 scale2 = scale;
            float num = Math.Max(0f, (float)((farmerY + 1) * 64 - 24) / 10000f) + (float)farmerX * 1E-05f;
            spriteBatch.Draw(txSign, position, sourceRectangle, color, 0f, origin, (scale.Y > 1f) ? getScale().Y : 1f, ((bool)flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, num);
            // Utility.drawTextWithShadow(spriteBatch, "Hello", Game1.smallFont, new Vector2((float)(x * 64 + 25), (float)((y - 1) * 64 + 32)), Color.White, 1f, num + .1f, -1, -1, 0.0f, 3);

        }
        private System.Drawing.Rectangle GetSysRectangle(Rectangle rXNA)
        {
            return new System.Drawing.Rectangle(rXNA.X, rXNA.Y, rXNA.Width, rXNA.Height);
        }
    }
}