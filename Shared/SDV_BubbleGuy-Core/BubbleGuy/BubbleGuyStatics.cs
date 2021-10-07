

using StardewValley;
using System.IO;

namespace SDV_Speaker.Speaker
{
   internal static class BubbleGuyStatics
    {
        public static string ModPath;
        public static string AssetsPath;
        public static string SavesPath;
        public static string BubbleGuyName;
        public static string BubbleGuyPrefix => "BubbleGuy_";
        public static void Initialize(string sModPath)
        {
            ModPath = sModPath;
            AssetsPath =  Path.Combine(sModPath, "assets", "bubbleguy");
            SavesPath = Path.Combine(sModPath, "saves");
        }
        public static void SetBubbleGuyId()
        {
            BubbleGuyName = $"{BubbleGuyPrefix}{Game1.player.uniqueMultiplayerID}";
        }
    }
}
