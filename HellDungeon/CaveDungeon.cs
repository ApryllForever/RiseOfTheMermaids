

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;
using xTile;

using StardewValley.Monsters;
using StardewModdingAPI;
using StardewModdingAPI.Enums;
using StardewModdingAPI.Events;

using SpaceShared;

using xTile.Tiles;
using StardewValley.Objects;

using Object = StardewValley.Object;

using StardewValley.Tools;




namespace RestStopLocations.Game.Locations.DungeonLevelGenerators
{
    public class CaveDungeonLevelGenerator : BaseDungeonLevelGenerator
    {
        private static IMonitor Monitor { get; set; }
        private static IModHelper Helper { get; set; }

        IModHelper helper;



        public override void Generate(HellDungeon location, ref Vector2 warpFromPrev, ref Vector2 warpFromNext)
        {
            Random rand = new Random(location.genSeed.Value);
            location.isIndoorLevel = true;

            var caveMap = Game1.game1.xTileContent.Load<Map>(Mod.instance.Helper.ModContent.GetInternalAssetName("assets/maps/HellDungeonCave.tmx").BaseName);

            int x = (location.Map.Layers[0].LayerWidth - caveMap.Layers[0].LayerWidth) / 2;
            int y = (location.Map.Layers[0].LayerHeight - caveMap.Layers[0].LayerHeight) / 2;

            location.ApplyMapOverride(caveMap, "actual_map", null, new Rectangle(x, y, caveMap.Layers[0].LayerWidth, caveMap.Layers[0].LayerHeight));

            warpFromPrev = new Vector2(x + 27, y + 50);
            //location.warps.Add(new Warp(x + 6, y + 11, "Custom_HellDungeon" + location.level.Value / 100, 1, location.level.Value % 100, false));
            PlaceNextWarp(location, 55, 37);

            


            {
                Vector2 position = new Vector2(x + 1, y + 3);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)441", 25));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 6, y + 3);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)441",25));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 10, y + 3);
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
                Vector2 position = new Vector2(x + 12, y + 3);
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
                Vector2 position = new Vector2(x + 21, y + 3);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)288", 5));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 4, y + 14);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)441", 25));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 1, y + 30);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)441", 25));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }


            {
                Vector2 position = new Vector2(x + 1, y + 31);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(new Ring("863"));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }


            {
                Vector2 position = new Vector2(x + 23, y + 51);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)604", 25));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 36, y + 51);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)253", 10));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }

            

            {
                Vector2 objectPos = new Vector2(x + 13, y + 3);
                location.Objects.Add(objectPos, new Object("166", 1) //Treasure Chest
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }

            {
                Vector2 objectPos = new Vector2(x + 1, y + 23);
                location.Objects.Add(objectPos, new Object("336", 1)
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }

            {
                Vector2 objectPos = new Vector2(x + 4, y + 40);
                location.Objects.Add(objectPos, new Object("348", 1)
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }

            {
                Vector2 objectPos = new Vector2(x + 5, y + 40);
                location.Objects.Add(objectPos, new Object("216", 1)
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }

            {
                Vector2 objectPos = new Vector2(x + 54, y + 40);
                location.Objects.Add(objectPos, new Object("348", 1)
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }

            {
                Vector2 objectPos = new Vector2(x + 55, y + 40);
                location.Objects.Add(objectPos, new Object("216", 1)
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }

            
            {
                PlaceMonsterAt(location, rand, x + 10, y + 40);
                PlaceMonsterAt(location, rand, x + 16, y + 41);
                PlaceMonsterAt(location, rand, x + 14, y + 41);
                PlaceMonsterAt(location, rand, x + 17, y + 42);
                PlaceMonsterAt(location, rand, x + 42, y + 40);
                PlaceMonsterAt(location, rand, x + 40, y + 42);
                PlaceMonsterAt(location, rand, x + 47, y + 41);
                PlaceMonsterAt(location, rand, x + 45, y + 41);
                PlaceMonsterAt(location, rand, x + 25, y + 41);
                PlaceMonsterAt(location, rand, x + 2, y + 41);
                PlaceMonsterAt(location, rand, x + 9, y + 58);
                PlaceMonsterAt(location, rand, x + 5, y + 46);
                PlaceMonsterAt(location, rand, x + 1, y + 53);
                PlaceMonsterAt(location, rand, x + 1, y + 56);
                PlaceMonsterAt(location, rand, x + 50, y + 57);
                PlaceMonsterAt(location, rand, x + 50, y + 41);
                PlaceMonsterAt(location, rand, x + 57, y + 51);
                PlaceMonsterAt(location, rand, x + 45, y + 51);
                PlaceMonsterAt(location, rand, x + 58, y + 48);
                PlaceMonsterAt(location, rand, x + 58, y + 48);
                PlaceMonsterAt(location, rand, x + 10, y + 12);
                PlaceMonsterAt(location, rand, x + 13, y + 24);
                PlaceMonsterAt(location, rand, x + 16, y + 26);
                PlaceMonsterAt(location, rand, x + 18, y + 28);
                PlaceMonsterAt(location, rand, x + 20, y + 30);
                PlaceMonsterAt(location, rand, x + 47, y + 11);
                PlaceMonsterAt(location, rand, x + 47, y + 13);
                PlaceMonsterAt(location, rand, x + 47, y + 15);
            }

            {
                PlaceMonster(location, 8, x + 7, y + 16);
                PlaceMonster(location, 8, x + 7, y + 23);
                PlaceMonster(location, 8, x + 7, y + 31);
                PlaceMonster(location, 8, x + 25, y + 3);
                PlaceMonster(location, 8, x + 27, y + 3);
                PlaceMonster(location, 8, x + 29, y + 3);
                PlaceMonster(location, 8, x + 39, y + 12);
                PlaceMonster(location, 8, x + 39, y + 14);
                PlaceMonster(location, 8, x + 39, y + 16);
                PlaceMonster(location, 8, x + 57, y + 31);
            }
            {
                PlaceBreakableAt(location, rand, x + 3, y + 30);
                PlaceBreakableAt(location, rand, x + 3, y + 31);
                PlaceBreakableAt(location, rand, x + 3, y + 32);

            } 

        }

        public override void performTouchAction(string actionStr, Vector2 tileLocation)
        {
            string[] split = actionStr.Split(' ');
            string action = split[0];
            int tx = (int)tileLocation.X;
            int ty = (int)tileLocation.Y;


            base.performTouchAction(actionStr, tileLocation);
        }

    }








}


