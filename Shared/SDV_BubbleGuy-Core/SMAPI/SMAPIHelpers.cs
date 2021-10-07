using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;

namespace SDV_Speaker.SMAPIInt
{
    internal static class SMAPIHelpers
    {
        public static IModHelper helper;
 
        public static void Initialize(IModHelper helper)
        {
            SMAPIHelpers.helper = helper;
         }
    }
}
