

using System;
using Microsoft.Xna.Framework;
using StardewValley;
using xTile;
using StardewModdingAPI;
using xTile.ObjectModel;
using StardewValley.Objects;
using Object = StardewValley.Object;
using System.Xml.Serialization;
using StardewValley.Network;
using Netcode;
using xTile.Dimensions;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Microsoft.Xna.Framework.Graphics;
using xTile.Layers;
using StardewValley.BellsAndWhistles;
using Microsoft.Xna.Framework.Audio;

namespace RestStopLocations.Game.Locations.DungeonLevelGenerators
{
    public class SubwayDungeonLevelGenerator : BaseDungeonLevelGenerator
    {
        private static IMonitor Monitor { get; set; }
        private static IModHelper Helper { get; set; }

        IModHelper helper;

        public SubwayDungeonLevelGenerator() : base()
        {
            isOutdoors.Value = false;
            isIndoorLevel = true;


        }

        public override void Generate(HellDungeon location, ref Vector2 warpFromPrev, ref Vector2 warpFromNext)
        {
            Random rand = new Random(location.genSeed.Value);
            location.isIndoorLevel = true;

            var caveMap = Game1.game1.xTileContent.Load<Map>(Mod.instance.Helper.ModContent.GetInternalAssetName("assets/maps/HellDungeonSubway.tmx").BaseName);

            int x = (location.Map.Layers[0].LayerWidth - caveMap.Layers[0].LayerWidth) / 2;
            int y = (location.Map.Layers[0].LayerHeight - caveMap.Layers[0].LayerHeight) / 2;

            location.ApplyMapOverride(caveMap, "actual_map", null, new Rectangle(x, y, caveMap.Layers[0].LayerWidth, caveMap.Layers[0].LayerHeight));

            warpFromPrev = new Vector2(x + 36, y + 35);
            //location.warps.Add(new Warp(x + 6, y + 11, "Custom_HellDungeon" + location.level.Value / 100, 1, location.level.Value % 100, false));
            PlaceNextWarp(location, 97, 97);



            {
                Vector2 position = new Vector2(x + 12, y + 3);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(ItemRegistry.Create("(O)773", 5)); // Life Elixir
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }


            {
                Vector2 objectPos = new Vector2(x + 48, y + 19);
                Object o = new Object(objectPos, "166"); //Treasure Chest
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }
            {
                Vector2 objectPos = new Vector2(x + 49, y + 19);
                Object o = new Object(objectPos, "166"); //Treasure Chest
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }
            {
                Vector2 objectPos = new Vector2(x + 50, y + 19);
                Object o = new Object(objectPos, "166"); //Treasure Chest
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }
            {
                Vector2 objectPos = new Vector2(x + 51, y + 19);
                Object o = new Object(objectPos, "166"); //Treasure Chest
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }
            {
                Vector2 objectPos = new Vector2(x + 52, y + 19);
                Object o = new Object(objectPos, "166"); //Treasure Chest
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }
            {
                Vector2 objectPos = new Vector2(x + 53, y + 19);
                Object o = new Object(objectPos, "166"); //Treasure Chest
                o.IsSpawnedObject = true;
                o.CanBeGrabbed = true;
                location.Objects.Add(objectPos, o);
            }

            {
                int silverkeyint = 99;// ExternalAPIs.JA.GetObjectId("Silver Key");
                string silverkey = Convert.ToString(silverkeyint);
                Vector2 position = new Vector2(x + 75, y + 80);
                Chest chest = new Chest(playerChest: false, position);
                chest.dropContents.Value = true;
                chest.synchronized.Value = true;
                chest.type.Value = "interactive";
                chest.SetBigCraftableSpriteIndex(227);
                chest.addItem(new Object(silverkey, 1));
                if (location.netObjects.ContainsKey(position))
                    location.netObjects.Remove(position);
                location.netObjects.Add(position, chest);
            }


            {
                PlaceMonsterAt(location, rand, x + 43, y + 54);
                PlaceMonsterAt(location, rand, x + 41, y + 56);
                PlaceMonsterAt(location, rand, x + 39, y + 55);
                PlaceMonsterAt(location, rand, x + 36, y + 54);

                PlaceMonsterAt(location, rand, x + 51, y + 51);
                PlaceMonsterAt(location, rand, x + 52, y + 54);
                PlaceMonsterAt(location, rand, x + 54, y + 57);
                PlaceMonsterAt(location, rand, x + 53, y + 60);
                PlaceMonsterAt(location, rand, x + 57, y + 58);
                PlaceMonsterAt(location, rand, x + 52, y + 60);
                PlaceMonsterAt(location, rand, x + 53, y + 58);

                PlaceMonsterAt(location, rand, x + 1, y + 46);
                PlaceMonsterAt(location, rand, x + 1, y + 53);
                PlaceMonsterAt(location, rand, x + 1, y + 56);

                PlaceMonsterAt(location, rand, x + 50, y + 57);
                PlaceMonsterAt(location, rand, x + 50, y + 41);
                PlaceMonsterAt(location, rand, x + 57, y + 51);
                PlaceMonsterAt(location, rand, x + 45, y + 51);

                PlaceMonsterAt(location, rand, x + 65, y + 37);
                PlaceMonsterAt(location, rand, x + 65, y + 35);

                PlaceMonsterAt(location, rand, x + 80, y + 60);
                PlaceMonsterAt(location, rand, x + 79, y + 62);

                PlaceMonsterAt(location, rand, x + 24, y + 95);
                PlaceMonsterAt(location, rand, x + 22, y + 90);
                PlaceMonsterAt(location, rand, x + 20, y + 92);
                PlaceMonsterAt(location, rand, x + 46, y + 8);
                PlaceMonsterAt(location, rand, x + 45, y + 7);
                PlaceMonsterAt(location, rand, x + 50, y + 20);
            }

            {
                   //MachineGun Monsters
                PlaceMonster(location, 10, x + 53, y + 88);
                PlaceMonster(location, 10, x + 55, y + 88);
                PlaceMonster(location, 10, x + 49, y + 88);

                PlaceMonster(location, 10, x + 50, y + 96);
                PlaceMonster(location, 10, x + 52, y + 96);
                PlaceMonster(location, 8, x + 54, y + 96);

                PlaceMonster(location, 8, x + 96, y + 20);
                PlaceMonster(location, 8, x + 97, y + 21);
                PlaceMonster(location, 8, x + 98, y + 22);
                PlaceMonster(location, 8, x + 95, y + 98);

                PlaceMonster(location, 10, x + 49, y + 83);
                PlaceMonster(location, 10, x + 51, y + 83);
                PlaceMonster(location, 10, x + 53, y + 83);
                PlaceMonster(location, 10, x + 55, y + 83);
                PlaceMonster(location, 10, x + 56, y + 86);
                PlaceMonster(location, 10, x + 47, y + 83);
                PlaceMonster(location, 10, x + 47, y + 81);
                PlaceMonster(location, 10, x + 47, y + 86);
                PlaceMonster(location, 10, x + 49, y + 86);
                PlaceMonster(location, 10, x + 51, y + 86);
                PlaceMonster(location, 10, x + 53, y + 86);

                PlaceMonster(location, 11, x + 48, y + 85);
                PlaceMonster(location, 11, x + 50, y + 85);
                PlaceMonster(location, 11, x + 52, y + 85);
                PlaceMonster(location, 11, x + 54, y + 85);

                PlaceMonster(location, 11, x + 50, y + 89);
                PlaceMonster(location, 11, x + 52, y + 89);
                PlaceMonster(location, 11, x + 54, y + 89);
                PlaceMonster(location, 11, x + 56, y + 89);

            



            }
            {
                PlaceBreakableAt(location, rand, x + 3, y + 30);
                PlaceBreakableAt(location, rand, x + 3, y + 31);
                PlaceBreakableAt(location, rand, x + 3, y + 32);

            }

        }







		public const int trainSoundDelay = 15000;

		[XmlIgnore]
		public readonly NetRef<Train> train = new NetRef<Train>();

		[XmlIgnore]
		private readonly NetBool hasTrainPassed = new NetBool(value: false);

		private int trainTime = -1;

		[XmlIgnore]
		public readonly NetInt trainTimer = new NetInt(0);

		public static ICue trainLoop;

		

		public override void startEvent(Event evt)
		{
			if (evt != null && evt.id == "528052")
			{
				evt.eventPositionTileOffset.X -= 8f;
				evt.eventPositionTileOffset.Y -= 2f;
			}
			base.startEvent(evt);
		}

		protected override void initNetFields()
		{
			base.initNetFields();
			
			
			
		}


		protected override void resetLocalState()
		{
			base.resetLocalState();
			if (Game1.getMusicTrackName().ToLower().Contains("ambient"))
			{
				Game1.changeMusicTrack("none");
			}
			if (!Game1.IsWinter)
			{
				AmbientLocationSounds.addSound(new Vector2(15f, 56f), 0);
			}
		}

		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			if (SubwayDungeonLevelGenerator.trainLoop != null)
			{
				SubwayDungeonLevelGenerator.trainLoop.Stop(AudioStopOptions.Immediate);
			}
			SubwayDungeonLevelGenerator.trainLoop = null;
		}

	

		public override void DayUpdate(int dayOfMonth)
		{
			base.DayUpdate(dayOfMonth);
			//this.hasTrainPassed.Value = false;
			//this.trainTime = -1;
			Random r = new Random((int)Game1.uniqueIDForThisGame / 2 + (int)Game1.stats.DaysPlayed);
			if (r.NextDouble() < 0.2 && Game1.isLocationAccessible("Railroad"))
			{
				this.trainTime = r.Next(900, 1800);
				this.trainTime -= this.trainTime % 10;
			}
		}

		public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character)
		{
			if (!Game1.eventUp && this.train.Value != null && this.train.Value.getBoundingBox().Intersects(position))
			{
				return true;
			}
			return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character);
		}

		public void setTrainComing(int delay)
		{
			this.trainTimer.Value = delay;
			
		}

	

        public static void Update(GameTime time)
        {






        }




		public override void updateEvenIfFarmerIsntHere(GameTime time, bool skipWasUpdatedFlush = false)
		{
			
			if (this.train.Value != null && this.train.Value.Update(time, this) && Game1.IsMasterGame)
			{
				this.train.Value = null;
			}
			if (!Game1.IsMasterGame || Game1.currentLocation != this)
			{
				return;
			}
			if ( (int)this.trainTimer.Value == 0 && !Game1.isFestival() && this.train.Value == null)
			{
				this.setTrainComing(45000);
			}
			if ((int)this.trainTimer.Value > 0)
			{
				this.trainTimer.Value -= time.ElapsedGameTime.Milliseconds;
				if ((int)this.trainTimer.Value <= 0)
				{
					this.train.Value = new Train();
					base.playSound("trainWhistle");
				}
				if ((int)this.trainTimer.Value < 3500 && Game1.currentLocation == this && Game1.soundBank != null && (SubwayDungeonLevelGenerator.trainLoop == null || !SubwayDungeonLevelGenerator.trainLoop.IsPlaying))
				{
                    SubwayDungeonLevelGenerator.trainLoop = Game1.soundBank.GetCue("trainLoop");
                    SubwayDungeonLevelGenerator.trainLoop.SetVariable("Volume", 0f);
                    SubwayDungeonLevelGenerator.trainLoop.Play();
				}
			}
			if (this.train.Value != null)
			{
				if (Game1.currentLocation == this && Game1.soundBank != null && (SubwayDungeonLevelGenerator.trainLoop == null || !SubwayDungeonLevelGenerator.trainLoop.IsPlaying))
				{
                    SubwayDungeonLevelGenerator.trainLoop = Game1.soundBank.GetCue("trainLoop");
                    SubwayDungeonLevelGenerator.trainLoop.SetVariable("Volume", 0f);
                    SubwayDungeonLevelGenerator.trainLoop.Play();
				}
				if (SubwayDungeonLevelGenerator.trainLoop != null && SubwayDungeonLevelGenerator.trainLoop.GetVariable("Volume") < 100f)
				{
                    SubwayDungeonLevelGenerator.trainLoop.SetVariable("Volume", SubwayDungeonLevelGenerator.trainLoop.GetVariable("Volume") + 0.5f);
				}
			}
			else if (SubwayDungeonLevelGenerator.trainLoop != null && (int)this.trainTimer.Value <= 0)
			{
                SubwayDungeonLevelGenerator.trainLoop.SetVariable("Volume", SubwayDungeonLevelGenerator.trainLoop.GetVariable("Volume") - 0.15f);
				if (SubwayDungeonLevelGenerator.trainLoop.GetVariable("Volume") <= 0f)
				{
                    SubwayDungeonLevelGenerator.trainLoop.Stop(AudioStopOptions.Immediate);
                    SubwayDungeonLevelGenerator.trainLoop = null;
				}
			}
			else if ((int)this.trainTimer.Value > 0 && SubwayDungeonLevelGenerator.trainLoop != null)
			{
                SubwayDungeonLevelGenerator.trainLoop.SetVariable("Volume", SubwayDungeonLevelGenerator.trainLoop.GetVariable("Volume") + 0.15f);
			}
		}

		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			if (this.train.Value != null && !Game1.eventUp)
			{
				this.train.Value.draw(b);
			}
		}

















































	}
}
















