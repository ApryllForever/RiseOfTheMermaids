

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

using Object = StardewValley.Object;

namespace RestStopLocations.Game.Locations.DungeonLevelGenerators
{
    public class BeltDungeonLevelGenerator : BaseDungeonLevelGenerator
    {

        private static IMonitor Monitor { get; set; }
        private static IModHelper Helper { get; set; }

        IModHelper helper;

     

        public override void Generate(HellDungeon location, ref Vector2 warpFromPrev, ref Vector2 warpFromNext)
        { 

            Random rand = new Random(location.genSeed.Value);

            location.isIndoorLevel = false;


            var BeltMap = Game1.game1.xTileContent.Load<Map>(Mod.instance.Helper.ModContent.GetInternalAssetName("assets/maps/HellDungeonBelt.tmx").BaseName);


            int x = (location.Map.Layers[0].LayerWidth - BeltMap.Layers[0].LayerWidth) / 2;
            int y = (location.Map.Layers[0].LayerHeight - BeltMap.Layers[0].LayerHeight) / 2;

            location.ApplyMapOverride(BeltMap, "actual_map", null, new Rectangle(x, y, BeltMap.Layers[0].LayerWidth, BeltMap.Layers[0].LayerHeight));



            warpFromPrev = new Vector2(x + 2, y + 5);

            warpFromNext = new Vector2(x + 3, y + 4);

            //location.warps.Add(new Warp(x + 6, y + 21, "Custom_HellDungeon" + location.level.Value / 100, 1, location.level.Value % 100, false));
            PlaceNextWarp(location, 78, 78);



            {
                Vector2 position = new Vector2(x + 56, y + 6);
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
                Vector2 position = new Vector2(x + 29, y + 34);
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
                Vector2 position = new Vector2(x + 19, y + 34);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 7, y + 32);
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
                Vector2 position = new Vector2(x + 7, y + 40);
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
                Vector2 position = new Vector2(x + 53, y + 42);
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
                Vector2 position = new Vector2(x + 51, y + 42);
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
                Vector2 position = new Vector2(x + 49, y + 42);
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
                Vector2 position = new Vector2(x + 54, y + 48);
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
                Vector2 position = new Vector2(x + 14, y + 35);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)773", 5));    
                Game1.player.Money += 200;
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 14, y + 37);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)773", 5));
                Game1.player.Money += 200;
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }

            {
             PlaceMonsterAt(location, rand, x + 4, y + 6);
             PlaceMonsterAt(location, rand, x + 6, y + 6);
             PlaceMonsterAt(location, rand, x + 14, y + 4);
             PlaceMonsterAt(location, rand, x + 8, y + 5);
             PlaceMonsterAt(location, rand, x + 54, y + 9);
             PlaceMonsterAt(location, rand, x + 54, y + 7);
                PlaceMonsterAt(location, rand, x + 52, y + 7);
                PlaceMonsterAt(location, rand, x + 55, y + 8);
                PlaceMonsterAt(location, rand, x + 8, y + 6);
             PlaceMonsterAt(location, rand, x + 4, y + 4);
                PlaceMonsterAt(location, rand, x + 57, y + 30);
                PlaceMonsterAt(location, rand, x + 55, y + 30);
                PlaceMonsterAt(location, rand, x + 24, y + 56);
                PlaceMonsterAt(location, rand, x + 1, y + 56);
                PlaceMonsterAt(location, rand, x + 5, y + 56);
                PlaceMonsterAt(location, rand, x + 3, y + 53);
                PlaceMonsterAt(location, rand, x + 1, y + 53);
                PlaceMonsterAt(location, rand, x + 1, y + 50);
                PlaceMonsterAt(location, rand, x + 5, y + 48);
                PlaceMonsterAt(location, rand, x + 1, y + 48);
            }

            {
                PlaceMonster(location, 8, x + 24, y + 26);
                PlaceMonster(location, 8, x + 24, y + 56);
                PlaceMonster(location, 8, x + 20, y + 25);
                PlaceMonster(location, 5, x + 23, y + 27);
                PlaceMonster(location, 5, x + 21, y + 28);
                PlaceMonster(location, 5, x + 26, y + 25);
                PlaceMonster(location, 8, x + 52, y + 48);
                PlaceMonster(location, 8, x + 50, y + 48);
                PlaceMonster(location, 8, x + 50, y + 46);
                PlaceMonster(location, 8, x + 52, y + 50);
            }

            {
                Vector2 objectPos = new Vector2(x + 6, y + 41);
                Object o = new Object(objectPos, "(O)166");
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);

            }
            {
                Vector2 objectPos = new Vector2(x + 6, y + 31);
                Object o = new Object(objectPos, "(O)166");
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }
            {
                Vector2 objectPos = new Vector2(x + 9, y + 48);
                Object o = new Object(objectPos, "(O)166");
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }
            {
                Vector2 objectPos = new Vector2(x + 9, y + 53);
                Object o = new Object(objectPos, "(O)166");
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }
            {
                Vector2 objectPos = new Vector2(x + 18, y + 53);
                Object o = new Object(objectPos, "(O)166");
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }

            {
                Vector2 objectPos = new Vector2(x + 18, y + 48);
                Object o = new Object(objectPos, "(O)166");
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }
            {
                Vector2 objectPos = new Vector2(x + 31, y + 43);
                Object o = new Object(objectPos, "(O)216");
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }
            {
                Vector2 objectPos = new Vector2(x + 37, y + 43);
                Object o = new Object(objectPos, "(O)216"); //Bread
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }
            {
                Vector2 objectPos = new Vector2(x + 42, y + 43);
                Object o = new Object(objectPos, "(O)216");
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }
            {
                Vector2 objectPos = new Vector2(x + 42, y + 56);
                Object o = new Object(objectPos, "(O)773"); //Life Elixer
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }


            /*
            {
                location.characters.Add(new ShadowBrute(new Vector2(x + 10, y + 35)));
                location.characters.Add(new ShadowBrute(new Vector2(x + 10, y + 37)));
                location.characters.Add(new ShadowShaman(new Vector2(x + 47, y + 50)));
                location.characters.Add(new ShadowShaman(new Vector2(x + 50, y + 48)));
                location.characters.Add(new ShadowShaman(new Vector2(x + 50, y + 50)));
                location.characters.Add(new ShadowBrute(new Vector2(x + 52, y + 48)));
                location.characters.Add(new ShadowBrute(new Vector2(x + 52, y + 50)));
                location.characters.Add(new ShadowBrute(new Vector2(x + 54, y + 50)));
                location.characters.Add(new ShadowBrute(new Vector2(x + 53, y + 46)));
                location.characters.Add(new ShadowBrute(new Vector2(x + 53, y + 44)));
                location.characters.Add(new ShadowBrute(new Vector2(x + 49, y + 44)));
                location.characters.Add(new ShadowBrute(new Vector2(x + 49, y + 57)));
                location.characters.Add(new ShadowBrute(new Vector2(x + 52, y + 57)));
                location.characters.Add(new HellBat(new Vector2(x + 49, y + 54)));
                location.characters.Add(new HellBat(new Vector2(x + 49, y + 54)));
                location.characters.Add(new Ghost(new Vector2(x + 25, y + 6)));
            } */
        }

        public override void checkForMusic(GameTime time)
        {
                {
                    Game1.changeMusicTrack("bigDrums", track_interruptable: false);
                }
            
        }
    }

}




