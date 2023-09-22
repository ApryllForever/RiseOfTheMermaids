using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Monsters;
using StardewModdingAPI;
using StardewModdingAPI.Enums;
using StardewModdingAPI.Events;
using Netcode;
using StardewValley.Projectiles;



namespace RestStopLocations
{

	[XmlType("Mods_ApryllForever_RestStopLocationss_DangerShooter")]
	public class DangerShooter : Shooter
	{
		public DangerShooter()
		{
		}
		public DangerShooter(Vector2 position)
			: base( position)
		{
			Sprite = new AnimatedSprite(Mod.instance.Helper.ModContent.GetInternalAssetName("assets/Monsters/DangerShooter.png").BaseName);
			Sprite.SpriteHeight = 32;
			Sprite.SpriteWidth = 32;
			forceOneTileWide.Value = true;
			Sprite.UpdateSourceRect();

			this.Name = "DangerShooter";
			//this.reloadSprite();
			//this.Sprite.UpdateSourceRect();
			//this.color.Value = Color.White;

			parseMonsterInfo("Shadow Sniper");
			Health = MaxHealth = 1200;
			DamageToFarmer = 50;
			displayName = "Grim Adept";
			projectileSpeed = 30;
			projectileDebuff = "12";
			numberOfShotsPerFire = 3;
			aimTime = 0.1f;
			burstTime = 0.25f;
			aimEndTime = 0.2f;
			firedProjectile = 0;
			projectileRange = 33;
			desiredDistance = 10;
			fireRange = 23;


	}


	}
}