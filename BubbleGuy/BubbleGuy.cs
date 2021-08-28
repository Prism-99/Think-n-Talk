using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;
using StardewValley;

using StardewModHelpers;

namespace SDV_Speaker.Speaker
{
    class BubbleGuy : NPC
    {
        StardewBitmap sbBackground = null;
        private readonly object oBackground = new object();
        public bool IsThought;
        public List<string> lTextLines;
        private readonly string sModDirectory;
        public BubbleGuy(bool bIsThought, string sText, string sModDir)
        {
            sModDirectory = sModDir;
            base.forceOneTileWide.Value = true;
            base.name.Value = "BubbleGuy";

            SetTexture(bIsThought);
            string[] arWords = sText.Split(' ');
            int iCurLen = 0;
            List<string> lLineWords = new List<string> { };
            lTextLines = new List<string> { };

            foreach (string word in arWords)
            {
                if (iCurLen + word.Length + lLineWords.Count / 2 < 20)
                {
                    lLineWords.Add(word);
                    iCurLen += word.Length;
                }
                else
                {
                    lTextLines.Add(string.Join(" ", lLineWords));
                    iCurLen = 0;
                    lLineWords = new List<string> { word };
                }
            }
            if (lLineWords.Count > 0)
            {
                lTextLines.Add(string.Join(" ", lLineWords));
            }
        }
        private void SetTexture(bool bIsThought)
        {
            IsThought = bIsThought;
            lock (oBackground)
            {
                if (bIsThought)
                {
                    sbBackground = new StardewBitmap(Path.Combine(sModDirectory, "think_bubble.png"));
                }
                else
                {
                    sbBackground = new StardewBitmap(Path.Combine(sModDirectory, "talk_bubble.png"));
                }
                sbBackground.ResizeImage(300, 200);
            }
        }
        public override void update(GameTime time, GameLocation location)
        {
            try
            {
                //
                //  used to keep the bubble above the NPC
                //
                float newX = Game1.player.position.X;
                float newY = Game1.player.position.Y;
                position.Set(new Vector2(newX, newY));
            }
            catch { }
            //setTileLocation(Game1.player.getTileLocation());
        }
        public override void dayUpdate(int dayOfMonth)
        {

        }
        public override void updateMovement(GameLocation location, GameTime time)
        {
            try
            {
                //base.updateMovement(location, time);
                float newX = Game1.player.position.X + 20;
                float newY = Game1.player.position.Y - 10;
                position.Set(new Vector2(newX, newY));
                //setTileLocation(Game1.player.getTileLocation());
            }
            catch { }

        }
        public override bool CanSocialize => false;

        public override bool canTalk()
        {
            return false;
        }

        public override void doEmote(int whichEmote, bool playSound, bool nextEventCommand = true)
        {
        }


        public override void draw(SpriteBatch b)
        {

            try
            {
                Vector2 origin = new Vector2(8f, 8f);
                Vector2 bgPosition = new Vector2(Position.X - 100, Position.Y - 265);
                Vector2 txtPosition;
                if (IsThought)
                {
                    txtPosition = new Vector2(Position.X - 80, Position.Y - 230);
                }
                else
                {
                    txtPosition = new Vector2(Position.X - 100, Position.Y - 265);
                }

               // float num = 1;
                float num2 = Math.Max(0f, (float)((bgPosition.Y + 1) - 24) / 10000f) + (float)bgPosition.X * 1E-05f;

                b.Draw(sbBackground.Texture(), Game1.GlobalToLocal(Game1.viewport, bgPosition), new Rectangle(0, 0, sbBackground.Width, sbBackground.Height), Color.White, 0f, origin, 1f, SpriteEffects.None, 0.99999f);

                foreach (string line in lTextLines)
                {
                    Utility.drawTextWithShadow(b, line, Game1.smallFont, Game1.GlobalToLocal(Game1.viewport, txtPosition), Color.Black, 1f, 1f, -1, -1, 0.0f, 3);
                    txtPosition = new Vector2(txtPosition.X, txtPosition.Y + 25);
                }
            }
            catch { }
        }
    }
}