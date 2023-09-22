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
    [XmlType("Mods_ApryllForever_RestStopLocations_SapphireDecoratableLocation")]
    public class SapphireDecoratableLocation : DecoratableLocation
    {
       

        public SapphireDecoratableLocation() { }
        public SapphireDecoratableLocation(IModContentHelper content, string mapPath, string mapName)
        : base(content.GetInternalAssetName("assets/maps/" + mapPath + ".tmx").BaseName, "Custom_" + mapName)
        {
        }

        protected override void initNetFields()
        {
            base.initNetFields();


        }




        
    }
}