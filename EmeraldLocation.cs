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
using StardewValley.BellsAndWhistles;
using xTile.Layers;

namespace RestStopLocations.Game.Locations
{
    [XmlType("Mods_ApryllForever_RestStopLocations_EmeraldLocation")]
    public class EmeraldLocation : GameLocation
    {
        public Random generationRandom;

        [XmlIgnore]
        protected List<TemporaryAnimatedSprite> underwaterSprites = new List<TemporaryAnimatedSprite>();




        public EmeraldLocation() { }
        public EmeraldLocation(IModContentHelper content, string mapPath, string mapName)
        : base(content.GetInternalAssetName("assets/maps/" + mapPath + ".tmx").BaseName, "Custom_" + mapName)
        {

        }

        protected override void initNetFields()
        {
            base.initNetFields();
            

            //terrainFeatures.OnValueAdded += (sender, added) =>
            //{
                //if (added is Grass grass)
                //{
                    //grass.grassType.Value = Grass.lavaGrass;
                    //grass.loadSprite();
                //}
              
            //};
        }
      

        protected override void resetLocalState()
        {
            base.resetLocalState();
            if (!this.map.Properties.TryGetValue("Music", out var musicValue) || musicValue == null)
            {
                Game1.changeMusicTrack("woodsTheme");
            }
            else
            {
                string[] split2 = musicValue.ToString().Split(' ');
                if (split2.Length > 1)
                {
                    if (Game1.timeOfDay >= Convert.ToInt32(split2[0]) && Game1.timeOfDay < Convert.ToInt32(split2[1]) && !split2[2].Equals(Game1.getMusicTrackName()))
                    {
                        Game1.changeMusicTrack(split2[2]);
                    }
                }
                else if (Game1.getMusicTrackName() == "none" || Game1.isMusicContextActiveButNotPlaying() || !split2[0].Equals(Game1.getMusicTrackName()))
                {
                    Game1.changeMusicTrack(split2[0]);
                }
            }

        }


        public override void UpdateWhenCurrentLocation(GameTime time)
        {
            base.UpdateWhenCurrentLocation(time);
            for (int i = underwaterSprites.Count - 1; i >= 0; i--)
            {
                if (underwaterSprites[i].update(time))
                {
                    underwaterSprites.RemoveAt(i);
                }
            }
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
            base.tryToAddCritters(onlyIfOnScreen);
        }

        new public void addClouds(double chance, bool onlyIfOnScreen = false)
        {
            if (!Game1.currentSeason.Equals("spring") || Game1.IsRainingHere(this) || Game1.weatherIcon == 4 || Game1.timeOfDay >= Game1.getStartingToGetDarkTime() - 100)
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

       

        static string Meri = "Buy a MeriCola!!! Make yourself faster or something!";
        public override bool performAction(string actionStr, Farmer who, xTile.Dimensions.Location tileLocation)
        {
            string[] split = actionStr.Split(' ');
            string action = split[0];
            int tx = tileLocation.X;
            int ty = tileLocation.Y;
            Layer layer = Map.GetLayer("Buildings");


            if (action == "SilverKey")
            {
                int silverkeyint = 99;// ExternalAPIs.JA.GetObjectId("Silver Key");
                string silverkey = Convert.ToString(silverkeyint);
                if (Game1.player.ActiveObject != null && Utility.IsNormalObjectAtParentSheetIndex(Game1.player.ActiveObject, silverkey))
                {
                    layer.Tiles[tx, ty] = null;
                    layer.Tiles[tx, ty - 1] = null;
                    layer.Tiles[tx, ty - 2] = null;
                    Game1.playSound("doorCreak");
                    Game1.player.removeItemFromInventory(ItemRegistry.Create(silverkey));
                    who.ActiveObject = null;
                    DelayedAction.playSoundAfterDelay("treethud", 1000);
                }
                else
                    Game1.addHUDMessage(new HUDMessage("You need the silver key in your hand to open this gate.", 1));
                Game1.playSound("batScreech");
            }
            else if (action == "GoldKey")
            {
                int goldketint = 98;// ExternalAPIs.JA.GetObjectId("Gold Key");
                string goldkey = Convert.ToString(goldketint);
                if (Game1.player.ActiveObject != null && Utility.IsNormalObjectAtParentSheetIndex(Game1.player.ActiveObject, goldkey))
                {
                    layer.Tiles[tx, ty] = null;
                    layer.Tiles[tx, ty - 1] = null;
                    layer.Tiles[tx, ty - 2] = null;
                    Game1.playSound("doorCreak");
                    Game1.player.removeItemFromInventory(ItemRegistry.Create(goldkey));
                    who.ActiveObject = null;
                    DelayedAction.playSoundAfterDelay("treethud", 1000);
                }
                else
                    Game1.addHUDMessage(new HUDMessage("You need the gold key in your hand to open this gate.", 1));
                Game1.playSound("trainWhistle");
            }
            else if (action == "IridiumKey")
            {
                int iridiumkeyint = 97;// ExternalAPIs.JA.GetObjectId("Iridium Key");
                string iridiumkey = Convert.ToString(iridiumkeyint);
                if (Game1.player.ActiveObject != null && Utility.IsNormalObjectAtParentSheetIndex(Game1.player.ActiveObject, iridiumkey))
                {
                    layer.Tiles[tx, ty] = null;
                    layer.Tiles[tx, ty - 1] = null;
                    layer.Tiles[tx, ty - 2] = null;
                    Game1.playSound("doorCreak");
                    Game1.player.removeItemFromInventory(ItemRegistry.Create(iridiumkey));
                    who.ActiveObject = null;
                    DelayedAction.playSoundAfterDelay("treethud", 1000);
                }
                else
                    Game1.addHUDMessage(new HUDMessage("You need the iridium key in your hand to open this gate.", 1));
                Game1.playSound("cacklingWitch");
            }
            else if (action == "DiamondKey")
            {
                int diamondkeyint = 96;// ExternalAPIs.JA.GetObjectId("Diamond Key");
                string diamondkey = Convert.ToString(diamondkeyint);
                if (Game1.player.ActiveObject != null && Utility.IsNormalObjectAtParentSheetIndex(Game1.player.ActiveObject, diamondkey))
                {
                    layer.Tiles[tx, ty] = null;
                    layer.Tiles[tx, ty - 1] = null;
                    layer.Tiles[tx, ty - 2] = null;
                    Game1.playSound("doorCreak");
                    Game1.player.removeItemFromInventory(ItemRegistry.Create(diamondkey));
                    who.ActiveObject = null;
                    DelayedAction.playSoundAfterDelay("treethud", 1000);
                }
                else
                    Game1.addHUDMessage(new HUDMessage("You need the diamond key in your hand to open this gate.", 1));
                Game1.playSound("dogs");
            }
            else if (action == "HeartKey")
            {
                int heartkeyint = 95;/// ExternalAPIs.JA.GetObjectId("Heart Key");
                string heartkey = Convert.ToString(heartkeyint);
                if (Game1.player.ActiveObject != null && Utility.IsNormalObjectAtParentSheetIndex(Game1.player.ActiveObject, heartkey))
                {
                    layer.Tiles[tx, ty] = null;
                    layer.Tiles[tx, ty - 1] = null;
                    layer.Tiles[tx, ty - 2] = null;
                    Game1.playSound("doorCreak");
                    Game1.player.removeItemFromInventory(ItemRegistry.Create(heartkey));
                    who.ActiveObject = null;
                    DelayedAction.playSoundAfterDelay("treethud", 1000);
                }
                else

                    Game1.addHUDMessage(new HUDMessage("You need the heart key in your hand to open this gate.", 1));
                Game1.playSound("snowyStep", 1900);
                DelayedAction.playSoundAfterDelay("snowyStep", 300, (Game1.player.currentLocation), Game1.player.Position, 1400);
                DelayedAction.playSoundAfterDelay("snowyStep", 1400, Game1.player.currentLocation, Game1.player.Position, 1900);
                DelayedAction.playSoundAfterDelay("snowyStep", 1700, Game1.player.currentLocation, Game1.player.Position, 1400);
                DelayedAction.playSoundAfterDelay("snowyStep", 2800, Game1.player.currentLocation, Game1.player.Position, 1900);
                DelayedAction.playSoundAfterDelay("snowyStep", 3100, Game1.player.currentLocation, Game1.player.Position, 1400);
                DelayedAction.playSoundAfterDelay("snowyStep", 4200, Game1.player.currentLocation, Game1.player.Position, 1900);
                DelayedAction.playSoundAfterDelay("snowyStep", 4500, Game1.player.currentLocation, Game1.player.Position, 1400);

            }
            else if (action == "NoKey")
            {
                {
                    layer.Tiles[tx, ty] = null;
                    layer.Tiles[tx, ty - 1] = null;
                    layer.Tiles[tx, ty - 2] = null;
                    Game1.playSound("doorCreak");
                    DelayedAction.playSoundAfterDelay("treethud", 1000);
                }
            }

            else if (action == "HugeDoor")
            {
                {
                    layer.Tiles[tx, ty] = null;
                    layer.Tiles[tx - 1, ty] = null;
                    layer.Tiles[tx + 1, ty] = null;
                    layer.Tiles[tx, ty - 1] = null;
                    layer.Tiles[tx - 1, ty - 1] = null;
                    layer.Tiles[tx + 1, ty - 1] = null;
                    Game1.playSound("doorCreak");
                    DelayedAction.playSoundAfterDelay("treethud", 1000);
                }
            }
            else if (action == "MeriCoke")
            {
                createQuestionDialogue(Meri, createYesNoResponses(), "MeriCola");
            }

            return base.performAction(actionStr, who, tileLocation);
        }

        public override void cleanupBeforePlayerExit()
        {
           
            base.cleanupBeforePlayerExit();
        }

        public override bool answerDialogue(Response answer)
        {
            if (lastQuestionKey != null && afterQuestion == null)
            {
                string qa = lastQuestionKey.Split(' ')[0] + "_" + answer.responseKey;
                switch (qa)
                {
                    case "MeriCola_Yes":

                        if (Game1.player.Money >= 300)
                        {
                            int buttcoke = 94;// ExternalAPIs.JA.GetObjectId("MeriCola");
                            string mericola = Convert.ToString(buttcoke); 
                            Game1.player.Money -= 300;
                            Game1.player.addItemByMenuIfNecessary(ItemRegistry.Create(mericola));
                        }
                        else
                        {
                            Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney1"));
                        }


                        return true;
                }
            }

            return base.answerDialogue(answer);
        }

        public override void drawWater(SpriteBatch b)
        {
            foreach (TemporaryAnimatedSprite underwaterSprite in underwaterSprites)
            {
                underwaterSprite.draw(b);
            }
            base.drawWater(b);
        }


    }
}