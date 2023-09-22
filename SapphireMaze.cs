
/*
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

using Object = StardewValley.Object;

namespace RestStopLocations.Game.Locations.DungeonLevelGenerators
{
    public class MazeLevelGenerator : SapphireBaseLevelGenerator
    {

        private static IMonitor Monitor { get; set; }
        private static IModHelper Helper { get; set; }

        IModHelper helper;

     

        public override void Generate(SapphireVolcano location, ref Vector2 warpFromPrev, ref Vector2 warpFromNext)
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
                chest.addItem(new Object(287, 5));
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
                chest.addItem(new Object(287, 5));
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
                chest.addItem(new Object(287, 5));
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
                chest.addItem(new Object(287, 5));
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
                chest.addItem(new Object(287, 5));
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
                chest.addItem(new Object(287, 5));
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
                chest.addItem(new Object(773, 5));
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
                chest.addItem(new Object(287, 5));
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
                chest.addItem(new Object(773, 10));
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
                chest.addItem(new Object(72, 10));
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
                chest.addItem(new Object(773, 5));
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
                chest.addItem(new Object(773, 5));
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
                chest.addItem(new Object(220, 10));
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
                chest.addItem(new Object(222, 10));
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
                chest.addItem(new Object(386, 50));
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
                chest.addItem(new Object(275, 5));
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
                chest.addItem(new Object(394, 11));
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
                chest.addItem(new Object(253, 7));
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
                chest.addItem(new Object(394, 11));
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
                chest.addItem(new Object(414, 11));
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
                chest.addItem(new Object(403, 11));
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
                chest.addItem(new Object(403, 11));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x +11, y + 64);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(new Object(403, 11));
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
                chest.addItem(new Object(386, 69));
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
                chest.addItem(new MeleeWeapon(34, 1));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }

            /*
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
                Vector2 objectPos = new Vector2(x + 4, y + 38);
                Object o = new Object(166, 1);
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);

            }
            {
                Vector2 objectPos = new Vector2(x + 5, y + 38);
                Object o = new Object(166, 1);
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }
            {
                Vector2 objectPos = new Vector2(x + 29, y + 97);
                Object o = new Object(166, 1);
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }
            {
                Vector2 objectPos = new Vector2(x + 30, y + 97);
                Object o = new Object(166, 1);
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }
            {
                Vector2 objectPos = new Vector2(x + 31, y + 97);
                Object o = new Object(166, 1);
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }

            {
                Vector2 objectPos = new Vector2(x + 32, y + 97);
                Object o = new Object(166, 1);
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }
            {
                Vector2 objectPos = new Vector2(x + 33, y + 97);
                Object o = new Object(166, 1);
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }
            {
                Vector2 objectPos = new Vector2(x + 34, y + 97);
                Object o = new Object(166, 1);
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }
            {
                Vector2 objectPos = new Vector2(x + 31, y + 43);
                Object o = new Object(216, 1); //Bread
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }
            {
                Vector2 objectPos = new Vector2(x + 37, y + 43);
                Object o = new Object(216, 1); //Bread
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }
            {
                Vector2 objectPos = new Vector2(x + 42, y + 43);
                Object o = new Object(216, 1); //Bread
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }
            {
                Vector2 objectPos = new Vector2(x + 42, y + 56);
                Object o = new Object(773, 1); //Life Elixer
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }



        }





    }







}



*/
