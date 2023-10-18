

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
using StardewValley.Monsters;

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

        [XmlElement("addedSubwayHellSlimesToday")]
        private readonly NetBool addedSubwayHellSlimesToday = new NetBool();

        protected override void resetSharedState()
        {
            base.resetSharedState();
            if ((bool)this.addedSubwayHellSlimesToday)
            {
                return;
            }
            this.addedSubwayHellSlimesToday.Value = true;
            Random rand;
            rand = Utility.CreateRandom(Game1.stats.DaysPlayed, Game1.uniqueIDForThisGame, 12.0);
            Microsoft.Xna.Framework.Rectangle spawnArea;
            spawnArea = new Microsoft.Xna.Framework.Rectangle(67, 83, 15, 8);
            for (int tries = 60; tries > 0; tries--)
            {
                Vector2 tile;
                tile = Utility.getRandomPositionInThisRectangle(spawnArea, rand);
                if (this.CanItemBePlacedHere(tile))
                {
                    GreenSlime i;
                    i = new GreenSlime(tile * 64f, 0);
                    i.makeTigerSlime();
                    base.characters.Add(i);
                }
            }
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
            }*/

            {
               
                Vector2 position = new Vector2(x + 54, y + 4);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(new Object("ApryllForever.RiseMermaids_SilverKey", 1));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }

            {

                Vector2 position = new Vector2(x + 61, y + 77);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(new Object("ApryllForever.RiseMermaids_GoldKey", 1));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 objectPos = new Vector2(x + 58, y + 72);
                location.Objects.Add(objectPos, new Object("773", 1) //Lifeelixire
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }
            {
                Vector2 objectPos = new Vector2(x + 57, y + 72);
                location.Objects.Add(objectPos, new Object("773", 1) //Lifeelixire
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }
            {
                Vector2 objectPos = new Vector2(x + 56, y + 72);
                location.Objects.Add(objectPos, new Object("773", 1) //Lifeelixire
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }

            {
                Vector2 objectPos = new Vector2(x + 55, y + 72);
                location.Objects.Add(objectPos, new Object("166", 1) //Treasure Chest
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }
            {
                Vector2 objectPos = new Vector2(x + 54, y + 72);
                location.Objects.Add(objectPos, new Object("166", 1) //Treasure Chest
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }

            {
                Vector2 objectPos = new Vector2(x + 53, y + 72);
                location.Objects.Add(objectPos, new Object("166", 1) //Treasure Chest
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }

            {
                Vector2 objectPos = new Vector2(x + 52, y + 72);
                location.Objects.Add(objectPos, new Object("166", 1) //Treasure Chest
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }
            {
                Vector2 objectPos = new Vector2(x + 51, y + 72);
                location.Objects.Add(objectPos, new Object("166", 1) //Treasure Chest
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }
            {
                Vector2 objectPos = new Vector2(x + 50, y + 72);
                location.Objects.Add(objectPos, new Object("166", 1) //Treasure Chest
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }
            {
                Vector2 objectPos = new Vector2(x + 49, y + 72);
                location.Objects.Add(objectPos, new Object("166", 1) //Treasure Chest
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }
            {
                Vector2 objectPos = new Vector2(x + 48, y + 72);
                location.Objects.Add(objectPos, new Object("166", 1) //Treasure Chest
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }
            {
                Vector2 objectPos = new Vector2(x + 47, y + 72);
                location.Objects.Add(objectPos, new Object("166", 1) //Treasure Chest
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }
            {
                Vector2 objectPos = new Vector2(x + 47, y + 73);
                location.Objects.Add(objectPos, new Object("166", 1) //Treasure Chest
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }
            {
                Vector2 objectPos = new Vector2(x + 47, y + 74);
                location.Objects.Add(objectPos, new Object("166", 1) //Treasure Chest
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }
            {
                Vector2 objectPos = new Vector2(x + 48, y + 74);
                location.Objects.Add(objectPos, new Object("166", 1) //Treasure Chest
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }

            {
                Vector2 objectPos = new Vector2(x + 49, y + 74);
                location.Objects.Add(objectPos, new Object("166", 1) //Treasure Chest
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }
            {
                Vector2 objectPos = new Vector2(x + 50, y + 74);
                location.Objects.Add(objectPos, new Object("166", 1) //Treasure Chest
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }
            {
                Vector2 objectPos = new Vector2(x + 51, y + 74);
                location.Objects.Add(objectPos, new Object("166", 1) //Treasure Chest
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }
            {
                Vector2 objectPos = new Vector2(x + 52, y + 74);
                location.Objects.Add(objectPos, new Object("166", 1) //Treasure Chest
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }
            {
                Vector2 objectPos = new Vector2(x + 53, y + 74);
                location.Objects.Add(objectPos, new Object("166", 1) //Treasure Chest
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }

            {
                Vector2 objectPos = new Vector2(x + 54, y + 74);
                location.Objects.Add(objectPos, new Object("166", 1) //Treasure Chest
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }
            {
                Vector2 objectPos = new Vector2(x + 55, y + 74);
                location.Objects.Add(objectPos, new Object("166", 1) //Treasure Chest
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }

            {
                Vector2 objectPos = new Vector2(x + 56, y + 74);
                location.Objects.Add(objectPos, new Object("166", 1) //Treasure Chest
                {
                    IsSpawnedObject = true,
                    CanBeGrabbed = true
                });
            }


            {
                Vector2 position = new Vector2(x + 16, y + 35);
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
                Vector2 position = new Vector2(x + 16, y + 36);
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
                Vector2 position = new Vector2(x + 1, y + 96);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)441", 20));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 1, y + 97);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)441", 20));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 3, y + 39);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)441", 20));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 4, y + 39);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)441", 20));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }

            {
                Vector2 position = new Vector2(x + 18, y + 20);
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
                Vector2 position = new Vector2(x + 19, y + 20);
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
                Vector2 position = new Vector2(x + 41, y + 64);
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
                Vector2 position = new Vector2(x + 42, y + 64);
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
                Vector2 position = new Vector2(x + 43, y + 64);
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
                Vector2 position = new Vector2(x + 39, y + 97);
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
                Vector2 position = new Vector2(x + 40, y + 97);
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
                chest.addItem(ItemRegistry.Create("(O)773", 5));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }
            {
                Vector2 position = new Vector2(x + 51, y + 97);
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
                Vector2 position = new Vector2(x + 52, y + 97);
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
                Vector2 position = new Vector2(x + 57, y + 97);
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
                Vector2 position = new Vector2(x + 58, y + 97);
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
                PlaceMonsterAt(location, rand, x + 22, y + 42);
                PlaceMonsterAt(location, rand, x + 26, y + 42);
                PlaceMonsterAt(location, rand, x + 24, y + 40);

                PlaceMonsterAt(location, rand, x + 23, y + 62);
                PlaceMonsterAt(location, rand, x + 25, y + 62);
                PlaceMonsterAt(location, rand, x + 24, y + 60);

                PlaceMonsterAt(location, rand, x + 17, y + 63);

                PlaceMonsterAt(location, rand, x + 13, y + 66);
                PlaceMonsterAt(location, rand, x + 13, y + 58);

                PlaceMonsterAt(location, rand, x + 9, y + 94);
                PlaceMonsterAt(location, rand, x + 9, y + 97);

                PlaceMonsterAt(location, rand, x + 1, y + 48);
                PlaceMonsterAt(location, rand, x + 1, y + 53);
                PlaceMonsterAt(location, rand, x + 1, y + 55);
                PlaceMonsterAt(location, rand, x + 29, y + 4);
                PlaceMonsterAt(location, rand, x + 4, y + 5);

                PlaceMonsterAt(location, rand, x + 37, y + 56);
                PlaceMonsterAt(location, rand, x + 48, y + 56);

                PlaceMonsterAt(location, rand, x + 22, y + 77);
                PlaceMonsterAt(location, rand, x + 30, y + 77);

                PlaceMonsterAt(location, rand, x + 66, y + 76);
                PlaceMonsterAt(location, rand, x + 68, y + 76);
                PlaceMonsterAt(location, rand, x + 70, y + 76);
                PlaceMonsterAt(location, rand, x + 72, y + 76);
                PlaceMonsterAt(location, rand, x + 74, y + 76);
                PlaceMonsterAt(location, rand, x + 76, y + 76);
                PlaceMonsterAt(location, rand, x + 78, y + 76);
                PlaceMonsterAt(location, rand, x + 80, y + 76);
                PlaceMonsterAt(location, rand, x + 82, y + 76);
            }
            
            {
                   //MachineGun Monsters
                PlaceMonster(location, 8, x + 15, y + 62);

                PlaceMonster(location, 8, x + 13, y + 47);
                PlaceMonster(location, 8, x + 13, y + 73);
                PlaceMonster(location, 8, x + 9, y + 12);
                PlaceMonster(location, 8, x + 10, y + 12);
                PlaceMonster(location, 8, x + 25, y + 88);

                PlaceMonster(location, 8, x + 36, y + 45);
                PlaceMonster(location, 8, x + 43, y + 45);
                PlaceMonster(location, 8, x + 50, y + 45);
                PlaceMonster(location, 8, x + 44, y + 54);


                PlaceMonster(location, 8, x + 36, y + 36);
                PlaceMonster(location, 8, x + 43, y + 36);
                PlaceMonster(location, 8, x + 50, y + 36);
                PlaceMonster(location, 8, x + 36, y + 18);
                PlaceMonster(location, 8, x + 43, y + 18);
                PlaceMonster(location, 8, x + 50, y + 18);
                PlaceMonster(location, 8, x + 36, y + 10);
                PlaceMonster(location, 8, x + 43, y + 10);
                PlaceMonster(location, 8, x + 50, y + 10);
                PlaceMonster(location, 8, x + 36, y + 4);
                PlaceMonster(location, 8, x + 43, y + 4);
                PlaceMonster(location, 8, x + 50, y + 4);

                PlaceMonster(location, 8, x + 39, y + 95);
                PlaceMonster(location, 8, x + 40, y + 95);

                PlaceMonster(location, 8, x + 45, y + 95);
                PlaceMonster(location, 8, x + 46, y + 95);
                PlaceMonster(location, 8, x + 51, y + 95);
                PlaceMonster(location, 8, x + 51, y + 95);
                PlaceMonster(location, 8, x + 57, y + 95);
                PlaceMonster(location, 8, x + 58, y + 95);

                //Slimes!!!!!




            }
            {
                //PlaceBreakableAt(location, rand, x + 3, y + 30);
              //  PlaceBreakableAt(location, rand, x + 3, y + 31);
               // PlaceBreakableAt(location, rand, x + 3, y + 32);

            }


            {
                //if ((bool)this.addedSubwayHellSlimesToday)
               // {
                //    return;
               // }
                //location.addedSubwayHellSlimesToday.Value = true;
                Random rando;
                rando = Utility.CreateRandom(Game1.stats.DaysPlayed, Game1.uniqueIDForThisGame, 12.0);
                Microsoft.Xna.Framework.Rectangle spawnArea;
                spawnArea = new Microsoft.Xna.Framework.Rectangle(67, 83, 15, 8);
                for (int tries = 60; tries > 0; tries--)
                {
                    Vector2 tile;
                    tile = Utility.getRandomPositionInThisRectangle(spawnArea, rando);
                    if (location.CanItemBePlacedHere(tile))
                    {
                        GreenSlime i;
                        i = new GreenSlime(tile * 64f, 0);
                        i.makeTigerSlime();
                        location.characters.Add(i);
                    }
                }
            }    
        }
	}
}
