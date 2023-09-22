using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using xTile.Dimensions;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;



namespace RestStopLocations.Game.Locations
{
    [XmlType("Mods_ApryllForever_RestStopLocations_AspenCanyon")]
    public class AspenCanyon : AspenLocation
    {


        internal static IMonitor Monitor { get; set; }

        protected NetInt _currentState = new NetInt();

        protected Color _ambientLightColor = Color.White;

        private List<Wisp> _wisps;

        private List<WeatherDebris> weatherDebris;

        protected Texture2D _rayTexture;

        protected Random _rayRandom;

        protected int _raySeed;

        bool assfuck = false;



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

        public AspenCanyon()
        {
        }

        public AspenCanyon(IModContentHelper content)
        : base(content, "AspenCanyon", "AspenCanyon")
        {
           
                
                updateSeasonalTileSheets();

        }

        public override void checkForMusic(GameTime time)
        {
            if (base.IsOutdoors && Game1.isMusicContextActiveButNotPlaying() && !Game1.IsRainingHere(this) && !Game1.eventUp)
            {
                if (!Game1.isDarkOut())
                {
                    Game1.changeMusicTrack("kindadumbautumn", track_interruptable: false);
                }
                else if (Game1.isDarkOut() && Game1.timeOfDay < 2500)
                {
                    Game1.changeMusicTrack("spring_night_ambient", track_interruptable: true);
                }
            }
        }

       

        

        public void addDesertButterflies(double chance, bool onlyIfOnScreen = false)
        {
            bool island_location = this.InIslandContext();
            //bool island_location = GetLocationContext() == LocationContext.Island;
            //bool firefly = (Game1.currentSeason.Equals("summer") | island_location) && Game1.isDarkOut();
            if ((Game1.timeOfDay >= 2100 ) || !((Game1.currentSeason.Equals("spring") || Game1.currentSeason.Equals("summer") || Game1.currentSeason.Equals("fall")) | island_location))
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
        protected override void initNetFields()
        {
            base.initNetFields();
           
      
        }

        public override bool performToolAction(Tool t, int tileX, int tileY)
        {
            Farmer who = new Farmer();
            if (t is Axe)
            {
                who.UsingTool = false;
            }
            return base.performToolAction(t, tileX, tileY);
        }


        protected override void resetLocalState()
        {
            _raySeed = (int)Game1.currentGameTime.TotalGameTime.TotalMilliseconds;
            _rayTexture = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\LightRays");
            //_ambientLightColor = new Color(200, 120, 50);
            ignoreOutdoorLighting.Value = false;

            base.resetLocalState();

            updateSeasonalTileSheets();

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

        public override void DayUpdate(int dayOfMonth)
        {
            
            base.DayUpdate(dayOfMonth);
        }

        public override void performTenMinuteUpdate(int timeOfDay)
        {
            base.performTenMinuteUpdate(timeOfDay);
            //if (Game1.currentLocation is SouthRestStop)
            //Game1.changeMusicTrack("AbigailFlute", track_interruptable: false);
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
        }

        public override void updateEvenIfFarmerIsntHere(GameTime time, bool skipWasUpdatedFlush = false)
        {
            base.updateEvenIfFarmerIsntHere(time, skipWasUpdatedFlush);
          
        }

        public override void UpdateWhenCurrentLocation(GameTime time)
        {
            base.UpdateWhenCurrentLocation(time);
           
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

        public override void drawWaterTile(SpriteBatch b, int x, int y)
        {
            drawWaterTile(b, x, y, this.waterColor.Value);
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


       

        public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
        {
            base.drawAboveAlwaysFrontLayer(b);
           
        }


    }
}