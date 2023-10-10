

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SpaceShared;
using StardewValley;
using xTile;
using xTile.Tiles;
using StardewModdingAPI;
using StardewValley.Objects;
using StardewValley.Monsters;
using StardewModdingAPI.Enums;
using StardewModdingAPI.Events;
using StardewValley.Tools;
//using JsonAssets;

using Object = StardewValley.Object;

namespace RestStopLocations.Game.Locations.DungeonLevelGenerators
{
    public class VineDungeonLevelGenerator : BaseDungeonLevelGenerator
    {

        private static IMonitor Monitor { get; set; }
        private static IModHelper Helper { get; set; }

        IModHelper helper;



        public override void Generate(HellDungeon location, ref Vector2 warpFromPrev, ref Vector2 warpFromNext)
        {

            Random rand = new Random(location.genSeed.Value);

            location.isIndoorLevel = false;


            var BeltMap = Game1.game1.xTileContent.Load<Map>(Mod.instance.Helper.ModContent.GetInternalAssetName("assets/maps/HellDungeonVines.tmx").BaseName);


            int x = (location.Map.Layers[0].LayerWidth - BeltMap.Layers[0].LayerWidth) / 2;
            int y = (location.Map.Layers[0].LayerHeight - BeltMap.Layers[0].LayerHeight) / 2;

            location.ApplyMapOverride(BeltMap, "actual_map", null, new Rectangle(x, y, BeltMap.Layers[0].LayerWidth, BeltMap.Layers[0].LayerHeight));



            warpFromPrev = new Vector2(x + 3, y + 5);

            warpFromNext = new Vector2(x + 3, y + 4);

            //location.warps.Add(new Warp(x + 6, y + 21, "Custom_HellDungeon" + location.level.Value / 100, 1, location.level.Value % 100, false));
            PlaceNextWarp(location, 98, 98);



            {
                Vector2 position = new Vector2(x + 29, y + 97);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)287", 5));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 30, y + 97);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)287", 5));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 31, y + 97);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)287", 5));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 32, y + 97);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)287", 5));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 33, y + 97);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)287", 5));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 34, y + 97);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)287", 5));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 4, y + 38);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)773", 5));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 5, y + 38);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)287", 5));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }

            {
                Vector2 position = new Vector2(x + 42, y + 97);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)773", 10));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 43, y + 97);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)72", 10));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 44, y + 97);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)773", 5));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 45, y + 97);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)773", 5));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 46, y + 97);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)220", 10));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 47, y + 97);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)222", 10));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 48, y + 97);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)386", 50));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 49, y + 97);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)275", 50));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 50, y + 97);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)394", 11));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 2, y + 20);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)253", 7));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 3, y + 20);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)394", 11));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 4, y + 20);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)414", 11));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 5, y + 20);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)403", 11));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 10, y + 64);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)403", 11));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 11, y + 64);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)403", 11));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 61, y + 26);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)386", 69));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }

            {
                Vector2 position = new Vector2(x + 60, y + 26);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(new MeleeWeapon("34"));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                int silverkeyint = 99;//ExternalAPIs.JA.GetObjectId("Silver Key");
                string silverkey = Convert.ToString(silverkeyint);
                Vector2 position = new Vector2(x + 85, y + 20);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(new Object(silverkey, 1));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                int goldkeyint = 98;//ExternalAPIs.JA.GetObjectId("Gold Key");
                string goldkey = Convert.ToString(goldkeyint);
                Vector2 position = new Vector2(x + 75, y + 8);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(new Object(goldkey, 1));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                int iridiumkeyint = 97;//ExternalAPIs.JA.GetObjectId("Iridium Key");
                string iridiumkey = Convert.ToString(iridiumkeyint);
                Vector2 position = new Vector2(x + 84, y + 58);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(new Object(iridiumkey, 1));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                int diamondkeyint = 96; // ExternalAPIs.JA.GetObjectId("Diamond Key");
                string diamondkey = Convert.ToString(diamondkeyint);
                Vector2 position = new Vector2(x + 36, y + 39);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(new Object(diamondkey, 1));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                int heartkeyint = 95; // ExternalAPIs.JA.GetObjectId("Heart Key");
                string heartkey = Convert.ToString(heartkeyint);
                Vector2 position = new Vector2(x + 29, y + 4);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(new Object(heartkey, 1));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 69, y + 53);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(new Slingshot("34"));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 67, y + 23);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(new MeleeWeapon("14"));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }




            {
                PlaceMonster(location, 9, x + 85, y + 41);
                PlaceMonster(location, 1, x + 15, y + 16);
                PlaceMonster(location, 1, x + 85, y + 23);
                PlaceMonster(location, 1, x + 78, y + 7);
                PlaceMonster(location, 1, x + 95, y + 5);
                PlaceMonster(location, 2, x + 16, y + 16);
                PlaceMonster(location, 2, x + 93, y + 5);
                PlaceMonster(location, 2, x + 74, y + 13);
                PlaceMonster(location, 2, x + 74, y + 21);
                PlaceMonster(location, 2, x + 75, y + 21);
                PlaceMonster(location, 2, x + 74, y + 22);
                PlaceMonster(location, 1, x + 17, y + 17);
                PlaceMonster(location, 2, x + 18, y + 19);
                PlaceMonster(location, 2, x + 16, y + 26);
                PlaceMonster(location, 2, x + 29, y + 30);
                PlaceMonster(location, 1, x + 47, y + 49);
                PlaceMonster(location, 1, x + 49, y + 57);
                PlaceMonster(location, 1, x + 57, y + 36);
                PlaceMonster(location, 1, x + 52, y + 69);
                PlaceMonster(location, 1, x + 69, y + 83);
                PlaceMonster(location, 1, x + 63, y + 91);

                //Dark Zombie
                PlaceMonster(location, 13, x + 5, y + 27);
                PlaceMonster(location, 13, x + 5, y + 25);
                PlaceMonster(location, 13, x + 5, y + 40);
                PlaceMonster(location, 13, x + 17, y + 98);
                PlaceMonster(location, 13, x + 69, y + 72);
                PlaceMonster(location, 13, x + 75, y + 41);
                PlaceMonster(location, 13, x + 17, y + 98);
                PlaceMonster(location, 13, x + 84, y + 11);
                PlaceMonster(location, 13, x + 85, y + 9);
                PlaceMonster(location, 13, x + 87, y + 8);
                PlaceMonster(location, 13, x + 89, y + 9);
                PlaceMonster(location, 13, x + 79, y + 8);
                PlaceMonster(location, 13, x + 76, y + 12);

                //Green Zombie
                PlaceMonster(location, 14, x + 43, y + 25);
                PlaceMonster(location, 14, x + 46, y + 26);
                PlaceMonster(location, 14, x + 49, y + 25);
                PlaceMonster(location, 14, x + 44, y + 20);
                PlaceMonster(location, 14, x + 49, y + 17);
                PlaceMonster(location, 14, x + 48, y + 7);
                PlaceMonster(location, 14, x + 45, y + 5);
                PlaceMonster(location, 14, x + 55, y + 10);
                PlaceMonster(location, 14, x + 57, y + 14);
                PlaceMonster(location, 14, x + 59, y + 12);
                PlaceMonster(location, 14, x + 60, y + 18);
                PlaceMonster(location, 14, x + 63, y + 15);
                PlaceMonster(location, 14, x + 65, y + 13);
                PlaceMonster(location, 14, x + 62, y + 6);
                PlaceMonster(location, 14, x + 66, y + 17);
                PlaceMonster(location, 14, x + 57, y + 22);
                PlaceMonster(location, 14, x + 66, y + 23);
                PlaceMonster(location, 14, x + 55, y + 29);
                PlaceMonster(location, 14, x + 29, y + 79);
                PlaceMonster(location, 14, x + 36, y + 79);


            }

        }



        public override void checkForMusic(GameTime time)
        {
            //base.checkForMusic(time);

                    if (Game1.currentSong == null ) 
                {
                    Game1.changeMusicTrack("desolate", track_interruptable: false);
                }   
        }
        public override void performTenMinuteUpdate(int timeOfDay)
        {
            base.performTenMinuteUpdate(timeOfDay);
           
        }
    }
}




