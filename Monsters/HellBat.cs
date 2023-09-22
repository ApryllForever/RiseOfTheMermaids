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
    [XmlType("Mods_ApryllForever_RestStopLocationss_HellBat")]
    public class HellBat : Bat
    {
        private static IMonitor Monitor { get; set; }
        private static IModHelper Helper { get; set; }

        IModHelper helper;
        public HellBat() { }

        private float extraVelocity;
        public HellBat(Vector2 pos)
            : base(pos, 0)
        {
            this.Name = "Iridium Bat";
            this.reloadSprite();
            this.Sprite.SpriteHeight = 24;
            this.Sprite.UpdateSourceRect();
            //this.color.Value = Color.White;

            parseMonsterInfo("Iridium Bat");
            extraVelocity = 5f;
            Health = MaxHealth = 1200;
            DamageToFarmer = 80;
            displayName = "Hell Bat";
        }


        /*
        public override void reloadSprite()
        {
            this.Sprite = new AnimatedSprite(helper.Content.GetActualAssetKey("assets/enemies/bat" + (Game1.random.Next(2) + 1) + ".png"), 0, 16, 16);
        }
        */

    }
}