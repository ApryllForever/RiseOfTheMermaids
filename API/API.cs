using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
//using DynamicMapTiles;
//using JsonAssets;

namespace RestStopLocations
{
    internal class ExternalAPIs
    {
        public static IContentPatcherAPI CP;
       // public static IJsonAssetsApi JA;
        //public static IWearMoreRingsApi MR;
        //public static ISpaceCoreApi SC;
        //public static IDynamicMapTilesApi DMT;

        private static IMonitor Monitor { get; set; }
        private static IModHelper Helper { get; set; }

        internal static void Initialize(IModHelper helper)
        {
            Helper = helper;
            
            CP = Helper.ModRegistry.GetApi<IContentPatcherAPI>("Pathoschild.ContentPatcher");
            if (CP is null)
            {
                Monitor.Log("Content Patcher is not installed; Rest Stop requires CP to run. Please install CP and restart your game.");
                return;
            }
            /*
            JA = Helper.ModRegistry.GetApi<IJsonAssetsApi>("spacechase0.JsonAssets");
            if (JA == null)
            {

                Monitor.Log("Json Assets API not found. If you end the day and save the game, You are doomed. The shuffle which will result is beyond comprehension.");
            }  */
            /*
            MR = Helper.ModRegistry.GetApi<IWearMoreRingsApi>("bcmpinc.WearMoreRings");
            if (MR == null)
            {
                Monitor.Log("Wear More Rings API not found.");
            }

            SC = Helper.ModRegistry.GetApi<ISpaceCoreApi>("spacechase0.SpaceCore");
            if (SC == null)
            {
                Monitor.Log("SpaceCore API not found. This could lead to issues.");
            }

            */
            /*
            DMT = Helper.ModRegistry.GetApi<IDynamicMapTilesApi>("aedenthorn.DynamicMapTiles");
            if (DMT == null)
            {
                Monitor.Log("Dynamic Map Tiles API not found.");
            }*/

        }
    }
}