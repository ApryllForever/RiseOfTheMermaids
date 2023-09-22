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




namespace RestStopLocations.Game.Locations
{
    [XmlType("Mods_ApryllForever_RestStopLocations_AspenMountainTop")]
    public class AspenMountainTop : AspenLocation
    {

        [XmlIgnore]
        public List<SuspensionBridge> suspensionBridges = new List<SuspensionBridge>();
        internal static IMonitor Monitor { get; set; }

        protected NetInt _currentState = new NetInt();

        protected Color _ambientLightColor = Color.White;

        private List<WeatherDebris> weatherDebris;

        new public List<TerrainFeature> _activeTerrainFeatures = new List<TerrainFeature>();

        public AspenMountainTop()
        {
        }

        public AspenMountainTop(IModContentHelper content)
        : base(content, "AspenMountainTop", "AspenMountainTop")
        {
          

        }

        public override void checkForMusic(GameTime time)
        {
            if (base.IsOutdoors && Game1.isMusicContextActiveButNotPlaying() && !Game1.IsRainingHere(this) && !Game1.eventUp)
            {
                if (!Game1.isDarkOut())
                {
                    Game1.changeMusicTrack("sadpiano", track_interruptable: false);
                }
                else if (Game1.isDarkOut() && Game1.timeOfDay < 2500)
                {
                    Game1.changeMusicTrack("sadpiano", track_interruptable: true);
                }
            }
        }

       

        
        protected override void initNetFields()
        {
            base.initNetFields();
           
      
        }


        public override bool IsLocationSpecificPlacementRestriction(Vector2 tileLocation)
        {
            foreach (SuspensionBridge suspensionBridge in this.suspensionBridges)
            {
                if (suspensionBridge.CheckPlacementPrevention(tileLocation))
                {
                    return true;
                }
            }
            return base.IsLocationSpecificPlacementRestriction(tileLocation);
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
          Texture2D starry = Game1.temporaryContent.Load<Texture2D>(Mod.instance.Helper.ModContent.GetInternalAssetName("assets/maps/BackGround_WinterMountains.png").BaseName);
            ignoreOutdoorLighting.Value = false;

            base.resetLocalState();
            Game1.background = new Background(this,Color.Blue,true);


            //public Background(GameLocation location, Texture2D bgImage, int seedValue, int chunksWide, int chunksHigh, int chunkWidth, int chunkHeight, float zoom, int defaultChunkIndex, int numChunksInSheet, double chanceForDeviation, Color c)



            suspensionBridges.Clear();
            SuspensionBridge bridge = new SuspensionBridge(62, 112);
            suspensionBridges.Add(bridge);

           
           
            weatherDebris = new List<WeatherDebris>();
            int spacing = 160;
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

       

        public override void performTenMinuteUpdate(int timeOfDay)
        {
            base.performTenMinuteUpdate(timeOfDay);
            if (Game1.currentLocation is AspenMountainTop)
            Game1.changeMusicTrack("sadpiano", track_interruptable: false);
            if (Game1.currentLocation is AspenMountainTop)
            {
                int oxytankint = 11;// ExternalAPIs.JA.GetObjectId("OxyTank");
                string oxytank = Convert.ToString(oxytankint);
                if (!Game1.player.Items.ContainsId(oxytank, 1))
                Game1.player.takeDamage(30, false, null);
            }
           
        }

        public override void drawBackground(SpriteBatch b)
        {
            base.drawBackground(b);
            DrawParallaxHorizon(b, horizontal_parallax: false);
            
        }

        public override void draw(SpriteBatch b)
        {
            base.draw(b);
            foreach (SuspensionBridge suspensionBridge in suspensionBridges)
            {
                suspensionBridge.Draw(b);
            }
        }

        public override void updateEvenIfFarmerIsntHere(GameTime time, bool skipWasUpdatedFlush = false)
        {
            base.updateEvenIfFarmerIsntHere(time, skipWasUpdatedFlush);
          
        }

        public override void UpdateWhenCurrentLocation(GameTime time)
        {
            base.UpdateWhenCurrentLocation(time);
            foreach (SuspensionBridge suspensionBridge in suspensionBridges)
            {
                suspensionBridge.Update(time);
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

    }
}