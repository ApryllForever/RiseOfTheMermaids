using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RestStopLocations.Game.Locations.DungeonLevelGenerators;
using Netcode;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Network;
using StardewValley.Objects;
using StardewValley.Tools;
using StardewModdingAPI;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardewValley.TerrainFeatures;
using xTile;
using xTile.Dimensions;
using xTile.Layers;
using xTile.ObjectModel;
using xTile.Tiles;
using StardewValley.Locations;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI.Events;
using SpaceCore.Events;
using Object = StardewValley.Object;
using UtilitiesStuff;
using RestStopLocations.VirtualProperties;

namespace RestStopLocations.Game.Locations
{
	[XmlType("Mods_ApryllForever_RestStopLocations_HellDungeon")]
	public class HellDungeon : HellLocation
	{

		internal static IMonitor Monitor { get; set; }
       

        static IMonitor monitor;
		public readonly NetList<Point, NetPoint> doneSwitches = new();
		[XmlIgnore]
		public readonly NetEvent1Field<Point, NetPoint> doSwitch = new();

		public const string BaseLocationName = "HellDungeon";
		public const string LocationRoomInfix = "Room";
		public const string LocationCaveInfix = "Cave";
		public const string LocationVineInfix = "Vine";
		public const string LocationMetalInfix = "Metal";
		public const string LocationSubwayInfix = "Subway";
		public const int BossLevel = 10;

		static string Meri = "Buy a MeriCola!!! Make yourself faster or something!";

		public enum LevelType
		{
			Outside,
			Room,
			Cave,
			Vine,
			Metal,
			Subway,
		}

		public static List<BaseDungeonLevelGenerator> NormalDungeonGenerators = new()
		{
			//new BlankDungeonLevelGenerator(),
			new BeltDungeonLevelGenerator(),
		};
		public static List<BaseDungeonLevelGenerator> RoomDungeonGenerators = new()
		{
			new RoomDungeonLevelGenerator(),
		};
		public static List<BaseDungeonLevelGenerator> CaveDungeonGenerators = new()
		{
			new CaveDungeonLevelGenerator(),
		};
		public static List<BaseDungeonLevelGenerator> VineDungeonGenerators = new()
		{
			new VineDungeonLevelGenerator(),
		};
		public static List<BaseDungeonLevelGenerator> MetalDungeonGenerators = new()
		{
			new MetalDungeonLevelGenerator(),
		};
		public static List<BaseDungeonLevelGenerator> SubwayDungeonGenerators = new()
		{
			new SubwayDungeonLevelGenerator(),
		};
		public static List<BaseDungeonLevelGenerator> BossDungeonGenerators = new()
		{
			new BossIslandDungeonLevelGenerator(),
		};
		internal static List<HellDungeon> activeLevels = new();


		public static HellDungeon GetLevelInstance(string locName)
			{
				foreach (var level in activeLevels)
				{
					if (level.Name == locName)
						return level;
				}

				LevelType t = LevelType.Outside;
				int l = 0;
			if (char.IsNumber(locName[BaseLocationName.Length]))
			{
				t = LevelType.Outside;
				l = int.Parse(locName.Substring(BaseLocationName.Length));
			}
				else if (locName.StartsWith(BaseLocationName + LocationRoomInfix))
				{
					t = LevelType.Room;
					string[] parts = locName.Substring((BaseLocationName + LocationRoomInfix).Length).Split('_');
					l = int.Parse(parts[0]) * 100 + int.Parse(parts[1]);
				}
				else if (locName.StartsWith(BaseLocationName + LocationCaveInfix))
				{
					t = LevelType.Cave;
					string[] parts = locName.Substring((BaseLocationName + LocationCaveInfix).Length).Split('_');
					l = int.Parse(parts[0]) * 100 + int.Parse(parts[1]);
				}
			else if (locName.StartsWith(BaseLocationName + LocationVineInfix))
			{
				t = LevelType.Vine;
				string[] parts = locName.Substring((BaseLocationName + LocationVineInfix).Length).Split('_');
				l = int.Parse(parts[0]) * 100 + int.Parse(parts[1]);
			}
			else if (locName.StartsWith(BaseLocationName + LocationMetalInfix))
			{
				t = LevelType.Metal;
				string[] parts = locName.Substring((BaseLocationName + LocationMetalInfix).Length).Split('_');
				l = int.Parse(parts[0]) * 100 + int.Parse(parts[1]);
			}
			else if (locName.StartsWith(BaseLocationName + LocationSubwayInfix))
			{
				t = LevelType.Subway;
				string[] parts = locName.Substring((BaseLocationName + LocationSubwayInfix).Length).Split('_');
				l = int.Parse(parts[0]) * 100 + int.Parse(parts[1]);
			}

			HellDungeon newLevel = new HellDungeon(t, l, locName);
				activeLevels.Add(newLevel);

				if (Game1.IsMasterGame)
					newLevel.Generate();
				else
					newLevel.reloadMap();

				return newLevel;
			}

			public static void UpdateLevels(GameTime time)
			{
				foreach (var level in activeLevels.ToList())
				{
					if (level.farmers.Count > 0)
						level.UpdateWhenCurrentLocation(time);
					level.updateEvenIfFarmerIsntHere(time);
				}
			}

			public static void UpdateLevels10Minutes(int time)
			{
				if (Game1.IsClient)
					return;

				foreach (var level in activeLevels.ToList())
				{
					if (level.farmers.Count > 0)
						level.performTenMinuteUpdate(time);
				}
			}

			public static void ClearAllLevels()
			{
				foreach (var level in activeLevels)
				{
					level.mapContent.Dispose();
				}
				activeLevels.Clear();
			}

			private bool generated = false;
			private LocalizedContentManager mapContent;
			public readonly NetEnum<LevelType> levelType = new();
			public readonly NetInt level = new();
			public readonly NetInt genSeed = new();
			private readonly NetVector2 warpFromPrev = new();
			private readonly NetVector2 warpFromNext = new();
			private Random genRandom;
			public bool isIndoorLevel = false;

			public HellDungeon()
			{
				mapContent = Game1.game1.xTileContent.CreateTemporary();
				mapPath.Value = Mod.instance.Helper.ModContent.GetInternalAssetName("assets/maps/HellDungeonTemplate.tmx").BaseName;

		}

			public HellDungeon(LevelType type, int level, string name)
			: this()
			{
            if (!Game1.IsMultiplayer)
            {
                base.ExtraMillisecondsPerInGameMinute = 60000;
            }
            levelType.Value = type;
				this.level.Value = level;
				this.name.Value = name;
			}

			protected override void initNetFields()
			{
				base.initNetFields();
				NetFields.AddField( this.level).AddField(this.genSeed).AddField(warpFromPrev).AddField(warpFromNext);      // Lava  coolLavaEvent,  dwarfGates, cooledLavaTiles.NetFields, layoutIndex, dwarfGates

			
		}



		protected override LocalizedContentManager getMapLoader()
			{
				return mapContent;
			}

			protected override void resetLocalState()
			{
				if (!generated)
					Generate();

				base.resetLocalState();



			
				if (Game1.player.Tile.X == 0)
				{
					if (Game1.player.Tile.Y == 0)
					{
						Game1.player.Position = new Vector2(warpFromPrev.X * Game1.tileSize, warpFromPrev.Y * Game1.tileSize);
					}
					else if (Game1.player.Tile.Y == 1)
					{
						Game1.player.Position = new Vector2(warpFromNext.X * Game1.tileSize, warpFromNext.Y * Game1.tileSize);
					}
				}
				

				if (level.Value == 10)
				{
					Game1.addHUDMessage(new HUDMessage("You hear the wailing of a Banshee...", 1));
				Game1.playSound("ghost", 2100);

				}
			

		}



		public void Generate()
			{
				generated = true;

				BaseDungeonLevelGenerator gen = null;

			//int idForGame = (int)Game1.uniqueIDForThisGame; int daysPlayedForGame = (int)Game1.stats.DaysPlayed; int levelValueForGame = level.Value;

			//if (levelType.Value == LevelType.Outside)
			if(level == 1 || level == 10)
			{
				if (level != BossLevel)
				{
					Random r = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed + level.Value);
					gen = NormalDungeonGenerators[r.Next(NormalDungeonGenerators.Count)];
				}
				else
				{
					Random r = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed / 7 + level.Value);
					gen = BossDungeonGenerators[r.Next(BossDungeonGenerators.Count)];
				}
			}
			else if (level == 2)
			{
				Random r = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed / 3 + level.Value);
				gen = RoomDungeonGenerators[r.Next(RoomDungeonGenerators.Count)];
			}
			else if (level == 3)
			{
				Random r = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed / 4 + level.Value);
				gen = CaveDungeonGenerators[r.Next(CaveDungeonGenerators.Count)];
			}
			else if (level == 4)
			{
				Random r = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed / 3 + level.Value);
				gen = VineDungeonGenerators[r.Next(VineDungeonGenerators.Count)];
			}
			else if (level == 5)
			{
				Random r = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed / 4 + level.Value);
				gen = MetalDungeonGenerators[r.Next(MetalDungeonGenerators.Count)];
			}
			else if (level == 6)
			{
				Random r = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed / 4 + level.Value);
				gen = SubwayDungeonGenerators[r.Next(SubwayDungeonGenerators.Count)];
			}
			else if (level == 7)
			{
				Random r = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed / 3 + level.Value);
				gen = RoomDungeonGenerators[r.Next(RoomDungeonGenerators.Count)];
			}
			else if (level == 8)
			{
				Random r = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed / 3 + level.Value);
				gen = RoomDungeonGenerators[r.Next(RoomDungeonGenerators.Count)];
			}
			else if (level == 9)
			{
				Random r = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed / 4 + level.Value);
				gen = CaveDungeonGenerators[r.Next(CaveDungeonGenerators.Count)];
			}

			if (Game1.IsMasterGame)
				{
					genSeed.Value = (int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed * level.Value * SDate.Now().DaysSinceStart;
				}

			Vector2 warpPrev = Vector2.Zero, warpNext = Vector2.Zero;
				reloadMap();

			if (Map.GetLayer("Buildings1") == null)
			{
				Map.AddLayer(new xTile.Layers.Layer("Buildings1", Map, Map.Layers[0].LayerSize, Map.Layers[0].TileSize));
			}

			try
				{
					gen.Generate(this, ref warpPrev, ref warpNext);
					warpFromPrev.Value = warpPrev;
					warpFromNext.Value = warpNext;
				}
				catch (Exception e)
				{
					Monitor.Log("Exception generating dungeon: " + e);
				}
		}

		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			base.UpdateWhenCurrentLocation(time);

	
			
		}


			public override bool performAction(string actionStr, Farmer who, xTile.Dimensions.Location tileLocation)
			{
				string[] split = actionStr.Split(' ');
				string action = split[0];
				int tx = tileLocation.X;
				int ty = tileLocation.Y;
				Layer layer = Map.GetLayer("Buildings");


			if (action == "HellWarpPrevious")
			{
				string prev = HellDungeon.BaseLocationName + (level.Value - 1);
				if (level.Value == 1)
					prev = "Custom_DungeonEntrance";

				performTouchAction("MagicWarp " + prev + " 0 1", Game1.player.Position);
			}
			else if (action == "HellWarpNext")
			{
				if (warpFromPrev == warpFromNext) // boss level
					performTouchAction("MagicWarp Custom_SouthRestStop 33 9", Game1.player.Position);
				else
				{
					string next = HellDungeon.BaseLocationName + (level.Value + 1);
					performTouchAction("MagicWarp " + next + " 0 0", Game1.player.Position);
				}
			}
			else if (action == "SilverKey")
			{

				//Motherfuckery until Spacecore saves the day

				//int silverkeyint = 99;// ExternalAPIs.JA.GetObjectId("Silver Key");
				//string silverkey = Convert.ToString(silverkeyint);
				if (Game1.player.ActiveObject != null && Utility.IsNormalObjectAtParentSheetIndex(Game1.player.ActiveObject, "ApryllForever.RiseMermaids_SilverKey"))
				{
					layer.Tiles[tx, ty] = null;
					layer.Tiles[tx, ty - 1] = null;
					layer.Tiles[tx, ty - 2] = null;
					Game1.playSound("doorCreak");
					Game1.player.removeItemFromInventory(ItemRegistry.Create("ApryllForever.RiseMermaids_SilverKey"));
					who.ActiveObject = null;
					DelayedAction.playSoundAfterDelay("treethud", 1000);
				}
				else
					Game1.addHUDMessage(new HUDMessage("You need the silver key in your hand to open this gate.", 1));
				Game1.playSound("batScreech");
			}
			else if (action == "GoldKey")
			{
				//int goldketint = 98;// ExternalAPIs.JA.GetObjectId("Gold Key");
				//string goldkey = Convert.ToString(goldketint);
				if (Game1.player.ActiveObject != null && Utility.IsNormalObjectAtParentSheetIndex(Game1.player.ActiveObject, "ApryllForever.RiseMermaids_GoldKey"))
				{
					layer.Tiles[tx, ty] = null;
					layer.Tiles[tx, ty - 1] = null;
					layer.Tiles[tx, ty - 2] = null;
					Game1.playSound("doorCreak");
					Game1.player.removeItemFromInventory(ItemRegistry.Create("ApryllForever.RiseMermaids_GoldKey"));
					who.ActiveObject = null;
					DelayedAction.playSoundAfterDelay("treethud", 1000);
				}
				else
					Game1.addHUDMessage(new HUDMessage("You need the gold key in your hand to open this gate.", 1));
				Game1.playSound("trainWhistle");
			}
			else if (action == "IridiumKey")
			{
				//int iridiumkeyint = 97;// ExternalAPIs.JA.GetObjectId("Iridium Key");
				//string iridiumkey = Convert.ToString(iridiumkeyint);
				if (Game1.player.ActiveObject != null && Utility.IsNormalObjectAtParentSheetIndex(Game1.player.ActiveObject, "ApryllForever.RiseMermaids_IridiumKey"))
				{
					layer.Tiles[tx, ty] = null;
					layer.Tiles[tx, ty - 1] = null;
					layer.Tiles[tx, ty - 2] = null;
					Game1.playSound("doorCreak");
					Game1.player.removeItemFromInventory(ItemRegistry.Create("ApryllForever.RiseMermaids_IridiumKey"));
					who.ActiveObject = null;
					DelayedAction.playSoundAfterDelay("treethud", 1000);
				}
				else
					Game1.addHUDMessage(new HUDMessage("You need the iridium key in your hand to open this gate.", 1));
				Game1.playSound("cacklingWitch");
			}
			else if (action == "DiamondKey")
			{
				//int diamondkeyint = 96;// ExternalAPIs.JA.GetObjectId("Diamond Key");
				//string diamondkey = Convert.ToString(diamondkeyint);
				if (Game1.player.ActiveObject != null && Utility.IsNormalObjectAtParentSheetIndex(Game1.player.ActiveObject, "ApryllForever.RiseMermaids_DiamondKey"))
				{
					layer.Tiles[tx, ty] = null;
					layer.Tiles[tx, ty - 1] = null;
					layer.Tiles[tx, ty - 2] = null;
					Game1.playSound("doorCreak");
					Game1.player.removeItemFromInventory(ItemRegistry.Create("ApryllForever.RiseMermaids_DiamondKey"));
					who.ActiveObject = null;
					DelayedAction.playSoundAfterDelay("treethud", 1000);
				}
				else
					Game1.addHUDMessage(new HUDMessage("You need the diamond key in your hand to open this gate.", 1));
				Game1.playSound("dogs");
			}
			else if (action == "HeartKey")
			{
				//int heartkeyint = 95;// ExternalAPIs.JA.GetObjectId("Heart Key");
				//string heartkey = Convert.ToString(heartkeyint);
				if (Game1.player.ActiveObject != null && Utility.IsNormalObjectAtParentSheetIndex(Game1.player.ActiveObject, "ApryllForever.RiseMermaids_HeartKey"))
				{
					layer.Tiles[tx, ty] = null;
					layer.Tiles[tx, ty - 1] = null;
					layer.Tiles[tx, ty - 2] = null;
					Game1.playSound("doorCreak");
					Game1.player.removeItemFromInventory(ItemRegistry.Create("ApryllForever.RiseMermaids_HeartKey"));
					who.ActiveObject = null;
					DelayedAction.playSoundAfterDelay("treethud", 1000);
				}
				else
					Game1.addHUDMessage(new HUDMessage("You need the heart key in your hand to open this gate.", 1));
				Game1.playSound("snowyStep", 1900);
				DelayedAction.playSoundAfterDelay("snowyStep", 300, (Game1.player.currentLocation), Game1.player.Position, 1400);
				DelayedAction.playSoundAfterDelay("snowyStep", 1400, Game1.player.currentLocation, Game1.player.Position, 1900);
				DelayedAction.playSoundAfterDelay("snowyStep", 1700, Game1.player.currentLocation, Game1.player.Position, 1400);
				DelayedAction.playSoundAfterDelay("snowyStep", 2800, Game1.player.currentLocation, Game1.player.Position, 1900);
				DelayedAction.playSoundAfterDelay("snowyStep", 3100, Game1.player.currentLocation, Game1.player.Position, 1400);
				DelayedAction.playSoundAfterDelay("snowyStep", 4200, Game1.player.currentLocation, Game1.player.Position, 1900);
				DelayedAction.playSoundAfterDelay("snowyStep", 4500, Game1.player.currentLocation, Game1.player.Position, 1400);
			}
			else if (action == "NoKey")
			{
				{
					layer.Tiles[tx, ty] = null;
					layer.Tiles[tx, ty - 1] = null;
					layer.Tiles[tx, ty - 2] = null;
					Game1.playSound("doorCreak");
					DelayedAction.playSoundAfterDelay("treethud", 1000);
				}
			}

			else if (action == "HugeSilverKey")
			{
				if (Game1.player.ActiveObject != null && Utility.IsNormalObjectAtParentSheetIndex(Game1.player.ActiveObject, "ApryllForever.RiseMermaids_SilverKey"))
				{
					layer.Tiles[tx + 1, ty - 2] = null;
					layer.Tiles[tx - 1, ty - 2] = null;
					layer.Tiles[tx, ty - 2] = null;

					layer.Tiles[tx, ty] = null;
					layer.Tiles[tx - 1, ty] = null;
					layer.Tiles[tx + 1, ty] = null;
					layer.Tiles[tx, ty - 1] = null;
					layer.Tiles[tx - 1, ty - 1] = null;
					layer.Tiles[tx + 1, ty - 1] = null;

					Game1.playSound("doorCreak");
					Game1.player.removeItemFromInventory(ItemRegistry.Create("ApryllForever.RiseMermaids_SilverKey"));
					who.ActiveObject = null;
					DelayedAction.playSoundAfterDelay("boulderCrack", 1000);
				}
				else
					Game1.addHUDMessage(new HUDMessage("You need the silver key in your hand to open this gate.", 1));
				Game1.playSound("batScreech");
			}
			else if (action == "HugeGoldKey")
			{
				//int goldketint = 98;// ExternalAPIs.JA.GetObjectId("Gold Key");
				//string goldkey = Convert.ToString(goldketint);
				if (Game1.player.ActiveObject != null && Utility.IsNormalObjectAtParentSheetIndex(Game1.player.ActiveObject, "ApryllForever.RiseMermaids_GoldKey"))
				{
					layer.Tiles[tx + 1, ty - 2] = null;
					layer.Tiles[tx - 1, ty - 2] = null;
					layer.Tiles[tx, ty - 2] = null;

					layer.Tiles[tx, ty] = null;
					layer.Tiles[tx - 1, ty] = null;
					layer.Tiles[tx + 1, ty] = null;
					layer.Tiles[tx, ty - 1] = null;
					layer.Tiles[tx - 1, ty - 1] = null;
					layer.Tiles[tx + 1, ty - 1] = null;
					Game1.playSound("doorCreak");
					Game1.player.removeItemFromInventory(ItemRegistry.Create("ApryllForever.RiseMermaids_GoldKey"));
					who.ActiveObject = null;
					DelayedAction.playSoundAfterDelay("boulderCrack", 1000);
				}
				else
					Game1.addHUDMessage(new HUDMessage("You need the gold key in your hand to open this gate.", 1));
				Game1.playSound("cowboy_gunload");
			}


			else if (action == "HugeDoor")
			{
				{
					layer.Tiles[tx, ty] = null;
					layer.Tiles[tx - 1, ty] = null;
					layer.Tiles[tx + 1, ty] = null;
					layer.Tiles[tx, ty - 1] = null;
					layer.Tiles[tx - 1, ty - 1] = null;
					layer.Tiles[tx + 1, ty - 1] = null;
					Game1.playSound("doorCreak");
					DelayedAction.playSoundAfterDelay("treethud", 1000);
				}
			}
			else if (action == "MeriCoke")
			{
				createQuestionDialogue(Meri, createYesNoResponses(), "MeriCola");
			}

			else if (action == "Mermaid.Enlightenment")
			{
				Game1.playSound("secret1");
				Game1.addHUDMessage(new HUDMessage("You have received Enlightenment!!!"));
				Game1.player.team.hasEnlightenment().Value = true;
			}




            return base.performAction(action, who, tileLocation);
			}


		public override bool answerDialogue(Response answer)
		{
			if (lastQuestionKey != null && afterQuestion == null)
			{
				string qa = lastQuestionKey.Split(' ')[0] + "_" + answer.responseKey;
				switch (qa)
				{
					case "MeriCola_Yes":

                        if (Game1.player.Money >= 300)
                        {
							int buttcoke = 94;// ExternalAPIs.JA.GetObjectId("MeriCola");
                            string mericola = Convert.ToString(buttcoke);
                            Game1.player.Money -= 300;
                            Game1.player.addItemByMenuIfNecessary(ItemRegistry.Create(mericola));
                        }
                        else
						{
							Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney1"));
						}


						return true;
				}
			}

			return base.answerDialogue(answer);
		}

        /*
			public override void performTouchAction(string actionStr, Vector2 tileLocation)
			{
				string[] split = actionStr.Split(' ');
				string action = split[0];
				int tx = (int)tileLocation.X;
				int ty = (int)tileLocation.Y;

			if (action == "RSSwitch")
			{
				doSwitch.Fire(new Point(tx, ty));
				Game1.playSound("shiny4");
			}

	


			base.performTouchAction(actionStr, tileLocation);
			}
		*/




        public override void performTouchAction(string actionStr, Vector2 tileLocation)
        {
            string[] split = actionStr.Split(' ');
            string action = split[0];
            int tx = (int)tileLocation.X;
            int ty = (int)tileLocation.Y;

            Vector2 mermaidHoleRight = new Vector2(tx + 8, ty);
            Vector2 mermaidHoleLeft = new Vector2(tx - 8, ty);

            if (action == "Mermaid.HoleRight")
            {


                Game1.player.setTileLocation(mermaidHoleRight);

                Game1.playSound("fallDown");
            }

            if (action == "Mermaid.HoleLeft")
            {


                Game1.player.setTileLocation(mermaidHoleLeft);

                Game1.playSound("fallDown");
            }


            base.performTouchAction(actionStr, tileLocation);
        }




        public override bool CanPlaceThisFurnitureHere(Furniture furniture)
			{
				return false;
			}		}
	}








