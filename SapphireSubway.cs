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
using Microsoft.Xna.Framework.Audio;


namespace RestStopLocations.Game.Locations.Sapphire
{
    [XmlType("Mods_ApryllForever_RestStopLocations_SapphireSubway")]
    public class SapphireSubway : SapphireLocation
    {
		static IModHelper Helper;

		public static IMonitor Monitor;

	
		

		internal static void Setup(IModHelper Helper)
		{
			SapphireSubway.Helper = Helper;
			//Helper.Events.GameLoop.DayStarted += OnDayStarted;
		}

		public SapphireSubway() { }


		public SapphireSubway(IModContentHelper content)
        : base(content, "SapphireSubway", "SapphireSubway")
        {
		}

		public const int mermaidtrainSoundDelay = 15000;

		[XmlIgnore]
		public readonly NetRef<MermaidTrain> mermaidtrain = new NetRef<MermaidTrain>();

		[XmlElement("hasMermaidTrainPassed")]
		private readonly NetBool hasMermaidTrainPassed = new NetBool(value: false);

		private int mermaidtrainTime = -1;

		[XmlIgnore]
		public readonly NetInt mermaidtrainTimer = new NetInt(0);

		public static ICue mermaidtrainLoop;

		public const int trainSoundDelay = 15000;

		[XmlIgnore]
		public readonly NetRef<Train> train = new NetRef<Train>();

		[XmlElement("hasTrainPassed")]
		private readonly NetBool hasTrainPassed = new NetBool(value: false);

		private int trainTime = -1;

		[XmlIgnore]
		public readonly NetInt trainTimer = new NetInt(0);

		public void setTrainComing(int delay)
		{
			this.trainTimer.Value = delay;
			if (Game1.IsMasterGame)
			{
				this.PlayMermaidTrainApproach();
				
			}
		}

		protected override void initNetFields()
		{
			base.initNetFields();
			//base.NetFields.AddFields(mermaidtrain, train, hasMermaidTrainPassed, hasTrainPassed, mermaidtrainTimer, trainTimer);
			base.NetFields.AddField(this.mermaidtrain, "mermaidtrain").AddField(this.train, "train").AddField(this.hasMermaidTrainPassed, "hasMermaidTrainPassed").AddField(this.hasTrainPassed, "hasTrainPassed").AddField(this.mermaidtrainTimer, "mermaidtrainTimer").AddField(this.trainTimer, "trainTimer");
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
			if (mermaidtrainLoop != null)
			{
				mermaidtrainLoop.Stop(AudioStopOptions.Immediate);
			}
			mermaidtrainLoop = null;
		}

		public override void checkForMusic(GameTime time)
		{
			if (Game1.timeOfDay < 1800 && !Game1.isRaining && !Game1.eventUp)
			{
				string currentSeason = Game1.currentSeason;
				if (currentSeason == "summer" || currentSeason == "fall" || currentSeason == "spring")
				{
					Game1.changeMusicTrack(Game1.currentSeason + "_day_ambient");
				}
			}
			else if (Game1.timeOfDay >= 2000 && !Game1.isRaining && !Game1.eventUp)
			{
				string currentSeason = Game1.currentSeason;
				if (currentSeason == "summer" || currentSeason == "fall" || currentSeason == "spring")
				{
					Game1.changeMusicTrack("spring_night_ambient");
				}
			}
		}

        public override void performTenMinuteUpdate(int timeOfDay)

        {
            base.performTenMinuteUpdate(timeOfDay);

			if (this.mermaidtrain.Value == null && Game1.currentLocation == this)
				mermaidtrain.Value = new MermaidTrain();
			if (this.train.Value == null && Game1.currentLocation == this)
				train.Value = new Train();
			//playSound("trainWhistle");

		}

        public override void DayUpdate(int dayOfMonth)
		{
			base.DayUpdate(dayOfMonth);
			hasMermaidTrainPassed.Value = false;
			mermaidtrainTime = -1;
			Random r = new Random((int)Game1.uniqueIDForThisGame / 2 + (int)Game1.stats.DaysPlayed);
			if (r.NextDouble() < 0.9999999999 )
			{
				mermaidtrainTime = (700);         //r.Next(600, 2500);
				//mermaidtrainTime -= mermaidtrainTime % 10;
			}
		}

		public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character)
		{
			if ( mermaidtrain.Value != null && mermaidtrain.Value.getBoundingBox().Intersects(position))
			{
				Game1.player.takeDamage(20, overrideParry: true, null);
				return true;
			}
			return base.isCollidingPosition(position, viewport, isFarmer, 20, glider, character);
		}

		public void setMermaidTrainComing(int delay)
		{
			mermaidtrainTimer.Value = delay;
			if (Game1.IsMasterGame)
			{
				PlayMermaidTrainApproach();
			
			}
		}

		public void PlayMermaidTrainApproach()
		{
			if ( !Game1.isFestival())
			{
				
				if (Game1.soundBank != null)
				{
					ICue cue = Game1.soundBank.GetCue("distantTrain");
					cue.SetVariable("Volume", 100f);
					//cue.Play();
				}
			}
		}

		

		public override void updateEvenIfFarmerIsntHere(GameTime time, bool skipWasUpdatedFlush = false)
		{
			base.updateEvenIfFarmerIsntHere(time, skipWasUpdatedFlush);
			{
				if (mermaidtrain.Value != null && mermaidtrain.Value.Update(time, this) && Game1.IsMasterGame)
				{
					mermaidtrain.Value = null;
				}
				if (!Game1.IsMasterGame)
				{
					return;
				}
				if ((int)mermaidtrainTimer.Value == 0 && !Game1.isFestival() && mermaidtrain.Value == null)
				{
					setMermaidTrainComing(15000);
				}
				if ((int)mermaidtrainTimer.Value > 0)
				{
					mermaidtrainTimer.Value -= time.ElapsedGameTime.Milliseconds;
					if ((int)mermaidtrainTimer.Value <= 0)
					{
						//mermaidtrain.Value = new MermaidTrain();
						playSound("trainWhistle");
					}
					if ((int)mermaidtrainTimer.Value < 3500 && Game1.currentLocation == this && Game1.soundBank != null && (mermaidtrainLoop == null || !mermaidtrainLoop.IsPlaying))
					{
						mermaidtrainLoop = Game1.soundBank.GetCue("trainLoop");
						mermaidtrainLoop.SetVariable("Volume", 0f);
						mermaidtrainLoop.Play();
					}
				}
				if (mermaidtrain.Value != null)
				{
					if (Game1.currentLocation == this && Game1.soundBank != null && (mermaidtrainLoop == null || !mermaidtrainLoop.IsPlaying))
					{
						mermaidtrainLoop = Game1.soundBank.GetCue("trainLoop");
						mermaidtrainLoop.SetVariable("Volume", 0f);
						mermaidtrainLoop.Play();
					}
					if (mermaidtrainLoop != null && mermaidtrainLoop.GetVariable("Volume") < 100f)
					{
						mermaidtrainLoop.SetVariable("Volume", mermaidtrainLoop.GetVariable("Volume") + 0.5f);
					}
				}
				else if (mermaidtrainLoop != null && (int)mermaidtrainTimer.Value <= 0)
				{
					mermaidtrainLoop.SetVariable("Volume", mermaidtrainLoop.GetVariable("Volume") - 0.15f);
					if (mermaidtrainLoop.GetVariable("Volume") <= 0f)
					{
						mermaidtrainLoop.Stop(AudioStopOptions.Immediate);
						mermaidtrainLoop = null;
					}
				}
				else if ((int)mermaidtrainTimer.Value > 0 && mermaidtrainLoop != null)
				{
					mermaidtrainLoop.SetVariable("Volume", mermaidtrainLoop.GetVariable("Volume") + 0.15f);
				}
			}

            {

				if (this.train.Value != null && this.train.Value.Update(time, this) && Game1.IsMasterGame)
				{
					this.train.Value = null;
				}
				if (!Game1.IsMasterGame)
				{
					return;
				}
				if (Game1.timeOfDay == this.trainTime - this.trainTime % 10 && (int)this.trainTimer.Value == 0 && !Game1.isFestival() && this.train.Value == null)
				{
					this.setTrainComing(15000);
				}
				if ((int)this.trainTimer.Value > 0)
				{
					this.trainTimer.Value -= time.ElapsedGameTime.Milliseconds;
					if ((int)this.trainTimer.Value <= 0)
					{
						//this.train.Value = new Train();
						base.playSound("trainWhistle");
					}
					if ((int)this.trainTimer.Value < 3500 && Game1.currentLocation == this && Game1.soundBank != null && (Railroad.trainLoop == null || !Railroad.trainLoop.IsPlaying))
					{
						Railroad.trainLoop = Game1.soundBank.GetCue("trainLoop");
						Railroad.trainLoop.SetVariable("Volume", 0f);
						Railroad.trainLoop.Play();
					}
				}
				if (this.train.Value != null)
				{
					if (Game1.currentLocation == this && Game1.soundBank != null && (Railroad.trainLoop == null || !Railroad.trainLoop.IsPlaying))
					{
						Railroad.trainLoop = Game1.soundBank.GetCue("trainLoop");
						Railroad.trainLoop.SetVariable("Volume", 0f);
						Railroad.trainLoop.Play();
					}
					if (Railroad.trainLoop != null && Railroad.trainLoop.GetVariable("Volume") < 100f)
					{
						Railroad.trainLoop.SetVariable("Volume", Railroad.trainLoop.GetVariable("Volume") + 0.5f);
					}
				}
				else if (Railroad.trainLoop != null && (int)this.trainTimer.Value <= 0)
				{
					Railroad.trainLoop.SetVariable("Volume", Railroad.trainLoop.GetVariable("Volume") - 0.15f);
					if (Railroad.trainLoop.GetVariable("Volume") <= 0f)
					{
						Railroad.trainLoop.Stop(AudioStopOptions.Immediate);
						Railroad.trainLoop = null;
					}
				}
				else if ((int)this.trainTimer.Value > 0 && Railroad.trainLoop != null)
				{
					Railroad.trainLoop.SetVariable("Volume", Railroad.trainLoop.GetVariable("Volume") + 0.15f);
				}

			}

		}

		public override void draw(SpriteBatch b)
		{
			base.draw(b);
			if (mermaidtrain.Value != null )
			{
				mermaidtrain.Value.draw(b);
			}
			if (train.Value != null)
			{
				train.Value.draw(b, this);
			}

		}

	}
}