using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.BellsAndWhistles;

namespace RestStopLocations
{
    public class Items
    {


        private static bool Enabled { get; set; } = false;
        /// <summary>The SMAPI helper instance to use for events and other API access.</summary>
        private static IModHelper Helper { get; set; } = null;
        /// <summary>The monitor instance to use for log messages. Null if not provided.</summary>
        private static IMonitor Monitor { get; set; } = null;

        /// <summary>Initializes and enables this class.</summary>
        /// <param name="harmony">The <see cref="Harmony"/> created with this mod's ID.</param>
        /// <param name="monitor">The <see cref="IMonitor"/> provided to this mod by SMAPI. Used for log messages.</param>
        public static void Enable(IModHelper helper, IMonitor monitor)
        {
            if (!Enabled && helper != null && monitor != null) //if NOT already enabled
            {
                Helper = helper; //store helper
                Monitor = monitor; //store monitor

                //enable SMAPI event(s)
                Helper.Events.GameLoop.SaveLoaded += GameLoop_SaveLoaded;

                Enabled = true;
            }
        }

        public static void GameLoop_SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
           


        }








    }
}
