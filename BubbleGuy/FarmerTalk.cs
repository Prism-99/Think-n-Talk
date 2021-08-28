using System;


#if Classic
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
#endif

using StardewValley;

namespace SDV_Speaker.Speaker
{
    class FarmerTalk
    {
        public static bool Draw(SpriteBatch b)
        {
            return Draw(b, "Hello");
        }
        public static bool Draw(SpriteBatch b, string sText)
        {
             int x;
            int y;
         
            try
            {
                if (Game1.hasLoadedGame)
                {
                    x = Game1.player.getTileX() + 3;
                    y = Game1.player.getTileY() - 3;

                    float num = Math.Max(0f, (float)((y + 1) * 64 - 24) / 10000f) + (float)x * 1E-05f;

                    Utility.drawTextWithShadow(b, sText, Game1.dialogueFont, new Vector2((float)(x * 64 + 25), (float)((y) * 64 + 32)), Color.Yellow, 1f, num + .1f, -1, -1, 0.0f, 3);
                }
            }
            catch { }
            return true;
        }
    }
}
