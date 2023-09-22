using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewModdingAPI;
using StardewValley;
using StardewValley.TerrainFeatures;
using StardewValley.BellsAndWhistles;


namespace RestStopLocations.Game.Locations
{
    [XmlType("Mods_ApryllForever_RestStopLocations_AspenLocation")]
    public class AspenLocation : GameLocation
    {
        public Random generationRandom;

        [XmlIgnore]
        protected Texture2D _dayParallaxTexture;

        [XmlIgnore]
        protected Texture2D _nightParallaxTexture;


        public AspenLocation() { }
        public AspenLocation(IModContentHelper content, string mapPath, string mapName)
        : base(content.GetInternalAssetName("assets/maps/" + mapPath + ".tmx").BaseName, "Custom_" + mapName)
        {
           
        }

        protected override void initNetFields()
        {
            base.initNetFields();
           

            terrainFeatures.OnValueAdded += (sender, added) =>
            {
                if (added is Grass grass)
                {
                    grass.grassType.Value = Grass.springGrass;
                    grass.loadSprite();
                }
              
            };
        }

       

        protected override void resetLocalState()
        {
            base.resetLocalState();

           // Game1.changeMusicTrack("woodsTheme");



            
        }
        public override void cleanupBeforePlayerExit()
        {
            if (_dayParallaxTexture != null)
            {
                _dayParallaxTexture = null;
            }
            if (_nightParallaxTexture != null)
            {
                _nightParallaxTexture = null;
            }
            base.cleanupBeforePlayerExit();
          
        }

        public virtual void DrawParallaxHorizon(SpriteBatch b, bool horizontal_parallax = true)
        {
            float draw_zoom = 4f;
            if (_dayParallaxTexture == null)
            {
                _dayParallaxTexture = Game1.temporaryContent.Load<Texture2D>(Mod.instance.Helper.ModContent.GetInternalAssetName("assets/maps/BackGround_WinterMountains.png").BaseName);
            }
            if (_nightParallaxTexture == null)
            {
                _nightParallaxTexture = Game1.temporaryContent.Load<Texture2D>(Mod.instance.Helper.ModContent.GetInternalAssetName("assets/maps/BackGround_WinterMountains_Night.png").BaseName);
            }
            float horizontal_parallax_amount = (float)_dayParallaxTexture.Width * draw_zoom - (float)map.DisplayWidth;
            float t2 = 0f;
            int background_y_adjustment = -640;
            int y = (int)((float)Game1.viewport.Y * 0.2f + (float)background_y_adjustment);
            if (horizontal_parallax)
            {
                if (map.DisplayWidth - Game1.viewport.Width < 0)
                {
                    t2 = 0.5f;
                }
                else if (map.DisplayWidth - Game1.viewport.Width > 0)
                {
                    t2 = (float)Game1.viewport.X / (float)(map.DisplayWidth - Game1.viewport.Width);
                }
            }
            else
            {
                t2 = 0.5f;
            }
            if (Game1.game1.takingMapScreenshot)
            {
                y = background_y_adjustment;
                t2 = 0.5f;
            }
            float arc = 0.25f;
            t2 = Utility.Lerp(0.5f + arc, 0.5f - arc, t2);
            float day_night_transition2 = (float)Utility.ConvertTimeToMinutes(Game1.timeOfDay + (int)((float)Game1.gameTimeInterval / 7000f * 10f % 10f) - Game1.getStartingToGetDarkTime()) / (float)Utility.ConvertTimeToMinutes(Game1.getTrulyDarkTime() - Game1.getStartingToGetDarkTime());
            day_night_transition2 = Utility.Clamp(day_night_transition2, 0f, 1f);
            b.Draw(Game1.staminaRect, Game1.GlobalToLocal(Game1.viewport, new Microsoft.Xna.Framework.Rectangle(0, 0, map.DisplayWidth, map.DisplayHeight)), new Color(1, 122, 217, 255));
            b.Draw(Game1.staminaRect, Game1.GlobalToLocal(Game1.viewport, new Microsoft.Xna.Framework.Rectangle(0, 0, map.DisplayWidth, map.DisplayHeight)), new Color(0, 7, 63, 255) * day_night_transition2);
            Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle((int)((0f - horizontal_parallax_amount) * t2), y, (int)((float)_dayParallaxTexture.Width * draw_zoom), (int)((float)_dayParallaxTexture.Height * draw_zoom));
            Microsoft.Xna.Framework.Rectangle source_rect = new Microsoft.Xna.Framework.Rectangle(0, 0, _dayParallaxTexture.Width, _dayParallaxTexture.Height);
            int left_boundary = 0;
            if (rectangle.X < left_boundary)
            {
                int offset2 = left_boundary - rectangle.X;
                rectangle.X += offset2;
                rectangle.Width -= offset2;
                source_rect.X += (int)((float)offset2 / draw_zoom);
                source_rect.Width -= (int)((float)offset2 / draw_zoom);
            }
            int right_boundary = map.DisplayWidth;
            if (rectangle.X + rectangle.Width > right_boundary)
            {
                int offset = rectangle.X + rectangle.Width - right_boundary;
                rectangle.Width -= offset;
                source_rect.Width -= (int)((float)offset / draw_zoom);
            }
            if (source_rect.Width > 0 && rectangle.Width > 0)
            {
                b.Draw(_dayParallaxTexture, Game1.GlobalToLocal(Game1.viewport, rectangle), source_rect, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                b.Draw(_nightParallaxTexture, Game1.GlobalToLocal(Game1.viewport, rectangle), source_rect, Color.White * day_night_transition2, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            }
        }

        public override void checkForMusic(GameTime time)
        {
            
        }

        public override bool SeedsIgnoreSeasonsHere()
        {
            return false;
        }

        public override bool CanPlantSeedsHere(string itemId, int tileX, int tileY, bool isGardenPot, out string deniedMessage)
        {
            deniedMessage = string.Empty;
            return true;
        }

        public override bool CanPlantTreesHere(string itemId, int tileX, int tileY, out string deniedMessage)
        {
            deniedMessage = string.Empty;
            return true;
        }

        public override void UpdateWhenCurrentLocation(GameTime time)
        {
            base.UpdateWhenCurrentLocation(time);
          
        }

        public override void updateEvenIfFarmerIsntHere(GameTime time, bool ignoreWasUpdatedFlush = false)
        {
            base.updateEvenIfFarmerIsntHere(time, ignoreWasUpdatedFlush);

            if (!Context.CanPlayerMove)
                return;

            
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


        public override bool performToolAction(Tool t, int tileX, int tileY)
        {
           

            return base.performToolAction(t, tileX, tileY);
        }

        public override StardewValley.Object getFish(float millisecondsAfterNibble, string bait, int waterDepth, Farmer who, double baitPotency, Vector2 bobberTile, string locationName = null)
        {
            return (StardewValley.Object)base.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency, bobberTile, locationName);
        }


        public override void tryToAddCritters(bool onlyIfOnScreen = false)
        {
            if (Game1.CurrentEvent != null)
            {
                return;
            }
            double mapArea = map.Layers[0].LayerWidth * map.Layers[0].LayerHeight;
            double butterflyChance;
            double birdieChance;
            double num = butterflyChance = (birdieChance = Math.Max(0.15, Math.Min(0.7, mapArea / 5000.0)));
            double bunnyChance = num / 1.0;
            double squirrelChance = num / 1.0;
            double woodPeckerChance = num / 1.0;
            double cloudChange = num * 2.0;
          
           
            if ( critters.Count <= 600)
            {
                addBirdies(birdieChance, onlyIfOnScreen);
                addButterflies(butterflyChance, onlyIfOnScreen);
                addBunnies(bunnyChance, onlyIfOnScreen);
                addSquirrels(squirrelChance, onlyIfOnScreen);
                addWoodpecker(woodPeckerChance, onlyIfOnScreen);
               
                  if (Game1.IsRainingHere(this))
            {
                return;
            }
                
                addClouds(cloudChange / (double)(onlyIfOnScreen ? 2f : 1f), onlyIfOnScreen);
            }
        }

        new public void addClouds(double chance, bool onlyIfOnScreen = false)
        {
            if (!Game1.currentSeason.Equals("summer") || Game1.IsRainingHere(this) || Game1.weatherIcon == 4 || Game1.timeOfDay >= Game1.getStartingToGetDarkTime() - 100)
            {
                return;
            }
            while (Game1.random.NextDouble() < Math.Min(0.9, chance))
            {
                Vector2 v = getRandomTile();
                if (onlyIfOnScreen)
                {
                    v = ((Game1.random.NextDouble() < 0.5) ? new Vector2(map.Layers[0].LayerWidth, Game1.random.Next(map.Layers[0].LayerHeight)) : new Vector2(Game1.random.Next(map.Layers[0].LayerWidth), map.Layers[0].LayerHeight));
                }
                if (onlyIfOnScreen || !Utility.isOnScreen(v * 64f, 1280))
                {
                    Cloud cloud = new Cloud(v);
                    bool freeToAdd = true;
                    if (critters != null)
                    {
                        foreach (Critter c in critters)
                        {
                            if (c is Cloud && c.getBoundingBox(0, 0).Intersects(cloud.getBoundingBox(0, 0)))
                            {
                                freeToAdd = false;
                                break;
                            }
                        }
                    }
                    if (freeToAdd)
                    {
                        addCritter(cloud);
                    }
                }
            }
        }

        public override void drawWater(SpriteBatch b)
        {
           
        }

       
    }
}