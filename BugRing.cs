/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using StardewValley.Locations;
using StardewModdingAPI.Utilities;
using SpaceCore.Events;
using StardewValley.Tools;
using xTile.Tiles;
using StardewModdingAPI.Enums;
using xTile;
using Microsoft.Xna.Framework.Input;
using Netcode;
using StardewValley.BellsAndWhistles;
using StardewValley.Buildings;
using StardewValley.Characters;
using StardewValley.Menus;
using StardewValley.Minigames;
using StardewValley.Monsters;
using StardewValley.Network;
using StardewValley.Objects;
using StardewValley.Projectiles;
using StardewValley.TerrainFeatures;
using StardewValley.Util;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using xTile.Dimensions;
using xTile.Layers;
using xTile.ObjectModel;
using Object = StardewValley.Object;


namespace RestStopLocations
{
    [XmlType("Mods_ApryllForever_RestStopLocations_BugRing")]
    internal class BugRing:Ring
    {
		[XmlIgnore]
		public override string DisplayName
		{
			get
			{
				if (displayName == null)
				{
					loadDisplayFields();
				}
				return displayName;
			}
			set
			{
				displayName = value;
			}
		}

		[XmlIgnore]
		public override int Stack
		{
			get
			{
				if (zeroStack)
				{
					return 0;
				}
				return 1;
			}
			set
			{
				if (value == 0)
				{
					zeroStack = true;
				}
				else
				{
					zeroStack = false;
				}
			}
		}

		public BugRing()
		{
			base.NetFields.AddFields(price, indexInTileSheet, uniqueID);
		}

		protected string loadDescription()
		{
			
			{
				string bugRingDescr = "Bug Slayer Ring/ 1500 / -300 / Ring / Bug Slayer Ring / Allows you to slay unkillable bugs.";
				return Game1.parseText(Game1.content.LoadString(bugRingDescr), Game1.smallFont, 320);
			}
		}

			public BugRing(int which)
			: this()
		{
			string[] data = Game1.objectInformation[which].Split('/');
			base.Category = -96;
			Name = data[0];
			price.Value = Convert.ToInt32(data[1]);
			indexInTileSheet.Value = which;
			base.ParentSheetIndex = indexInTileSheet;
			uniqueID.Value = Game1.year + Game1.dayOfMonth + Game1.timeOfDay + (int)indexInTileSheet + Game1.player.getTileX() + (int)Game1.stats.MonstersKilled + (int)Game1.stats.itemsCrafted;
			loadDisplayFields();
		}

		

		public virtual void onEquip(Farmer who, GameLocation location)
		{
			
			switch ((int)indexInTileSheet)
			{
				case 516:
					_lightSourceID = (int)uniqueID + (int)who.UniqueMultiplayerID;
					while (location.sharedLights.ContainsKey(_lightSourceID.Value))
					{
						_lightSourceID = _lightSourceID.Value + 1;
					}
					location.sharedLights[_lightSourceID.Value] = new LightSource(1, new Vector2(who.Position.X + 21f, who.Position.Y + 64f), 5f, new Color(0, 50, 170), (int)uniqueID + (int)who.UniqueMultiplayerID, LightSource.LightContext.None, who.UniqueMultiplayerID);
					break;
				case 517:
					_lightSourceID = (int)uniqueID + (int)who.UniqueMultiplayerID;
					while (location.sharedLights.ContainsKey(_lightSourceID.Value))
					{
						_lightSourceID = _lightSourceID.Value + 1;
					}
					location.sharedLights[_lightSourceID.Value] = new LightSource(1, new Vector2(who.Position.X + 21f, who.Position.Y + 64f), 10f, new Color(0, 30, 150), (int)uniqueID + (int)who.UniqueMultiplayerID, LightSource.LightContext.None, who.UniqueMultiplayerID);
					break;
				case 518:
					who.magneticRadius.Value += 64;
					break;
				case 519:
					who.magneticRadius.Value += 128;
					break;
				case 888:
					_lightSourceID = (int)uniqueID + (int)who.UniqueMultiplayerID;
					while (location.sharedLights.ContainsKey(_lightSourceID.Value))
					{
						_lightSourceID = _lightSourceID.Value + 1;
					}
					location.sharedLights[_lightSourceID.Value] = new LightSource(1, new Vector2(who.Position.X + 21f, who.Position.Y + 64f), 10f, new Color(0, 80, 0), (int)uniqueID + (int)who.UniqueMultiplayerID, LightSource.LightContext.None, who.UniqueMultiplayerID);
					who.magneticRadius.Value += 128;
					break;
				case 527:
					_lightSourceID = (int)uniqueID + (int)who.UniqueMultiplayerID;
					while (location.sharedLights.ContainsKey(_lightSourceID.Value))
					{
						_lightSourceID = _lightSourceID.Value + 1;
					}
					location.sharedLights[_lightSourceID.Value] = new LightSource(1, new Vector2(who.Position.X + 21f, who.Position.Y + 64f), 10f, new Color(0, 80, 0), (int)uniqueID + (int)who.UniqueMultiplayerID, LightSource.LightContext.None, who.UniqueMultiplayerID);
					who.magneticRadius.Value += 128;
					who.attackIncreaseModifier += 0.1f;
					break;
				case 529:
					who.knockbackModifier += 0.1f;
					break;
				case 530:
					who.weaponPrecisionModifier += 0.1f;
					break;
				case 531:
					who.critChanceModifier += 0.1f;
					break;
				case 532:
					who.critPowerModifier += 0.1f;
					break;
				case 533:
					who.weaponSpeedModifier += 0.1f;
					break;
				case 534:
					who.attackIncreaseModifier += 0.1f;
					break;
				case 810:
					who.resilience += 5;
					break;
				case 859:
					who.addedLuckLevel.Value++;
					break;
				case 887:
					who.immunity += 4;
					break;
			}
		}

		protected void _OnDealDamage(Monster monster, GameLocation location, Farmer who, ref int amount)
		{
			if (monster is Grub || monster is Fly || monster is Bug || monster is Leaper || monster is LavaCrab || monster is RockCrab)
			{
				amount = (int)((float)amount * 2f);
			}
		}

		public virtual void onUnequip(Farmer who, GameLocation location)
		{
		
		}

		public override string getCategoryName()
		{
			return Game1.content.LoadString("Strings\\StringsFromCSFiles:Ring.cs.1");
		}

		public override void onNewLocation(Farmer who, GameLocation environment)
		{
		}

		public override void onLeaveLocation(Farmer who, GameLocation environment)
		{
			
		}

		public override int salePrice()
		{
			return price;
		}


		public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
		{
			spriteBatch.Draw(Game1.objectSpriteSheet, location + new Vector2(32f, 32f) * scaleSize, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, indexInTileSheet, 16, 16), color * transparency, 0f, new Vector2(8f, 8f) * scaleSize, scaleSize * 4f, SpriteEffects.None, layerDepth);
		}



		public override int maximumStackSize()
		{
			return 1;
		}

		public override int addToStack(Item stack)
		{
			return 1;
		}


		public virtual bool GetsEffectOfRing(int ring_index)
		{
			return (int)indexInTileSheet == ring_index;
		}

		public virtual int GetEffectsOfRingMultiplier(int ring_index)
		{
			if (GetsEffectOfRing(ring_index))
			{
				return 1;
			}
			return 0;
		}



		public override string getDescription()
		{
			if (description == null)
			{
				loadDisplayFields();
			}
			return Game1.parseText(description, Game1.smallFont, getDescriptionWidth());
		}

		public override bool isPlaceable()
		{
			return false;
		}

		public override Item getOne()
		{
			Ring ring = new Ring(indexInTileSheet);
			ring._GetOneFrom(this);
			return ring;
		}

		protected virtual bool loadDisplayFields()
		{
			if (Game1.objectInformation != null && indexInTileSheet != null)
			{
				string[] data = Game1.objectInformation[indexInTileSheet].Split('/');
				displayName = data[4];
				description = data[5];
				return true;
			}
			return false;
		}

		

		




	}
}

*/