using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using xTile.Dimensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using xTile;
using StardewModdingAPI.Utilities;
using StardewValley.Network;
using StardewValley.Objects;
using xTile.Tiles;
using RestStopLocations.Game.Locations.DungeonLevelGenerators;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using StardewValley.Monsters;


namespace RestStopLocations.Game.Locations
{
    [XmlType("Mods_ApryllForever_RestStopLocations_SapphireVolcano")]
    public class SapphireVolcano : SapphireLocation, IAnimalLocation
    {

        public const string BaseLocationName = "SapphireVolcano";
        public const string LocationFortInfix = "Fort";
        public const string LocationClimbInfix = "Climb";
        public const string LocationMazeInfix = "Maze";
        public const int BossLevel = 5;
        public enum LevelType
        {
            Outside,
            Fort,
            Climb,
            Maze,
        }
        public readonly NetLongDictionary<FarmAnimal, NetRef<FarmAnimal>> animals = new();
        public NetLongDictionary<FarmAnimal, NetRef<FarmAnimal>> Animals => animals;

        public static List<SapphireBaseLevelGenerator> NormalDungeonGenerators = new()
        {
            //new BlankDungeonLevelGenerator(),
            new CanoFarmLevelGenerator(),
        };
        public static List<SapphireBaseLevelGenerator> FortGenerators = new()
        {
            new FortLevelGenerator(),
        };
       
        /*
        public static List<SapphireBaseLevelGenerator> ClimbGenerators = new()
        {
            new ClimbLevelGenerator(),
        };
        public static List<SapphireBaseLevelGenerator> MazeGenerators = new()
        {
            new ClimbLevelGenerator(), //MazeLevelGenerator(),
        }; */

        public static List<SapphireBaseLevelGenerator> SapphireBossGenerators = new()
        {
            new SapphireBossLevelGenerator(),
        };
        internal static List<SapphireVolcano> activeLevels = new();

       


        public static SapphireVolcano GetLevelInstance(string locName)
        {
            foreach (var level in activeLevels)
            {
                if (level.Name == locName)
                    return level;
            }

            LevelType t = LevelType.Outside;
            int l = 0;
            if (char.IsNumber(locName[BaseLocationName.Length]))
            {
                t = LevelType.Outside;
                l = int.Parse(locName.Substring(BaseLocationName.Length));
            }
            else if (locName.StartsWith(BaseLocationName + LocationFortInfix))
            {
                t = LevelType.Fort;
                string[] parts = locName.Substring((BaseLocationName + LocationFortInfix).Length).Split('_');
                l = int.Parse(parts[0]) * 100 + int.Parse(parts[1]);
            }
            else if (locName.StartsWith(BaseLocationName + LocationClimbInfix))
            {
                t = LevelType.Climb;
                string[] parts = locName.Substring((BaseLocationName + LocationClimbInfix).Length).Split('_');
                l = int.Parse(parts[0]) * 100 + int.Parse(parts[1]);
            }
            else if (locName.StartsWith(BaseLocationName + LocationMazeInfix))
            {
                t = LevelType.Maze;
                string[] parts = locName.Substring((BaseLocationName + LocationMazeInfix).Length).Split('_');
                l = int.Parse(parts[0]) * 100 + int.Parse(parts[1]);
            }

            SapphireVolcano newLevel = new SapphireVolcano(t, l, locName);
            activeLevels.Add(newLevel);

            if (Game1.IsMasterGame)
                newLevel.Generate();
            else
                newLevel.reloadMap();

            return newLevel;
        }

        public static void UpdateLevels(GameTime time)
        {
            foreach (var level in activeLevels.ToList())
            {
                if (level.farmers.Count > 0)
                    level.UpdateWhenCurrentLocation(time);
                level.updateEvenIfFarmerIsntHere(time);
            }
          

        }

        public void UpdateLevels10Minutes(int time)
        {
            if (Game1.IsClient)
                return;

            foreach (var level in activeLevels.ToList())
            {
                if (level.farmers.Count > 0)
                    level.performTenMinuteUpdate(time);
            }
            if (Game1.IsMasterGame)
            {
                foreach (FarmAnimal value in this.animals.Values)
                {
                    value.updatePerTenMinutes(Game1.timeOfDay, this);
                }
            }
        }

        public static void ClearAllLevels()
        {
            foreach (var level in activeLevels)
            {
                level.mapContent.Dispose();
            }
            activeLevels.Clear();
        }

        private bool generated = false;
        private LocalizedContentManager mapContent;
        public readonly NetEnum<LevelType> levelType = new();
        public readonly NetInt level = new();
        public readonly NetInt genSeed = new();
        private readonly NetVector2 warpFromPrev = new();
        private readonly NetVector2 warpFromNext = new();
        private Random genRandom;
        public bool isIndoorLevel = false;

        public SapphireVolcano()
        {
            mapContent = Game1.game1.xTileContent.CreateTemporary();
            mapPath.Value = Mod.instance.Helper.ModContent.GetInternalAssetName("assets/maps/HellDungeonTemplate.tmx").BaseName;

        }

        public SapphireVolcano(LevelType type, int level, string name)
        : this()
        {
            levelType.Value = type;
            this.level.Value = level;
            this.name.Value = name;
        }
        internal static IMonitor Monitor { get; set; }



        protected override LocalizedContentManager getMapLoader()
        {
            return mapContent;
        }

        protected override void resetLocalState()
        {
            if (!generated)
                Generate();

            base.resetLocalState();

            if (Game1.timeOfDay < 1800)
            {
                Game1.ambientLight = Color.White;
            }

            mapBaseTilesheet = Game1.temporaryContent.Load<Texture2D>(map.TileSheets[0].ImageSource);



            if (Game1.player.Tile.X == 0)
            {
                if (Game1.player.Tile.Y == 0)
                {
                    Game1.player.Position = new Vector2(warpFromPrev.X * Game1.tileSize, warpFromPrev.Y * Game1.tileSize);
                }
                else if (Game1.player.Tile.Y == 1)
                {
                    Game1.player.Position = new Vector2(warpFromNext.X * Game1.tileSize, warpFromNext.Y * Game1.tileSize);
                }
            }
            if (level.Value == 5)
            {
                Game1.addHUDMessage(new HUDMessage("Beware!!! Dangerous Enemies Ahead!!!"));
            }
        }

        public void Generate()
        {
            generated = true;

            SapphireBaseLevelGenerator gen = null;

            //int idForGame = (int)Game1.uniqueIDForThisGame; int daysPlayedForGame = (int)Game1.stats.DaysPlayed; int levelValueForGame = level.Value;

            //if (levelType.Value == LevelType.Outside)
            if (level == 1 || level == 5)
            {
                if (level != BossLevel)
                {
                    Random r = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed + level.Value);
                    gen = NormalDungeonGenerators[r.Next(NormalDungeonGenerators.Count)];
                }
                else
                {
                    Random r = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed / 7 + level.Value);
                    gen = SapphireBossGenerators[r.Next(SapphireBossGenerators.Count)];
                }
            }
            else if (level == 2)
            {
                Random r = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed / 3 + level.Value);
                gen = FortGenerators[r.Next(FortGenerators.Count)];
            }
            /*
            else if (level == 3)
            {
                Random r = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed / 4 + level.Value);
                gen = ClimbGenerators[r.Next(ClimbGenerators.Count)];
            }
            else if (level == 4)
            {
                Random r = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed / 3 + level.Value);
                gen = MazeGenerators[r.Next(MazeGenerators.Count)];
            } */
            

            if (Game1.IsMasterGame)
            {
                genSeed.Value = (int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed * level.Value * SDate.Now().DaysSinceStart;
            }

            Vector2 warpPrev = Vector2.Zero, warpNext = Vector2.Zero;
            reloadMap();

            if (Map.GetLayer("Buildings1") == null)
            {
                Map.AddLayer(new xTile.Layers.Layer("Buildings1", Map, Map.Layers[0].LayerSize, Map.Layers[0].TileSize));
            }
            long id2 = 6942069696969696902;
            long id3 = 6942069696969696903;
            long id4 = 6942069696969696904;
            long id5 = 6942069696969696905;
            long id6 = 6942069696969696906;
            long id7 = 6942069696969696907;


            //SapphireSprings location = new SapphireSprings();

            try
            {
                gen.Generate(this, ref warpPrev, ref warpNext);
                warpFromPrev.Value = warpPrev;
                warpFromNext.Value = warpNext;
            }
            catch (Exception e)
            {
                Monitor.Log("Exception generating dungeon: " + e);
            }
        }

        static string EnterDungeon = "Do you wish to enter this forboding gate?";

        public override bool performAction(string actionStr, Farmer who, xTile.Dimensions.Location tileLocation)
        {
            string[] split = actionStr.Split(' ');
            string action = split[0];
            int tx = tileLocation.X;
            int ty = tileLocation.Y;
            if (action == "RS.Bluebella")
            {
                createQuestionDialogue(EnterDungeon, createYesNoResponses(), "HellDungeonEntrance");
            }


            if (action == "HellWarpPrevious")
            {
                string prev = HellDungeon.BaseLocationName + (level.Value - 1);
                if (level.Value == 1)
                    prev = "Custom_DungeonEntrance";

                performTouchAction("MagicWarp " + prev + " 0 1", Game1.player.Position);
            }
            else if (action == "HellWarpNext")
            {
                if (warpFromPrev == warpFromNext) // boss level
                    performTouchAction("MagicWarp Custom_SouthRestStop 33 9", Game1.player.Position);
                else
                {
                    string next = HellDungeon.BaseLocationName + (level.Value + 1);
                    performTouchAction("MagicWarp " + next + " 0 0", Game1.player.Position);
                }
            }


            return base.performAction(action, who, tileLocation);
        }
        public override bool answerDialogue(Response answer)
        {
            if (lastQuestionKey != null && afterQuestion == null)
            {
                string qa = lastQuestionKey.Split(' ')[0] + "_" + answer.responseKey;
                switch (qa)
                {
                    case "HellDungeonEntrance_Yes":
                        Game1.playSound("crit");
                        Game1.playSound("doorCreak");
                        Game1.playSound("crit");
                        performTouchAction("Warp " + "BluebellaDungeon0 31 50", Game1.player.Position);
                        return true;
                }
            }
            return base.answerDialogue(answer);
        }


        public override bool CanPlaceThisFurnitureHere(Furniture furniture)
        {
            return false;
        }

        //Begin Lava

        [XmlIgnore]
        public Texture2D mapBaseTilesheet;

        protected static Dictionary<int, Point> _blobIndexLookup = null;

        protected static Dictionary<int, Point> _lavaBlobIndexLookup = null;
        public enum TileNeighbors
        {
            N = 1,
            S = 2,
            E = 4,
            W = 8,
            NW = 0x10,
            NE = 0x20
        }
        [XmlIgnore]
        public NetEvent1Field<Point, NetPoint> coolLavaEvent = new NetEvent1Field<Point, NetPoint>();

        [XmlIgnore]
        public NetVector2Dictionary<bool, NetBool> cooledLavaTiles = new NetVector2Dictionary<bool, NetBool>();

        [XmlIgnore]
        public Dictionary<Vector2, Point> localCooledLavaTiles = new Dictionary<Vector2, Point>();

        private int lavaSoundsPlayedThisTick;

        protected override void initNetFields()
        {
            base.initNetFields();
            NetFields.AddField(this.level).AddField(this.genSeed).AddField(this.warpFromPrev).AddField(this.warpFromNext);
           
        }



        public override void UpdateWhenCurrentLocation(GameTime time)
        {
            base.UpdateWhenCurrentLocation(time);
           
        }


        //Complete Lava, For the Most Part

        public override void performTenMinuteUpdate(int timeOfDay)
        {
            base.performTenMinuteUpdate(timeOfDay);
            if (Game1.random.NextDouble() < 0.3)
            {
                int numsprites = 0;
                foreach (NPC character in characters)
                {
                    if (character is Bat)
                    {
                        numsprites++;
                    }
                }
                if (numsprites < farmers.Count * 4)
                {
                    spawnFlyingMonsterOffScreen();
                }
            }
        }

        public void spawnFlyingMonsterOffScreen()
        {
            Vector2 spawnLocation = Vector2.Zero;
            switch (Game1.random.Next(4))
            {
                case 0:
                    spawnLocation.X = Game1.random.Next(map.Layers[0].LayerWidth);
                    break;
                case 3:
                    spawnLocation.Y = Game1.random.Next(map.Layers[0].LayerHeight);
                    break;
                case 1:
                    spawnLocation.X = map.Layers[0].LayerWidth - 1;
                    spawnLocation.Y = Game1.random.Next(map.Layers[0].LayerHeight);
                    break;
                case 2:
                    spawnLocation.Y = map.Layers[0].LayerHeight - 1;
                    spawnLocation.X = Game1.random.Next(map.Layers[0].LayerWidth);
                    break;
            }
            playSound("magma_sprite_spot");
            characters.Add(new Bat(spawnLocation, (Game1.random.NextDouble() < 0.5) ? (-556) : (-555))
            {
                focusedOnFarmers = true
            });
        }

        public override void checkForMusic(GameTime time)
        {
            if (Game1.getMusicTrackName() == "rain" || Game1.getMusicTrackName() == "none" || Game1.getMusicTrackName() == "IslandMusic" || Game1.isMusicContextActiveButNotPlaying() || (Game1.getMusicTrackName().EndsWith("_Ambient") && Game1.getMusicTrackName() != "Volcano_Ambient"))
            {
                Game1.changeMusicTrack("Volcano_Ambient");
            }
        }
        public override void tryToAddCritters(bool onlyIfOnScreen = false)
        {
            if (Game1.random.NextDouble() < 0.3)
            {
                Vector2 origin2 = Vector2.Zero;
                origin2 = ((Game1.random.NextDouble() < 0.75) ? new Vector2((float)Game1.viewport.X + Utility.RandomFloat(0f, Game1.viewport.Width), Game1.viewport.Y - 64) : new Vector2(Game1.viewport.X + Game1.viewport.Width + 64, Utility.RandomFloat(0f, Game1.viewport.Height)));
                int parrots_to_spawn = 1;
                if (Game1.random.NextDouble() < 0.5)
                {
                    parrots_to_spawn++;
                }
                if (Game1.random.NextDouble() < 0.5)
                {
                    parrots_to_spawn++;
                }
                for (int i = 0; i < parrots_to_spawn; i++)
                {
                    addCritter(new OverheadParrot(origin2 + new Vector2(i * 64, -i * 64)));
                }
            }
            if (!Game1.IsRainingHere(this))
            {
                double mapArea = map.Layers[0].LayerWidth * map.Layers[0].LayerHeight;
                double butterflyChance = Math.Max(0.9, Math.Min(0.25, mapArea / 15000.0));
                addButterflies(butterflyChance, onlyIfOnScreen);
            }
            if (Game1.IsRainingHere(this))
            {
                double mapArea = map.Layers[0].LayerWidth * map.Layers[0].LayerHeight;
                double butterflyChance = Math.Max(0.9, Math.Min(0.25, mapArea / 1500.0));
                addButterflies(butterflyChance, onlyIfOnScreen);
            }
            //if (Game1.IsRainingHere(this))
            {
                //double mapArea = map.Layers[0].LayerWidth * map.Layers[0].LayerHeight;
                //double frogChance = Math.Max(0.5, Math.Min(0.25, mapArea / 1500.0));
                addFrog();
            }
            {
                double mapArea = map.Layers[0].LayerWidth * map.Layers[0].LayerHeight;
                double birdieChance = Math.Max(0.7, Math.Min(0.25, mapArea / 15000.0));
                addBirdies(birdieChance, onlyIfOnScreen);
            }
            {
                double mapArea = map.Layers[0].LayerWidth * map.Layers[0].LayerHeight;
                double bunniesChance = Math.Max(0.7, Math.Min(0.25, mapArea / 15000.0));
                addBunnies(bunniesChance, onlyIfOnScreen);
            }
            {
                double mapArea = map.Layers[0].LayerWidth * map.Layers[0].LayerHeight;
                double squirrelChance = Math.Max(0.7, Math.Min(0.25, mapArea / 15000.0));
                addSquirrels(squirrelChance, onlyIfOnScreen);
            }
            {
                double mapArea = map.Layers[0].LayerWidth * map.Layers[0].LayerHeight;
                double woodpeckerChance = Math.Max(0.7, Math.Min(0.25, mapArea / 15000.0));
                addWoodpecker(woodpeckerChance, onlyIfOnScreen);
            }
            if (Game1.currentSeason == "winter" && Game1.isDarkOut())
            {
                addMoonlightJellies(50, new Random((int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame - 24917), new Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0));
            }
        }
        
        public override void drawWater(SpriteBatch b)
        {
            if (currentEvent != null)
            {
                currentEvent.drawUnderWater(b);
            }
            if (waterTiles == null)
            {
                return;
            }
            for (int y = Math.Max(0, Game1.viewport.Y / 64 - 1); y < Math.Min(map.Layers[0].LayerHeight, (Game1.viewport.Y + Game1.viewport.Height) / 64 + 2); y++)
            {
                for (int x = Math.Max(0, Game1.viewport.X / 64 - 1); x < Math.Min(map.Layers[0].LayerWidth, (Game1.viewport.X + Game1.viewport.Width) / 64 + 1); x++)
                {
                    if (waterTiles[x, y])
                    {
                        drawWaterTile(b, x, y);
                    }
                }
            }
        }
        

        public override void DayUpdate(int dayOfMonth)
        {
           
            base.DayUpdate(dayOfMonth);
        }



        public override void draw(SpriteBatch b)
        {
            base.draw(b);

            foreach (KeyValuePair<long, FarmAnimal> pair in this.animals.Pairs)
            {
                pair.Value.draw(b);
            }
        }
        public override void updateEvenIfFarmerIsntHere(GameTime time, bool skipWasUpdatedFlush = false)
        {
            base.updateEvenIfFarmerIsntHere(time, skipWasUpdatedFlush);

            if (!Game1.currentLocation.Equals(this))
            {
                NetDictionary<long, FarmAnimal, NetRef<FarmAnimal>, SerializableDictionary<long, FarmAnimal>, NetLongDictionary<FarmAnimal, NetRef<FarmAnimal>>>.PairsCollection pairs = this.animals.Pairs;
                for (int i = pairs.Count() - 1; i >= 0; i--)
                {
                    pairs.ElementAt(i).Value.updateWhenNotCurrentLocation(null, time, this);
                }
            }
        }

 

     





    }
}