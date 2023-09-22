

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SpaceShared;
using StardewValley;
using StardewValley.Objects;
using RestStopLocations;
using StardewModdingAPI;
using StardewModdingAPI.Enums;
using StardewModdingAPI.Events;
using StardewValley.Monsters;
using Object = StardewValley.Object;
using RestStopLocations.Game.Locations.Sapphire;



namespace RestStopLocations.Game.Locations.DungeonLevelGenerators
{
    public abstract class SapphireBaseLevelGenerator : SapphireVolcano
    {

            public abstract void Generate(SapphireVolcano location, ref Vector2 warpFromPrev, ref Vector2 warpFromNext);
            public List<Monster> Monsters { get; private set; }
            static IMonitor Monitor;
            protected string GetNextLocationName(SapphireVolcano location)
            {
                return SapphireVolcano.BaseLocationName + (location.level + 1);
            }

            protected string GetPreviousLocationName(SapphireVolcano location)
            {
                if (location.level == 1)
                    return "Custom_SapphireSprings";
                return SapphireVolcano.BaseLocationName + (location.level - 1);
            }

            protected void PlacePreviousWarp(SapphireVolcano location, int centerX, int groundY)
            {
                int ts = location.Map.TileSheets.IndexOf(location.Map.GetTileSheet("HellDungeonTileSheet"));

                //Monitor.Log("Placing previous warp @ " + centerX + ", " + groundY);
                //location.setMapTile(centerX + -1, groundY - 2, 503, "Front", null, ts);
                // location.setMapTile(centerX + 0, groundY - 2, 504, "Front", null, ts);
                //location.setMapTile(centerX + 1, groundY - 2, 505, "Front", null, ts);
                //location.setMapTile(centerX + -1, groundY - 1, 378, "Front", null, ts);
                location.setMapTile(centerX + 0, groundY - 1, 505, "Front", null, ts);
                //location.setMapTile(centerX + 1, groundY - 1, 380, "Front", null, ts);
                //location.setMapTile(centerX + -1, groundY - 0, 362, "Buildings", "HellWarpPrevious", ts);
                location.setMapTile(centerX + 0, groundY - 0, 521, "Buildings", "HellWarpPrevious", ts);
                //location.setMapTile(centerX + 1, groundY - 0, 364, "Buildings", "HellWarpPrevious", ts);
            }

            protected void PlaceNextWarp(SapphireVolcano location, int centerX, int groundY)
            {
                int ts = location.Map.TileSheets.IndexOf(location.Map.GetTileSheet("HellDungeonTileSheet"));

                //Monitor.Log("Placing next warp @ " + centerX + ", " + groundY);
                //location.setMapTile(centerX + -1, groundY - 2, 503 + 9, "Front", null, ts);
                //location.setMapTile(centerX + 0, groundY - 2, 504 + 9, "Front", null, ts);
                //location.setMapTile(centerX + 1, groundY - 2, 505 + 9, "Front", null, ts);
                //location.setMapTile(centerX + -1, groundY - 1, 378 + 9, "Front", null, ts);
                location.setMapTile(centerX + 0, groundY - 1, 496 + 9, "Front", null, ts);
                //location.setMapTile(centerX + 1, groundY - 1, 380 + 9, "Front", null, ts);
                //location.setMapTile(centerX + -1, groundY - 0, 362 + 9, "Buildings", "HellWarpNext", ts);
                location.setMapTile(centerX + 0, groundY - 0, 512 + 9, "Buildings", "HellWarpNext", ts);
                //location.setMapTile(centerX + 1, groundY - 0, 364 + 9, "Buildings", "HellWarpNext", ts);
            }

        /*  Aint like I even used this bullshit method. 1.6
            protected void PlaceMinableAt(SapphireVolcano location, Random rand, int sx, int sy)
            {
                double r = rand.NextDouble();
                if (r < 0.65)
                {
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), rand.NextDouble() < 0.5 ? 846 : 847, 1)
                    {
                        Name = "Stone",
                        MinutesUntilReady = 12
                    });
                }
                else if (r < 0.85)
                {
                    int[] ores = new int[] { 751, 290, 764, int.MaxValue, int.MaxValue, int.MaxValue };
                    int[] breaks = new int[] { 15, 15, 6, 8, 10, 12 };
                    int ore_ = rand.Next(ores.Length);
                    int ore = ores[ore_];

                    {
                        location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), ore, 1)
                        {
                            Name = "Stone",
                            MinutesUntilReady = breaks[ore_]
                        });
                    }
                }
                else if (r < 0.95)
                {
                    int[] gems = new int[] { 2, 4, 6, 8, 10, 12, 14, 44, 44, 44, 46, 46 };
                    int gem_ = rand.Next(gems.Length);
                    int gem = gems[gem_];
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), gem, 1)
                    {
                        Name = "Stone",
                        MinutesUntilReady = 10
                    });
                }
                else
                {
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), 819, 1)
                    {
                        Name = "Stone",
                        MinutesUntilReady = 10
                    });
                }
            } */

        protected void PlaceMinable(HellDungeon location, int WhichMinable, int sx, int sy)
        {
            switch (WhichMinable)
            {
                case 0:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "750")); //Weeds
                    break;
                case 1:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "751")); //Copper
                    break;
                case 2:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "760")); //Stone
                    break;
                case 3:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "762")); //Stone
                    break;
                case 4:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "764")); //Gold
                    break;
                case 5:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "765")); //Iridium
                    break;
                case 6:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "290")); //Iron
                    break;
                case 7:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "816")); //Bone
                    break;
                case 8:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "817")); //Bone
                    break;
                case 9:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "818")); //Clay
                    break;
                case 10:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "819")); //Omni Geode
                    break;
                case 11:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "32")); //Stone
                    break;
                case 12:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "38")); //Stone
                    break;
                case 13:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "40")); //Stone
                    break;
                case 14:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "42")); //Stone
                    break;
                case 15:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "77")); //Lava Geode
                    break;
                case 16:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "76")); //Frozen G
                    break;
                case 17:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "75")); //Reg G
                    break;
                case 18:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "2")); //Diamond
                    break;
                case 19:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "4")); //Ruby
                    break;
                case 20:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "6")); //Jade
                    break;
                case 21:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "8")); //Amethyst 
                    break;
                case 22:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "10")); //Topaz
                    break;
                case 23:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "12")); //Emerald
                    break;
                case 24:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "14")); //Aquamarine
                    break;
                case 25:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "95")); //Radioactive Ore
                    break;
                case 26:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "25")); //Oysters
                    break;
                case 27:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "32")); //Brown Stone
                    break;
                case 28:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "34")); //Gray Stone
                    break;
                case 29:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "36")); //Gray Stone
                    break;
                case 30:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "38")); //Brown Stone
                    break;
                case 31:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "40")); //Brown Stone
                    break;
                case 32:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "42")); //Brown Stone
                    break;
                case 33:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "44")); //Gem Node
                    break;
                case 34:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "46")); //Mythic Node
                    break;
                case 35:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "48")); //Blue Stone
                    break;
                case 36:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "50")); //Blue Stone
                    break;
                case 37:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "52")); //Blue Stone
                    break;
                case 38:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "54")); //Blue Stone
                    break;
                case 39:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "56")); //Red Stone
                    break;
                case 40:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "58")); //Red Stone
                    break;
                case 41:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "75")); //Reg Geode
                    break;
                case 42:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "76")); //Frozen Geode
                    break;
                case 43:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "77")); //Lava Geode
                    break;
                case 44:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "339")); //White Snow Puff
                    break;
                case 45:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "668")); //Gray Stone
                    break;
                case 46:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "670")); //Gray Stone
                    break;
                case 47:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "843")); //Cinder Shards
                    break;
                case 48:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "844")); //Cinder Shards
                    break;
                case 49:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "845")); //Black Stone
                    break;
                case 50:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "846")); //Black Stone
                    break;
                case 51:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "847")); //Black Stone
                    break;
                case 52:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "849")); //Black Stone
                    break;
                case 53:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "850")); //Black Stone
                    break;
                case 54:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "450")); //Farm Stone
                    break;
            }

        }
        protected void PlaceObject(HellDungeon location, int WhichObject, int sx, int sy)
        {
            switch (WhichObject)
            {
                case 0:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "166")); //Treasure Chest
                    break;
                case 1:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "751")); //Copper
                    break;
                case 2:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "760")); //Stone
                    break;
                case 3:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "762")); //Stone
                    break;
                case 4:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "764")); //Gold
                    break;
                case 5:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "765")); //Iridium
                    break;
                case 6:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "290")); //Iron
                    break;
                case 7:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "816")); //Bone
                    break;
                case 8:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "817")); //Bone
                    break;
                case 9:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "818")); //Clay
                    break;
                case 10:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "819")); //Omni Geode
                    break;
                case 11:
                    location.netObjects.Add(new Vector2(sx, sy), new StardewValley.Object(new Vector2(sx, sy), "32")); //Stone
                    break;
            }
        }


        protected void PlaceMonsterAt(SapphireVolcano location, Random rand, int tx, int ty)
            {
                switch (rand.Next(7))
                {
                    case 0:
                        location.characters.Add(new Ghost(new Vector2(tx * Game1.tileSize, ty * Game1.tileSize)));
                        break;
                    case 1:
                        location.characters.Add(new HellBat(new Vector2(tx * Game1.tileSize, ty * Game1.tileSize)));
                        break;
                    case 2:
                        location.characters.Add(new GreenSlime(new Vector2(tx * Game1.tileSize, ty * Game1.tileSize)));
                        break;
                    case 3:
                        location.characters.Add(new ShadowBrute(new Vector2(tx * Game1.tileSize, ty * Game1.tileSize)));
                        break;
                    case 4:
                        location.characters.Add(new SquidKid(new Vector2(tx * Game1.tileSize, ty * Game1.tileSize)));
                        break;
                    case 5:
                        location.characters.Add(new HellBat(new Vector2(tx * Game1.tileSize, ty * Game1.tileSize)));
                        break;
                    case 6:
                        location.characters.Add(new RockCrab(new Vector2(tx * Game1.tileSize, ty * Game1.tileSize)));
                        break;
                }
            }


        
        protected void PlaceMonster(SapphireVolcano location, int WhichMonster, int tx, int ty)
        {
            switch (WhichMonster)
            {
                case 0:
                    location.characters.Add(new Ghost(new Vector2(tx * Game1.tileSize, ty * Game1.tileSize)));
                    break;
                case 1:
                    location.characters.Add(new HellBat(new Vector2(tx * Game1.tileSize, ty * Game1.tileSize)));
                    break;
                case 2:
                    location.characters.Add(new GreenSlime(new Vector2(tx * Game1.tileSize, ty * Game1.tileSize)));
                    break;
                case 3:
                    location.characters.Add(new ShadowBrute(new Vector2(tx * Game1.tileSize, ty * Game1.tileSize)));
                    break;
                case 4:
                    location.characters.Add(new SquidKid(new Vector2(tx * Game1.tileSize, ty * Game1.tileSize)));
                    break;
                case 5:
                    location.characters.Add(new DinoMonster(new Vector2(tx * Game1.tileSize, ty * Game1.tileSize)));
                    break;
                case 6:
                    location.characters.Add(new RockCrab(new Vector2(tx * Game1.tileSize, ty * Game1.tileSize)));
                    break;
                case 7:
                    location.characters.Add(new Shooter(new Vector2(tx * Game1.tileSize, ty * Game1.tileSize)));
                    break;
                case 8:
                    location.characters.Add(new DangerShooter(new Vector2(tx * Game1.tileSize, ty * Game1.tileSize)));
                    break;
            }
        } 

        protected void PlaceBreakableAt(SapphireVolcano location, Random rand, int tx, int ty)
        {
            Vector2 position = new Vector2(tx, ty);
            if (location.netObjects.ContainsKey(position))
                return;

            BreakableContainer bcontainer = new BreakableContainer(position, "174", 4, 14, "clank", "boulderBreak");
            bcontainer.setHealth(6);

            location.netObjects.Add(position, bcontainer);
        }

        protected void PlaceChestAt(SapphireVolcano location, Random rand, int tx, int ty, bool rare)
        {
            Vector2 position = new Vector2(tx, ty);
            Chest chest = new Chest(playerChest: false, position);
            chest.dropContents.Value = true;
            chest.synchronized.Value = true;
            chest.Type = "interactive";
           
        }
    }
}
