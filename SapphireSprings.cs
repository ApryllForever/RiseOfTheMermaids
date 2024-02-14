using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using xTile.Dimensions;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley.BellsAndWhistles;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using xTile;
using StardewValley.Characters;
using StardewValley.Network;
using StardewValley.Objects;
using System.Linq;
using xTile.Tiles;
using StardewValley.Locations;
using Object = StardewValley.Object;
using StardewValley.GameData;
using StardewValley.Menus;
using UtilitiesStuff;
using RestStopLocations.Game.Locations;
using RestStopLocations.Game.Locations.DungeonLevelGenerators;




namespace RestStopLocations.Game.Locations.Sapphire
{
    [XmlType("Mods_ApryllForever_RestStopLocations_SapphireSprings")]
    public class SapphireSprings : SapphireLocation
    {
		static IModHelper Helper;

		public static IMonitor Monitor;

		[XmlIgnore]
		public List<SuspensionBridge> suspensionBridges = new List<SuspensionBridge>();

		private readonly NetEvent1Field<int, NetInt> rumbleAndFadeEvent = new NetEvent1Field<int, NetInt>();

		public readonly NetLongDictionary<FarmAnimal, NetRef<FarmAnimal>> animals = new();
		public NetLongDictionary<FarmAnimal, NetRef<FarmAnimal>> Animals => animals;

		[XmlIgnore]
		public readonly NetObjectList<FarmAnimal> Chickies = new NetObjectList<FarmAnimal>();

		private static Multiplayer multiplayer;


		//Mermaid Related Code Below
		[XmlIgnore]
		public NetEvent0 sapphiremermaidPuzzleSuccess = new NetEvent0();
		public Vector2 scale;
		[XmlIgnore]
		public ICue internalSound;
		[XmlIgnore]
		public int shakeTimer;
		[XmlIgnore]
		public int lastNoteBlockSoundTime;
		[XmlElement("preservedParentSheetIndex")]
		public readonly NetInt preservedParentSheetIndex = new NetInt();

		[XmlIgnore]
		public Texture2D mermaidSprites;
		[XmlIgnore]
		public int[] mermaidIdle = new int[1];

		[XmlIgnore]
		public int[] mermaidWave = new int[4]
		{
			1,
			1,
			2,
			2
		};

		[XmlIgnore]
		public int lastPlayedNote = -1;

		[XmlIgnore]
		public int songIndex = -1;
		public int[] mermaidChill = new int[6]
{
			0, 0, 0, 0, 0, 0
		
};
		[XmlIgnore]
		public int[] mermaidDance = new int[6]
{
			5,
			5,
			5,
			6,
			6,
			6
};
		[XmlIgnore]
		public int[] mermaidReward = new int[7]
		{
			3,
			3,
			3,
			3,
			3,
			4,
			4
		};
		[XmlIgnore]
		public int[] mermaidLongDance = new int[13]
{
			5,
			1,
			5,
			1,
			5,
			6,
			5,
			6,
			5,
			6,
			5,
			1,
			2
};

		[XmlIgnore]
		public int mermaidFrameIndex;

		[XmlIgnore]
		public int[] currentMermaidAnimation;

		[XmlIgnore]
		public float mermaidFrameTimer;

		[XmlIgnore]
		public float mermaidDanceTime;
		[XmlIgnore]
		public float mermaidRewardTime;
		[XmlIgnore]
		public float mermaidWaveTime;
		[XmlIgnore]
		public float mermaidLongDanceTime;
		[XmlIgnore]
		public float mermaidChillTime;
		// Mermaid Related Code is Above

		[XmlIgnore]
		private float smokeTimer;

		[XmlIgnore]
		public float hissTime;
		[XmlIgnore]
		public float hissTimer;

		internal static void Setup(IModHelper Helper)
		{
			SapphireSprings.Helper = Helper;
			//Helper.Events.GameLoop.DayStarted += OnDayStarted;
		}

		public SapphireSprings() { }

		

		public SapphireSprings(IModContentHelper content)
        : base(content, "SapphireSprings", "SapphireSprings")
        {

			//this.seasonOverride = "spring";

			long pain1 = 696969696969696969;
			Chickies.Add(new FarmAnimal("Void Chicken", pain1, -1L));
			Chickies[0].Position = new Vector2(80 * Game1.tileSize, 40 * Game1.tileSize);

			long pain2 = 696969696969696968;
			Chickies.Add(new FarmAnimal("Void Chicken", pain2, -1L));
			Chickies[1].Position = new Vector2(81 * Game1.tileSize, 40 * Game1.tileSize);


			long pain3 = 696969696969696967;
			Chickies.Add(new FarmAnimal("Void Chicken", pain3, -1L));
			Chickies[2].Position = new Vector2(83 * Game1.tileSize, 40 * Game1.tileSize);
			

			long pain4 = 696969696969696966;
			Chickies.Add(new FarmAnimal("Brown Chicken", pain4, -1L));
			Chickies[3].Position = new Vector2(81 * Game1.tileSize, 41 * Game1.tileSize);

			long pain5 = 696969696969696965;
			Chickies.Add(new FarmAnimal("Brown Chicken", pain5, -1L));
			Chickies[4].Position = new Vector2(83 * Game1.tileSize, 41 * Game1.tileSize);

			long pain6 = 696969696969696965;
			Chickies.Add(new FarmAnimal("White Chicken", pain6, -1L));
			Chickies[5].Position = new Vector2(83 * Game1.tileSize, 42 * Game1.tileSize);

			long pain7 = 696969696969696965;
			Chickies.Add(new FarmAnimal("White Chicken", pain7, -1L));
			Chickies[6].Position = new Vector2(83 * Game1.tileSize, 43 * Game1.tileSize);

			long pain8 = 696969696969696964;
			Chickies.Add(new FarmAnimal("Duck", pain8, -1L));
			Chickies[7].Position = new Vector2(28 * Game1.tileSize, 55 * Game1.tileSize);

			long pain9 = 696969696969696963;
			Chickies.Add(new FarmAnimal("Duck", pain9, -1L));
			Chickies[8].Position = new Vector2(29 * Game1.tileSize, 55 * Game1.tileSize);

			long pain10 = 696969696969696962;
			Chickies.Add(new FarmAnimal("Duck", pain10, -1L));
			Chickies[9].Position = new Vector2(30 * Game1.tileSize, 55 * Game1.tileSize);

			long pain11 = 696969696969696961;
			Chickies.Add(new FarmAnimal("Duck", pain11, -1L));
			Chickies[10].Position = new Vector2(31 * Game1.tileSize, 55 * Game1.tileSize);

			long pain12 = 696969696969696960;
			Chickies.Add(new FarmAnimal("Duck", pain12, -1L));
			Chickies[11].Position = new Vector2(32 * Game1.tileSize, 55 * Game1.tileSize);

			long pain13 = 696969696969696960;
			Chickies.Add(new FarmAnimal("Duck", pain13, -1L));
			Chickies[12].Position = new Vector2(32 * Game1.tileSize, 56 * Game1.tileSize);

			long pain14 = 696969696969696960;
			Chickies.Add(new FarmAnimal("Duck", pain14, -1L));
			Chickies[13].Position = new Vector2(31 * Game1.tileSize, 56 * Game1.tileSize);

			long pain15 = 666969696969696915;
			Chickies.Add(new FarmAnimal("Goat", pain15, -1L));
			Chickies[14].Position = new Vector2(54 * Game1.tileSize, 51 * Game1.tileSize);

			long pain16 = 666969696969696916;
			Chickies.Add(new FarmAnimal("Goat", pain16, -1L));
			Chickies[15].Position = new Vector2(57 * Game1.tileSize, 51 * Game1.tileSize);

			long pain17 = 666969696969696917;
			Chickies.Add(new FarmAnimal("Goat", pain17, -1L));
			Chickies[16].Position = new Vector2(57 * Game1.tileSize, 53 * Game1.tileSize);
			/*
			var mp = Mod.instance.Helper.Reflection.GetField<Multiplayer>(typeof(Game1), "multiplayer").GetValue();
			FarmAnimal whiteCow1 = new FarmAnimal("White Cow", mp.getNewID(), -1L)
			{
				Position = new Vector2(96 * Game1.tileSize, 19 * Game1.tileSize)
			};
			(Game1.getLocationFromName("SapphireSprings") as SapphireSprings).Chickies.Add(whiteCow1); */

		}

		
		/*public void Generate()
		{
			SapphireGenerator gen = null;
			generated = true;

			Random rand = new Random(location.genSeed.Value);
			genSeed.Value = (int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed;
			var cave
		
		
		
		
		= Game1.game1.xTileContent.Load<Map>(Mod.instance.Helper.ModContent.GetInternalAssetName("assets/maps/SapphireSprings.tmx").BaseName);

			int x = (location.Map.Layers[0].LayerWidth - caveMap.Layers[0].LayerWidth) / 2;
			int y = (location.Map.Layers[0].LayerHeight - caveMap.Layers[0].LayerHeight) / 2;

			location.ApplyMapOverride(caveMap, "actual_map", null, new Rectangle(x, y, caveMap.Layers[0].LayerWidth, caveMap.Layers[0].LayerHeight));

			//var mp = Mod.instance.Helper.Reflection.GetField<Multiplayer>(typeof(Game1), "multiplayer").GetValue();
			//long id = mp.getNewID();
			//HellAnimalType type = rand.Next(2) == 0 ? HellAnimalType.WhiteCow : HellAnimalType.WhiteChicken;
			//
			//
			//HellAnimalType type1 = HellAnimalType.Ostrich;
			//HellAnimalType type = Game1.random.Next(9)

			location.Animals.Add(id, new HellAnimal(type, new Vector2(x + 4, y + 15) * Game1.tileSize, id));
			location.Animals.Add(id, new HellAnimal(type1, new Vector2(x + 6, y + 16) * Game1.tileSize, id));

			Vector2 spotA = Vector2.Zero, spotB = Vector2.Zero;

			reloadMap();
			gen.Generate(this, ref spotA, ref spotB);
			
		} */


		//Shop Related Code
		public int xPositionOnScreen;
		public int yPositionOnScreen;
		public string potraitPersonDialogue;
		public NPC portraitPerson;


		private LocalizedContentManager mapLoader;
		protected override LocalizedContentManager getMapLoader()
		{
			if (mapLoader == null)
			{
				mapLoader = Game1.game1.xTileContent.CreateTemporary();
			}
			return mapLoader;
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
		public override void performTenMinuteUpdate(int timeOfDay)
		{
			base.performTenMinuteUpdate(timeOfDay);
			if (Game1.IsMasterGame)
			{
				foreach (FarmAnimal value in this.Chickies)
				{
					value.updatePerTenMinutes(Game1.timeOfDay, this);
				}
			}
		}

		protected override void initNetFields()
        {
            base.initNetFields();
            base.NetFields.AddField(rumbleAndFadeEvent).AddField(Chickies); //genSeed
		}
        public override bool IsLocationSpecificPlacementRestriction(Vector2 tileLocation)
        {
            foreach (SuspensionBridge suspensionBridge in this.suspensionBridges)
            {
                if (suspensionBridge.CheckPlacementPrevention(tileLocation))
                {
                    return true;
                }
            }
            return base.IsLocationSpecificPlacementRestriction(tileLocation);
        }
        //static string EnterDungeon = "Do you wish to enter this forboding gate?";
        protected override void resetLocalState()
        {
			//this.seasonOverride = "spring";
			base.resetLocalState();

			suspensionBridges.Clear();
			SuspensionBridge bridge = new SuspensionBridge(44, 135);
			suspensionBridges.Add(bridge);
			


			int numSeagulls = Game1.random.Next(6);
            foreach (Vector2 tile in Utility.getPositionsInClusterAroundThisTile(new Vector2(Game1.random.Next(base.map.DisplayWidth / 64), Game1.random.Next(12, base.map.DisplayHeight / 64)), numSeagulls))
            {
                if (!base.isTileOnMap(tile) || (!this.CanItemBePlacedHere(tile) && !base.isWaterTile((int)tile.X, (int)tile.Y)))
                {
                    continue;
                }
                int state;
                state = 3;
                if (base.isWaterTile((int)tile.X, (int)tile.Y) && this.doesTileHaveProperty((int)tile.X, (int)tile.Y, "Passable", "Buildings") == null)
                {
                    state = 2;
                    if (Game1.random.NextDouble() < 0.7)
                    {
                        continue;
                    }
                }
                base.critters.Add(new Seagull(tile * 64f + new Vector2(32f, 32f), state));
            }



			mermaidSprites = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\temporary_sprites_1");

			if (Game1.player.Tile.Y == 1)
			{
                if (Game1.player.Tile.X == 1)
                {
					Game1.player.Position = new Vector2(25.5f * Game1.tileSize, 19 * Game1.tileSize);
				}
			}

			{
				Point offset = new Point(0, 0); 
				Vector2 vector_offset = new Vector2(offset.X, offset.Y);
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(38f, 48f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(41f, 45f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(46f, 45f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(50f, 45f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(45f, 52f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(51f, 52f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(63f, 45f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(66f, 53f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(66f, 62f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(66f, 69f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(48f, 69f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(31f, 69f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(24f, 69f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(83f, 48f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(83f, 52f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(83f, 56f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(83f, 60f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(83f, 64f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(83f, 68f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(83f, 72f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(83f, 76f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(83f, 80f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(22f, 107f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(22f, 111f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(22f, 115f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(22f, 119f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(22f, 123f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(22f, 127f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(25f, 120f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(28f, 120f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(31f, 120f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
				Game1.currentLightSources.Add(new LightSource(4, (new Vector2(34f, 120f) + vector_offset) * 64f, 1f, LightSource.LightContext.None, 0L));
			}

		}

		public override void TransferDataFromSavedLocation(GameLocation l)
		{/*
		  * 
		  * Commented out because could not figure out how to generate animal in reset local state only once, so makes a new animal daily, crashing the game.
		  * 
			var other = l as SapphireSprings;
			Animals.MoveFrom(other.Animals);
			foreach (var animal in Animals.Values)
			{
				animal.reload(null);
			}*/
			base.TransferDataFromSavedLocation(l);
		}
		public override void DayUpdate(int dayOfMonth)
		{
			for (int i = this.animals.Count() - 1; i >= 0; i--)
			{
				var animal = this.animals.Pairs.ElementAt(i).Value;
				
					animal.dayUpdate(this);
			}
			//foreach (FarmAnimal a2 in (Game1.currentLocation as SapphireSprings).Chickies)
			//{
				//a2.age.Value = (byte)a2.ageWhenMature - 1;
				//a2.dayUpdate(Game1.currentLocation);
			//}
			base.DayUpdate(dayOfMonth);
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


        public bool isTileOpenBesidesTerrainFeatures(Vector2 tile)
        {
            foreach (KeyValuePair<long, FarmAnimal> pair in base.animals.Pairs)
            {
                if (pair.Value.Tile == tile)
                {
                    return true;
                }
            }
            base.isTilePassable(new Location((int)tile.X, (int)tile.Y), Game1.viewport);
            return true;
        }


        public override void updateEvenIfFarmerIsntHere(GameTime time, bool skipWasUpdatedFlush = false)
		{
			base.updateEvenIfFarmerIsntHere(time, skipWasUpdatedFlush);
			rumbleAndFadeEvent.Poll();
			if (!Game1.currentLocation.Equals(this))
			{
				NetDictionary<long, FarmAnimal, NetRef<FarmAnimal>, SerializableDictionary<long, FarmAnimal>, NetLongDictionary<FarmAnimal, NetRef<FarmAnimal>>>.PairsCollection pairs = this.animals.Pairs;
				for (int i = pairs.Count() - 1; i >= 0; i--)
				{
					pairs.ElementAt(i).Value.updateWhenNotCurrentLocation(null, time, this);
				}
			}
		}

		private void rumbleAndFade(int milliseconds)
		{
			rumbleAndFadeEvent.Fire(milliseconds);
		}

		private void performRumbleAndFade(int milliseconds)
		{
			if (Game1.currentLocation == this)
			{
				Rumble.rumbleAndFade(1f, milliseconds);
			}
		}

		public void cannonExplode()
		{
			/*
			int radius = 12;
			Vector2 tileLocation = new Vector2(0, 0);
			bool insideCircle = false;
			updateMap();
			Vector2 currentTile = new Vector2(Math.Min(map.Layers[0].LayerWidth - 1, Math.Max(0f, tileLocation.X - (float)radius)), Math.Min(map.Layers[0].LayerHeight - 1, Math.Max(0f, tileLocation.Y - (float)radius)));
			bool[,] circleOutline2 = Game1.getCircleOutlineGrid(radius);
			Microsoft.Xna.Framework.Rectangle areaOfEffect = new Microsoft.Xna.Framework.Rectangle((int)(tileLocation.X - (float)radius - 1f) * 64, (int)(tileLocation.Y - (float)radius - 1f) * 64, (radius * 2 + 1) * 64, (radius * 2 + 1) * 64);
			
			List<TemporaryAnimatedSprite> sprites = new List<TemporaryAnimatedSprite>();
			sprites.Add(new TemporaryAnimatedSprite(23, 9999f, 6, 1, new Vector2(currentTile.X * 64f, currentTile.Y * 64f), flicker: false, (Game1.random.NextDouble() < 0.5) ? true : false)
			{
				light = true,
				lightRadius = radius,
				lightcolor = Color.Black,
				alphaFade = 0.03f - (float)radius * 0.003f,
				Parent = this
			});
			rumbleAndFade(300 + radius * 100);
			for (int n = terrainFeatures.Count() - 1; n >= 0; n--)
			{
				KeyValuePair<Vector2, TerrainFeature> i = terrainFeatures.Pairs.ElementAt(n);
				if (i.Value.getBoundingBox(i.Key).Intersects(areaOfEffect) && i.Value.performToolAction(null, radius / 2, i.Key, this))
				{
					terrainFeatures.Remove(i.Key);
				}
			}
			for (int m = 0; m < radius * 2 + 1; m++)
			{
				for (int j = 0; j < radius * 2 + 1; j++)
				{
					if (m == 0 || j == 0 || m == radius * 2 || j == radius * 2)
					{
						insideCircle = circleOutline2[m, j];
					}
					else if (circleOutline2[m, j])
					{
						insideCircle = !insideCircle;
						if (!insideCircle)
						{
							
							if (Game1.random.NextDouble() < 0.45)
							{
								if (Game1.random.NextDouble() < 0.5)
								{
									sprites.Add(new TemporaryAnimatedSprite(362, Game1.random.Next(30, 90), 6, 1, new Vector2(currentTile.X * 64f, currentTile.Y * 64f), flicker: false, (Game1.random.NextDouble() < 0.5) ? true : false)
									{
										delayBeforeAnimationStart = Game1.random.Next(700)
									});
								}
								else
								{
									sprites.Add(new TemporaryAnimatedSprite(5, new Vector2(currentTile.X * 64f, currentTile.Y * 64f), Color.White, 8, flipped: false, 50f)
									{
										delayBeforeAnimationStart = Game1.random.Next(200),
										scale = (float)Game1.random.Next(5, 15) / 10f
									});
								}
							}
						}
					}
					if (insideCircle)
					{
						explosionAt(currentTile.X, currentTile.Y);
						
						if (Game1.random.NextDouble() < 0.45)
						{
							if (Game1.random.NextDouble() < 0.5)
							{
								sprites.Add(new TemporaryAnimatedSprite(362, Game1.random.Next(30, 90), 6, 1, new Vector2(currentTile.X * 64f, currentTile.Y * 64f), flicker: false, (Game1.random.NextDouble() < 0.5) ? true : false)
								{
									delayBeforeAnimationStart = Game1.random.Next(700)
								});
							}
							else
							{
								sprites.Add(new TemporaryAnimatedSprite(5, new Vector2(currentTile.X * 64f, currentTile.Y * 64f), Color.White, 8, flipped: false, 50f)
								{
									delayBeforeAnimationStart = Game1.random.Next(200),
									scale = (float)Game1.random.Next(5, 15) / 10f
								});
							}
						}
						sprites.Add(new TemporaryAnimatedSprite(6, new Vector2(currentTile.X * 64f, currentTile.Y * 64f), Color.White, 8, Game1.random.NextDouble() < 0.5, Vector2.Distance(currentTile, tileLocation) * 20f));
					}
					currentTile.Y += 1f;
					currentTile.Y = Math.Min(map.Layers[0].LayerHeight - 1, Math.Max(0f, currentTile.Y));
				}
				currentTile.X += 1f;
				currentTile.Y = Math.Min(map.Layers[0].LayerWidth - 1, Math.Max(0f, currentTile.X));
				currentTile.Y = tileLocation.Y - (float)radius;
				currentTile.Y = Math.Min(map.Layers[0].LayerHeight - 1, Math.Max(0f, currentTile.Y));
			}
			//var multiplayer = Helper.Reflection.GetField<Multiplayer>(typeof(Game1), "multiplayer").GetValue();
			//multiplayer.broadcastSprites(this, sprites);
	
			radius /= 2;
			circleOutline2 = Game1.getCircleOutlineGrid(radius);
			currentTile = new Vector2((int)(tileLocation.X - (float)radius), (int)(tileLocation.Y - (float)radius));
			for (int l = 0; l < radius * 2 + 1; l++)
			{
				for (int k = 0; k < radius * 2 + 1; k++)
				{
					if (l == 0 || k == 0 || l == radius * 2 || k == radius * 2)
					{
						insideCircle = circleOutline2[l, k];
					}
					else if (circleOutline2[l, k])
					{
						makeHoeDirt(currentTile);
					}
					if (insideCircle && !objects.ContainsKey(currentTile) && Game1.random.NextDouble() < 0.9 && doesTileHaveProperty((int)currentTile.X, (int)currentTile.Y, "Diggable", "Back") != null && !isTileHoeDirt(currentTile))
					{
					
						makeHoeDirt(currentTile);
					}
					currentTile.Y += 1f;
					currentTile.Y = Math.Min(map.Layers[0].LayerHeight - 1, Math.Max(0f, currentTile.Y));
				}
				currentTile.X += 1f;
				currentTile.Y = Math.Min(map.Layers[0].LayerWidth - 1, Math.Max(0f, currentTile.X));
				currentTile.Y = tileLocation.Y - (float)radius;
				currentTile.Y = Math.Min(map.Layers[0].LayerHeight - 1, Math.Max(0f, currentTile.Y));
			} */
		}

		public void mermaidDanceShow()
        {
			string mermaidDanceSpeech = "Fiona: Hey Lovelies! Thanks for coming to my performance!!!";
			Game1.drawObjectDialogue(mermaidDanceSpeech);
			mermaidChillTime = 5f;
			mermaidDanceTime = 9f;
			mermaidLongDanceTime = 13f;
			mermaidDanceTime = 9f;
			mermaidWaveTime = 3f;
			mermaidRewardTime = 4f;
			currentMermaidAnimation = mermaidIdle;
		}

		static string EnterDungeon = "Extreme Danger lurks beyond this gate!!! Proceed?";

		public void cannonFire()
        {

			GameLocation location = new GameLocation();
			location.netAudio.StopPlaying("fuse");

		}

		public override bool performAction(string action, Farmer who, Location tileLocation)
		{
			TemporaryAnimatedSprite sprite = new TemporaryAnimatedSprite();
			GameLocation location = new GameLocation();
			GameTime time = new GameTime();
			if (action == "RS.VolcanoEntrance")
			{
				createQuestionDialogue(EnterDungeon, createYesNoResponses(), "SapphireVolcanoEntrance");
			}

			if (action == "RS.RCannon")
			{
				//int idNum = Game1.random.Next();
				//GameLocation location = Game1.currentLocation;
				//List < TemporaryAnimatedSprite > cannonSprites = new List<TemporaryAnimatedSprite>();
				//location.explode = 9f;


				//if (hissTime == 0f)
				//{
					//location.netAudio.StartPlaying("fuse");
					//Game1.pauseThenDoFunction(2000, cannonFire);
				Game1.flashAlpha = 1f;
				Game1.playSound("explosion");
				Game1.player.jump();
				Game1.player.xVelocity = -16f;

				//}
				//Game1.pauseThenDoFunction(100, cannonExplode);
				//int radius = 3;
				//RestStopLocations.Mod mod = new RestStopLocations.Mod();
				//var Game1_multiplayer = mod.Helper.Reflection.GetField<Multiplayer>(typeof(Game1), "multiplayer").GetValue();
				//multiplayer = Game1_multiplayer;
				//List<TemporaryAnimatedSprite> sprites = new List<TemporaryAnimatedSprite>();
				//var fuckMe = Game1.player.position.X;
				//Vector2 currentTile = new Vector2(Math.Min(map.Layers[0].LayerWidth - 1, Math.Max(0f, tileLocation.X - (float)radius)), Math.Min(map.Layers[0].LayerHeight - 1, Math.Max(0f, tileLocation.Y - (float)radius)));
				//sprites.Add(new TemporaryAnimatedSprite(23, 9999f, 6, 1, new Vector2(currentTile.X * 64f, currentTile.Y * 64f), flicker: false, (Game1.random.NextDouble() < 0.5) ? true : false)
				//{
				//light = true,
				//lightRadius = radius,
				//lightcolor = Color.Black,
				//alphaFade = 0.03f - (float)radius * 0.003f,
				//Parent = this
				//});
				//Game1_multiplayer.broadcastSprites(location, sprites);


				Vector2 placementTile = new Vector2(Game1.player.position.X);
				Utility.addSmokePuff(location, (placementTile * 64f + new Vector2(3f, 0f) * 64f));
				smokeTimer -= (float)time.ElapsedGameTime.TotalMilliseconds;
				if (smokeTimer <= 0f)
				{
					Utility.addSmokePuff(this, new Vector2(25.6f, 5.7f) * 64f);
					Utility.addSmokePuff(this, new Vector2(34f, 7.2f) * 64f);
					smokeTimer = 9000f;
				}

				//{
					//location.temporarySprites.Add(new TemporaryAnimatedSprite(25, new Vector2(fuckMe + 3f, 0f *64F), Color.White, 8, flipped: false, 100f, 0, 64, 1f, 128));
				//}


					//new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Microsoft.Xna.Framework.Rectangle(598, 1279, 3, 4), 53f, 5, 9, (placementTile * 64f + new Vector2(3f, 0f) * 64f), flicker: true, flipped: false, 5f, 0f, Color.Yellow, 4f, 0f, 0f, 0f, local: true);

			};
			if (action == "RS.LCannon")
			{
				Game1.flashAlpha = 1f;
				Game1.playSound("explosion");
				Game1.player.jump();
				Game1.player.xVelocity = 16f;
			}
				if (action == "RS.MermaidKiss")
			{
				Game1.currentSong = null;
				string mermaidDanceSpeech2 = "Fiona: Have a seat, Lovelies, the show is about to begin!";
				Game1.playSound("dwop");
				mermaidWaveTime = 3f;
				mermaidChillTime = 6f;
				Game1.player.jitterStrength = 8f;
				
				Game1.drawObjectDialogue(mermaidDanceSpeech2);
				Game1.screenOverlayTempSprites.AddRange(Utility.sparkleWithinArea(new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), 500, Color.White, 10, 2000));
				DelayedAction.playSoundAfterDelay("gusviolin", 2500);
				Game1.player.freezePause = 500;
				Game1.pauseTime = 60f;

				Game1.pauseThenDoFunction(6000, mermaidDanceShow);
				currentMermaidAnimation = mermaidIdle;
				//Game1.createItemDebris(new Object(372, 1), new Vector2(32f, 33f) * 64f, 0, this, 0); //Clam
				
			}
			if (action == "RS.MermaidReward")
			{

				Game1.playSound("seagulls");
				mermaidRewardTime = 8f;
				Game1.freezeControls = true;
				DelayedAction.playSoundAfterDelay("dwop", 300);
				DelayedAction.playSoundAfterDelay("dwop", 1000);
				Game1.screenOverlayTempSprites.AddRange(Utility.sparkleWithinArea(new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), 500, Color.White, 10, 2000));
				currentMermaidAnimation = mermaidIdle;
				Game1.createItemDebris (ItemRegistry.Create("(O)638"), new Vector2(32f, 33f) * 64f, 0, this, 0);
				Game1.freezeControls = false;

               


            }
			if (action == "RS.MermaidLoot")
			{
				Game1.playSound("bubbles");
				switch (Game1.random.Next(13))
				{
					case 0:
						Game1.createItemDebris(ItemRegistry.Create("(O)169"), new Vector2(91f, 115f) * 64f, 0, this, 0); //Driftwood
						break;
					case 1:
						Game1.createItemDebris(ItemRegistry.Create("(O)390"), new Vector2(93f, 115f) * 64f, 0, this, 0); //Stone
						break;
					case 2:
						Game1.createItemDebris(ItemRegistry.Create("(O)110"), new Vector2(98f, 115f) * 64f, 0, this, 0); //Spoon
						break;
					case 3:
						Game1.createItemDebris(ItemRegistry.Create("(O)105"), new Vector2(97f, 114f) * 64f, 0, this, 0); //Chewing Stick
						break;
					case 4:
						Game1.createItemDebris(ItemRegistry.Create("(O)372"), new Vector2(98f, 114f) * 64f, 0, this, 0); //Clam
						break;
					case 5:
						Game1.createItemDebris(ItemRegistry.Create("(O)392"), new Vector2(97f, 113f) * 64f, 0, this, 0); //Nautilus
						break;
					case 6:
						Game1.createItemDebris(ItemRegistry.Create("(O)372"), new Vector2(98f, 116f) * 64f, 0, this, 0); //Clam
						break;
					case 7:
						Game1.createItemDebris(ItemRegistry.Create("(O)372"), new Vector2(94f, 118f) * 64f, 0, this, 0); //Clam
						break;
					case 8:
						Game1.createItemDebris(ItemRegistry.Create("(O)723"), new Vector2(95f, 117f) * 64f, 0, this, 0); //Oyster
						break;
					case 9:
						Game1.createItemDebris(ItemRegistry.Create("(O)718"), new Vector2(96f, 115f) * 64f, 0, this, 0); //Cockle
						break;
					case 10:
						Game1.createItemDebris(ItemRegistry.Create("(O)719"), new Vector2(98f, 115f) * 64f, 0, this, 0); //Mussel
						break;
					case 11:
						Game1.createItemDebris(ItemRegistry.Create("(O)80"), new Vector2(95f, 113f) * 64f, 0, this, 0); //Quartz
						break;
					case 12:
						Game1.createItemDebris(ItemRegistry.Create("(O)100"), new Vector2(94f, 113f) * 64f, 0, this, 0); //Amphora
						break;
				}
			}

			else if (action == "RS.MermaidStore")
			{
				/*

				string text = "Hey Lovely! Care to peruse my fine wares?";
				portraitPerson = Game1.getCharacterFromName("Caitlynn");
				Random r = new Random((int)(Game1.stats.DaysPlayed + 898 + Game1.uniqueIDForThisGame));
				Dictionary<ISalable, int[]> stock = new Dictionary<ISalable, int[]>();
				stock.Add(new Boots("854"), new int[4]
				{
						0,
						2147483647,
						337,
						70
				});
				stock.Add(new MeleeWeapon("14"), new int[4]
				{
						0,
						2147483647,
						336,
						71
				});
				stock.Add(new Hat("85"), new int[4]
				{
						0,
						2147483647,
						335,
						20
				});
				stock.Add(new Hat("62"), new int[4]
				{
						0,
						2147483647,
						335,
						20
				});
				stock.Add(new Hat("69"), new int[4]
				{
						0,
						2147483647,
						335,
						20
				});
				stock.Add(new Object(797, 1), new int[4]
				{
						0,
						2147483647,
						336,
						10
				});
				stock.Add(new Furniture(1299, Vector2.Zero), new int[4]
				{
						0,
						2147483647,
						334,
						10
				});
				stock.Add(new Furniture(1367, Vector2.Zero), new int[4]
				{
						0,
						2147483647,
						334,
						5
				});
				//Utility.AddStock(stock, new Object(Vector2.Zero, 286, int.MaxValue), 150);
				//Utility.AddStock(stock, new Object(Vector2.Zero, 287, int.MaxValue), 300);
				//Utility.AddStock(stock, new Object(Vector2.Zero, 288, int.MaxValue), 500);
				Utility.AddStock(stock, new Object(Vector2.Zero, 745, int.MaxValue), 40);
				Utility.AddStock(stock, new Object(Vector2.Zero, 628, int.MaxValue), 1200);

				/*                HOW TO MAKE THINGS BE RANDOM

				if (r.NextDouble() < 0.5)
				{
					Utility.AddStock(stock, new Object(Vector2.Zero, 244, int.MaxValue), 600);
				}
				else
				{
					Utility.AddStock(stock, new Object(Vector2.Zero, 237, int.MaxValue), 600);
				}  


				var shop = new ShopMenu(stock, 0, "Caitlynn", null, null, "Caitlynn")
				{

					
				};

				shop.portraitPerson = Game1.getCharacterFromName("Caitlynn");

                //Game1.objectDialoguePortraitPerson = 
                //if (portraitPerson != null)
                //{ }
                shop.potraitPersonDialogue = Game1.parseText(text, Game1.dialogueFont, 304);
                //portraitPerson = Game1.objectDialoguePortraitPerson;
                Game1.activeClickableMenu = shop;
				*/

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
					case "SapphireVolcanoEntrance_Yes":
						performTouchAction("MagicWarp " + SapphireVolcano.BaseLocationName + "1 0 0", Game1.player.Position);
						return true;
				}
			}

            return base.answerDialogue(answer);
        }
        
        public override bool SeedsIgnoreSeasonsHere()
        {
            return true;
        }

        public override bool CanPlantSeedsHere(string itemId, int tileX, int tileY, bool isGardenPot, out string deniedMessage)
        {
			deniedMessage = string.Empty;
            return true;
        }

        public override bool CanPlantTreesHere(string itemId, int tileX, int tileY, out string deniedMessage)
        {
			deniedMessage = string.Empty;
            return true;
        }

        public override void tryToAddCritters(bool onlyIfOnScreen = false)
        {
            if (Game1.random.NextDouble() < 0.3)
            {
                Vector2 origin2 = Vector2.Zero;
                origin2 = ((Game1.random.NextDouble() < 0.75) ? new Vector2((float)Game1.viewport.X + Utility.RandomFloat(0f, Game1.viewport.Width), Game1.viewport.Y - 64) : new Vector2(Game1.viewport.X + Game1.viewport.Width + 64, Utility.RandomFloat(0f, Game1.viewport.Height)));
                int parrots_to_spawn = 1;
                if (Game1.random.NextDouble() < 0.5)
                {
                    parrots_to_spawn++;
                }
                if (Game1.random.NextDouble() < 0.5)
                {
                    parrots_to_spawn++;
                }
                for (int i = 0; i < parrots_to_spawn; i++)
                {
                    addCritter(new OverheadParrot(origin2 + new Vector2(i * 64, -i * 64)));
                }
            }
            if (!Game1.IsRainingHere(this))
            {
                double mapArea = map.Layers[0].LayerWidth * map.Layers[0].LayerHeight;
                double butterflyChance = Math.Max(0.4, Math.Min(0.25, mapArea / 15000.0));
                addButterflies(butterflyChance, onlyIfOnScreen);
            }
            if (Game1.IsRainingHere(this))
            {
                double mapArea = map.Layers[0].LayerWidth * map.Layers[0].LayerHeight;
                double butterflyChance = Math.Max(0.5, Math.Min(0.25, mapArea / 1500.0));
                addButterflies(butterflyChance, onlyIfOnScreen);
            }
			if (Game1.IsRainingHere(this))
			{
				double mapArea = map.Layers[0].LayerWidth * map.Layers[0].LayerHeight;
				//double frogChance = Math.Max(0.5, Math.Min(0.25, mapArea / 1500.0));
				addFrog();
			}
			if (!Game1.IsRainingHere(this))
			{
				double mapArea = map.Layers[0].LayerWidth * map.Layers[0].LayerHeight;
				double butterflyChance = Math.Max(0.7, Math.Min(0.25, mapArea / 15000.0));
				addBirdies(butterflyChance, onlyIfOnScreen);
			}
			if (Game1.currentSeason == "winter" && Game1.isDarkOut())
			{
				addMoonlightJellies(50, new Random((int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame - 24917), new Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0));
			}
		}


       

		public override void UpdateWhenCurrentLocation(GameTime time)
		{
			GameLocation location = new GameLocation();
			base.UpdateWhenCurrentLocation(time);
			foreach (SuspensionBridge suspensionBridge in suspensionBridges)
			{
				suspensionBridge.Update(time);
			}
			foreach (FarmAnimal item in Chickies)
			{
				item.updateWhenCurrentLocation(time, this);
			}
			foreach (KeyValuePair<long, FarmAnimal> kvp in this.Animals.Pairs)
			{
				kvp.Value.updateWhenCurrentLocation(time, this);
			}
			bool should_wave = false; 
			
				foreach (Farmer farmer in farmers)
				{
					Point point = farmer.TilePoint;
					if (point.X > 96 && point.Y > 114 || point.X < 90 && point.Y < 115)
					{
						should_wave = true;
						break;
					}
				}

			hissTime += (float)time.ElapsedGameTime.TotalSeconds;
			if (hissTime > 0f)
				{
				location.netAudio.StartPlaying("fuse");
				}
			if (hissTime < 0f)
				{
				location.netAudio.StopPlaying("fuse");
				}


			if (should_wave && (currentMermaidAnimation == null || currentMermaidAnimation == mermaidIdle))
			{
				currentMermaidAnimation = mermaidWave;
				mermaidFrameIndex = 0;
				mermaidFrameTimer = 0f;
			}
			if (mermaidDanceTime > 0f)
			{
				if (currentMermaidAnimation == null || currentMermaidAnimation == mermaidIdle)
				{
					currentMermaidAnimation = mermaidDance;
					mermaidFrameTimer = 0f;
				}
				mermaidDanceTime -= (float)time.ElapsedGameTime.TotalSeconds;
				if (mermaidDanceTime < 0f && currentMermaidAnimation == mermaidDance)
				{
					currentMermaidAnimation = mermaidIdle;
					mermaidFrameTimer = 0f;
				}
			}
			if (mermaidRewardTime > 0f)
			{
				if (currentMermaidAnimation == null || currentMermaidAnimation == mermaidIdle)
				{
					currentMermaidAnimation = mermaidReward;
					mermaidFrameTimer = 0f;
				}
				mermaidRewardTime -= (float)time.ElapsedGameTime.TotalSeconds;
				if (mermaidRewardTime < 0f && currentMermaidAnimation == mermaidReward)
				{
					currentMermaidAnimation = mermaidIdle;
					mermaidFrameTimer = 0f;
				}
			}
			if (mermaidWaveTime > 0f)
			{
				if (currentMermaidAnimation == null || currentMermaidAnimation == mermaidIdle)
				{
					currentMermaidAnimation = mermaidWave;
					mermaidFrameTimer = 0f;
				}
				mermaidWaveTime -= (float)time.ElapsedGameTime.TotalSeconds;
				if (mermaidWaveTime < 0f && currentMermaidAnimation == mermaidWave)
				{
					currentMermaidAnimation = mermaidIdle;
					mermaidFrameTimer = 0f;
				}
			}
			if (mermaidLongDanceTime > 0f)
			{
				if (currentMermaidAnimation == null || currentMermaidAnimation == mermaidIdle)
				{
					currentMermaidAnimation = mermaidLongDance;
					mermaidFrameTimer = 0f;
				}
				mermaidLongDanceTime -= (float)time.ElapsedGameTime.TotalSeconds;
				if (mermaidLongDanceTime < 0f && currentMermaidAnimation == mermaidLongDance)
				{
					currentMermaidAnimation = mermaidIdle;
					mermaidFrameTimer = 0f;
				}
			}
			if (mermaidChillTime > 0f)
			{
				if (currentMermaidAnimation == null || currentMermaidAnimation == mermaidIdle)
				{
					currentMermaidAnimation = mermaidLongDance;
					mermaidFrameTimer = 0f;
				}
				mermaidChillTime -= (float)time.ElapsedGameTime.TotalSeconds;
				if (mermaidChillTime < 0f && currentMermaidAnimation == mermaidChill)
				{
					currentMermaidAnimation = mermaidIdle;
					mermaidFrameTimer = 0f;
				}
			}

			mermaidFrameTimer += (float)time.ElapsedGameTime.TotalSeconds;
			if (!(mermaidFrameTimer > 0.25f))
			{
				return;
			}
			mermaidFrameTimer = 0f;
			mermaidFrameIndex++;
			if (currentMermaidAnimation == null)
			{
				mermaidFrameIndex = 0;
			}

			else
			{
				if (mermaidFrameIndex < currentMermaidAnimation.Length)
				{
					return;
				}
				mermaidFrameIndex = 0;
				if (currentMermaidAnimation == mermaidReward)
				{
					if (should_wave)
					{
						currentMermaidAnimation = mermaidWave;
					}
					else
					{
						currentMermaidAnimation = mermaidIdle;
					}
				}
				else if (!should_wave && currentMermaidAnimation == mermaidWave)
				{
					currentMermaidAnimation = mermaidIdle;
				}

			}

			}

		public bool MermaidIsHere()
		{
			return Game1.IsRainingHere(this);
		}

		public override void draw(SpriteBatch b)
		{

;
			base.draw(b);

			foreach (KeyValuePair<long, FarmAnimal> pair in this.animals.Pairs)
			{
				pair.Value.draw(b);
			}

			foreach (SuspensionBridge suspensionBridge in suspensionBridges)
			{
				suspensionBridge.Draw(b);
			}

			foreach (FarmAnimal item in Chickies)
			{
				item.draw(b);
			}
			//if (MermaidIsHere())
			{
				int frame = 0;
				if (currentMermaidAnimation != null && mermaidFrameIndex < currentMermaidAnimation.Length)
				{
					frame = currentMermaidAnimation[mermaidFrameIndex];
				}
				b.Draw(mermaidSprites, Game1.GlobalToLocal(new Vector2(95f, 111f) * 64f + new Vector2(0f, -8f) * 4f), new Microsoft.Xna.Framework.Rectangle(304 + 28 * frame, 592, 28, 36), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.0009f);
			}
			int num4 = xPositionOnScreen - 320;
			if (num4 > 0 && Game1.options.showMerchantPortraits)
			{
				Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2(num4, yPositionOnScreen), new Microsoft.Xna.Framework.Rectangle(603, 414, 74, 74), Color.White, 0f, Vector2.Zero, 4f, flipped: false, 0.91f);
				if (portraitPerson.Portrait != null)
				{
					b.Draw(portraitPerson.Portrait, new Vector2(num4 + 20,  yPositionOnScreen + 20), new Microsoft.Xna.Framework.Rectangle(0, 0, 64, 64), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.92f);
				}
			}
		}

		

		public virtual void farmerAdjacentAction(GameLocation location)
		{
			if (name.Equals("Flute Block") && (internalSound == null || ((int)Game1.currentGameTime.TotalGameTime.TotalMilliseconds - lastNoteBlockSoundTime >= 1000 && !internalSound.IsPlaying)) && !Game1.dialogueUp)
			{
				if (Game1.soundBank != null)
				{
					internalSound = Game1.soundBank.GetCue("flute");
					internalSound.SetVariable("Pitch", preservedParentSheetIndex.Value);
					internalSound.Play();
				}
				scale.Y = 1.3f;
				shakeTimer = 200;
				lastNoteBlockSoundTime = (int)Game1.currentGameTime.TotalGameTime.TotalMilliseconds;
				if (location is SapphireSprings)
				{
					(location as SapphireSprings).OnFlutePlayed(preservedParentSheetIndex.Value);
				}
			}
		
		}


		public virtual void OnFlutePlayed(int pitch)
		{
			
			if (songIndex == -1)
			{
				lastPlayedNote = pitch;
				songIndex = 0;
			}
			int relative_pitch = pitch - lastPlayedNote;
			if (relative_pitch == 900)
			{
				songIndex = 1;
				mermaidDanceTime = 5f;
			}
			else if (songIndex == 1)
			{
				if (relative_pitch == -200)
				{
					songIndex++;
					mermaidDanceTime = 5f;
				}
				else
				{
					songIndex = -1;
					mermaidDanceTime = 0f;
					currentMermaidAnimation = mermaidIdle;
				}
			}
			else if (songIndex == 2)
			{
				if (relative_pitch == -400)
				{
					songIndex++;
					mermaidDanceTime = 5f;
				}
				else
				{
					songIndex = -1;
					mermaidDanceTime = 0f;
					currentMermaidAnimation = mermaidIdle;
				}
			}
			else if (songIndex == 3)
			{
				if (relative_pitch == 200)
				{
					songIndex = 0;
					sapphiremermaidPuzzleSuccess.Fire();
					mermaidDanceTime = 0f;
				}
				else
				{
					songIndex = -1;
					mermaidDanceTime = 0f;

					currentMermaidAnimation = mermaidIdle;
				}
			}
			lastPlayedNote = pitch;
		}
		public override void checkForMusic(GameTime time)
		{
			if (base.IsOutdoors && Game1.isMusicContextActiveButNotPlaying() && !Game1.IsRainingHere(this) && !Game1.eventUp)
			{
				if (Game1.random.NextDouble() < 0.003 && Game1.timeOfDay < 2100)
				{
					localSound("seagulls");
				}
				else if (Game1.isDarkOut() && Game1.timeOfDay < 2500)
				{
					Game1.changeMusicTrack("spring_night_ambient", track_interruptable: true);
				}
			}

			base.checkForMusic(time);
		}
		public override void cleanupBeforePlayerExit()
		{
			base.cleanupBeforePlayerExit();
			
			if (mapLoader != null)
			{
				mapLoader.Dispose();
				mapLoader = null;
			}
		}

	}
}