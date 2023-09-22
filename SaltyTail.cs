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
using UtilitiesStuff;

namespace RestStopLocations.Game.Locations
{
    [XmlType("Mods_ApryllForever_RestStopLocations_SaltyTail")]
    public class SaltyTail : SapphireDecoratableLocation
    {
        public readonly NetBool visited = new();

        public int xPositionOnScreen;
        public int yPositionOnScreen;
        public string potraitPersonDialogue;
        public NPC portraitPerson;
        static string Meri = "Buy a MeriCola!!! Make yourself faster or something!";

       

        private Texture2D swimShadow;

        private int swimShadowTimer;

        private int swimShadowFrame;


        public SaltyTail() { }
        public SaltyTail(IModContentHelper content)
        : base(content, "SaltyTail", "SaltyTail")
        {
        }

        public override void TransferDataFromSavedLocation(GameLocation l)
        {
           
            this.visited.Value = (l as SaltyTail).visited.Value;
            base.TransferDataFromSavedLocation(l);
        }

        public override bool CanFreePlaceFurniture()
        {
            return true;
        }


        //static string EnterDungeon = "Do you wish to enter this forboding gate?";
        protected override void resetLocalState()
        {
           // base.resetLocalState();
            base.resetLocalState();
            Game1.changeMusicTrack("sappypiano");
            swimShadow = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\swimShadow");
            this.visited.Value = true;
            InitializeBeds();

        }

        protected override void initNetFields()
        {
            base.initNetFields();
            this.visited.InterpolationEnabled = false;
            this.visited.fieldChangeVisibleEvent += delegate
            {
                this.InitializeBeds();
            };


        }


        public virtual void InitializeBeds()
        {
            if (!Game1.IsMasterGame || Game1.gameMode == 6 || !this.visited.Value)
            {
                return;
            }
            int player_count = 0;
            foreach (Farmer allFarmer in Game1.getAllFarmers())
            {
                _ = allFarmer;
                player_count++;
            }
            string bedId;
            bedId = "2048";
           
            base.furniture.Add(new BedFurniture(bedId, new Vector2(3f, 32f)));   
            base.furniture.Add(new BedFurniture(bedId, new Vector2(6f, 32f)));
            base.furniture.Add(new BedFurniture(bedId, new Vector2(9f, 32f)));


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
        }


           
        }

        public override void checkForMusic(GameTime time)
        {
            base.checkForMusic(time);
        }


        public override bool performAction(string action, Farmer who, Location tileLocation)
        {
            
            if (action == "RS.SaltyTail")
            {
                /*
                string text = "Hey cutie! Want to take a look at my goods? And you can take a look at what the tavern is selling too!";
                portraitPerson = Game1.getCharacterFromName("Ciarra");
                Random r = new Random((int)(Game1.stats.DaysPlayed + 898 + Game1.uniqueIDForThisGame));
                Dictionary<ISalable, int[]> stock = new Dictionary<ISalable, int[]>();

                stock.Add(new Clothing(1142), new int[4]   //Tube Top
                {
                        0,
                        2147483647,
                        372,
                        5
                });

                Utility.AddStock(stock, new Clothing(1134), 90);
                Utility.AddStock(stock, new Clothing(1142), 200);
                Utility.AddStock(stock, new Clothing(1297), 800);
                Utility.AddStock(stock, new Clothing(6), 300);
                Utility.AddStock(stock, new Clothing(7), 800);
                Utility.AddStock(stock, new Clothing(1), 200);
                Utility.AddStock(stock, new Hat(13), 170);
                Utility.AddStock(stock, new Hat(88), 300);

                Utility.AddStock(stock, new Object(Vector2.Zero, 460, int.MaxValue), 10000);

                //Drinks
                Utility.AddStock(stock, new Object(Vector2.Zero, 348, int.MaxValue), 130);
                Utility.AddStock(stock, new Object(Vector2.Zero, 873, int.MaxValue), 170);
                Utility.AddStock(stock, new Object(Vector2.Zero, 459, int.MaxValue), 90);
                Utility.AddStock(stock, new Object(Vector2.Zero, 253, int.MaxValue), 20);
                Utility.AddStock(stock, new Object(Vector2.Zero, 167, int.MaxValue), 20);
                Utility.AddStock(stock, new Object(Vector2.Zero, 903, int.MaxValue), 50);

                //Pies
                Utility.AddStock(stock, new Object(Vector2.Zero, 222, int.MaxValue), 240);
                Utility.AddStock(stock, new Object(Vector2.Zero, 608, int.MaxValue), 200);
                Utility.AddStock(stock, new Object(Vector2.Zero, 604, int.MaxValue), 210);
                Utility.AddStock(stock, new Object(Vector2.Zero, 611, int.MaxValue), 210);
                Utility.AddStock(stock, new Object(Vector2.Zero, 234, int.MaxValue), 210);
                Utility.AddStock(stock, new Object(Vector2.Zero, 904, int.MaxValue), 210);

                Utility.AddStock(stock, new Object(Vector2.Zero, 233, int.MaxValue), 150);

                //Foods
                Utility.AddStock(stock, new Object(Vector2.Zero, 610, int.MaxValue), 150);

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


                var shop = new ShopMenu(stock, 0, "Ciarra", null, null, "Custom_SaltyTail")
                {
                    
                };


                shop.portraitPerson = Game1.getCharacterFromName("Ciarra");


                         switch (Game1.random.Next(5))
                {
                    case 0:
                        text = "Hey Hottie!!! Care to buy yourself a drink?";
                        break;
                    case 1:
                        text = "Hey cutie! Want to take a look at my goods? And you can take a look at what the tavern is selling too!";
                        break;
                    case 2:
                        text = "Come cool off in the hottest place in Sapphire Springs!";
                        break;
                    case 3:
                        text = "Hey sweetie! Nice to see you! Care to waste a little of your hard earned money on one of our delicious beverages?";
                        break;
                    case 4:
                        text = "Hey honey! Come taste my delicousness!!! Uhh, yes, I definitely meant food! Or did I?";
                        break;

                } 

                shop.potraitPersonDialogue = Game1.parseText(text, Game1.dialogueFont, 304);

                Game1.activeClickableMenu = shop;
                */
            };

            //Game1.objectDialoguePortraitPerson = 
            //if (portraitPerson != null)
            //{ }

            //portraitPerson = Game1.objectDialoguePortraitPerson;


            if (action == "MeriCoke")
            {
                createQuestionDialogue(Meri, createYesNoResponses(), "MeriCola");
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
                    case "MeriCola_Yes":

                        if (Game1.player.Money >= 200)
                        {
                            int mericolaint = 11;// ExternalAPIs.JA.GetObjectId("MeriCola");
                            string mericola = Convert.ToString(mericolaint);
                            Game1.player.Money -= 200;
                            Game1.player.addItemByMenuIfNecessary(new Object(mericola, 1));
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
        public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
        {
            base.drawAboveAlwaysFrontLayer(b);
            
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

            if (action == "RS.bed_message")
            {
                Game1.playSound("doorCreak", 2013);
                Game1.drawDialogueNoTyping("You hear the shrill yet pleasant voice of Bridget, the Ghost InnKeeper: The guests what pays are the ones what sleeps in the nice beds. The likes of you sleep in the free beds, in the underbelly o' this fair establishment.");
                Game1.playSound("cacklingWitch");
                DelayedAction.playSoundAfterDelay("doorCreak", 2000, this, null, 2013);
            }

            if (action == "stoneStep")
            {
                Game1.playSound("stoneStep");
            }
            base.performTouchAction(actionStr, tileLocation);
        }

        public override void UpdateWhenCurrentLocation(GameTime time)
        {
            base.UpdateWhenCurrentLocation(time);
           
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