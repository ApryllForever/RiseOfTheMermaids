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
using StardewValley.Locations;
using RestStopLocations.Extensions;
using RestStopLocations.Utilities;
using System.Runtime.CompilerServices;

namespace RestStopLocations.Game.Locations
{
    [XmlType("Mods_ApryllForever_RestStopLocations_SapphireForest")]
    public class SapphireForest : SapphireLocation
    {


        internal static IMonitor Monitor { get; set; }

        [XmlIgnore]
        public PerchingBirds birds;

        protected NetInt _currentState = new NetInt();

        public readonly NetLongDictionary<FarmAnimal, NetRef<FarmAnimal>> animals = new();
        public NetLongDictionary<FarmAnimal, NetRef<FarmAnimal>> Animals => animals;

        protected Color _ambientLightColor = Color.White;

        private List<Wisp> _wisps;

        private List<WeatherDebris> weatherDebris;

        protected Texture2D _rayTexture;

        protected Random _rayRandom;

        protected int _raySeed;

        private Texture2D swimShadow;

        private int swimShadowTimer;

        private int swimShadowFrame;

        

        public SapphireForest()
        {
        }

        [XmlIgnore]
        public readonly NetObjectList<FarmAnimal> Chickiedoos = new NetObjectList<FarmAnimal>();

        public SapphireForest(IModContentHelper content)
        : base(content, "SapphireForest", "SapphireForest")
        {

            long ass0 = 616969696969696900;
            Chickiedoos.Add(new FarmAnimal("Void Chicken", ass0, -1L));
            Chickiedoos[0].Position = new Vector2(57 * Game1.tileSize, 49 * Game1.tileSize);
            long ass1 = 616969696969696901;
            Chickiedoos.Add(new FarmAnimal("Void Chicken", ass1, -1L));
            Chickiedoos[1].Position = new Vector2(58 * Game1.tileSize, 48 * Game1.tileSize);
            long ass2 = 616969696969696902;
            Chickiedoos.Add(new FarmAnimal("Void Chicken", ass2, -1L));
            Chickiedoos[2].Position = new Vector2(59 * Game1.tileSize, 49 * Game1.tileSize);
            long ass3 = 616969696969696903;
            Chickiedoos.Add(new FarmAnimal("Goat", ass3, -1L));
            Chickiedoos[3].Position = new Vector2(55 * Game1.tileSize, 44 * Game1.tileSize);
            long ass4 = 616969696969696904;
            Chickiedoos.Add(new FarmAnimal("Goat", ass4, -1L));
            Chickiedoos[4].Position = new Vector2(50 * Game1.tileSize, 49 * Game1.tileSize);
            long ass5 = 616969696969696905;
            Chickiedoos.Add(new FarmAnimal("Pig", ass5, -1L));
            Chickiedoos[5].Position = new Vector2(50 * Game1.tileSize, 49 * Game1.tileSize);
            long ass6 = 616969696969696906;
            Chickiedoos.Add(new FarmAnimal("Sheep", ass6, -1L));
            Chickiedoos[6].Position = new Vector2(50 * Game1.tileSize, 49 * Game1.tileSize);
            long ass7 = 616969696969696907;
            Chickiedoos.Add(new FarmAnimal("Sheep", ass7, -1L));
            Chickiedoos[7].Position = new Vector2(50 * Game1.tileSize, 49 * Game1.tileSize);

            {
                int x = 112;
                int y = 18;
                Vector2 objectPos = new Vector2(x, y);
                StardewValley.Object o = new StardewValley.Object(objectPos, "10");
               
                //StardewValley.Object o = new StardewValley.Object(166, 1); //Treasure Chest
                o.bigCraftable.Value = true;
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = false;
                Objects.Add(objectPos, o);
            }
            {
                int x = 112;
                int y = 19;
                Vector2 objectPos = new Vector2(x, y);
               
                StardewValley.Object o = new StardewValley.Object(objectPos, "10");
               
               
                //StardewValley.Object o = new StardewValley.Object(166, 1); //Treasure Chest
                o.bigCraftable.Value = true;
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = false;
                Objects.Add(objectPos, o);
            }
            {
                int x = 112;
                int y = 20;
                Vector2 objectPos = new Vector2(x, y); 
                StardewValley.Object o = new StardewValley.Object(objectPos, "10");

                //StardewValley.Object o = new StardewValley.Object(166, 1); //Treasure Chest
                o.bigCraftable.Value = true;
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = false;
                Objects.Add(objectPos, o);
            }
            {
                int x = 112;
                int y = 21;
                Vector2 objectPos = new Vector2(x, y);
                StardewValley.Object o = new StardewValley.Object(objectPos, "10");
               
                //StardewValley.Object o = new StardewValley.Object(166, 1); //Treasure Chest
                o.bigCraftable.Value = true;
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = false;
                Objects.Add(objectPos, o);
            }
            {
                int x = 112;
                int y = 22;
                Vector2 objectPos = new Vector2(x, y);
                StardewValley.Object o = new StardewValley.Object(objectPos, "10");

                //StardewValley.Object o = new StardewValley.Object(166, 1); //Treasure Chest
                o.bigCraftable.Value = true;
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = false;
                Objects.Add(objectPos, o);
            }
            {
                int x = 112;
                int y = 23;
                Vector2 objectPos = new Vector2(x, y);
                StardewValley.Object o = new StardewValley.Object(objectPos, "10");

                //StardewValley.Object o = new StardewValley.Object(166, 1); //Treasure Chest
                o.bigCraftable.Value = true;
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = false;
                Objects.Add(objectPos, o);
            }
            {
                int x = 112;
                int y = 24;
                Vector2 objectPos = new Vector2(x, y);
                StardewValley.Object o = new StardewValley.Object(objectPos, "10");
                
                //StardewValley.Object o = new StardewValley.Object(166, 1); //Treasure Chest
                o.bigCraftable.Value = true;
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = false;
                Objects.Add(objectPos, o);
            }


            //LoadObjects();
        }

        public override bool SeedsIgnoreSeasonsHere()
        {
            return true;
        }
        public void addDesertButterflies(double chance, bool onlyIfOnScreen = false)
        {
            bool island_location = this.InIslandContext();
            //bool firefly = (Game1.currentSeason.Equals("summer") | island_location) && Game1.isDarkOut();
            if ((Game1.timeOfDay >= 2500) )
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
                    critters.Add(new DesertButterfly(v, island_location));
                }
                while (Game1.random.NextDouble() < 0.4)
                {
                    {
                        critters.Add(new DesertButterfly(v + new Vector2(Game1.random.Next(-2, 3), Game1.random.Next(-2, 3)), island_location));
                    }
                }
            }
        }
        public override void updateSeasonalTileSheets(Map map = null)
        {
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
            base.tryToAddCritters(onlyIfOnScreen);
        }
        protected override void initNetFields()
        {
            base.initNetFields();
            NetFields.AddField(this.Animals).AddField(Chickiedoos);

        }


       

        private bool plantGenerated = false;
        public void plantGenerating()
        {
            plantGenerated = true;

            Vector2[] iHaveNoMoreFucksToGive = new Vector2[]
            {
                new Vector2(114, 18),
                new Vector2(114, 19),
                new Vector2(114, 20),
                new Vector2(114, 21),
                new Vector2(114, 22),
                new Vector2(114, 23),
                new Vector2(115, 18),
                new Vector2(115, 19),
                new Vector2(115, 20),
                new Vector2(115, 21),
                new Vector2(115, 22),
                new Vector2(115, 23),
                new Vector2(116, 18),
                new Vector2(116, 19),
                new Vector2(116, 20),
                new Vector2(116, 21),
            };

            foreach (var fuckme in iHaveNoMoreFucksToGive)
            {
                AddCropIfNeccessary(fuckme, "427");
            }

            AddCropIfNeccessary(new Vector2(116, 22), "453");
            AddCropIfNeccessary(new Vector2(116, 23), "425");
        }

        private void AddCropIfNeccessary(Vector2 tilePosition, string cropId)
        {
            

            if (!isCropAtTile((int)tilePosition.X, (int)tilePosition.Y))
            {
                terrainFeatures[tilePosition] = new HoeDirt(1, new Crop(false, cropId, -1, -1, this));
            }
        }
        //public Crop(bool forageCrop, string which, int tileX, int tileY, GameLocation location)
        protected override void resetLocalState()
        {
            
            Vector2 creakysounds = new Vector2(50, 7);
            _raySeed = (int)Game1.currentGameTime.TotalGameTime.TotalMilliseconds;
            _rayTexture = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\LightRays");
            _ambientLightColor = new Color(150, 120, 50);
            ignoreOutdoorLighting.Value = false;
            //this.seasonOverride = "spring";
         
            //Game1.currentLocation.localSoundAt("doorCreak", creakysounds);
            base.resetLocalState();

            //AmbientishLocationSounds.addSound(new Vector2(113f, 7f), 2);

            //AmbientesqueLocationSounds.addSound(new Vector2(13f, 7f), 2);

            //LocationSounds sound = new LocationSounds(creakysounds, 2);

            {
                int x = 112;
                int y = 24;
                Vector2 objectPos = new Vector2(x, y);
                StardewValley.Object o = new StardewValley.Object(objectPos, "10");

                //StardewValley.Object o = new StardewValley.Object(166, 1); //Treasure Chest
                o.bigCraftable.Value = true;
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = false;
                Objects.TryAdd(objectPos, o);
            }
            {
                int x = 112;
                int y = 23;
                Vector2 objectPos = new Vector2(x, y);
                StardewValley.Object o = new StardewValley.Object(objectPos, "10");
                
                //StardewValley.Object o = new StardewValley.Object(166, 1); //Treasure Chest
                o.bigCraftable.Value = true;
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = false;
                Objects.TryAdd(objectPos, o);
            }
            {
                int x = 112;
                int y = 22;
                Vector2 objectPos = new Vector2(x, y);
                StardewValley.Object o = new StardewValley.Object(objectPos, "10");
              
                //StardewValley.Object o = new StardewValley.Object(166, 1); //Treasure Chest
                o.bigCraftable.Value = true;
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = false;
                Objects.TryAdd(objectPos, o);
            }
            {
                int x = 112;
                int y = 21;
                Vector2 objectPos = new Vector2(x, y);
                StardewValley.Object o = new StardewValley.Object(objectPos, "10");
               
                //StardewValley.Object o = new StardewValley.Object(166, 1); //Treasure Chest
                o.bigCraftable.Value = true;
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = false;
                Objects.TryAdd(objectPos, o);
            }
            {
                int x = 112;
                int y = 20;
                Vector2 objectPos = new Vector2(x, y);
                StardewValley.Object o = new StardewValley.Object(objectPos, "10");
              
                //StardewValley.Object o = new StardewValley.Object(166, 1); //Treasure Chest
                o.bigCraftable.Value = true;
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = false;
                Objects.TryAdd(objectPos, o);
            }
            {
                int x = 112;
                int y = 19;
                Vector2 objectPos = new Vector2(x, y);
                StardewValley.Object o = new StardewValley.Object(objectPos, "10");
               
                //StardewValley.Object o = new StardewValley.Object(166, 1); //Treasure Chest
                o.bigCraftable.Value = true;
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = false;
                Objects.TryAdd(objectPos, o);
            }
            {
                int x = 112;
                int y = 18;
                Vector2 objectPos = new Vector2(x, y);
                StardewValley.Object o = new StardewValley.Object(objectPos, "10");
               
                //StardewValley.Object o = new StardewValley.Object(166, 1); //Treasure Chest
                o.bigCraftable.Value = true;
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = false;
                Objects.TryAdd(objectPos, o);
            }
            {
                int x = 113;
                int y = 24;
                Vector2 objectPos = new Vector2(x, y);
                StardewValley.Object o = new StardewValley.Object(objectPos, "10");
               
                //StardewValley.Object o = new StardewValley.Object(166, 1); //Treasure Chest
                o.bigCraftable.Value = true;
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = false;
                Objects.TryAdd(objectPos, o);
            }
            {
                int x = 114;
                int y = 24;
                Vector2 objectPos = new Vector2(x, y);
                StardewValley.Object o = new StardewValley.Object(objectPos, "10");
               
                //StardewValley.Object o = new StardewValley.Object(166, 1); //Treasure Chest
                o.bigCraftable.Value = true;
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = false;
                Objects.TryAdd(objectPos, o);
            }
            {
                int x = 115;
                int y = 24;
                Vector2 objectPos = new Vector2(x, y);
                StardewValley.Object o = new StardewValley.Object(objectPos, "10");
               
                //StardewValley.Object o = new StardewValley.Object(166, 1); //Treasure Chest
                o.bigCraftable.Value = true;
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = false;
                Objects.TryAdd(objectPos, o);
            }
            {
                int x = 116;
                int y = 24;
                Vector2 objectPos = new Vector2(x, y);
                StardewValley.Object o = new StardewValley.Object(objectPos, "10");
               
                //StardewValley.Object o = new StardewValley.Object(166, 1); //Treasure Chest
                o.bigCraftable.Value = true;
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = false;
                Objects.TryAdd(objectPos, o);
            }
            {
                int x = 117;
                int y = 24;
                Vector2 objectPos = new Vector2(x, y);
                StardewValley.Object o = new StardewValley.Object(objectPos, "10");
               
                //StardewValley.Object o = new StardewValley.Object(166, 1); //Treasure Chest
                o.bigCraftable.Value = true;
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = false;
                Objects.TryAdd(objectPos, o);
            }


            if (!plantGenerated)
            {
                plantGenerating();
            }

            swimShadow = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\swimShadow");

            _updateWoodsLighting();
            new Random((int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame / 2);
            _wisps = new List<Wisp>();
            for (int i = 0; i < 30; i++)
            {
                Wisp wisp = new Wisp(i);
                _wisps.Add(wisp);
            }
            weatherDebris = new List<WeatherDebris>();
            int spacing = 192;
            int leafType = 3;
            for (int j = 0; j < 10; j++)
            {
                weatherDebris.Add(new WeatherDebris(new Vector2(j * spacing % Game1.graphics.GraphicsDevice.Viewport.Width + Game1.random.Next(spacing), j * spacing / Game1.graphics.GraphicsDevice.Viewport.Width * spacing % Game1.graphics.GraphicsDevice.Viewport.Height + Game1.random.Next(spacing)), leafType, (float)Game1.random.Next(15) / 500f, (float)Game1.random.Next(-10, 0) / 50f, (float)Game1.random.Next(10) / 50f));
            }

           
            //if ((Game1.timeOfDay < 1800))
            //{
            // Game1.ambientLight = Color.White;
            //}

            waterColor.Value = new Color(130, 80, 255) * 0.5f;
            int numSeagulls = Game1.random.Next(6);
            foreach (Vector2 tile in Utility.getPositionsInClusterAroundThisTile(new Vector2(Game1.random.Next(map.DisplayWidth / 64), Game1.random.Next(12, map.DisplayHeight / 64)), numSeagulls))
            {
                if (isTileOnMap(tile) && (CanItemBePlacedHere(tile) || doesTileHaveProperty((int)tile.X, (int)tile.Y, "Water", "Back") != null))
                {
                    int state = 3;
                    if (doesTileHaveProperty((int)tile.X, (int)tile.Y, "Water", "Back") != null)
                    {
                        state = 2;
                        if (Game1.random.NextDouble() < 0.5)
                        {
                            continue;
                        }
                    }
                    critters.Add(new Seagull(tile * 64f + new Vector2(32f, 32f), state));
                }
            }
            var v1 = new Vector2(53);
            var v2 = new Vector2(22);
            var v3 = new Vector2(34);
            var v4 = new Vector2(67);
            var v5 = new Vector2(76);
            var v6 = new Vector2(51);
            addCritter(new Crow((int)v1.X, (int)v2.Y)); 
            //addCritter(new Crow((int)v3.X, (int)v4.Y));
            //addCritter(new Crow((int)v5.X, (int)v6.Y));
            addCritter(new CrabCritter(new Vector2(0f, 104f) * 64f));
            addCritter(new CrabCritter(new Vector2(76f, 45f) * 64f));
            addCritter(new CrabCritter(new Vector2(73f, 48f) * 64f));
            addCritter(new CrabCritter(new Vector2(28f, 113f) * 64f));
            addCritter(new CrabCritter(new Vector2(62f, 117f) * 64f));
            addCritter(new CrabCritter(new Vector2(55f, 72f) * 64f));
            addCritter(new CrabCritter(new Vector2(64f, 53f) * 64f));
            addCritter(new CrabCritter(new Vector2(31f, 40f) * 64f));
           /* birds = new PerchingBirds(Game1.birdsSpriteSheet, 2, 16, 16, new Vector2(28f, 113f), new Point[3]
                {
                new Point(8, 55),
                new Point(31, 90),
                new Point(14, 64)
                },
                new Point[1]
                {
                new Point(20, 87)
                }
                );
            {
                birds.roosting = (_currentState.Value == 2);
                for (int i = 0; i < Game1.random.Next(2, 5); i++)
                {
                    int bird_type = Game1.random.Next(0, 4);
                    if (Game1.currentSeason == "fall")
                    {
                        bird_type = 10;
                    }
                    birds.AddBird(bird_type);
                }
                if (Game1.timeOfDay > 2100 && Game1.random.NextDouble() < 0.5)
                {
                    birds.AddBird(11);
                }
            }*/

        }

        public override void cleanupBeforePlayerExit()
        {
            if (_wisps != null)
            {
                _wisps.Clear();
            }
            if (weatherDebris != null)
            {
                weatherDebris.Clear();
            }
            base.cleanupBeforePlayerExit();
            if (Game1.player.swimming.Value)
            {
                Game1.player.swimming.Value = false;
            }
            if (Game1.locationRequest != null && !Game1.locationRequest.Name.Contains("BathHouse"))
            {
                Game1.player.bathingClothes.Value = false;
            }


        }
        new public void updateWater(GameTime time)
        {
            waterAnimationTimer -= time.ElapsedGameTime.Milliseconds;
            if (waterAnimationTimer <= 0)
            {
                waterAnimationIndex = (waterAnimationIndex + 1) % 10;
                waterAnimationTimer = 200;
            }
            if (!isFarm)
            {
                waterPosition += (float)((Math.Sin((float)time.TotalGameTime.Milliseconds / 1000f) + 1.0) * 0.15000000596046448);
            }
            else
            {
                waterPosition += 0.1f;
            }
            if (waterPosition >= 64f)
            {
                waterPosition -= 64f;
                waterTileFlip = !waterTileFlip;
            }
        }
        public override void TransferDataFromSavedLocation(GameLocation l)
        {
            var other = l as SapphireForest;
           // Chickiedoos.MoveFrom(other.Chickiedoos);
            //foreach (FarmAnimal item in Chickiedoos)
            //{
              //  item.reload(null);
            //}

            Animals.MoveFrom(other.Animals);
            foreach (var animal in Animals.Values)
            {
                animal.reload(null);
            }
            base.TransferDataFromSavedLocation(l);
        }

        public bool CheckInspectAnimal(Vector2 position, Farmer who)
        {
            foreach (FarmAnimal item in Chickiedoos)
            {
                if (item.wasPet.Value && item.GetCursorPetBoundingBox().Contains((int)position.X, (int)position.Y))
                {
                    item.pet(who);
                    return true;
                }
            }


            foreach (var animal in Animals.Values)
            {
                if (animal.wasPet.Value && animal.GetCursorPetBoundingBox().Contains((int)position.X, (int)position.Y))
                {
                    animal.pet(who);
                    return true;
                }
            }

            return false;
        }

        public bool CheckInspectAnimal(Microsoft.Xna.Framework.Rectangle rect, Farmer who)
        {
            foreach (FarmAnimal item in Chickiedoos)
            {
                if (item.wasPet.Value && item.GetBoundingBox().Intersects(rect))
                {
                    item.pet(who);
                    return true;
                }
            }
            foreach (var animal in Animals.Values)
            {
                if (animal.wasPet.Value && animal.GetBoundingBox().Intersects(rect))
                {
                    animal.pet(who);
                    return true;
                }
            }

            return false;
        }

        public bool CheckPetAnimal(Vector2 position, Farmer who)
        {
            foreach (FarmAnimal item in Chickiedoos)
            {
                if (item.wasPet.Value && item.GetCursorPetBoundingBox().Contains((int)position.X, (int)position.Y))
                {
                    item.pet(who);
                    return true;
                }
            }


            foreach (var animal in Animals.Values)
            {
                if (!animal.wasPet.Value && animal.GetCursorPetBoundingBox().Contains((int)position.X, (int)position.Y))
                {
                    animal.pet(who);
                    return true;
                }
            }

            return false;
        }

        public bool CheckPetAnimal(Microsoft.Xna.Framework.Rectangle rect, Farmer who)
        {
            foreach (FarmAnimal item in Chickiedoos)
            {
                if (item.wasPet.Value && item.GetBoundingBox().Intersects(rect))
                {
                    item.pet(who);
                    return true;
                }
            }
                foreach (var animal in Animals.Values)
            {
                if (!animal.wasPet.Value && animal.GetBoundingBox().Intersects(rect))
                {
                    animal.pet(who);
                    return true;
                }
            }

            return false;
        }

        public override void DayUpdate(int dayOfMonth)
        {
            for (int i = this.animals.Count() - 1; i >= 0; i--)
            {
                var chickiedooAnimal = this.Chickiedoos[i];
                var animal = this.animals.Pairs.ElementAt(i).Value;
               
                    animal.dayUpdate(this);
            }
            base.DayUpdate(dayOfMonth);
        }

        public override void performTenMinuteUpdate(int timeOfDay)
        {
            base.performTenMinuteUpdate(timeOfDay);
            if (Game1.IsMasterGame)
            {
                foreach (FarmAnimal item in this.Chickiedoos)
                {
                    item.updatePerTenMinutes(Game1.timeOfDay, this);
                }
                foreach (FarmAnimal value in this.animals.Values)
                {
                    value.updatePerTenMinutes(Game1.timeOfDay, this);
                }
            }
        }

        public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character, bool pathfinding, bool projectile = false, bool ignoreCharacterRequirement = false)
        {
            if (!glider)
            {
                if (character != null && !(character is FarmAnimal))
                {
                    Microsoft.Xna.Framework.Rectangle playerBox = Game1.player.GetBoundingBox();
                    Farmer farmer = (isFarmer ? (character as Farmer) : null);

                    foreach (FarmAnimal item in this.Chickiedoos)
                    {
                        if (position.Intersects(item.GetBoundingBox()) && (!isFarmer || !playerBox.Intersects(item.GetBoundingBox())))
                        {
                            if (farmer != null && farmer.TemporaryPassableTiles.Intersects(position))
                            {
                                break;
                            }
                            item.farmerPushing();
                            return true;
                        }
                    }
                    foreach (FarmAnimal animal in this.animals.Values)
                    {
                        if (position.Intersects(animal.GetBoundingBox()) && (!isFarmer || !playerBox.Intersects(animal.GetBoundingBox())))
                        {
                            if (farmer != null && farmer.TemporaryPassableTiles.Intersects(position))
                            {
                                break;
                            }
                            animal.farmerPushing();
                            return true;
                        }
                    }
                }
            }
            return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character, pathfinding, projectile, ignoreCharacterRequirement);
        }




        public bool isTileOpenBesidesTerrainFeatures(Vector2 tile)
        {

           
            foreach (FarmAnimal item in Chickiedoos)
            {
                 if (item.Position.Equals(tile))
                {
                    return true;
                }
            } 

            foreach (KeyValuePair<long, FarmAnimal> pair in base.animals.Pairs)
            {
                if (pair.Value.Tile == tile)
                {
                    return true;
                }
            }
            base.isTilePassable(new Location((int)tile.X, (int)tile.Y), Game1.viewport);
            return true;
        }


        public override void draw(SpriteBatch b)
        {
            base.draw(b);
            if (_wisps != null)
            {
                for (int i = 0; i < _wisps.Count; i++)
                {
                    _wisps[i].Draw(b);
                }
            }


            {
                if (currentEvent != null)
                {
                    foreach (NPC j in currentEvent.actors)
                    {
                        if ((bool)j.swimming.Value)
                        {
                            b.Draw(swimShadow, Game1.GlobalToLocal(Game1.viewport, j.Position + new Vector2(0f, j.Sprite.SpriteHeight / 3 * 4 + 4)), new Microsoft.Xna.Framework.Rectangle(swimShadowFrame * 16, 0, 16, 16), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);
                        }
                    }
                }
                else
                {
                    foreach (NPC i in characters)
                    {
                        if ((bool)i.swimming.Value)
                        {
                            b.Draw(swimShadow, Game1.GlobalToLocal(Game1.viewport, i.Position + new Vector2(0f, i.Sprite.SpriteHeight / 3 * 4 + 4)), new Microsoft.Xna.Framework.Rectangle(swimShadowFrame * 16, 0, 16, 16), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);
                        }
                    }
                    foreach (Farmer f in farmers)
                    {
                        if ((bool)f.swimming.Value)
                        {
                            b.Draw(swimShadow, Game1.GlobalToLocal(Game1.viewport, f.Position + new Vector2(0f, f.Sprite.SpriteHeight / 4 * 4)), new Microsoft.Xna.Framework.Rectangle(swimShadowFrame * 16, 0, 16, 16), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);
                        }
                    }
                }
                _ = (bool)Game1.player.swimming.Value;

                foreach (FarmAnimal item in Chickiedoos)
                {
                    item.draw(b);
                }

                foreach (KeyValuePair<long, FarmAnimal> pair in this.animals.Pairs)
                {
                    pair.Value.draw(b);
                }
            }
        }
        public override void updateEvenIfFarmerIsntHere(GameTime time, bool skipWasUpdatedFlush = false)
        {
            base.updateEvenIfFarmerIsntHere(time, skipWasUpdatedFlush);
            foreach (FarmAnimal item in Chickiedoos)
            {
                item.updateWhenNotCurrentLocation(null, time, this);
            }
            if (!Game1.currentLocation.Equals(this))
            {
                NetDictionary<long, FarmAnimal, NetRef<FarmAnimal>, SerializableDictionary<long, FarmAnimal>, NetLongDictionary<FarmAnimal, NetRef<FarmAnimal>>>.PairsCollection pairs = this.animals.Pairs;
                for (int i = pairs.Count() - 1; i >= 0; i--)
                {
                    pairs.ElementAt(i).Value.updateWhenNotCurrentLocation(null, time, this);
                }
            }
        }

        public override void UpdateWhenCurrentLocation(GameTime time)
        {
            base.UpdateWhenCurrentLocation(time);
            AmbientishLocationSounds.update(time);
            AmbientesqueLocationSounds.update(time);
            LocationSounds.update(time);    

            if (swimShadowTimer <= 0)
            {
                swimShadowTimer = 70;
                swimShadowFrame++;
                swimShadowFrame %= 10;
            }

            foreach (FarmAnimal item in Chickiedoos)
            {
                item.updateWhenCurrentLocation(time, this);
            }
            foreach (KeyValuePair<long, FarmAnimal> kvp in this.Animals.Pairs)
            {
                kvp.Value.updateWhenCurrentLocation(time, this);
            }
            _updateWoodsLighting();
            if (_wisps != null)
            {
                for (int i = 0; i < _wisps.Count; i++)
                {
                    _wisps[i].Update(time);
                }
            }
            if (weatherDebris != null)
            {
                foreach (WeatherDebris weatherDebri in weatherDebris)
                {
                    weatherDebri.update();
                }
                Game1.updateDebrisWeatherForMovement(weatherDebris);
            }
        }
        protected void _updateWoodsLighting()
        {
            if (Game1.currentLocation == this)
            {
                int fade_start_time = Utility.ConvertTimeToMinutes(Game1.getModeratelyDarkTime()) - 60;
                int fade_end_time = Utility.ConvertTimeToMinutes(Game1.getTrulyDarkTime());
                int light_fade_start_time = Utility.ConvertTimeToMinutes(Game1.getStartingToGetDarkTime());
                int light_fade_end_time = Utility.ConvertTimeToMinutes(Game1.getModeratelyDarkTime());
                float num = (float)Utility.ConvertTimeToMinutes(Game1.timeOfDay) + (float)Game1.gameTimeInterval / 7000f * 10f;
                float lerp = Utility.Clamp((num - (float)fade_start_time) / (float)(fade_end_time - fade_start_time), 0f, 1f);
                float light_lerp = Utility.Clamp((num - (float)light_fade_start_time) / (float)(light_fade_end_time - light_fade_start_time), 0f, 1f);
                Game1.ambientLight.R = (byte)Utility.Lerp((int)_ambientLightColor.R, (int)Game1.eveningColor.R, lerp);
                Game1.ambientLight.G = (byte)Utility.Lerp((int)_ambientLightColor.G, (int)Game1.eveningColor.G, lerp);
                Game1.ambientLight.B = (byte)Utility.Lerp((int)_ambientLightColor.B, (int)Game1.eveningColor.B, lerp);
                Game1.ambientLight.A = (byte)Utility.Lerp((int)_ambientLightColor.A, (int)Game1.eveningColor.A, lerp);
                Color light_color = Color.Black;
                light_color.A = (byte)Utility.Lerp(255f, 0f, light_lerp);
                foreach (LightSource light in Game1.currentLightSources)
                {
                    if (light.lightContext.Value == LightSource.LightContext.MapLight)
                    {
                        light.color.Value = light_color;
                    }
                }
            }
        }
        static string EnterDungeon = "Do you wish to enter this forboding gate?";

        public override bool performAction(string action, Farmer who, Location tileLocation)
        {
            if (action == "RSDungeonEntrance")
            {
                createQuestionDialogue(EnterDungeon, createYesNoResponses(), "HellDungeonEntrance");
            }
            if (action == "RSDeath")
            {
                Game1.playSound("cacklingWitch");
                Game1.screenOverlayTempSprites.AddRange(Utility.sparkleWithinArea(new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), 500, Color.Lime, 10, 2000));
                Game1.player.health = 0;
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

        public override void performTouchAction(string actionStr, Vector2 tileLocation)
        {
            string[] split = actionStr.Split(' ');
            string action = split[0];
            int tx = (int)tileLocation.X;
            int ty = (int)tileLocation.Y;

            if (action == "RS.Diveboard")
            {
                Game1.player.jump();
                Game1.player.xVelocity = -16f;
                Game1.player.swimTimer = 800;
                Game1.player.swimming.Value = true;
                Game1.playSound("pullItemFromWater");
                Game1.playSound("bubbles");
            }

            if (action == "RS.jumpup")
            {
                Game1.player.changeIntoSwimsuit();
                Game1.player.jump();
                Game1.player.yVelocity = -2f;
                Game1.player.swimTimer = 800;
                Game1.player.swimming.Value = true;
                Game1.playSound("pullItemFromWater");
                Game1.playSound("bubbles");
            }
            if (action == "RS.jumpdown")
            {
                Game1.player.changeIntoSwimsuit();
                Game1.player.jump();
                Game1.player.yVelocity = 2f;
                Game1.player.swimTimer = 800;
                Game1.player.swimming.Value = true;
                Game1.playSound("pullItemFromWater");
                Game1.playSound("bubbles");
            }
            if (action == "RS.jumpright")
            {
                Game1.player.changeIntoSwimsuit();
                Game1.player.jump();
                Game1.player.xVelocity = 2f;
                Game1.player.swimTimer = 800;
                Game1.player.swimming.Value = true;
                Game1.playSound("pullItemFromWater");
                Game1.playSound("bubbles");
            }
            if (action == "RS.jumpleft")
            {
                Game1.player.changeIntoSwimsuit();
                Game1.player.jump();
                Game1.player.xVelocity = -2f;
                Game1.player.swimTimer = 800;
                Game1.player.swimming.Value = true;
                Game1.playSound("pullItemFromWater");
                Game1.playSound("bubbles");
            }


            base.performTouchAction(actionStr, tileLocation);
        }


        public static void UpdateLevels10Minutes(int time)
        {
            if (Game1.IsClient)
                return;

           
        }

        public void CreakyNoise()
        {
            Vector2 creakysounds = new Vector2(113, 7);

            AmbientishLocationSounds.addSound(new Vector2(113f, 7f), 2);
            Game1.currentLocation.localSound("doorCreak", creakysounds);
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

        public override void drawWaterTile(SpriteBatch b, int x, int y)
        {
            drawWaterTile(b, x, y, this.waterColor.Value);
            base.drawWaterTile(b, x, y);
        }

        new public void drawWaterTile(SpriteBatch b, int x, int y, Color color)
        {
            bool num = y == map.Layers[0].LayerHeight - 1 || !waterTiles[x, y + 1];
            bool topY = y == 0 || !waterTiles[x, y - 1];
            b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - (int)((!topY) ? waterPosition : 0f))), new Microsoft.Xna.Framework.Rectangle(waterAnimationIndex * 64, 2064 + (((x + y) % 2 != 0) ? ((!waterTileFlip) ? 128 : 0) : (waterTileFlip ? 128 : 0)) + (topY ? ((int)waterPosition) : 0), 64, 64 + (topY ? ((int)(0f - waterPosition)) : 0)), color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.56f);
            if (num)
            {
                b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, (y + 1) * 64 - (int)waterPosition)), new Microsoft.Xna.Framework.Rectangle(waterAnimationIndex * 64, 2064 + (((x + (y + 1)) % 2 != 0) ? ((!waterTileFlip) ? 128 : 0) : (waterTileFlip ? 128 : 0)), 64, 64 - (int)(64f - waterPosition) - 1), color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.56f);
            }
        }
        
        public virtual void DrawRays(SpriteBatch b)
        {
            Random random = new Random(_raySeed);
            float zoom = (float)Game1.graphics.GraphicsDevice.Viewport.Height * 0.6f / 128f;
            int num = -(int)(128f / zoom);
            int max = Game1.graphics.GraphicsDevice.Viewport.Width / (int)(32f * zoom);
            for (int i = num; i < max; i++)
            {
                Color color = Color.White;
                float deg2 = (float)Game1.viewport.X * Utility.RandomFloat(0.75f, 1f, random) + (float)Game1.viewport.Y * Utility.RandomFloat(0.2f, 0.5f, random) + (float)Game1.currentGameTime.TotalGameTime.TotalSeconds * 20f;
                _ = deg2 / 360f;
                deg2 %= 360f;
                float rad = deg2 * ((float)Math.PI / 180f);
                color *= Utility.Clamp((float)Math.Sin(rad), 0f, 1f) * Utility.RandomFloat(0.15f, 0.4f, random);
                float offset = Utility.Lerp(0f - Utility.RandomFloat(24f, 32f, random), 0f, deg2 / 360f);
                b.Draw(_rayTexture, new Vector2(((float)(i * 32) - offset) * zoom, Utility.RandomFloat(0f, -32f * zoom, random)), new Microsoft.Xna.Framework.Rectangle(128 * random.Next(0, 2), 0, 128, 128), color, 0f, Vector2.Zero, zoom, SpriteEffects.None, 1f);
            }
        }

        public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
        {
            base.drawAboveAlwaysFrontLayer(b);
            DrawRays(b);
        }

        /*public override void spawnObjects()
        {
            base.spawnObjects();
            StardewValley.Object o = new StardewValley.Object(10, 1);
            int x = 112;
            int y = 18;
            Vector2 objectPos = new Vector2(x, y);
            //StardewValley.Object o = new StardewValley.Object(166, 1); //Treasure Chest
            o.bigCraftable.Value = true;
            o.IsSpawnedObject = true;
            o.CanBeGrabbed = true;
            Objects.Add(objectPos, o);


        }
        public void LoadObjects()
        {
            StardewValley.Object o = new StardewValley.Object(10, 1);
            int x = 112;
            int y = 18;
            Vector2 objectPos = new Vector2(x, y);
            //StardewValley.Object o = new StardewValley.Object(166, 1); //Treasure Chest
            o.bigCraftable.Value = true;
            o.IsSpawnedObject = true;
            o.CanBeGrabbed = true;
            Objects.Add(objectPos, o);

        }*/

    }
}