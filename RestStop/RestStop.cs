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
using RestStopLocations.Utilities;

namespace RestStopLocations.Game.Locations
{
    [XmlType("Mods_ApryllForever_RestStopLocations_RestStop")]
    public class RestStop : RestStopLocation
    {
        /*
        public const int sound_babblingBrook = 0;

        public const int sound_cracklingFire = 1;

        public const int sound_engine = 2;

        public const int sound_cricket = 3;

        public const int numberOfSounds = 4;

        public const float doNotPlay = 9999999f;

        private static Dictionary<Vector2, int> sounds = new Dictionary<Vector2, int>();

        private static int updateTimer = 100;

        private static int farthestSoundDistance = 1024;

        private static float shortestDistanceForCue = 4;

        private static ICue babblingBrook;

        private static ICue cracklingFire;

        private static ICue engine;

        private static ICue cricket;

        private static float volumeOverrideForLocChange;

        public Vector2 position;

        public int which;*/







        internal static IMonitor Monitor { get; set; }

        protected NetInt _currentState = new NetInt();

        protected Color _ambientLightColor = Color.White;

        private List<WeatherDebris> weatherDebris;

        public RestStop()
        {
        }

        public RestStop(IModContentHelper content)
        : base(content, "RestStop", "RestStop")
        {
            //AmbientesqueLocationSounds.InitShared();
            //AmbientishLocationSounds.Initialize();

        }

        /*
        public static void InitShared()
        {
            if (Game1.soundBank != null)
            {
                if (RestStop.babblingBrook == null)
                {
                    RestStop.babblingBrook = Game1.soundBank.GetCue("babblingBrook");
                    RestStop.babblingBrook.Play();
                    RestStop.babblingBrook.Pause();
                }
                if (RestStop.cracklingFire == null)
                {
                    RestStop.cracklingFire = Game1.soundBank.GetCue("cracklingFire");
                    RestStop.cracklingFire.Play();
                    RestStop.cracklingFire.Pause();
                }
                if (RestStop.engine == null)
                {
                    RestStop.engine = Game1.soundBank.GetCue("heavyEngine");
                    RestStop.engine.Play();
                    RestStop.engine.Pause();
                }
                if (RestStop.cricket == null)
                {
                    RestStop.cricket = Game1.soundBank.GetCue("cricketsAmbient");
                    RestStop.cricket.Play();
                    RestStop.cricket.Pause();
                }
            }
        }*/







        //[XmlIgnore]
        //new bool isOutdoors = true;


        public void addDesertButterflies(double chance, bool onlyIfOnScreen = false)
        {
            bool island_location = this.InIslandContext();
            //bool firefly = (Game1.currentSeason.Equals("summer") | island_location) && Game1.isDarkOut();
            if (Game1.timeOfDay >= 2100 )
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
            Vector2 creakysounds = new Vector2(43, 9);
            //_ambientLightColor = new Color(200, 120, 50);
            //isOutdoors.Value = true;
            ignoreOutdoorLighting.Value = false;
            base.resetLocalState();

            //AmbientishLocationSounds.addSound(new Vector2(31f, 7f), 2);

            //AmbientesqueLocationSounds.addSound(new Vector2(13f, 7f), 2);

            //LocationSounds sound = new LocationSounds(creakysounds, 2);

            //RestStop.addSound(new Vector2(31f, 7f), 2);
            //RestStop.addSound(new Vector2(13f, 7f), 3);
            //RestStop.addSound(new Vector2(3f, 7f), 1);
            //RestStop.addSound(new Vector2(31f, 17f), 0);

            new Random((int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame / 2);
            weatherDebris = new List<WeatherDebris>();
            int spacing = 192;
            int leafType = 3;
            for (int j = 0; j < 10; j++)
            {
                weatherDebris.Add(new WeatherDebris(new Vector2(j * spacing % Game1.graphics.GraphicsDevice.Viewport.Width + Game1.random.Next(spacing), j * spacing / Game1.graphics.GraphicsDevice.Viewport.Width * spacing % Game1.graphics.GraphicsDevice.Viewport.Height + Game1.random.Next(spacing)), leafType, (float)Game1.random.Next(15) / 500f, (float)Game1.random.Next(-10, 0) / 50f, (float)Game1.random.Next(10) / 50f));
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


        public override void DayUpdate(int dayOfMonth)
        {
            
            base.DayUpdate(dayOfMonth);
        }

        public override void performTenMinuteUpdate(int timeOfDay)
        {
            base.performTenMinuteUpdate(timeOfDay);
          
        }

       

        public override void draw(SpriteBatch b)
        {
            base.draw(b);
           

            
        }
        public override void updateEvenIfFarmerIsntHere(GameTime time, bool skipWasUpdatedFlush = false)
        {
            base.updateEvenIfFarmerIsntHere(time, skipWasUpdatedFlush);
          
        }

        public override void UpdateWhenCurrentLocation(GameTime time)
        {
            base.UpdateWhenCurrentLocation(time);

           // AmbientishLocationSounds.update(time);
           // AmbientesqueLocationSounds.update(time);
            //LocationSounds.update(time);

            if (weatherDebris != null)
            {
                foreach (WeatherDebris weatherDebri in weatherDebris)
                {
                    weatherDebri.update();
                }
                Game1.updateDebrisWeatherForMovement(weatherDebris);
            }


            /*
            {
                if (RestStop.sounds.Count == 0)
                {
                    return;
                }
                if (RestStop.volumeOverrideForLocChange < 1f)
                {
                    RestStop.volumeOverrideForLocChange += (float)time.ElapsedGameTime.Milliseconds * 0.0003f;
                }
                RestStop.updateTimer -= time.ElapsedGameTime.Milliseconds;
                if (RestStop.updateTimer > 0)
                {
                    return;
                }
                for (int j = 0; j < RestStop.shortestDistanceForCue; j++)
                {
                    RestStop.shortestDistanceForCue = 9999999f;
                }
                Vector2 farmerPosition = Game1.player.getStandingPosition();
                
                foreach (KeyValuePair<Vector2, int> pair in RestStop.sounds)
                {
                    float distance = Vector2.Distance(pair.Key, farmerPosition);
                   if (RestStop.shortestDistanceForCue > distance)
                    {
                        RestStop.shortestDistanceForCue = distance;
                    }
                }
                if (RestStop.volumeOverrideForLocChange >= 0f)
                {
                    for (int i = 0; i < RestStop.shortestDistanceForCue; i++)
                    {
                        if (RestStop.shortestDistanceForCue <= (float)RestStop.farthestSoundDistance)
                        {
                            float volume = Math.Min(RestStop.volumeOverrideForLocChange, Math.Min(1f, 1f - RestStop.shortestDistanceForCue / (float)RestStop.farthestSoundDistance));
                            switch (i)
                            {
                                case 0:
                                    if (RestStop.babblingBrook != null)
                                    {
                                        RestStop.babblingBrook.SetVariable("Volume", volume * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
                                        RestStop.babblingBrook.Resume();
                                    }
                                    break;
                                case 1:
                                    if (RestStop.cracklingFire != null)
                                    {
                                        RestStop.cracklingFire.SetVariable("Volume", volume * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
                                        RestStop.cracklingFire.Resume();
                                    }
                                    break;
                                case 2:
                                    if (RestStop.engine != null)
                                    {
                                        RestStop.engine.SetVariable("Volume", volume * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
                                        RestStop.engine.Resume();
                                    }
                                    break;
                                case 3:
                                    if (RestStop.cricket != null)
                                    {
                                        RestStop.cricket.SetVariable("Volume", volume * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
                                        RestStop.cricket.Resume();
                                    }
                                    break;
                            }
                            continue;
                        }
                        switch (i)
                        {
                            case 0:
                                if (RestStop.babblingBrook != null)
                                {
                                    RestStop.babblingBrook.Pause();
                                }
                                break;
                            case 1:
                                if (RestStop.cracklingFire != null)
                                {
                                    RestStop.cracklingFire.Pause();
                                }
                                break;
                            case 2:
                                if (RestStop.engine != null)
                                {
                                    RestStop.engine.Pause();
                                }
                                break;
                            case 3:
                                if (RestStop.cricket != null)
                                {
                                    RestStop.cricket.Pause();
                                }
                                break;
                        }
                    }
                }
                RestStop.updateTimer = 100;
            }*/


        }

        /*
        public static void onLocationLeave()
        {
            RestStop.sounds.Clear();
            RestStop.volumeOverrideForLocChange = -0.5f;
            if (RestStop.babblingBrook != null)
            {
                RestStop.babblingBrook.Pause();
            }
            if (RestStop.cracklingFire != null)
            {
                RestStop.cracklingFire.Pause();
            }
            if (RestStop.engine != null)
            {
                RestStop.engine.SetVariable("Frequency", 100f);
                RestStop.engine.Pause();
            }
            if (RestStop.cricket != null)
            {
                RestStop.cricket.Pause();
            }
        }


        public static void changeSpecificVariable(string variableName, float value, int whichSound)
        {
            if (whichSound == 2 && RestStop.engine != null)
            {
                RestStop.engine.SetVariable(variableName, value);
            }
        }

        public static void addSound(Vector2 tileLocation, int whichSound)
        {
            if (!RestStop.sounds.ContainsKey(tileLocation * 64f))
            {
                RestStop.sounds.Add(tileLocation * 64f, whichSound);
            }
        }

        public static void removeSound(Vector2 tileLocation)
        {
            if (!RestStop.sounds.ContainsKey(tileLocation * 64f))
            {
                return;
            }
            switch (RestStop.sounds[tileLocation * 64f])
            {
                case 0:
                    if (RestStop.babblingBrook != null)
                    {
                        RestStop.babblingBrook.Pause();
                    }
                    break;
                case 1:
                    if (RestStop.cracklingFire != null)
                    {
                        RestStop.cracklingFire.Pause();
                    }
                    break;
                case 2:
                    if (RestStop.engine != null)
                    {
                        RestStop.engine.Pause();
                    }
                    break;
                case 3:
                    if (RestStop.cricket != null)
                    {
                        RestStop.cricket.Pause();
                    }
                    break;
            }
            RestStop.sounds.Remove(tileLocation * 64f);
        } */









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


        public static void UpdateLevels10Minutes(int time)
        {
            if (Game1.IsClient)
                return;

           
        }



       

        public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
        {
            base.drawAboveAlwaysFrontLayer(b);
           
        }


    }
}