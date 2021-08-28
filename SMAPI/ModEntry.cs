using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewValley.Menus;
using SDV_Speaker.Speaker;
using System.IO;

#if Classic
using Harmony;
#endif
namespace SDV_Speaker.SMAPI
{




    internal class ModEntry : Mod
    {
        private IModHelper oHelper;
        private BubbleGuyManager oManager;
        public override void Entry(IModHelper helper)
        {
            oHelper = helper;
            oManager = new BubbleGuyManager(Path.Combine(helper.DirectoryPath, "saves"), Path.Combine(helper.DirectoryPath,"sprites") , helper,Monitor);
            BubbleChat.Initialize(oManager);
#if Current
                Harmony harmony = new Harmony(ModManifest.UniqueID);
#elif Classic
            var harmony = HarmonyInstance.Create(ModManifest.UniqueID);
#endif
            harmony.Patch(
          original: AccessTools.Method(typeof(ChatBox), "runCommand", new Type[] { typeof(string) }),
          prefix: new HarmonyMethod(typeof(BubbleChat), nameof(BubbleChat.RunCommand))
          );
            Monitor.Log($"Harmony patch applied", LogLevel.Info);

        }
    }
}