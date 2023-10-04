

using System;
using Microsoft.Xna.Framework;
using StardewValley;
using xTile;
using StardewModdingAPI;
using xTile.ObjectModel;
using StardewValley.Objects;
using Object = StardewValley.Object;
using System.Xml.Serialization;
using StardewValley.Network;
using Netcode;
using xTile.Dimensions;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Microsoft.Xna.Framework.Graphics;
using xTile.Layers;
using StardewValley.BellsAndWhistles;
using Microsoft.Xna.Framework.Audio;

namespace RestStopLocations.Game.Locations.DungeonLevelGenerators
{
    public class SubwayDungeonLevelGenerator : BaseDungeonLevelGenerator
    {
        private static IMonitor Monitor { get; set; }
        private static IModHelper Helper { get; set; }

        IModHelper helper;

        public SubwayDungeonLevelGenerator() : base()
        {
           


        }

        public override void Generate(HellDungeon location, ref Vector2 warpFromPrev, ref Vector2 warpFromNext)
        {
            Random rand = new Random(location.genSeed.Value);
            //location.isIndoorLevel = true;

            var caveMap = Game1.game1.xTileContent.Load<Map>(Mod.instance.Helper.ModContent.GetInternalAssetName("assets/maps/HellDungeonSubway.tmx").BaseName);

            int x = (location.Map.Layers[0].LayerWidth - caveMap.Layers[0].LayerWidth) / 2;
            int y = (location.Map.Layers[0].LayerHeight - caveMap.Layers[0].LayerHeight) / 2;

            location.ApplyMapOverride(caveMap, "actual_map", null, new Rectangle(x, y, caveMap.Layers[0].LayerWidth, caveMap.Layers[0].LayerHeight));

            warpFromPrev = new Vector2(x + 24, y + 36);
            //location.warps.Add(new Warp(x + 6, y + 11, "Custom_HellDungeon" + location.level.Value / 100, 1, location.level.Value % 100, false));
            PlaceNextWarp(location, 33, 4);

            /*

            {
                Vector2 position = new Vector2(x + 12, y + 3);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)773", 5)); // Life Elixir
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }


            {
                Vector2 objectPos = new Vector2(x + 48, y + 19);
                Object o = new Object(objectPos, "166"); //Treasure Chest
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }
            {
                Vector2 objectPos = new Vector2(x + 49, y + 19);
                Object o = new Object(objectPos, "166"); //Treasure Chest
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }
            {
                Vector2 objectPos = new Vector2(x + 50, y + 19);
                Object o = new Object(objectPos, "166"); //Treasure Chest
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }
            {
                Vector2 objectPos = new Vector2(x + 51, y + 19);
                Object o = new Object(objectPos, "166"); //Treasure Chest
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }
            {
                Vector2 objectPos = new Vector2(x + 52, y + 19);
                Object o = new Object(objectPos, "166"); //Treasure Chest
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }
            {
                Vector2 objectPos = new Vector2(x + 53, y + 19);
                Object o = new Object(objectPos, "166"); //Treasure Chest
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }

            {
                int silverkeyint = 99;// ExternalAPIs.JA.GetObjectId("Silver Key");
                string silverkey = Convert.ToString(silverkeyint);
                Vector2 position = new Vector2(x + 75, y + 80);
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
                PlaceMonsterAt(location, rand, x + 43, y + 54);
                PlaceMonsterAt(location, rand, x + 41, y + 56);
                PlaceMonsterAt(location, rand, x + 39, y + 55);
                PlaceMonsterAt(location, rand, x + 36, y + 54);

                PlaceMonsterAt(location, rand, x + 51, y + 51);
                PlaceMonsterAt(location, rand, x + 52, y + 54);
                PlaceMonsterAt(location, rand, x + 54, y + 57);
                PlaceMonsterAt(location, rand, x + 53, y + 60);
                PlaceMonsterAt(location, rand, x + 57, y + 58);
                PlaceMonsterAt(location, rand, x + 52, y + 60);
                PlaceMonsterAt(location, rand, x + 53, y + 58);

                PlaceMonsterAt(location, rand, x + 1, y + 46);
                PlaceMonsterAt(location, rand, x + 1, y + 53);
                PlaceMonsterAt(location, rand, x + 1, y + 56);

                PlaceMonsterAt(location, rand, x + 50, y + 57);
                PlaceMonsterAt(location, rand, x + 50, y + 41);
                PlaceMonsterAt(location, rand, x + 57, y + 51);
                PlaceMonsterAt(location, rand, x + 45, y + 51);

                PlaceMonsterAt(location, rand, x + 65, y + 37);
                PlaceMonsterAt(location, rand, x + 65, y + 35);

                PlaceMonsterAt(location, rand, x + 80, y + 60);
                PlaceMonsterAt(location, rand, x + 79, y + 62);

                PlaceMonsterAt(location, rand, x + 24, y + 95);
                PlaceMonsterAt(location, rand, x + 22, y + 90);
                PlaceMonsterAt(location, rand, x + 20, y + 92);
                PlaceMonsterAt(location, rand, x + 46, y + 8);
                PlaceMonsterAt(location, rand, x + 45, y + 7);
                PlaceMonsterAt(location, rand, x + 50, y + 20);
            }

            {
                   //MachineGun Monsters
                PlaceMonster(location, 10, x + 53, y + 88);
                PlaceMonster(location, 10, x + 55, y + 88);
                PlaceMonster(location, 10, x + 49, y + 88);

                PlaceMonster(location, 10, x + 50, y + 96);
                PlaceMonster(location, 10, x + 52, y + 96);
                PlaceMonster(location, 8, x + 54, y + 96);

                PlaceMonster(location, 8, x + 96, y + 20);
                PlaceMonster(location, 8, x + 97, y + 21);
                PlaceMonster(location, 8, x + 98, y + 22);
                PlaceMonster(location, 8, x + 95, y + 98);

                PlaceMonster(location, 10, x + 49, y + 83);
                PlaceMonster(location, 10, x + 51, y + 83);
                PlaceMonster(location, 10, x + 53, y + 83);
                PlaceMonster(location, 10, x + 55, y + 83);
                PlaceMonster(location, 10, x + 56, y + 86);
                PlaceMonster(location, 10, x + 47, y + 83);
                PlaceMonster(location, 10, x + 47, y + 81);
                PlaceMonster(location, 10, x + 47, y + 86);
                PlaceMonster(location, 10, x + 49, y + 86);
                PlaceMonster(location, 10, x + 51, y + 86);
                PlaceMonster(location, 10, x + 53, y + 86);

                PlaceMonster(location, 11, x + 48, y + 85);
                PlaceMonster(location, 11, x + 50, y + 85);
                PlaceMonster(location, 11, x + 52, y + 85);
                PlaceMonster(location, 11, x + 54, y + 85);

                PlaceMonster(location, 11, x + 50, y + 89);
                PlaceMonster(location, 11, x + 52, y + 89);
                PlaceMonster(location, 11, x + 54, y + 89);
                PlaceMonster(location, 11, x + 56, y + 89);

            



            }
            {
                PlaceBreakableAt(location, rand, x + 3, y + 30);
                PlaceBreakableAt(location, rand, x + 3, y + 31);
                PlaceBreakableAt(location, rand, x + 3, y + 32);

            }
            */
        }
           



	}
}
















