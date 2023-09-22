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
    [XmlType("Mods_ApryllForever_RestStopLocationss_HellSkell")]
    public class HellSkell : Skeleton
    {
      

       
        public HellSkell() { }

        private float extraVelocity;
        public HellSkell(Vector2 pos)
            : base(pos, false)
        {
            this.Name = "Zombie";
            this.reloadSprite();
            

            parseMonsterInfo("Zombie");
            extraVelocity = 5f;
            Health = MaxHealth = 800;
            DamageToFarmer = 50;
            displayName = "Zombie";

        }


        /*
        public override void reloadSprite()
        {
            this.Sprite = new AnimatedSprite(Helper.ModContent.GetInternalAssetName("Assets/Monsters/HellSkell.png").BaseName, 0, 16, 32);
        }
        */

    }
}