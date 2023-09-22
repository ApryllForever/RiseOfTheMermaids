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
    [XmlType("Mods_ApryllForever_RestStopLocations_EmeraldForestShrine")]
    public class EmeraldForestShrine : SapphireLocation
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
            EmeraldForestShrine.Helper = Helper;
			//Helper.Events.GameLoop.DayStarted += OnDayStarted;
		}

		public EmeraldForestShrine() { }

		

		public EmeraldForestShrine(IModContentHelper content)
        : base(content, "EmeraldForestShrine", "EmeraldForestShrine")
        {

		}


		private LocalizedContentManager mapLoader;
		protected override LocalizedContentManager getMapLoader()
		{
			if (mapLoader == null)
			{
				mapLoader = Game1.game1.xTileContent.CreateTemporary();
			}
			return mapLoader;
		}

		public override void performTenMinuteUpdate(int timeOfDay)
		{
			base.performTenMinuteUpdate(timeOfDay);
			
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
			SuspensionBridge bridge = new SuspensionBridge(61, 19);
			suspensionBridges.Add(bridge);
            SuspensionBridge bridge2 = new SuspensionBridge(71, 19);
            suspensionBridges.Add(bridge2);


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
		{

			base.TransferDataFromSavedLocation(l);
		}
		public override void DayUpdate(int dayOfMonth)
		{
			
			base.DayUpdate(dayOfMonth);
		}



     


        public override void updateEvenIfFarmerIsntHere(GameTime time, bool skipWasUpdatedFlush = false)
		{
			base.updateEvenIfFarmerIsntHere(time, skipWasUpdatedFlush);
			
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

				Game1.flashAlpha = 1f;
				Game1.playSound("explosion");
				Game1.player.jump();
				Game1.player.xVelocity = -16f;


				Vector2 placementTile = new Vector2(Game1.player.position.X);
				Utility.addSmokePuff(location, (placementTile * 64f + new Vector2(3f, 0f) * 64f));
				smokeTimer -= (float)time.ElapsedGameTime.TotalMilliseconds;
				if (smokeTimer <= 0f)
				{
					Utility.addSmokePuff(this, new Vector2(25.6f, 5.7f) * 64f);
					Utility.addSmokePuff(this, new Vector2(34f, 7.2f) * 64f);
					smokeTimer = 9000f;
				}


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

			
			//if (MermaidIsHere())
			{
				int frame = 0;
				if (currentMermaidAnimation != null && mermaidFrameIndex < currentMermaidAnimation.Length)
				{
					frame = currentMermaidAnimation[mermaidFrameIndex];
				}
				b.Draw(mermaidSprites, Game1.GlobalToLocal(new Vector2(95f, 111f) * 64f + new Vector2(0f, -8f) * 4f), new Microsoft.Xna.Framework.Rectangle(304 + 28 * frame, 592, 28, 36), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0.0009f);
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