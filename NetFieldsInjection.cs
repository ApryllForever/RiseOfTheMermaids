using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using RestStopLocations.VirtualProperties;
using StardewValley;
using StardewValley.Monsters;
using StardewValley.Network;

namespace RestStopLocations.Patches
{

    
    

    [HarmonyPatch(typeof(FarmerTeam), MethodType.Constructor)]
    public static class FarmerTeamInjectNetFieldsPatch
    {
        public static void Postfix(FarmerTeam __instance)
        {
            __instance.NetFields.AddField(__instance.hasEnlightenment());
        }
    }

    
}