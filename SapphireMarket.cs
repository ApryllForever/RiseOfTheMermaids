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
using StardewValley.Menus;
using Object = StardewValley.Object;

namespace RestStopLocations.Game.Locations
{
    [XmlType("Mods_ApryllForever_RestStopLocations_SapphireMarket")]
    public class SapphireMarket : SapphireLocation
    {

        public int xPositionOnScreen;
        public int yPositionOnScreen;
        public string potraitPersonDialogue;
        public NPC portraitPerson;

        public const float steamZoom = 2f;

        public const float steamYMotionPerMillisecond = 0.1f;

        public const float millisecondsPerSteamFrame = 150f;

        private Texture2D steamAnimation;

        private Texture2D swimShadow;

        private Vector2 steamPosition;

        private float steamYOffset;

        private int swimShadowTimer;

        private int swimShadowFrame;


        public SapphireMarket() { }
        public SapphireMarket(IModContentHelper content)
        : base(content, "SapphireMarket", "SapphireMarket")
        {
        }

        static string EnterDungeon = "Do you wish to enter this forboding gate?";
        protected override void resetLocalState()
        {
            base.resetLocalState();
            base.resetLocalState();
            Game1.changeMusicTrack("sappypiano");
            steamPosition = new Vector2(-Game1.viewport.X, -Game1.viewport.Y);
            steamAnimation = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\steamAnimation");
            swimShadow = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\swimShadow");

        }
        public override void cleanupBeforePlayerExit()
        {
            base.cleanupBeforePlayerExit();
            if (Game1.player.swimming.Value)
            {
                Game1.player.swimming.Value = false;
            }
            if (Game1.locationRequest != null && !Game1.locationRequest.Name.Contains("BathHouse"))
            {
                Game1.player.bathingClothes.Value = false;
            }
            Game1.changeMusicTrack("none");
        }

        public override void draw(SpriteBatch b)
        {
            base.draw(b);
            int num4 = xPositionOnScreen - 320;
            if (num4 > 0 && Game1.options.showMerchantPortraits)
            {
                Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2(num4, yPositionOnScreen), new Microsoft.Xna.Framework.Rectangle(603, 414, 74, 74), Color.White, 0f, Vector2.Zero, 4f, flipped: false, 0.91f);
                if (portraitPerson.Portrait != null)
                {
                    b.Draw(portraitPerson.Portrait, new Vector2(num4 + 20, yPositionOnScreen + 20), new Microsoft.Xna.Framework.Rectangle(0, 0, 64, 64), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.92f);
                }
            }

            { 
            if (currentEvent != null)
            {
                foreach (NPC j in currentEvent.actors)
                {
                    if ((bool)j.swimming)
                    {
                        b.Draw(swimShadow, Game1.GlobalToLocal(Game1.viewport, j.Position + new Vector2(0f, j.Sprite.SpriteHeight / 3 * 4 + 4)), new Microsoft.Xna.Framework.Rectangle(swimShadowFrame * 16, 0, 16, 16), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);
                    }
                }
            }
            else
            {
                foreach (NPC i in characters)
                {
                    if ((bool)i.swimming)
                    {
                        b.Draw(swimShadow, Game1.GlobalToLocal(Game1.viewport, i.Position + new Vector2(0f, i.Sprite.SpriteHeight / 3 * 4 + 4)), new Microsoft.Xna.Framework.Rectangle(swimShadowFrame * 16, 0, 16, 16), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);
                    }
                }
                foreach (Farmer f in farmers)
                {
                    if ((bool)f.swimming)
                    {
                        b.Draw(swimShadow, Game1.GlobalToLocal(Game1.viewport, f.Position + new Vector2(0f, f.Sprite.SpriteHeight / 4 * 4)), new Microsoft.Xna.Framework.Rectangle(swimShadowFrame * 16, 0, 16, 16), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);
                    }
                }
            }
            _ = (bool)Game1.player.swimming;
        }


           
        }

        public override void checkForMusic(GameTime time)
        {
            base.checkForMusic(time);
        }


        public override bool performAction(string action, Farmer who, Location tileLocation)
        {

            if (action == "RS.Hospital")
            {
                /*
                string text = "Hey cutie! Want to take a look at my goods? And you can take a look at what the tavern is selling too!";
                portraitPerson = Game1.getCharacterFromName("SapphireMelanie");
                Random r = new Random((int)(Game1.stats.DaysPlayed + 898 + Game1.uniqueIDForThisGame));
                Dictionary<ISalable, int[]> stock = new Dictionary<ISalable, int[]>();

                stock.Add(new Clothing(1142), new int[4]   //Tube Top
                {
                        0,
                        2147483647,
                        372,
                        5
                });

                //Drinks
                Utility.AddStock(stock, new Object(Vector2.Zero, 773, int.MaxValue), 200); //Life Elixir
                Utility.AddStock(stock, new Object(Vector2.Zero, 351, int.MaxValue), 10);  //Muscle Rub
                Utility.AddStock(stock, new Object(Vector2.Zero, 903, int.MaxValue), 150); //Ginger Ale
                Utility.AddStock(stock, new Object(Vector2.Zero, 253, int.MaxValue), 40); //Expresso
                Utility.AddStock(stock, new Object(Vector2.Zero, 167, int.MaxValue), 20); //JojoCola
                Utility.AddStock(stock, new Object(Vector2.Zero, 874, int.MaxValue), 50); //Bug Steaks

                //Candies
                Utility.AddStock(stock, new Object(Vector2.Zero, 612, int.MaxValue), 50);

                /*                HOW TO MAKE THINGS BE RANDOM

				if (r.NextDouble() < 0.5)
				{
					Utility.AddStock(stock, new Object(Vector2.Zero, 244, int.MaxValue), 600);
				}
				else
				{
					Utility.AddStock(stock, new Object(Vector2.Zero, 237, int.MaxValue), 600);
				}  


                Game1.activeClickableMenu = new ShopMenu(stock, 0, "SapphireHospital", null, null, "SapphireMelanie")
                {
                    portraitPerson = Game1.getCharacterFromName("SapphireMelanie")

                };

                //Game1.objectDialoguePortraitPerson = 
                //if (portraitPerson != null)
                //{ }
                potraitPersonDialogue = Game1.parseText(text, Game1.dialogueFont, 304);
                portraitPerson = Game1.objectDialoguePortraitPerson;
            }
            */
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
        public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
        {
            base.drawAboveAlwaysFrontLayer(b);
            for (float x = steamPosition.X; x < (float)Game1.graphics.GraphicsDevice.Viewport.Width + 256f; x += 256f)
            {
                for (float y = steamPosition.Y + steamYOffset; y < (float)(Game1.graphics.GraphicsDevice.Viewport.Height + 128); y += 256f)
                {
                    b.Draw(steamAnimation, new Vector2(x, y), new Microsoft.Xna.Framework.Rectangle(0, 0, 64, 64), Color.White * 0.8f, 0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
                }
            }
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

            base.performTouchAction(actionStr, tileLocation);
        }

        public override void UpdateWhenCurrentLocation(GameTime time)
        {
            base.UpdateWhenCurrentLocation(time);
            steamYOffset -= (float)time.ElapsedGameTime.Milliseconds * 0.1f;
            steamYOffset %= -256f;
            steamPosition -= Game1.getMostRecentViewportMotion();
            swimShadowTimer -= time.ElapsedGameTime.Milliseconds;
            if (swimShadowTimer <= 0)
            {
                swimShadowTimer = 70;
                swimShadowFrame++;
                swimShadowFrame %= 10;
            }
        }

    }
}