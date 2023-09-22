using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Monsters;
using StardewModdingAPI;
using StardewModdingAPI.Enums;
using StardewModdingAPI.Events;


namespace RestStopLocations
{
    [XmlType("Mods_ApryllForever_RestStopLocationss_LavaBurst")]
    public class LavaBurst : SquidKid
    {
       
        public LavaBurst() { }



        public LavaBurst(Vector2 position)
            //: base("Squid Kid", position)
        {
            Sprite.SpriteHeight = 16;
            base.IsWalkingTowardPlayer = false;
            Sprite.UpdateSourceRect();
            base.HideShadow = true;
        

         //parseMonsterInfo("Iridium Bat");
          
            Health = MaxHealth = 900;
            DamageToFarmer = 80;
            displayName = "Lava Burst";
        }


        /*
        public override void reloadSprite()
        {
            this.Sprite = new AnimatedSprite(helper.Content.GetActualAssetKey("assets/enemies/bat" + (Game1.random.Next(2) + 1) + ".png"), 0, 16, 16);
        }
        */

    }
}