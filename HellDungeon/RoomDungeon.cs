

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;
using xTile;
using StardewValley.Locations;
using RestStopLocations.Game.Locations;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics;
using RestStopLocations.Game.Locations.DungeonLevelGenerators;
using Netcode;
using StardewModdingAPI.Utilities;
using StardewValley.Network;
using StardewValley.Objects;
using StardewValley.Tools;
using StardewModdingAPI;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardewValley.TerrainFeatures;
using xTile.Layers;
using xTile.ObjectModel;
using xTile.Tiles;
using StardewModdingAPI.Events;
using SpaceCore.Events;
using Object = StardewValley.Object;
using UtilitiesStuff;



namespace RestStopLocations.Game.Locations.DungeonLevelGenerators
{
    public class RoomDungeonLevelGenerator : BaseDungeonLevelGenerator
    {
        public override void Generate(HellDungeon location, ref Vector2 warpFromPrev, ref Vector2 warpFromNext)
        {
            Random rand = new Random(location.genSeed.Value);
            location.isIndoorLevel = true;

            var caveMap = Game1.game1.xTileContent.Load<Map>(Mod.instance.Helper.ModContent.GetInternalAssetName("assets/maps/HellDungeonRoom.tmx").BaseName);

            int x = (location.Map.Layers[0].LayerWidth - caveMap.Layers[0].LayerWidth) / 2;
            int y = (location.Map.Layers[0].LayerHeight - caveMap.Layers[0].LayerHeight) / 2;

            location.ApplyMapOverride(caveMap, "actual_map", null, new Rectangle(x, y, caveMap.Layers[0].LayerWidth, caveMap.Layers[0].LayerHeight));

            for (int ix = x + 4; ix <= x + 14; ++ix)
            {
                for (int iy = y + 4; iy <= y + 8; ++iy)
                {
                    if (rand.NextDouble() < 0.175)
                        PlaceBreakableAt(location, rand, ix, iy);
                }
            }

            PlaceChestAt(location, rand, x + caveMap.Layers[0].LayerWidth / 2 - 1, y + caveMap.Layers[0].LayerHeight / 2, rand.Next(3) == 0);

            warpFromPrev = new Vector2(x + 33, y + 33);
            //location.warps.Add(new Warp(x + 9, y + 11, "Custom_HellDungeon" + location.level.Value / 100, 1, location.level.Value % 100, false));
            PlaceNextWarp(location, 78, 25);


            //PlaceMinable(location, 0, x + 24, y + 30);
            //PlaceMinable(location, 1, x + 24, y + 31);
            //PlaceMinable(location, 2, x + 24, y + 32);
           // PlaceMinable(location, 3, x + 24, y + 33);
           // PlaceMinable(location, 4, x + 24, y + 34);
           // PlaceMinable(location, 5, x + 24, y + 35);
           // PlaceMinable(location, 6, x + 24, y + 36);
           // PlaceMinable(location, 7, x + 24, y + 37);
           // PlaceMinable(location, 8, x + 24, y + 38);
           // PlaceMinable(location, 9, x + 24, y + 39);
           // PlaceMinable(location, 10, x + 24, y + 40);
           // PlaceMinable(location, 11, x + 24, y + 41);
           // PlaceMinable(location, 12, x + 24, y + 42);
            //PlaceMinable(location, 13, x + 24, y + 43);
            //PlaceMinable(location, 14, x + 24, y + 44);
            //PlaceMinable(location, 15, x + 24, y + 45);
           // PlaceMinable(location, 44, x + 24, y + 46);
            //PlaceMinable(location, 0, x + 24, y + 47);
            //PlaceMinable(location, 0, x + 24, y + 48);


            
                {
                    PlaceMonsterAt(location, rand, x + 4, y + 6);

                }


            {
                Vector2 position = new Vector2(x + 6, y + 51);
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
                Vector2 position = new Vector2(x + 14, y + 58);
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
                Vector2 position = new Vector2(x + 19, y + 12);
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
                Vector2 position = new Vector2(x + 49, y + 15);
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
                Vector2 position = new Vector2(x + 50, y + 15);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)72", 7));
                Game1.player.Money += 200;
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 44, y + 33);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)72", 7));
                Game1.player.Money += 200;
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 45, y + 33);
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
                Vector2 position = new Vector2(x + 46, y + 33);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)287", 7));
                Game1.player.Money += 200;
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }


            PlaceMonsterAt(location, rand, x + 2, y + 35);
            PlaceMonsterAt(location, rand, x + 3, y + 37);
            PlaceMonsterAt(location, rand, x + 4, y + 39);
            PlaceMonsterAt(location, rand, x + 6, y + 41);
            PlaceMonsterAt(location, rand, x + 3, y + 43);
            PlaceMonsterAt(location, rand, x + 4, y + 58);
            PlaceMonsterAt(location, rand, x + 8, y + 58);
            PlaceMonsterAt(location, rand, x + 13, y + 44);
            PlaceMonsterAt(location, rand, x + 19, y + 49);
            PlaceMonsterAt(location, rand, x + 17, y + 47);
            PlaceMonsterAt(location, rand, x + 19, y + 47);
            PlaceMonsterAt(location, rand, x + 19, y + 44);
            PlaceMonsterAt(location, rand, x + 27, y + 48);
            PlaceMonsterAt(location, rand, x + 49, y + 4);
            PlaceMonsterAt(location, rand, x + 51, y + 5);
            PlaceMonsterAt(location, rand, x + 53, y + 5);
            PlaceMonsterAt(location, rand, x + 51, y + 4);
            PlaceMonsterAt(location, rand, x + 55, y + 6);
            PlaceMonsterAt(location, rand, x + 54, y + 4);
            PlaceMonsterAt(location, rand, x + 55, y + 4);
        }

        public override void checkForMusic(GameTime time)
        {
            {
                Game1.changeMusicTrack("SongLost", track_interruptable: false);
            }

        }









    }





} 






      