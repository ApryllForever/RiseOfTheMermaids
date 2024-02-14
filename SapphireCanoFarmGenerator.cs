


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SpaceShared;
using StardewValley;
using xTile;
using xTile.Tiles;
using StardewModdingAPI;
using StardewValley.Objects;
using StardewValley.Monsters;
using StardewModdingAPI.Enums;
using StardewModdingAPI.Events;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley.Network;


using Object = StardewValley.Object;

namespace RestStopLocations.Game.Locations.DungeonLevelGenerators
{
    public class CanoFarmLevelGenerator : SapphireBaseLevelGenerator
    {

        private static IMonitor Monitor { get; set; }
        private static IModHelper Helper { get; set; }

        IModHelper helper;

        public override void Generate(SapphireVolcano location, ref Vector2 warpFromPrev, ref Vector2 warpFromNext)
        {

            Random rand = new Random(location.genSeed.Value);

            location.isIndoorLevel = false;


            var BeltMap = Game1.game1.xTileContent.Load<Map>(Mod.instance.Helper.ModContent.GetInternalAssetName("assets/maps/SapphireCanoFarm.tmx").BaseName);


            int x = (location.Map.Layers[0].LayerWidth - BeltMap.Layers[0].LayerWidth) / 2;
            int y = (location.Map.Layers[0].LayerHeight - BeltMap.Layers[0].LayerHeight) / 2;

            location.ApplyMapOverride(BeltMap, "actual_map", null, new Microsoft.Xna.Framework.Rectangle(x, y, BeltMap.Layers[0].LayerWidth, BeltMap.Layers[0].LayerHeight));



            warpFromPrev = new Vector2(x + 3, y + 15);

            //warpFromNext = new Vector2(x + 3, y + 4);

            //location.warps.Add(new Warp(x + 6, y + 21, "Custom_HellDungeon" + location.level.Value / 100, 1, location.level.Value % 100, false));
            PlaceNextWarp(location, 99, 99);
            /*
            long id2 = 6942069696969696902;
            long id3 = 6942069696969696903;
            long id4 = 6942069696969696904;
            long id5 = 6942069696969696905;
            long id6 = 6942069696969696906;
            long id7 = 6942069696969696907;

            HellAnimalType type2 = HellAnimalType.WhiteCow;
            HellAnimalType type3 = HellAnimalType.Duck;
            HellAnimalType type4 = HellAnimalType.Rabbit;
            HellAnimalType type5 = HellAnimalType.BrownChicken;
            HellAnimalType type6 = HellAnimalType.WhiteChicken;
            HellAnimalType type7 = HellAnimalType.Ostrich;
            Animals.Add(id2, new HellAnimal(type2, new Vector2(37, 43) * Game1.tileSize, id2));
            Animals.Add(id3, new HellAnimal(type3, new Vector2(39, 44) * Game1.tileSize, id3));
            Animals.Add(id4, new HellAnimal(type4, new Vector2(38, 46) * Game1.tileSize, id4));
            Animals.Add(id5, new HellAnimal(type5, new Vector2(36, 49) * Game1.tileSize, id5));
            Animals.Add(id6, new HellAnimal(type6, new Vector2(37, 47) * Game1.tileSize, id6));
            Animals.Add(id7, new HellAnimal(type7, new Vector2(37, 45) * Game1.tileSize, id7));
            HellAnimalType type1 = HellAnimalType.Ostrich;


            //SapphireSprings location = new SapphireSprings();

            long id1 = 6942069696969696901;
            Animals.Add(id1, new HellAnimal(type1, new Vector2(28, 41) * Game1.tileSize, id1));*/


        }

        new public void updateWater(GameTime time)
        {
            waterAnimationTimer -= time.ElapsedGameTime.Milliseconds;
            if (waterAnimationTimer <= 0)
            {
                waterAnimationIndex = (waterAnimationIndex + 1) % 10;
                waterAnimationTimer = 200;
            }
            if (!isFarm)
            {
                waterPosition += (float)((Math.Sin((float)time.TotalGameTime.Milliseconds / 1000f) + 1.0) * 0.15000000596046448);
            }
            else
            {
                waterPosition += 0.1f;
            }
            if (waterPosition >= 64f)
            {
                waterPosition -= 64f;
                waterTileFlip = !waterTileFlip;
            }
        }
        public override void TransferDataFromSavedLocation(GameLocation l)
        {
            var other = l as SapphireForest;
            // Chickiedoos.MoveFrom(other.Chickiedoos);
            //foreach (FarmAnimal item in Chickiedoos)
            //{
            //  item.reload(null);
            //}

            Animals.MoveFrom(other.Animals);
            foreach (var animal in Animals.Values)
            {
                animal.reload(null);
            }
            base.TransferDataFromSavedLocation(l);
        }

        public bool CheckInspectAnimal(Vector2 position, Farmer who)
        {
           foreach (var animal in Animals.Values)
            {
                if (animal.wasPet.Value && animal.GetCursorPetBoundingBox().Contains((int)position.X, (int)position.Y))
                {
                    animal.pet(who);
                    return true;
                }
            }

            return false;
        }

        public bool CheckInspectAnimal(Microsoft.Xna.Framework.Rectangle rect, Farmer who)
        {
            foreach (var animal in Animals.Values)
            {
                if (animal.wasPet.Value && animal.GetBoundingBox().Intersects(rect))
                {
                    animal.pet(who);
                    return true;
                }
            }

            return false;
        }

        public bool CheckPetAnimal(Vector2 position, Farmer who)
        {
            foreach (var animal in Animals.Values)
            {
                if (!animal.wasPet.Value && animal.GetCursorPetBoundingBox().Contains((int)position.X, (int)position.Y))
                {
                    animal.pet(who);
                    return true;
                }
            }

            return false;
        }

        public bool CheckPetAnimal(Microsoft.Xna.Framework.Rectangle rect, Farmer who)
        {
            foreach (var animal in Animals.Values)
            {
                if (!animal.wasPet.Value && animal.GetBoundingBox().Intersects(rect))
                {
                    animal.pet(who);
                    return true;
                }
            }

            return false;
        }

        public override void DayUpdate(int dayOfMonth)
        {
            for (int i = this.animals.Count() - 1; i >= 0; i--)
            {
                var animal = this.animals.Pairs.ElementAt(i).Value;
               
                    animal.dayUpdate(this);
            }
            base.DayUpdate(dayOfMonth);
        }

        public override void performTenMinuteUpdate(int timeOfDay)
        {
            base.performTenMinuteUpdate(timeOfDay);
            if (Game1.IsMasterGame)
            {
                foreach (FarmAnimal value in this.animals.Values)
                {
                    value.updatePerTenMinutes(Game1.timeOfDay, this);
                }
            }
        }

        public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character)
        {
            if (!glider)
            {
                if (character != null && !(character is FarmAnimal))
                {
                    Microsoft.Xna.Framework.Rectangle playerBox = Game1.player.GetBoundingBox();
                    Farmer farmer = (isFarmer ? (character as Farmer) : null);
                    foreach (FarmAnimal animal in this.animals.Values)
                    {
                        if (position.Intersects(animal.GetBoundingBox()) && (!isFarmer || !playerBox.Intersects(animal.GetBoundingBox())))
                        {
                            if (farmer != null && farmer.TemporaryPassableTiles.Intersects(position))
                            {
                                break;
                            }
                            animal.farmerPushing();
                            return true;
                        }
                    }
                }
            }
            return base.isCollidingPosition(position, viewport, character is Farmer, damagesFarmer, glider, character, pathfinding: false);
        }

      

        public override void draw(SpriteBatch b)
        {
            base.draw(b);

            foreach (KeyValuePair<long, FarmAnimal> pair in this.animals.Pairs)
            {
                pair.Value.draw(b);
            }
        }
        public override void updateEvenIfFarmerIsntHere(GameTime time, bool skipWasUpdatedFlush = false)
        {
            base.updateEvenIfFarmerIsntHere(time, skipWasUpdatedFlush);

            if (!Game1.currentLocation.Equals(this))
            {
                NetDictionary<long, FarmAnimal, NetRef<FarmAnimal>, SerializableDictionary<long, FarmAnimal>, NetLongDictionary<FarmAnimal, NetRef<FarmAnimal>>>.PairsCollection pairs = this.animals.Pairs;
                for (int i = pairs.Count() - 1; i >= 0; i--)
                {
                    pairs.ElementAt(i).Value.updateWhenNotCurrentLocation(null, time, this);
                }
            }
        }

        public override void UpdateWhenCurrentLocation(GameTime time)
        {
            base.UpdateWhenCurrentLocation(time);

            foreach (KeyValuePair<long, FarmAnimal> kvp in this.Animals.Pairs)
            {
                kvp.Value.updateWhenCurrentLocation(time, this);
            }
        }
        public override void checkForMusic(GameTime time)
        {
            {
                Game1.changeMusicTrack("bigDrums", track_interruptable: false);
            }

        }


        public override void drawWater(SpriteBatch b)
        {
            if (currentEvent != null)
            {
                currentEvent.drawUnderWater(b);
            }
            if (waterTiles == null)
            {
                return;
            }
            for (int y = Math.Max(0, Game1.viewport.Y / 64 - 1); y < Math.Min(map.Layers[0].LayerHeight, (Game1.viewport.Y + Game1.viewport.Height) / 64 + 2); y++)
            {
                for (int x = Math.Max(0, Game1.viewport.X / 64 - 1); x < Math.Min(map.Layers[0].LayerWidth, (Game1.viewport.X + Game1.viewport.Width) / 64 + 1); x++)
                {
                    if (waterTiles[x, y])
                    {
                        drawWaterTile(b, x, y);
                    }
                }
            }
            base.drawWater(b);
        }

        public override void drawWaterTile(SpriteBatch b, int x, int y)
        {
            drawWaterTile(b, x, y, this.waterColor.Value);
            base.drawWaterTile(b, x, y, this.waterColor.Value);
        }

        new public void drawWaterTile(SpriteBatch b, int x, int y, Color color)
        {
            bool num = y == map.Layers[0].LayerHeight - 1 || !waterTiles[x, y + 1];
            bool topY = y == 0 || !waterTiles[x, y - 1];
            b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, y * 64 - (int)((!topY) ? waterPosition : 0f))), new Microsoft.Xna.Framework.Rectangle(waterAnimationIndex * 64, 2064 + (((x + y) % 2 != 0) ? ((!waterTileFlip) ? 128 : 0) : (waterTileFlip ? 128 : 0)) + (topY ? ((int)waterPosition) : 0), 64, 64 + (topY ? ((int)(0f - waterPosition)) : 0)), color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.56f);
            if (num)
            {
                b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(x * 64, (y + 1) * 64 - (int)waterPosition)), new Microsoft.Xna.Framework.Rectangle(waterAnimationIndex * 64, 2064 + (((x + (y + 1)) % 2 != 0) ? ((!waterTileFlip) ? 128 : 0) : (waterTileFlip ? 128 : 0)), 64, 64 - (int)(64f - waterPosition) - 1), color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.56f);
            }
            
        }




    }
}




