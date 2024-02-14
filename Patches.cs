

using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewValley;
using RestStopLocations.Game.Locations;
using RestStopLocations.Bluebella;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using RestStopLocations.Game.Locations.Sapphire;
using RestStopLocations.Utilities;
using StardewValley.Enchantments;
using System;
using System.Xml.Linq;
using StardewValley.GameData.Crops;
using StardewValley.GameData.WildTrees;

namespace RestStopLocations.Patches
{
    [HarmonyPatch(typeof(Game1), nameof(Game1.getLocationFromNameInLocationsList))]
    internal static class Game1FetchDungeonInstancePatch
    {
        public static bool Prefix(string name, bool isStructure, ref GameLocation __result)
        {
            if (name.StartsWith(HellDungeon.BaseLocationName))
            {
                __result = HellDungeon.GetLevelInstance(name);
                return false;
            }
            if (name.StartsWith(BluebellaDungeon.BaseLocationName))
            {
               __result = BluebellaDungeon.GetLevelInstance(name);
                return false;
            }
            if (name.StartsWith(SapphireVolcano.BaseLocationName))
            {
                __result = SapphireVolcano.GetLevelInstance(name);
                return false;
            }
            return true;

        }



        [HarmonyPatch(typeof(Tool), nameof(Tool.Forge))]
        public static class ToolForgePatch
        {
            public static void Postfix(ref bool __result, Tool __instance, Item item)
            {
                BaseEnchantment enchantment = BaseEnchantment.GetEnchantmentFromItem(__instance, item);

                if (__result && enchantment is PearlEanchantment && __instance is MeleeWeapon weapon && PearlEanchantment.IsMermaidWeapon(weapon))
                {
                    if (weapon.GetEnchantmentLevel<PearlEanchantment>() < 3) { return; }

                    int current_index = weapon.InitialParentTileIndex;
                    int sirenswordint = 30;// ExternalAPIs.JA.GetWeaponId("Siren's Blade");
                    int sirendaggerint = 31;// ExternalAPIs.JA.GetWeaponId("Siren Splinter");
                    int sirenrevengeint = 32;/// ExternalAPIs.JA.GetWeaponId("Siren's Revenge");
                    int sirensilenceint = 33;// ExternalAPIs.JA.GetWeaponId("Siren's Silence");

                    string sirensword = Convert.ToString(sirenswordint);
                    string sirendagger = Convert.ToString(sirendaggerint);
                    string sirenrevenge = Convert.ToString(sirenrevengeint);
                    string sirensilence = Convert.ToString(sirensilenceint);

                    if (current_index == sirenswordint)
                        weapon.transform(sirenrevenge);

                    else if (current_index == sirendaggerint)
                        weapon.transform(sirensilence);
                }
            }
        }


        [HarmonyPatch(typeof(BaseEnchantment), nameof(BaseEnchantment.GetEnchantmentFromItem))]
        public static class BaseEnchatmentPearlEnchatmentPatch
        {
            /// <summary>
            /// This patch injects custom enchantment 'PearlEnchantment' 
            /// when method for get item enchantment is called 
            /// and the base item is mermaid weapon
            /// and the enchanting item is a pearl
            /// </summary>
            public static void Postfix(ref BaseEnchantment __result, Item base_item, Item item)
            {
                if (base_item == null || (base_item is MeleeWeapon && !(base_item as MeleeWeapon).isScythe()))
                {
                    if (base_item != null && base_item is MeleeWeapon && PearlEanchantment.IsMermaidWeapon(base_item) && Utility.IsNormalObjectAtParentSheetIndex(item, "(O)797"))
                    {
                        __result = new PearlEanchantment();
                    }
                }
            }
        }





        [HarmonyPatch(typeof(Game1), "UpdateLocations")]
        public static class Game1UpdateDungeonLocationsPatch
        {
            public static void Postfix(GameTime time)
            {
                //if (Game1.menuUp && !Game1.IsMultiplayer)
                //{
                    //return;
                //}
                if (Game1.IsClient)
                {
                    return;
                }
                 //MermaidTrain mermaidTrain = new MermaidTrain();
                //mermaidTrain.Update(time);
                HellDungeon.UpdateLevels(time);
                BluebellaDungeon.UpdateLevels(time);
                SapphireVolcano.UpdateLevels(time);
                AmbientishLocationSounds.update(time);
               
            }
        }




        [HarmonyPatch(typeof(GameLocation), "UpdateWhenCurrentLocation")]
        public static class GameLocationUpdatePatch
        {
            public static void Postfix(GameTime time)
            {
                
                if (Game1.IsClient)
                {
                    return;
                }
                
                AmbientishLocationSounds.update(time);
                AmbientesqueLocationSounds.update(time);
            }
        }


        [HarmonyPatch(typeof(Game1), "Initialize")]
        public static class Game1InitializePatch
        {
            public static void Postfix()
            {
                //RestStop.InitShared();
                LocationSounds.InitShared();
                AmbientishLocationSounds.Initialize();
                AmbientesqueLocationSounds.InitShared();
            }
        }
















        /*                                                                  EXAMPLE of How to do the __instance to instantiate a thingie in Harmony like for to make an object reference and whatnot
        [HarmonyPatch(typeof(ShopMenu), "setUpShopOwner")]
        public static class ShopMenuPatch
        {
            public static void Postfix(ShopMenu __instance,  string who)
            {
                if (Game1.currentLocation is SapphireSprings)
                {
                    __instance.potraitPersonDialogue = Game1.parseText("Hey Lovely! Care to peruse my fine wares?", Game1.dialogueFont, 304);

                }            
            }
        }  */











    }
}


/*
//List<BaseEnchantment> GetAvailableEnchantments()

[HarmonyPatch(typeof(BaseEnchantment), nameof(BaseEnchantment.GetAvailableEnchantments))]
public static class BaseEnchantmentListPatch
{
    public static void PostFix(ref List<BaseEnchantment> __result)
    {

        



    }
}*/



/*
[HarmonyPatch(typeof(Tree), nameof(Tree.getBoundingBox))]
public static class SequoiaPatch
{
    public static bool Prefix(Tree __instance, ref Microsoft.Xna.Framework.Rectangle __result)
    {
       WildTreeData data = __instance.GetData();

        if (data != null && data.CustomFields.ContainsValue("MermaidSequoia"))

        //if (__instance != null && __instance.TextureName.StartsWith("MermaidSeq"))
        {
            Vector2 tileLocation = __instance.Tile;
           // if ( (int)__instance.growthStage < 4)
           // {
            //    __result = new Microsoft.Xna.Framework.Rectangle((int)tileLocation.X * 64, (int)tileLocation.Y * 64, 64, 64);
          //     return false;
           // }
           // else
                __result = new Microsoft.Xna.Framework.Rectangle((int)(tileLocation.X) * 64, (int)(tileLocation.Y) * 64, 192, 192);
            return false;
        }
       
        return true;
    }
}
*/
/*
[HarmonyPatch(typeof(Tree), nameof(Tree.getBoundingBox))]
public static class SequoiaPatch
{
    public static bool Prefix(Tree __instance, ref Microsoft.Xna.Framework.Rectangle __result)
    {
        if (int.TryParse(__instance.treeType.Value, out _)) // ignore vanilla trees
            return true;

        if (__instance.treeType.Name.Equals("Mermaid.Sequoia"))
        {
            Vector2 tileLocation = __instance.Tile;
            if ((int)__instance.growthStage < 4)
            {
                __result = new Microsoft.Xna.Framework.Rectangle((int)tileLocation.X * 64, (int)tileLocation.Y * 64, 64, 64);
                return false;
            }
            else
                __result = new Microsoft.Xna.Framework.Rectangle((int)(tileLocation.X - 1f) * 64, (int)(tileLocation.Y - 1f) * 64, 192, 192);
            return false;
        }
        return true;
    }
} */




[HarmonyPatch(typeof(Tree), nameof(Tree.performToolAction))]
    public static class TreeToolActionPatch
{
    public static void Prefix(Tree __instance, Tool t, int explosion)
    {
          
        if (t is Axe)
        {
           if (Game1.currentLocation is RestStop or SapphireForest or RealmofSpiritsWinter or SouthRestStop or SapphireSprings or JunkBeach or SovaraCanyon or AspenCanyon)
            {
                __instance.health.Value = 999999999999;
                Game1.player.Money -= 100;
                Game1.addHUDMessage(new HUDMessage("Ranger Wren tickets you for illegal wood harvesting in the national park! You got what was coming to you!!!", 1));
            }
        }
    }
}


[HarmonyPatch(typeof(Tree), nameof(Tree.performToolAction))]
public static class SevereTreeToolActionPatch
{
    public static void Prefix(Tree __instance, Tool t, int explosion)
    {

        if (t is Axe)
        {
            if (Game1.currentLocation is EmeraldForestShrine)
            {
                __instance.health.Value = 999999999999;
                Game1.player.Money -= 1000000000;
                Game1.player.health = 0;
                Game1.addHUDMessage(new HUDMessage("The fairies slaughter you for trying to cut wood in the Sacred Forest!!!", 1));
            }
        }
    }
}




/*
[HarmonyPatch(typeof(Multiplayer), nameof(Multiplayer.updateRoots))]
public static class MultiplayerUpdateDungeonRootsPatch
{
    public static void Postfix(Multiplayer __instance)
    {
        foreach (var level in HellDungeon.activeLevels)
        {
            if (level.Root.Value is not null)
            {
                level.Root.Clock.InterpolationTicks = __instance.interpolationTicks();
                __instance.updateRoot(level.Root);
            }
        }
    }
}*/


/*
[HarmonyPatch(typeof(Game1), nameof(Game1.updatePause))]
public static class Game1UpdatePausePatch
{
    public static void Postfix(GameTime gameTime)
    {


        //string HeckDungeon = (HellDungeon.BaseLocationName);
        if (Game1.killScreen && Game1.currentLocation is HellLocation)
        {


            Game1.warpFarmer("Custom_RestStopSouth", 15, 5, flip: false);


        }
        return;
        Game1.progressBar = false;
        //Game1.currentLocation.currentEvent.CurrentCommand++;

        if (Game1.killScreen && !Game1.eventUp)
        {
            {
                Game1.warpFarmer("Custom_SouthRestStop", 39, 18, flip: false);
                string rescuer3 = "Vika";
                string uniquemessage3 = "Data\\ExtraDialogue:Mines_PlayerKilled_Vika";
                switch (Game1.random.Next(7))
                {
                    case 0:
                        rescuer3 = "Wren";
                        uniquemessage3 = "Data\\ExtraDialogue:Mines_PlayerKilled_Wren";
                        break;
                    case 1:
                        rescuer3 = "Clint";
                        uniquemessage3 = "Data\\ExtraDialogue:Mines_PlayerKilled_Clint";
                        break;
                    case 2:
                        rescuer3 = "Maru";
                        uniquemessage3 = ((Game1.player.spouse != null && Game1.player.spouse.Equals("Maru")) ? "Data\\ExtraDialogue:Mines_PlayerKilled_Maru_Spouse" : "Data\\ExtraDialogue:Mines_PlayerKilled_Maru_NotSpouse");
                        break;
                    default:
                        rescuer3 = "Vika";
                        uniquemessage3 = "Data\\ExtraDialogue:Mines_PlayerKilled_Vika";
                        break;
                }
                if (Game1.random.NextDouble() < 0.1 && Game1.player.spouse != null && !Game1.player.isEngaged() && Game1.player.spouse.Length > 1)
                {
                    rescuer3 = Game1.player.spouse;
                    uniquemessage3 = (Game1.player.IsMale ? "Data\\ExtraDialogue:Mines_PlayerKilled_Spouse_PlayerMale" : "Data\\ExtraDialogue:Mines_PlayerKilled_Spouse_PlayerFemale");
                }
                Game1.currentLocation.currentEvent = new Event(Game1.content.LoadString("Data\\Events\\Custom_SouthRestStop:PlayerKilled", rescuer3, uniquemessage3, Game1.player.Name));
            }
            return;



        }
    }
}
*/

