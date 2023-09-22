using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.BellsAndWhistles;
using System.Collections.Generic;
using System.Linq;

namespace RestStopLocations
{
    /// <summary>Spawns a number of custom fireflies when the local player arrives at a map, based on preset conditions.</summary>
    public static class FireflySpawner
    {

        
        /// <summary>True if this fix is currently enabled.</summary>
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
                Helper.Events.GameLoop.DayStarted += GameLoop_DayStarted;
                Helper.Events.Player.Warped += Player_Warped;
                Helper.Events.GameLoop.UpdateTicked += GameLoop_UpdateTicked;

                Enabled = true;
            }
        }

        
        public static int NumberOfFireflies(string locationName)
        {
           


            if (locationName == "Custom_SapphireSprings" && Game1.timeOfDay > 1800)
            {
                return 37; //Game1.random.Next(30, 40); //random amount from 20 to 30; resets each time the farmhouse is entered
            }


            else if (locationName == "Custom_SovarraCanyon" && Game1.timeOfDay > 1800)
            {
                return 37;
            }
            else if (locationName == "Custom_RestStop" && Game1.timeOfDay > 1800)
            {
                return 37;
            }
            else if (locationName == "Custom_SouthRestStop" && Game1.timeOfDay > 1800)
            {
                return 37;
            }
            else if (locationName == "Custom_SapphireForest")
            {
                return 50;
            }

            return 0; //default to 0 fireflies in all locations not specificed
        }


        public static Critter CreateNewFirefly(string locationName, Vector2 tile)
        {
           
            if (locationName == "Custom_SovarraCanyon")
            {
                if (Game1.random.NextDouble() < 0.7) //70% chance
                    return new FireflyCritter(tile, true, Color.White); //red body, normal glow
                else //30% chance
                    return new FireflyCritter(tile, true, Color.White); //green body, normal glow
            }


            else if (locationName == "Custom_SapphireForest")
            {
                if (Game1.random.NextDouble() < 0.7) //70% chance
                    return new FireflyCritter(tile, true, Color.White); //red body, normal glow
                else //30% chance
                    return new FireflyCritter(tile, true, Color.White); //green body, normal glow
            }

            else if (locationName == "Custom_SapphireSprings")
            {
                return new FireflyCritter(tile, true, Color.White); //red body, normal glow
            }

            return new FireflyCritter(tile); //create a default firefly
        }


        private static void GameLoop_DayStarted(object sender, DayStartedEventArgs e)
        {
            Populate(Game1.player.currentLocation);
        }

        private static void Player_Warped(object sender, WarpedEventArgs e)
        {
            Populate(e.NewLocation);
        }

        /// <summary>True if there was an event active during the previous tick. False otherwise.</summary>
        private static bool wasEvent = false;
        /// <summary>The number of ticks before fireflies are repopulated at this location. -1 if inactive.</summary>
        private static int ticksUntilPopulate = -1;
        /// <summary>Detects when <see cref="Game1.CurrentEvent"/> is cleared and, after delaying a set number of ticks, repopulates fireflies at the player's location.</summary>
        private static void GameLoop_UpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (ticksUntilPopulate == 0) //if the countdown until firefly repopulation has finished
            {
                Populate(Game1.player.currentLocation);
                ticksUntilPopulate = -1; //reset the counter
            }
            else if (wasEvent == true && Game1.CurrentEvent == null) //if an event ended on this tick
            {
                ticksUntilPopulate = 10;
            }

            //update tracking fields
            wasEvent = Game1.CurrentEvent != null;

            if (ticksUntilPopulate > 0)
                ticksUntilPopulate--;
        }

        /// <summary>Sets the number of custom fireflies at a location.</summary>
        /// <param name="location">The location to check (generally the local player's current location).</param>
        private static void Populate(GameLocation location)
        {
            if (location == null)
                return; //do nothing

            string locationName = location.Name ?? ""; //get the location's name (blank if null)

            location.instantiateCrittersList(); //make sure the critter list isn't null

            int targetAmount = NumberOfFireflies(locationName); //get the target amount of fireflies for this location

            while (targetAmount > 0) //for each custom firefly that should spawn here
            {
                Vector2 tile = location.getRandomTile();
                location.critters.Add(CreateNewFirefly(locationName, tile)); //create a firefly at this tile
                targetAmount--; //count down

                double chance = Game1.random.NextDouble(); //get a random number from 0 to 1
                if (chance < 0.1 && targetAmount >= 2) //10% chance if 2+ fireflies still need to spawn
                {
                    //spawn 2 more fireflies near the previous firefly

                    Vector2 nearbyTile = new Vector2(tile.X + Game1.random.Next(-2, 3), tile.Y + Game1.random.Next(-2, 3)); //get a random nearby tile
                    location.critters.Add(CreateNewFirefly(locationName, nearbyTile)); //create a firefly at this tile
                    nearbyTile = new Vector2(tile.X + Game1.random.Next(-2, 3), tile.Y + Game1.random.Next(-2, 3)); //get another random nearby tile
                    location.critters.Add(CreateNewFirefly(locationName, nearbyTile)); //create a firefly at this tile

                    targetAmount -= 2; //count down by 2
                }
                else if (chance < 0.4 && targetAmount >= 1) //30% chance if 1+ firefly still needs to spawn (40% if *only* 1 firefly is left)
                {
                    //spawn 1 more firefly near the previous firefly

                    Vector2 nearbyTile = new Vector2(tile.X + Game1.random.Next(-2, 3), tile.Y + Game1.random.Next(-2, 3)); //get a random nearby tile
                    location.critters.Add(CreateNewFirefly(locationName, nearbyTile)); //create a firefly at this tile
                }
            }
        }

        public class FireflyCritter : Critter
        {
            /* Base SDV Firefly fields */
            /* (private -> protected)  */

            protected bool glowing;

            protected int glowTimer;

            protected int id;

            protected Vector2 motion;

            protected LightSource light;

            /* New fields */

            protected Color bodyColor;

            public FireflyCritter()
            {
            }

            /// <param name="tile">The firefly's spawn tile.</param>
            /// <param name="bodyColor">The firefly's body color.</param>
            /// <param name="lightColor">The color of light emitted by this firefly. Note that this is inverted and might not be applied to the whole texture.</param>
            /// <param name="lightType">The index of the light texture used by this firefly. Known valid IDs: 1, 2, 4, 5, 6, 7, 8. See <see cref="LightSource"/> code for details.</param>
            public FireflyCritter(Vector2 tile, bool glowing = true, Color? bodyColor = null, Color? lightColor = null, int lightType = 4)
            {
                baseFrame = -1;
                base.position = tile * 64f;    //renamed "position" argument to "tile" for clarity
                startingPosition = tile * 64f; //
                motion = new Vector2((float)Game1.random.Next(-10, 11) * 0.1f, (float)Game1.random.Next(-10, 11) * 0.1f);
                this.glowing = glowing; //set glowing
                if (glowing) //only set up light-related fields if glowing is enabled
                {
                    id = (int)(position.X * 10099f + position.Y * 77f + (float)Game1.random.Next(99999));
                    light = new LightSource(
                    lightType, //use lightType
                    position,
                    (float)Game1.random.Next(4, 6) * 0.1f,
                    lightColor ?? (Color.Purple * 0.8f), //use lightColor if provided
                    id,
                    LightSource.LightContext.None,
                    0L
                );
                    Game1.currentLightSources.Add(light);
                }

                this.bodyColor = bodyColor ?? Color.White; //set body color (default white if not provided)
            }

            public static Dictionary<Vector2, int> TEST_tiles = new Dictionary<Vector2, int>();

            public override bool update(GameTime time, GameLocation environment)
            {
                position += motion;
                motion.X += (float)Game1.random.Next(-1, 2) * 0.1f;
                motion.Y += (float)Game1.random.Next(-1, 2) * 0.1f;
                if (motion.X < -1f)
                {
                    motion.X = -1f;
                }
                if (motion.X > 1f)
                {
                    motion.X = 1f;
                }
                if (motion.Y < -1f)
                {
                    motion.Y = -1f;
                }
                if (motion.Y > 1f)
                {
                    motion.Y = 1f;
                }
                if (glowing)
                {
                    light.position.Value = position;
                }
                if (position.X < -128f || position.Y < -128f || position.X > (float)environment.map.DisplayWidth || position.Y > (float)environment.map.DisplayHeight)
                {
                    return true;
                }
                return false;
            }

            public override void drawAboveFrontLayer(SpriteBatch b)
            {
                b.Draw(Game1.staminaRect,
                    Game1.GlobalToLocal(position),
                    Game1.staminaRect.Bounds,
                    bodyColor, //use body color
                    0f,
                    Vector2.Zero,
                    4f,
                    SpriteEffects.None,
                    1f);
            }
        }
    }
}