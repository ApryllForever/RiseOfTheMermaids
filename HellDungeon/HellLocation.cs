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



namespace RestStopLocations.Game.Locations
{
    [XmlType("Mods_ApryllForever_RestStopLocations_HellLocation")]
    public class HellLocation : IslandLocation
    {
       

        public HellLocation() { }
        public HellLocation(IModContentHelper content, string mapPath, string mapName)
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
                    grass.grassType.Value = Grass.lavaGrass;
                    grass.loadSprite();
                }
                
            };
        }

        
        /* commented out because probably interfere with lava warps
        protected override void resetLocalState()
        {
            base.resetLocalState();

            //Game1.changeMusicTrack("into-the-spaceship");

            if (IsOutdoors)
            {
                Game1.drawLighting = true;
                int colValue = (14 - Game1.dayOfMonth % 14) * 7;
                if (Game1.dayOfMonth > 14)
                    colValue = (Game1.dayOfMonth % 14 - 14) * 7;
                colValue = 175 - colValue;
                Game1.ambientLight = Game1.outdoorLight = new Color(colValue, colValue, colValue);// new Color( 100, 120, 30 );
            }

            foreach (var tf in terrainFeatures.Values)
            {
                if (tf is HoeDirt hd)
                {
                    Mod.instance.Helper.Reflection.GetField<Texture2D>(hd, "texture").SetValue(Assets.HoeDirt);
                }
            }

           //Game1.background = new SpaceBackground(this.NameOrUniqueName == "Custom_MM_MoonPlanetOverlook");
        }
        public override void cleanupBeforePlayerExit()
        {
            base.cleanupBeforePlayerExit();
            Game1.ambientLight = Game1.outdoorLight = Color.Black;
            Game1.background = null;
        } */

        public override bool SeedsIgnoreSeasonsHere()
        {
            return true;
        }


        /*
        public override void checkForEvents()
        {
            if (Game1.killScreen && !Game1.eventUp)
            {
                if (name.Contains("HellDungeon"))
                {
                    Game1.warpFarmer("Custom_SouthRestStop", 39, 18, flip: false);
                    string rescuer3 = "Vika";
                    string unique
        3 = "Data\\ExtraDialogue:Mines_PlayerKilled_Vika";
                    switch (Game1.random.Next(7))
                    {
                        case 0:
                            rescuer3 = "Wren";
                            uniquemessage3 = "Data\\ExtraDialogue:Mines_PlayerKilled_Wren";
                            break;
                        case 1:
                            rescuer3 = "Clint";
                            uniquemessage3 = "Data\\ExtraDialogue:Mines_PlayerKilled_Clint";
                            break;
                        case 2:
                            rescuer3 = "Maru";
                            uniquemessage3 = ((Game1.player.spouse != null && Game1.player.spouse.Equals("Maru")) ? "Data\\ExtraDialogue:Mines_PlayerKilled_Maru_Spouse" : "Data\\ExtraDialogue:Mines_PlayerKilled_Maru_NotSpouse");
                            break;
                        default:
                            rescuer3 = "Vika";
                            uniquemessage3 = "Data\\ExtraDialogue:Mines_PlayerKilled_Vika";
                            break;
                    }
                    if (Game1.random.NextDouble() < 0.1 && Game1.player.spouse != null && !Game1.player.isEngaged() && Game1.player.spouse.Length > 1)
                    {
                        rescuer3 = Game1.player.spouse;
                        uniquemessage3 = (Game1.player.IsMale ? "Data\\ExtraDialogue:Mines_PlayerKilled_Spouse_PlayerMale" : "Data\\ExtraDialogue:Mines_PlayerKilled_Spouse_PlayerFemale");
                    }
                    currentEvent = new Event(Game1.content.LoadString("Data\\Events\\Custom_SouthRestStop:PlayerKilled", rescuer3, uniquemessage3, Game1.player.Name));
                }

            }
        }*/


        //public override string checkForBuriedItem(int xLocation, int yLocation, bool explosion, bool detectOnly, Farmer who)
        //{
        // Random r = new Random(xLocation * 3000 + yLocation + (int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed + Name.GetDeterministicHashCode());
        //  if (r.NextDouble() < 0.03)
        //   {
        //       Game1.createObjectDebris(424 /* cheese */, xLocation, yLocation, this);
        //   }
        //   return base.checkForBuriedItem(xLocation, yLocation, explosion, detectOnly, who);
        // }


       

        public override void tryToAddCritters(bool onlyIfOnScreen = false)
        {
            base.tryToAddCritters(onlyIfOnScreen);  
        }



        
    }
}