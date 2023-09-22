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
using StardewModdingAPI.Events;
using StardewValley.Network;
using StardewValley.Objects;
using xTile.Tiles;
using RestStopLocations.Game.Locations.DungeonLevelGenerators;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using StardewValley.Locations;
using xTile.Layers;

using StardewValley.Menus;
using StardewValley.Monsters;
using xTile.ObjectModel;
using Object = StardewValley.Object;
using SpaceShared;


namespace RestStopLocations.Game.Locations
{
    [XmlType("Mods_ApryllForever_RestStopLocations_UnderworldField")]
    public class UnderworldField : RestStopLocation
    {
        private static IModHelper Helper { get; set; }

        internal static IMonitor Monitor { get; set; }

        //protected NetInt _currentState = new NetInt();

        protected Color _ambientLightColor = Color.White;



        private List<WeatherDebris> weatherDebris;

        

        //private Dictionary<TerrainFeature, Vector2> mermaidPlants;

        new public List<TerrainFeature> _activeTerrainFeatures = new List<TerrainFeature>();

        new public bool isCropAtTile(int tileX, int tileY)
        {
            Vector2 key = new Vector2(tileX, tileY);
            if (terrainFeatures.ContainsKey(key) && terrainFeatures[key] is HoeDirt)
            {
                return ((HoeDirt)terrainFeatures[key]).crop != null;
            }

            return false;
        }

        public UnderworldField()
        {
        }

        public static void Initialize(IMod ModInstance, IModHelper helper, IMonitor monitor)
        {
            Helper = helper;
            Texture2D mooTilesheet = helper.ModContent.Load<Texture2D>("Assets/Maps/HellDungeonTileSheet.png");

            Helper.Events.GameLoop.DayEnding += OnDayEnding;
            Helper.Events.Content.AssetRequested += UnderworldField.OnAssetRequested;
        }


        public UnderworldField(IModContentHelper content)
        : base(content, "UnderworldField", "UnderworldField")
        {
           
            

        }

        public override void checkForMusic(GameTime time)
        {
            if (base.IsOutdoors && Game1.isMusicContextActiveButNotPlaying() && !Game1.IsRainingHere(this) && !Game1.eventUp)
            {
                //if (!Game1.isDarkOut())
                //{
                    //Game1.changeMusicTrack("AbigailFLute", track_interruptable: false);
               //}
                /*else*/ if (Game1.isDarkOut() && Game1.timeOfDay < 2500)
                {
                    Game1.changeMusicTrack("spring_night_ambient", track_interruptable: true);
                }
            }
        }

        public void addDesertButterflies(double chance, bool onlyIfOnScreen = false)
        {
            bool island_location = this.InIslandContext();
            //bool firefly = (Game1.currentSeason.Equals("summer") | island_location) && Game1.isDarkOut();
            if (Game1.timeOfDay >= 2500)
            {
                return;
            }
            chance = Math.Min(0.8, chance * 1.5);
            while (Game1.random.NextDouble() < chance)
            {
                Vector2 v = getRandomTile();
                if (onlyIfOnScreen && Utility.isOnScreen(v * 64f, 64))
                {
                    continue;
                }
                else
                {
                    critters.Add(new DesertButterfly(v,false));
                }
                while (Game1.random.NextDouble() < 0.4)
                {
                    {
                        critters.Add(new DesertButterfly(v + new Vector2(Game1.random.Next(-2, 3), Game1.random.Next(-2, 3)), true));
                    }
                }
            }
        }
        public override void tryToAddCritters(bool onlyIfOnScreen = false)
        {
           
            if (!Game1.IsRainingHere(this))
            {
                double mapArea = map.Layers[0].LayerWidth * map.Layers[0].LayerHeight;
                double butterflyChance = Math.Max(0.9, Math.Min(0.25, mapArea / 15000.0));
                addDesertButterflies(butterflyChance, onlyIfOnScreen);
            }
            if (Game1.IsRainingHere(this))
            {
                double mapArea = map.Layers[0].LayerWidth * map.Layers[0].LayerHeight;
                double butterflyChance = Math.Max(0.9, Math.Min(0.25, mapArea / 1500.0));
                addDesertButterflies(butterflyChance, onlyIfOnScreen);
            }
            {
                double mapArea = map.Layers[0].LayerWidth * map.Layers[0].LayerHeight;
                double birdieChance = Math.Max(0.7, Math.Min(0.25, mapArea / 15000.0));
                addBirdies(birdieChance, onlyIfOnScreen);
            }
            {
                double mapArea = map.Layers[0].LayerWidth * map.Layers[0].LayerHeight;
                double woodpeckerChance = Math.Max(0.7, Math.Min(0.25, mapArea / 15000.0));
                addWoodpecker(woodpeckerChance, onlyIfOnScreen);
            }
            
        }

      

        /*
        protected override void resetLocalState()
        {
           
            //_ambientLightColor = new Color(200, 120, 50);
            ignoreOutdoorLighting.Value = false;

            base.resetLocalState();


           
            new Random((int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame / 2);
            weatherDebris = new List<WeatherDebris>();
            int spacing = 192;
            int leafType = 3;
            for (int j = 0; j < 10; j++)
            {
                weatherDebris.Add(new WeatherDebris(new Vector2(j * spacing % Game1.graphics.GraphicsDevice.Viewport.Width + Game1.random.Next(spacing), j * spacing / Game1.graphics.GraphicsDevice.Viewport.Width * spacing % Game1.graphics.GraphicsDevice.Viewport.Height + Game1.random.Next(spacing)), leafType, (float)Game1.random.Next(15) / 500f, (float)Game1.random.Next(-10, 0) / 50f, (float)Game1.random.Next(10) / 50f));
            }



            if ((Game1.timeOfDay < 1800))
            {
                Game1.ambientLight = Color.White;
            }

            waterColor.Value = new Color(130, 80, 255) * 0.5f;
            
            var v1 = new Vector2(53);
            var v2 = new Vector2(22);
            var v3 = new Vector2(34);
            var v4 = new Vector2(67);
            var v5 = new Vector2(76);
            var v6 = new Vector2(51);
            addCritter(new Crow((int)14, (int)4)); 
            addCritter(new Crow((int)27, (int)15));
            addCritter(new Crow((int)25, (int)8));
            addCritter(new Crow((int)44, (int)19));
            addCritter(new Crow((int)27, (int)28));
            addCritter(new Crow((int)7, (int)46));
            addCritter(new Crow(11, 18));
            addCritter(new Crow(22, 42));
            addCritter(new Crow(33, 47));
            addCritter(new Crow(11, 18));
            addCritter(new Crow(45, 58));
            addCritter(new Crow(40, 51));
            addCritter(new Crow(36, 48));
            addCritter(new Crow(44, 39));
            addCritter(new Crow(38, 34));
            addCritter(new Crow(26, 8));
            addCritter(new Crow(27, 10));

        } */

        public override void cleanupBeforePlayerExit()
        {
            if (weatherDebris != null)
            {
                weatherDebris.Clear();
            }
            base.cleanupBeforePlayerExit();
        }
        
        
        public override void DayUpdate(int dayOfMonth)
        {
            
            base.DayUpdate(dayOfMonth);
        }

        public override void draw(SpriteBatch b)
        {
            base.draw(b);

        }

      

        /*
        public override void UpdateWhenCurrentLocation(GameTime time)
        {
            base.UpdateWhenCurrentLocation(time);
           
           
            if (weatherDebris != null)
            {
                foreach (WeatherDebris weatherDebri in weatherDebris)
                {
                    weatherDebri.update();
                }
                Game1.updateDebrisWeatherForMovement(weatherDebris);
            }
        }*/
       
        static string EnterDungeon = "Do you wish to enter this forboding gate?";

        public override bool performAction(string action, Farmer who, Location tileLocation)
        {
            if (action == "RSDungeonEntrance")
            {
                createQuestionDialogue(EnterDungeon, createYesNoResponses(), "HellDungeonEntrance");
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
                        performTouchAction("MagicWarp " + HellDungeon.BaseLocationName + "1 0 0", Game1.player.Position);
                        return true;
                }
            }

            return base.answerDialogue(answer);
        }


        


















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
        public Texture2D mooTilesheet;

        [XmlIgnore]
        public int mapWidth;

        [XmlIgnore]
        public int mapHeight;

        [XmlIgnore]
        public NetEvent1Field<Point, NetPoint> coolLavaEvent = new NetEvent1Field<Point, NetPoint>();

        [XmlIgnore]
        public NetVector2Dictionary<bool, NetBool> cooledLavaTiles = new NetVector2Dictionary<bool, NetBool>();

        [XmlIgnore]
        public Dictionary<Vector2, Point> localCooledLavaTiles = new Dictionary<Vector2, Point>();

        public HashSet<Point> dirtTiles = new HashSet<Point>();

        public Random generationRandom;

        //private LocalizedContentManager mapContent;

        [XmlIgnore]
        public Layer backLayer;

        [XmlIgnore]
        public Layer buildingsLayer;

        protected static Dictionary<int, Point> _blobIndexLookup = null;

        protected static Dictionary<int, Point> _lavaBlobIndexLookup = null;

        //public Vector2 coolinatedLava;

        public Color[] pixelMap;

        public int[] heightMap;

        private int lavaSoundsPlayedThisTick;

        private float steamTimer = 6000f;

        public static Dictionary<xTile.Tiles.StaticTile,Vector2> coolinatedLava = new Dictionary<xTile.Tiles.StaticTile,Vector2>();

    



        public override bool BlocksDamageLOS(int x, int y)
        {
            if (this.cooledLavaTiles.ContainsKey(new Vector2(x, y)))
            {
                return false;
            }
            return base.BlocksDamageLOS(x, y);
        }

        protected override void initNetFields()
        {
            base.initNetFields();
            base.NetFields.AddField(this.coolLavaEvent).AddField(this.cooledLavaTiles.NetFields);
            this.coolLavaEvent.onEvent += OnCoolLavaEvent;

            terrainFeatures.OnValueAdded += (sender, added) =>
            {
                if (added is Grass grass)
                {
                    grass.grassType.Value = Grass.lavaGrass;
                    grass.loadSprite();
                }

            };

        }

        

        public override bool CanPlaceThisFurnitureHere(Furniture furniture)
        {
            return false;
        }

        public virtual void OnCoolLavaEvent(Point point)
        {
            this.CoolLava(point.X, point.Y);
            this.UpdateLavaNeighbor(point.X, point.Y);
            this.UpdateLavaNeighbor(point.X - 1, point.Y);
            this.UpdateLavaNeighbor(point.X + 1, point.Y);
            this.UpdateLavaNeighbor(point.X, point.Y - 1);
            this.UpdateLavaNeighbor(point.X, point.Y + 1);
        }

        public Dictionary<int, Point> GetBlobLookup()
        {
            if (UnderworldField._blobIndexLookup == null)
            {
                UnderworldField._blobIndexLookup = new Dictionary<int, Point>();
                UnderworldField._blobIndexLookup[0] = new Point(0, 0);
                UnderworldField._blobIndexLookup[6] = new Point(1, 0);
                UnderworldField._blobIndexLookup[14] = new Point(2, 0);
                UnderworldField._blobIndexLookup[10] = new Point(3, 0);
                UnderworldField._blobIndexLookup[7] = new Point(1, 1);
                UnderworldField._blobIndexLookup[11] = new Point(3, 1);
                UnderworldField._blobIndexLookup[5] = new Point(1, 2);
                UnderworldField._blobIndexLookup[13] = new Point(2, 2);
                UnderworldField._blobIndexLookup[9] = new Point(3, 2);
                UnderworldField._blobIndexLookup[2] = new Point(0, 1);
                UnderworldField._blobIndexLookup[3] = new Point(0, 2);
                UnderworldField._blobIndexLookup[1] = new Point(0, 3);
                UnderworldField._blobIndexLookup[4] = new Point(1, 3);
                UnderworldField._blobIndexLookup[12] = new Point(2, 3);
                UnderworldField._blobIndexLookup[8] = new Point(3, 3);
                UnderworldField._blobIndexLookup[15] = new Point(2, 1);
            }
            return UnderworldField._blobIndexLookup;
        }


        public Dictionary<int, Point> GetLavaBlobLookup()
        {
            if (UnderworldField._lavaBlobIndexLookup == null)
            {
                UnderworldField._lavaBlobIndexLookup = new Dictionary<int, Point>(this.GetBlobLookup());
                UnderworldField._lavaBlobIndexLookup[63] = new Point(2, 1);
                UnderworldField._lavaBlobIndexLookup[47] = new Point(4, 3);
                UnderworldField._lavaBlobIndexLookup[31] = new Point(4, 2);
                UnderworldField._lavaBlobIndexLookup[15] = new Point(4, 1);
            }
            return UnderworldField._lavaBlobIndexLookup;
        }


        public virtual void CoolLava(int x, int y, bool playSound = true)
        {
        
            if (Game1.currentLocation == this)
            {
                for (int i = 0; i < 5; i++)
                {
                    base.temporarySprites.Add(new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Microsoft.Xna.Framework.Rectangle(372, 1956, 10, 10), new Vector2(x, (float)y - 0.5f) * 64f + new Vector2(Game1.random.Next(64), Game1.random.Next(64)), flipped: false, 0.007f, Color.White)
                    {
                        alpha = 0.75f,
                        motion = new Vector2(0f, -1f),
                        acceleration = new Vector2(0.002f, 0f),
                        interval = 99999f,
                        layerDepth = 1f,
                        scale = 4f,
                        scaleChange = 0.02f,
                        rotationChange = (float)Game1.random.Next(-5, 6) * (float)Math.PI / 256f,
                        delayBeforeAnimationStart = i * 35
                    });
                }
                if (playSound && this.lavaSoundsPlayedThisTick < 3)
                {
                    DelayedAction.playSoundAfterDelay("steam", this.lavaSoundsPlayedThisTick * 300);
                    this.lavaSoundsPlayedThisTick++;
                }
            }
            if (!this.cooledLavaTiles.ContainsKey(new Vector2(x, y)))
            {
                this.cooledLavaTiles[new Vector2(x, y)] = true;
                //this.drawFloorDecorations();

                this.dofloordecor();

            }
        }

        public virtual void UpdateLavaNeighbor(int x, int y)
        {
            if (this.IsCooledLava(x, y))
            {
                base.setTileProperty(x, y, "Buildings", "Passable", "T");
                int neighbors = 0;
                if (this.IsCooledLava(x, y - 1))
                {
                    neighbors++;
                }
                if (this.IsCooledLava(x, y + 1))
                {
                    neighbors += 2;
                }
                if (this.IsCooledLava(x - 1, y))
                {
                    neighbors += 8;
                }
                if (this.IsCooledLava(x + 1, y))
                {
                    neighbors += 4;
                }
                if (this.GetBlobLookup().ContainsKey(neighbors))
                {
                    this.localCooledLavaTiles[new Vector2(x, y)] = this.GetBlobLookup()[neighbors];
                }
            }
        }

        public virtual bool IsCooledLava(int x, int y)
        {
            if (x < 0 || x >= this.mapWidth)
            {
                return false;
            }
            if (y < 0 || y >= this.mapHeight)
            {
                return false;
            }
            if (this.cooledLavaTiles.ContainsKey(new Vector2(x, y)))
            {
                return true;
            }
            return false;
        }

        


        protected override void resetLocalState()
        {
           





            foreach (Vector2 position in this.cooledLavaTiles.Keys)
            {
                this.UpdateLavaNeighbor((int)position.X, (int)position.Y);
            }
          
            base.resetLocalState();

            

            //this.mapBaseTilesheet = Game1.temporaryContent.Load<Texture2D>(base.map.TileSheets[0].ImageSource);
            Game1.ambientLight = Color.White;

        }

        protected override void resetSharedState()
        {
            base.resetSharedState();
            if (Game1.timeOfDay >= 2300)
            {
                base.waterColor.Value = Color.White;
            }
        }

        public override bool CanRefillWateringCanOnTile(int tileX, int tileY)
        {
           
            if (Game1.timeOfDay >= 2300)
            {
                return true;
            }
            return false;
        }



        public bool isTileOnClearAndSolidGround(Vector2 v)
        {
            if (base.map.GetLayer("Back").Tiles[(int)v.X, (int)v.Y] == null)
            {
                return false;
            }
            if (base.map.GetLayer("Front").Tiles[(int)v.X, (int)v.Y] != null || base.map.GetLayer("Buildings").Tiles[(int)v.X, (int)v.Y] != null)
            {
                return false;
            }
            return true;
        }






        public override bool sinkDebris(Debris debris, Vector2 chunkTile, Vector2 chunkPosition)
        {
            if (this.cooledLavaTiles.ContainsKey(chunkTile))
            {
                return false;
            }
            return base.sinkDebris(debris, chunkTile, chunkPosition);
        }

        public override bool performToolAction(Tool t, int tileX, int tileY)
        {
           

            if (t is WateringCan && base.isTileOnMap(new Vector2(tileX, tileY)) && base.getTileIndexAt(tileX, tileY, "Back") == 124) /*base.waterTiles[tileX, tileY])*/ //(Game1.timeOfDay !>= 2300 && */t is WateringCan && base.isTileOnMap(new Vector2(tileX, tileY)) /*&& base.waterTiles[tileX, tileY] */&& !this.cooledLavaTiles.ContainsKey(new Vector2(tileX, tileY)))
            {
                if (getTileIndexAt(tileX, tileY, "Back") == 124)
                {
                    this.coolLavaEvent.Fire(new Point(tileX, tileY));

                    if (Game1.currentLocation == this)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            base.temporarySprites.Add(new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Microsoft.Xna.Framework.Rectangle(372, 1956, 10, 10), new Vector2(tileX, (float)tileY - 0.5f) * 64f + new Vector2(Game1.random.Next(64), Game1.random.Next(64)), flipped: false, 0.007f, Color.White)
                            {
                                alpha = 0.75f,
                                motion = new Vector2(0f, -1f),
                                acceleration = new Vector2(0.002f, 0f),
                                interval = 99999f,
                                layerDepth = 1f,
                                scale = 4f,
                                scaleChange = 0.02f,
                                rotationChange = (float)Game1.random.Next(-5, 6) * (float)Math.PI / 256f,
                                delayBeforeAnimationStart = i * 35
                            });
                        }
                        if (this.lavaSoundsPlayedThisTick < 3)
                        {
                            DelayedAction.playSoundAfterDelay("steam", this.lavaSoundsPlayedThisTick * 300);
                            this.lavaSoundsPlayedThisTick++;
                        }
                    }
                    Vector2 tile = default(Vector2);
                    Point point = default(Point);
                    for (int y = Game1.viewport.Y / 64 - 1; y < (Game1.viewport.Y + Game1.viewport.Height) / 64 + 1; y++)
                    {
                        for (int x = Game1.viewport.X / 64 - 1; x < (Game1.viewport.X + Game1.viewport.Width) / 64 + 1; x++)
                        {
                            tile.X = x;
                            tile.Y = y;
                            if (this.localCooledLavaTiles.TryGetValue(tile, out point))
                            {
                                point.X += 5;
                                point.Y += 16;
                                Game1.spriteBatch.Draw(this.mooTilesheet, Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64)), new Microsoft.Xna.Framework.Rectangle(point.X * 16, point.Y * 16, 16, 16), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.55f);
                            }
                        }
                    }



                    Layer layer = map.GetLayer("Buildings");
                    layer.Tiles[tileX, tileY] = new StaticTile(layer: layer, tileSheet: map.GetTileSheet("z_volcano_dungeon"), tileIndex: 264, blendMode: BlendMode.Alpha);
                    Mod.coolinatedLava.Add(tile);

                    base.setTileProperty(tileX, tileY, "Buildings", "Passable", "T");

                }


            }
            return base.performToolAction(t, tileX, tileY);
        }


        public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character, bool pathfinding, bool projectile = false, bool ignoreCharacterRequirement = false)
        {
            if (isFarmer && !glider && (position.Left < 0 || position.Right > base.map.DisplayWidth || position.Top < 0 || position.Bottom > base.map.DisplayHeight))
            {
                return true;
            }
            return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character, pathfinding, projectile, ignoreCharacterRequirement);
        }


        public override void UpdateWhenCurrentLocation(GameTime time)
        {
            base.UpdateWhenCurrentLocation(time);
            this.coolLavaEvent.Poll();
            this.lavaSoundsPlayedThisTick = 0;
            
            
        }
        
        public void dofloordecor()
        {


            Vector2 tile = default(Vector2);
            Point point = default(Point);
            for (int y = Game1.viewport.Y / 64 - 1; y < (Game1.viewport.Y + Game1.viewport.Height) / 64 + 1; y++)
            {
                for (int x = Game1.viewport.X / 64 - 1; x < (Game1.viewport.X + Game1.viewport.Width) / 64 + 1; x++)
                {
                    tile.X = x;
                    tile.Y = y;
                    if (this.localCooledLavaTiles.TryGetValue(tile, out point))
                    {
                        point.X += 5;
                        point.Y += 16;
                        Game1.spriteBatch.Draw(this.mooTilesheet, Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64)), new Microsoft.Xna.Framework.Rectangle(point.X * 16, point.Y * 16, 16, 16), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.55f);
                    }
                }
            }



        }

        public override void drawFloorDecorations(SpriteBatch b)
        {
            base.drawFloorDecorations(b);
            Vector2 tile = default(Vector2);
            Point point = default(Point);
            for (int y = Game1.viewport.Y / 64 - 1; y < (Game1.viewport.Y + Game1.viewport.Height) / 64 + 1; y++)
            {
                for (int x = Game1.viewport.X / 64 - 1; x < (Game1.viewport.X + Game1.viewport.Width) / 64 + 1; x++)
                {
                    tile.X = x;
                    tile.Y = y;
                    if (this.localCooledLavaTiles.TryGetValue(tile, out point))
                    {
                        point.X += 5;
                        point.Y += 16;
                        b.Draw(this.mooTilesheet, Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64)), new Microsoft.Xna.Framework.Rectangle(point.X * 16, point.Y * 16, 16, 16), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.55f);
                    }
                }
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
            base.drawWater(b);
        }

        /*
        public override void drawWaterTile(SpriteBatch b, int x, int y)
        {
            drawWaterTile(b, x, y, Color.HotPink);
            base.drawWaterTile(b, x, y);
        }
        */



        

        public override void drawWaterTile(SpriteBatch b, int x, int y)
        {
           
           
            bool num = y == base.map.Layers[0].LayerHeight - 1 || !base.waterTiles[x, y + 1];
            bool topY = y == 0 || !base.waterTiles[x, y - 1];
            int water_tile_upper_left_x = 0;
            int water_tile_upper_left_y = 320;
            b.Draw(this.mooTilesheet, Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - (int)((!topY) ? base.waterPosition : 0f))), new Microsoft.Xna.Framework.Rectangle(water_tile_upper_left_x + base.waterAnimationIndex * 16, water_tile_upper_left_y + (((x + y) % 2 != 0) ? ((!base.waterTileFlip) ? 32 : 0) : (base.waterTileFlip ? 32 : 0)) + (topY ? ((int)base.waterPosition / 4) : 0), 16, 16 + (topY ? ((int)(0f - base.waterPosition) / 4) : 0)), base.waterColor.Value, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.56f);
            if (num)
            {
                b.Draw(this.mooTilesheet, Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, (y + 1) * 64 - (int)base.waterPosition)), new Microsoft.Xna.Framework.Rectangle(water_tile_upper_left_x + base.waterAnimationIndex * 16, water_tile_upper_left_y + (((x + (y + 1)) % 2 != 0) ? ((!base.waterTileFlip) ? 32 : 0) : (base.waterTileFlip ? 32 : 0)), 16, 16 - (int)(16f - base.waterPosition / 4f) - 1), base.waterColor.Value, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.56f);
            }
        }   
        
        private static void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            if(e.Name.IsEquivalentTo("Maps/UnderworldField")) 
{
                e.Edit(asset =>
            {
                var editor = asset.AsMap();

                Map sourceMap = UnderworldField.Helper.ModContent.Load<Map>("Assets/Maps/UnderworldField.tmx");
                editor.PatchMap(sourceMap, targetArea: new Microsoft.Xna.Framework.Rectangle(0, 0, 50, 60));
            });

            }

        }
            public static void LavaTileClear()
        {

            //if()
            foreach (KeyValuePair<xTile.Tiles.StaticTile,Vector2> tile in coolinatedLava)
            {
                int tileX = new int();
                int tileY = new int();

                xTile.Map map = new xTile.Map();
                Layer layer = map.GetLayer("Buildings");
                layer.Tiles[tileX, tileY] = new StaticTile(layer: layer, tileSheet: map.GetTileSheet("z_UnderworldTile"), tileIndex: 1193, blendMode: BlendMode.Alpha);


            }


            /*
            foreach (NPC character in this.characters)
            {
                if (character is Bat)
                {
                    character.Removed();
                }
            }*/

        }


            public bool isWaterTile(int xTile, int yTile)
        {
            return doesTileHaveProperty(xTile, yTile, "Water", "Back") != null;
        }



        /*
        public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
        {
            base.drawAboveAlwaysFrontLayer(b);
            if (!Game1.game1.takingMapScreenshot )
            {
                int col = 2;
                Microsoft.Xna.Framework.Rectangle tsarea = Game1.game1.GraphicsDevice.Viewport.GetTitleSafeArea();
                SpriteText.drawString(b, "Where the Hell Am I???", tsarea.Left + 16, tsarea.Top + 16, 999999, -1, 999999, 1f, 1f, junimoText: false, 2, "", col);
            }
        }*/

        public override void performTenMinuteUpdate(int timeOfDay)
        {
            base.performTenMinuteUpdate(timeOfDay);
            /*
            if (!(Game1.random.NextDouble() < 0.1) )
            {
                return;
            }
            int numsprites = 0;
            foreach (NPC character in base.characters)
            {
                if (character is Bat)
                {
                    numsprites++;
                }
            }
            if (numsprites < base.farmers.Count * 9)
            {
                this.spawnFlyingMonsterOffScreen();
            }
            if(Game1.timeOfDay >= 2300)
            {
                foreach (NPC character in base.characters)
                {
                    if (character is Bat)
                    {
                        character.Removed();
                    }
                }

            } */
        }

        /*
        public override void updateEvenIfFarmerIsntHere(GameTime time, bool skipWasUpdatedFlush = false)
        {
            base.updateEvenIfFarmerIsntHere(time, skipWasUpdatedFlush);
            if (Game1.timeOfDay >= 2400 || Game1.showingEndOfNightStuff)
            {
                foreach (NPC character in base.characters)
                {
                    if (character is Bat)
                    {
                        character.Removed();
                    }
                }
            }
        } */

        public void spawnFlyingMonsterOffScreen()
        {
            Vector2 spawnLocation = Vector2.Zero;
            switch (Game1.random.Next(4))
            {
                case 0:
                    spawnLocation.X = Game1.random.Next(base.map.Layers[0].LayerWidth);
                    break;
                case 3:
                    spawnLocation.Y = Game1.random.Next(base.map.Layers[0].LayerHeight);
                    break;
                case 1:
                    spawnLocation.X = base.map.Layers[0].LayerWidth - 1;
                    spawnLocation.Y = Game1.random.Next(base.map.Layers[0].LayerHeight);
                    break;
                case 2:
                    spawnLocation.Y = base.map.Layers[0].LayerHeight - 1;
                    spawnLocation.X = Game1.random.Next(base.map.Layers[0].LayerWidth);
                    break;
            }
            base.playSound("magma_sprite_spot");

         
            base.characters.Add(new Bat(spawnLocation, ( Game1.random.NextDouble() < 0.5) ? (-556) : (-555))
            {
                focusedOnFarmers = true
            });
        }

        public int GetHeight(int x, int y, int max_height)
        {
            if (x < 0 || x >= this.mapWidth || y < 0 || y >= this.mapHeight)
            {
                return max_height + 1;
            }
            return this.heightMap[x + y * this.mapWidth];
        }
     
        public static int GetTileIndex(int x, int y)
        {
            return x + y * 16;
        }

        public void SetTile(Layer layer, int x, int y, int index)
        {
            if (x >= 0 && x < layer.LayerWidth && y >= 0 && y < layer.LayerHeight)
            {
                Location location = new Location(x, y);
                layer.Tiles[location] = new StaticTile(layer, base.map.TileSheets[0], BlendMode.Alpha, index);
            }
        }

       



        public virtual void CleanUp()
        {
            if (!Game1.IsMasterGame)
            {
                return;
            }
            int i = 0;
            while (i < base.debris.Count)
            {
                Debris d = base.debris[i];
                if (d.isEssentialItem() && Game1.IsMasterGame && d.collect(Game1.player))
                {
                    base.debris.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        private static void OnDayEnding(object sender, DayEndingEventArgs e)
        {
            //loadMap("UnderworldField");
            //GameLocation location = new GameLocation("(Mod.instance.Helper.ModContent.GetInternalAssetName(\"assets/maps/UnderworldField.tmx\").BaseName)", (Mod.instance.Helper.ModContent.GetInternalAssetName("assets/maps/UnderworldField.tmx").BaseName));
          //location.loadMap(Mod.instance.Helper.ModContent.GetInternalAssetName("assets/maps/UnderworldField.tmx").BaseName);

            //Mod.instance.Helper.ModContent.Load<Map>("assets/maps/HellDungeonBoss.tmx"
            //Game1.game1.xTileContent.Load<Map>(Mod.instance.Helper.ModContent.GetInternalAssetName("assets/maps/UnderworldField.tmx").BaseName);
        }


        public override void performTouchAction(string full_action_string, Vector2 player_standing_position)
        {
            if (Game1.eventUp)
            {
                return;
            }
            if (full_action_string.Split(' ')[0] == "Ass")
            {
               
            }
            base.performTouchAction(full_action_string, player_standing_position);
        }







        









    }
}