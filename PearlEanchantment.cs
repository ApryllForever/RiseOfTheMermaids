using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using SpaceCore.Events;
using SpaceCore.Interface;
using SpaceShared;
using SpaceShared.APIs;
using StardewModdingAPI;
using StardewModdingAPI.Enums;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Tools;
using System.Text;
using System.Threading.Tasks;
using StardewValley.Network;
using StardewModdingAPI.Utilities;
using StardewValley.BellsAndWhistles;
using StardewValley.Monsters;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using System.Xml.Serialization;
using StardewValley.Enchantments;

namespace RestStopLocations
{
	[XmlType("Mods_ApryllForever_RestStopLocations_PearlEnchantment")]
	public class PearlEanchantment : BaseEnchantment
	{

		static PearlEanchantment()
		{
			GetAvailableEnchantments(); // Initialize vanilla enchantemnts
			_enchantments.Add(new PearlEanchantment());
		}

		public static bool IsMermaidWeapon(Item item)
		{
			return item.Name.Contains("Siren") || item.Name.Contains("Mermaid");
		}

		public static BaseEnchantment GetEnchantmentFromItem(Item base_item, Item item)
	{
		if (base_item == null || (base_item is MeleeWeapon && !(base_item as MeleeWeapon).isScythe()))
		{
			if (base_item != null && base_item is MeleeWeapon && (base_item as MeleeWeapon).isGalaxyWeapon() && Utility.IsNormalObjectAtParentSheetIndex(item, "(O)896"))
			{
				return new PearlEanchantment();
			}
		}
		return null;
	}
		public override bool IsSecondaryEnchantment()
		{
			return true;
		}

		public override bool IsForge()
		{
			return false;
		}

		public override int GetMaximumLevel()
		{
			return 3;
		}

		public override bool ShouldBeDisplayed()
		{
			return false;
		}



	}
}
