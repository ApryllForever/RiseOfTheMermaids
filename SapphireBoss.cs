
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using StardewValley.Objects;
using xTile;
using StardewValley;
using StardewValley.Monsters;
using StardewModdingAPI;
using StardewModdingAPI.Enums;
using StardewModdingAPI.Events;

using SpaceShared;
using Object = StardewValley.Object;

namespace RestStopLocations.Game.Locations.DungeonLevelGenerators
{
    public class SapphireBossLevelGenerator : SapphireBaseLevelGenerator
    {

        private static IMonitor Monitor { get; set; }
        private static IModHelper Helper { get; set; }

        IModHelper helper;


        public override void Generate(SapphireVolcano location, ref Vector2 warpFromPrev, ref Vector2 warpFromNext)
        {
            Map place = Mod.instance.Helper.ModContent.Load<Map>("assets/maps/HellDungeonBoss.tmx");
            int offsetX = (location.Map.Layers[0].LayerWidth - place.Layers[0].LayerWidth) / 2;
            int offsetY = (location.Map.Layers[0].LayerHeight - place.Layers[0].LayerHeight) / 2;

            location.ApplyMapOverride(place, "actual_map", new Rectangle(0, 0, place.Layers[0].LayerWidth, place.Layers[0].LayerHeight), new Rectangle(offsetX, offsetY, place.Layers[0].LayerWidth, place.Layers[0].LayerHeight));

            warpFromPrev = warpFromNext = new Vector2(4 + offsetX, 4 + offsetY);

          

            PlaceNextWarp(location, offsetX + 77, offsetY + 77);



        }
    }
}

