/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Quests;
using StardewModdingAPI.Utilities;
using Microsoft.Xna.Framework.Graphics;
using SpaceCore.Events;
using RestStopLocations;
using UtilitiesStuff;

namespace RestStopLocations.Quests
{
	public class QuestController
	{
		static IModHelper Helper => Mod.instance.Helper;

		//static IModHelper Helper;
		static IMonitor Monitor;
		//static Lazy<Texture2D> questionMarkSprite = new Lazy<Texture2D>(() => Helper.Content.Load<Texture2D>(PathUtilities.NormalizePath("assets/questMark.png"), ContentSource.ModFolder));

		//store available quest data for each user
		internal static readonly PerScreen<QuestData> dailyQuestData = new PerScreen<QuestData>();

		private static readonly PerScreen<LocationForMarkers> CurrentLocationFormarkers = new(() => LocationForMarkers.Other);

		//is board unlocked in the current map (if there is one)
		private static readonly PerScreen<bool> SOBoardUnlocked = new();
		private static readonly PerScreen<bool> QuestBoardUnlocked = new();
		private static readonly PerScreen<bool> OrdersGenerated = new();

		internal static readonly PerScreen<HashSet<int>> FinishedQuests = new(() => new());

		private enum LocationForMarkers
		{
			SapphireSprings,
			RangerStation,
			Other
		}

		internal static void Initialize(IMod ModInstance)
		{
			//Helper = ModInstance.Helper;
			Monitor = ModInstance.Monitor;
			TileActionHandler.RegisterTileAction("MermaidBoard", OpenQuestBoard);
			TileActionHandler.RegisterTileAction("MermaidSpecialOrderBoard", OpenSOBoard);

			Helper.ConsoleCommands.Add("RS.refresh", "", (s1, s2) => {
				MermaidSpecialOrderBoard.UpdateAvailableMermaidSpecialOrders(force_refresh: true);
				Log.Info("Rise of the Mermaids Special Orders refreshed.");
			});

			Helper.ConsoleCommands.Add("RS.AddQuest", "", (s1, s2) => {
				var quest = QuestFactory.getQuestFromId(int.Parse(s2[0]));
				if (quest != null)
				{
					Game1.player.questLog.Add(quest);
					Log.Info("Quest added.");
				}
				else
				{

					Log.Info("Quest not found.");
				}
			});

			Helper.ConsoleCommands.Add("RS.QuestState", "", (s1, s2) => PrintQuestState());
			Helper.ConsoleCommands.Add("RS.CheckQuests", "", (s1, s2) => CheckQuests());
			Helper.Events.GameLoop.DayStarted += OnDayStarted;
			Helper.Events.Player.Warped += OnWarped;
			Helper.Events.Display.RenderedWorld += RenderQuestMarkersIfNeeded;
			Helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
			Helper.Events.GameLoop.DayEnding += OnDayEnding;
			SpaceEvents.OnEventFinished += OnEventFinished;

			OrdersGenerated.Value = false;
		}

		private static void CheckQuests()
		{
			var questData = Helper.GameContent.Load<Dictionary<int, string>>(PathUtilities.NormalizeAssetName("Data/Quests"));
			foreach (var key in questData.Keys)
			{
				try
				{
					Log.Debug($"Checking {key}");
					QuestFactory.getQuestFromId(key);

				}
				catch (Exception e)
				{
					Log.Error($"Failed for quest ID {key}. Stacktrace in Trace");
					Log.Trace(e.Message);
					Log.Trace(e.StackTrace);
				}
			}
		}

		static void PrintQuestState()
		{
			foreach (var data in dailyQuestData.GetActiveValues())
			{
				var questData = data.Value;
				Log.Debug($"MermaidQuest: {questData?.dailySapphireQuest?.id}");
				Log.Debug($"Mermaidquest accepted: {questData.acceptedDailySapphireQuest}");
				Log.Debug($"RangerQuest: {questData?.dailyRangerQuest?.id}");
				Log.Debug($"RangerQuest accepted: {questData.acceptedDailyRangerQuest}");
				Log.Debug($"Quests done: {string.Join(",", FinishedQuests.Value)}");

			}
		}

		//save the players dailies
		private static void OnDayEnding(object sender, DayEndingEventArgs e)
		{
			Game1.player.modData["RS.DailiesDone"] = string.Join(",", FinishedQuests.Value);
			if (OrdersGenerated.Value && Game1.dayOfMonth % 7 == 0 && Game1.player.IsMainPlayer)
			{
				OrdersGenerated.Value = false;
			}

		}

		//load the player's finished quests
		private static void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
		{
			if (Game1.player.modData.TryGetValue("RS.DailiesDone", out string dailyisDone))
			{

				Log.Trace($"dailies Done: {dailyisDone}");
				FinishedQuests.Value = new HashSet<int>();
				if (!string.IsNullOrEmpty(dailyisDone))
				{
					foreach (string id in dailyisDone.Split(","))
					{
						FinishedQuests.Value.Add(int.Parse(id));
					}
				}
			}
		}

		private static void OnWarped(object sender, WarpedEventArgs e)
		{
			if (e.Player == Game1.player)
			{
				if (e.NewLocation.Name.Equals(MermaidConstants.L_SAPPHIRESPRINGS))
				{
					CurrentLocationFormarkers.Value = LocationForMarkers.SapphireSprings;
					SOBoardUnlocked.Value = Game1.player.eventsSeen.Contains(MermaidConstants.E_SAPPHIREBOARD);

				}
				else if (e.NewLocation.Name.Equals(MermaidConstants.L_RANGERSTATION))
				{
					CurrentLocationFormarkers.Value = LocationForMarkers.RangerStation;
					SOBoardUnlocked.Value = Game1.player.eventsSeen.Contains(MermaidConstants.E_RANGERBOARD);
					QuestBoardUnlocked.Value = Game1.player.eventsSeen.Contains(MermaidConstants.E_RANGERQUESTS);
				}
				else
				{
					CurrentLocationFormarkers.Value = LocationForMarkers.Other;
					SOBoardUnlocked.Value = false;
					QuestBoardUnlocked.Value = false;
				}
			}

		}

		private static void RenderQuestMarkersIfNeeded(object sender, RenderedWorldEventArgs e)
		{
			SpriteBatch sb = e.SpriteBatch;
			switch (CurrentLocationFormarkers.Value)
			{
				case LocationForMarkers.SapphireSprings:
					float offset = 4f * (float)Math.Round(Math.Sin(Game1.currentGameTime.TotalGameTime.TotalMilliseconds / 250.0), 2);
					if (!dailyQuestData.Value.acceptedDailySapphireQuest)
					{
						sb.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(109f * 64f + 32f, 38.5f * 64f + offset)),
							new Rectangle(395, 497, 3, 8), Color.White, 0f, new Vector2(1f, 4f), 4f + Math.Max(0f, 0.25f - offset / 16f), SpriteEffects.None, 1f);

					}
					if (SOBoardUnlocked.Value && !Game1.player.team.acceptedSpecialOrderTypes.Contains("SapphireSO") && Game1.player.team.GetAvailableSpecialOrder(type: "SapphireSO") != null)
					{
						Vector2 questMarkPosition = new Vector2(119f * 64f + 27f, 39f * 64f);
						sb.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(119f * 64f + 32f, 39.5f * 64f + offset)),
							new Rectangle(395, 497, 3, 8), Color.White, 0f, new Vector2(1f, 4f), 4f + Math.Max(0f, 0.25f - offset / 16f), SpriteEffects.None, 1f);
					}

					break;
				case LocationForMarkers.RangerStation:
					offset = 4f * (float)Math.Round(Math.Sin(Game1.currentGameTime.TotalGameTime.TotalMilliseconds / 250.0), 2);
					if (!dailyQuestData.Value.acceptedDailyRangerQuest)
					{
						sb.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(6f * 64f + 32f, 5.5f * 64f + offset)),
							new Rectangle(395, 497, 3, 8), Color.White, 0f, new Vector2(1f, 4f), 4f + Math.Max(0f, 0.25f - offset / 16f), SpriteEffects.None, 1f);

					}
					if (SOBoardUnlocked.Value && !Game1.player.team.acceptedSpecialOrderTypes.Contains("RangerSO") && Game1.player.team.GetAvailableSpecialOrder(type: "RangerSO") != null)
					{
						sb.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(3f * 64f + 32f, 3f * 64f + offset)),
							new Rectangle(395, 497, 3, 8), Color.White, 0f, new Vector2(1f, 4f), 4f + Math.Max(0f, 0.25f - offset / 16f), SpriteEffects.None, 1f);
					}
					break;
			}
			return;
		}
		private static void OpenQuestBoard(string name, Vector2 position)
		{
			string type = name.Split()[^1];
			Log.Trace($"Opening MermaidBoard {type}");
			Log.Trace(dailyQuestData.ToString());
			Game1.activeClickableMenu = new MermaidBoard(dailyQuestData.Value, type);
		}


		private static void OpenSOBoard(string name, Vector2 position)
		{
			string type = name.Split()[^1];
			Log.Trace($"Opening MermaidSOBoard {type}");
			Game1.activeClickableMenu = new MermaidSpecialOrderBoard(type);
		}


		[EventPriority(EventPriority.Low - 101)]
		private static void OnDayStarted(object sender, DayStartedEventArgs e)
		{
			//if monday, update special orders
			if (Game1.dayOfMonth % 7 == 1 && Game1.player.IsMainPlayer)
			{
				MermaidSpecialOrderBoard.UpdateAvailableMermaidSpecialOrders(force_refresh: false);
			}
			try
			{
				Log.Trace($"Player has done following quests: {String.Join(",", FinishedQuests.Value)}");
				Quest townQuest = QuestFactory.GetDailyQuest();
				Quest ninjaQuest = null;
				if (Game1.player.eventsSeen.Contains(75160187))
				{
					ninjaQuest = QuestFactory.GetDailyRangerQuest();
				}
				dailyQuestData.Value = new QuestData(townQuest, ninjaQuest);
			}
			catch
			{
				dailyQuestData.Value = new QuestData(null, null);
				Log.Trace("Failed parsing new quests.");
			}
		}

		static void OnEventFinished(object sender, EventArgs e)
		{
			if (!Game1.player.IsMainPlayer)
				return;

			switch (Game1.CurrentEvent.id)
			{
				case MermaidConstants.E_MEETBELINDA:
					UtilFunctions.TryRemoveQuest(MermaidConstants.Q_PREPCOMPLETE);
					UtilFunctions.TryCompleteQuest(MermaidConstants.Q_NINJANOTE);
					UtilFunctions.TryAddQuest(MermaidConstants.Q_PREUNSEAL);
					break;

				case MermaidConstants.E_PREUNSEAL:
					UtilFunctions.TryCompleteQuest(MermaidConstants.Q_PREUNSEAL);
					// Crystal quests are then added
					break;

				case MermaidConstants.E_BLISSVISIT:
					// Comes after crystal quests are complete
					UtilFunctions.TryAddQuest(MermaidConstants.Q_RAEUNSEAL);
					break;

				case MermaidConstants.E_RAEUNSEAL:
					UtilFunctions.TryCompleteQuest(MermaidConstants.Q_RAEUNSEAL);
					UtilFunctions.TryAddQuest(MermaidConstants.Q_OPENPORTAL);
					break;

				case MermaidConstants.E_OPENPORTAL:
					UtilFunctions.TryCompleteQuest(MermaidConstants.Q_OPENPORTAL);
					if (Game1.player.IsMainPlayer)
					{
						Game1.player.team.specialOrders.Add(SpecialOrder.GetSpecialOrder(MermaidConstants.SO_CLEANSING, null));
					}
					break;

				case MermaidConstants.E_BLISSGH1:
					UtilFunctions.TryAddQuest(MermaidConstants.Q_CURSEDGH1);
					break;

				case MermaidConstants.E_SPIRITGH1:
					UtilFunctions.TryCompleteQuest(MermaidConstants.Q_CURSEDGH1);
					break;

				case MermaidConstants.E_BLISSGH2:
					UtilFunctions.TryAddQuest(MermaidConstants.Q_CURSEDGH2);
					break;

				case MermaidConstants.E_SPIRITGH2:
					UtilFunctions.TryCompleteQuest(MermaidConstants.Q_CURSEDGH2);
					// Greenhouse quest then added
					break;
			}
		}
	}


	internal class QuestData
	{
		internal Quest dailySapphireQuest;
		internal bool acceptedDailySapphireQuest;
		internal Quest dailyRangerQuest;
		internal bool acceptedDailyRangerQuest;

		internal QuestData(Quest dailySQuest, Quest dailyRSQuest)
		{
			this.dailySapphireQuest = dailySQuest;
			this.acceptedDailySapphireQuest = dailySQuest is null;
			this.dailyRangerQuest = dailyRSQuest;
			this.acceptedDailyRangerQuest = dailyRSQuest is null;
		}

		public override string ToString()
		{
			return $"Quest data: Sapphire ID {dailySapphireQuest?.id} {acceptedDailySapphireQuest}, Ranger ID {dailyRangerQuest?.id} {acceptedDailyRangerQuest}";
		}
	}
}
*/