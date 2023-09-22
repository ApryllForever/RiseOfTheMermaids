using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using SpaceShared;
using StardewModdingAPI;
using StardewValley;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using xTile;
using StardewValley.Locations;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Buildings;
using StardewValley.Characters;
using StardewValley.GameData.Movies;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Minigames;
using StardewValley.Monsters;
using StardewValley.Network;
using StardewValley.Objects;
using StardewValley.Projectiles;
using StardewValley.Util;
using System.Globalization;
using xTile.Dimensions;
using xTile.Layers;
using xTile.ObjectModel;
using xTile.Tiles;

namespace RestStopLocations.Game.Locations
{
    [XmlType("Mods_ApryllForever_RestStopLocations_RestStopLocation")]
    public class RestStopLocation : IslandLocation
    {
        public Random generationRandom;
       

        public const int SpaceTileIndex = 608;

      
       

        public RestStopLocation() { }
        public RestStopLocation(IModContentHelper content, string mapPath, string mapName)
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
            base.cleanupBeforePlayerExit();
          
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
            return false;
        }

        public override bool CanPlantTreesHere(string itemId, int tileX, int tileY, out string deniedMessage)
        {
            deniedMessage = string.Empty;
            return false;
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


        // public override string checkForBuriedItem(int xLocation, int yLocation, bool explosion, bool detectOnly, Farmer who)
        // {

        // Random chest_random = new Random(generationRandom.Next());
        // int random_count = 9;
        // int random = chest_random.Next(random_count);
        //switch (random)
        // {
        //    case 0:
        //if (Game1.random.NextDouble() < 0.1)
        //     {
        //             Game1.createObjectDebris(348 /* Wine */, xLocation, yLocation, this);
        //             break;
        //         }
        //    case 1:
        //         {
        //             Game1.createObjectDebris(394 /* Rainbow Shell */, xLocation, yLocation, this);
        //              break ;
        //          }
        //      case 2:
        //          {
        //             Game1.createObjectDebris(562 /* Tiger's Eye */, xLocation, yLocation, this);
        //             break;
        //         }
        //     case 3:
        //          {
        //              Game1.createObjectDebris(567 /* Marble */, xLocation, yLocation, this);
        //             break;
        //         }
        //     case 4:
        //        {
        //          Game1.createObjectDebris(330 /* Clay */, xLocation, yLocation, this);
        //             break;
        //       }
        //    case 5:
        //      {
        //           Game1.createObjectDebris(411 /* Cobblestone */, xLocation, yLocation, this);
        //           break;
        //      }
        //   case 6:
        //      {
        //          Game1.createObjectDebris(390 /* Stone */, xLocation, yLocation, this);
        //          break;
        //     }
        //   case 7:
        // {
        //  Game1.createObjectDebris(427 /* Tulip Bulb */, xLocation, yLocation, this);
        //    break;
        //  }
        //case 8:
        // {
        //       Game1.createObjectDebris(568 /* SandStone */, xLocation, yLocation, this);
        //         break;
        //       }
        // }
        //   return base.checkForBuriedItem(xLocation, yLocation, explosion, detectOnly, who);
        // }

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