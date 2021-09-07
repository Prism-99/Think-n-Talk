using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using System.Collections.Generic;
using System.Threading;
using StardewValley;
//using StardewWeb.Utilities;

namespace StardewModHelpers
{

    internal static class StardewTextureLoader
    {
        private static List<string> lImagesToLoad = new List<string> { };
        private static Dictionary<string, StardewBitmap> dcImages = new Dictionary<string, StardewBitmap> { };
        private static List<string> lSpriteSheetToLoad = new List<string> { };
        private static Dictionary<string, StardewBitmap> dcSpriteSheets = new Dictionary<string, StardewBitmap> { };
        private static  IModHelper oHelper;
        private static int mainThreadId;
        public static void Initialize(IModHelper helper)
        {
            oHelper = helper;
            // making assumpition being initialized in main thread
            mainThreadId = Thread.CurrentThread.ManagedThreadId;
        }
        public static StardewBitmap LoadSpriteSheet(string sSheetName)
        {
            StardewBitmap sbResult;
            if (Thread.CurrentThread.ManagedThreadId == mainThreadId)
            {
                sbResult = GetSpriteSheet(sSheetName);
            }
            else
            {
                lock (lSpriteSheetToLoad)
                {
                    lSpriteSheetToLoad.Add(sSheetName);
                }
                oHelper.Events.GameLoop.UpdateTicked += GameLoop_UpdateTicked_LoadSheet;

                while (!dcSpriteSheets.ContainsKey(sSheetName))
                {
                    Thread.Sleep(200);
                }

                lock (dcSpriteSheets)
                {
                    sbResult = dcSpriteSheets[sSheetName];
                    dcSpriteSheets.Remove(sSheetName);
                }
            }

            return sbResult;
        }
        private static StardewBitmap GetSpriteSheet(string sSheetName)
        {
           // StardewLogger.LogTrace("GetSpritesheet", $"sheetname '{sSheetName}'");
            switch (sSheetName)
            {
                case "emoteSpriteSheet":
                    return new StardewBitmap(Game1.emoteSpriteSheet);
                case "objectSpriteSheet":
                    return new StardewBitmap(Game1.objectSpriteSheet);
                case "bigCraftableSpriteSheet":
                    return new StardewBitmap(Game1.bigCraftableSpriteSheet);
                default:
                    return new StardewBitmap(oHelper.Content.Load<Texture2D>(sSheetName,ContentSource.GameContent));
            }
        }
        public static StardewBitmap LoadImageInUIThread(string sImage)
        {
            StardewBitmap sbResult;
            if (Thread.CurrentThread.ManagedThreadId == mainThreadId)
            {
                sbResult = new StardewBitmap(oHelper.Content.Load<Texture2D>(sImage, ContentSource.GameContent));
            }
            else
            {
                lock (lImagesToLoad)
                {
                    lImagesToLoad.Add(sImage);
                }
                oHelper.Events.GameLoop.UpdateTicked += GameLoop_UpdateTicked_LoadImage;

                while (!dcImages.ContainsKey(sImage))
                {
                    Thread.Sleep(200);
                }

                //}
                 lock (dcImages)
                {
                    sbResult = dcImages[sImage];
                    dcImages.Remove(sImage);
                }
            }

            return sbResult;
        }

        private static void GameLoop_UpdateTicked_LoadImage(object sender, StardewModdingAPI.Events.UpdateTickedEventArgs e)
        {
            lock (lImagesToLoad)
            {
                foreach (string sImagePath in lImagesToLoad)
                {
                    Texture2D txImage = oHelper.Content.Load<Texture2D>(sImagePath, ContentSource.GameContent);
                    lock (dcImages)
                    {
                        dcImages.Add(sImagePath, new StardewBitmap(txImage));
                    }
                }
                lImagesToLoad.Clear();
            }
            oHelper.Events.GameLoop.UpdateTicked -= GameLoop_UpdateTicked_LoadImage;
        }
        private static void GameLoop_UpdateTicked_LoadSheet(object sender, StardewModdingAPI.Events.UpdateTickedEventArgs e)
        {
            lock (lSpriteSheetToLoad)
            {
                foreach (string sImagePath in lSpriteSheetToLoad)
                {
 
                    lock (dcSpriteSheets)
                    {
                        dcSpriteSheets.Add(sImagePath, GetSpriteSheet(sImagePath));
                    }
                }
                lSpriteSheetToLoad.Clear();
            }
            oHelper.Events.GameLoop.UpdateTicked -= GameLoop_UpdateTicked_LoadSheet;
        }
    }
}
