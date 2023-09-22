using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Netcode;
using StardewValley.Locations;
using StardewValley.Objects;
using StardewValley.Monsters;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Projectiles;
using System.Xml.Serialization;

namespace RestStopLocations
{

	[XmlType("Mods_ApryllForever_RestStopLocationss_RockZombie")]
	public class RockZombie : RockGolem
	{
		

		public RockZombie()
		{
		}

		public RockZombie(Vector2 position)
			: base( position)
		{
			this.Name = "Dark Zombie";
			base.IsWalkingTowardPlayer = false;
			base.Slipperiness = 2;
			base.jitteriness.Value = 0.0;
			base.HideShadow = true;
			base.Health = base.MaxHealth = 1200;
			base.DamageToFarmer = 50;


		}


		/// <summary>
		/// constructor for wilderness golems that spawn on combat farm.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="difficultyMod">player combat level is good</param>
		public RockZombie(Vector2 position, int difficultyMod)
			: base( position)
		{
			this.Name = "Green Zombie";
			base.IsWalkingTowardPlayer = false;
			base.Slipperiness = 3;
			base.HideShadow = true;
			base.jitteriness.Value = 0.0;
			base.DamageToFarmer = 70;
			base.Health += 700; //(int)((float)(difficultyMod * difficultyMod) * 2f);
			base.ExperienceGained += difficultyMod;
			if ( Game1.random.NextDouble() < 0.05)
			{
				base.objectsToDrop.Add("749");
			}
			if ( Game1.random.NextDouble() < 0.2)
			{
				base.objectsToDrop.Add("770");
			}
			if (Game1.random.NextDouble() < 0.01)
			{
				base.objectsToDrop.Add("386");
			}
			if ( Game1.random.NextDouble() < 0.01)
			{
				base.objectsToDrop.Add("386");
			}
			if ( Game1.random.NextDouble() < 0.001)
			{
				base.objectsToDrop.Add("74");
			}
			this.Sprite.currentFrame = 16;
			this.Sprite.loop = false;
			this.Sprite.UpdateSourceRect();
			
		}

		public RockZombie(Vector2 position, bool alreadySpawned)
		: base( position)
		{
			if (alreadySpawned)
			{
				this.Name = "Dark Zombie";
				base.IsWalkingTowardPlayer = true;
				base.moveTowardPlayerThreshold.Value = 16;
				Health = MaxHealth = 1000;

			}
			else
			{
				base.IsWalkingTowardPlayer = false;
			}
			this.Sprite.loop = false;
			base.Slipperiness = 2;
		}

	

		/*
		public virtual void OnAttacked(Vector2 trajectory)
		{
			if (Game1.IsMasterGame )
			{
				
				if (trajectory.LengthSquared() == 0f)
				{
					trajectory = new Vector2(0f, -1f);
				}
				else
				{
					trajectory.Normalize();
				}
				trajectory *= 16f;
				BasicProjectile projectile = new BasicProjectile(base.DamageToFarmer / 3 * 2, 13, 3, 0, (float)Math.PI / 16f, trajectory.X, trajectory.Y, base.Position, "", "", explode: true, damagesMonsters: false, base.currentLocation, this);
				projectile.height.Value = 24f;
				
				projectile.ignoreMeleeAttacks.Value = true;
				projectile.hostTimeUntilAttackable = 0.1f;
				if (Game1.random.NextDouble() < 0.5)
				{
					projectile.debuff.Value = 12;
				}
				base.currentLocation.projectiles.Add(projectile);
			}
		}
		public override void BuffForAdditionalDifficulty(int additional_difficulty)
		{
			base.BuffForAdditionalDifficulty(additional_difficulty);
			base.resilience.Value *= 2;
			base.Speed++;
		}

		public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision, Farmer who)
		{
			int actualDamage = Math.Max(1, damage - (int)base.resilience);
			base.focusedOnFarmers = true;
			base.IsWalkingTowardPlayer = true;
			if (Game1.random.NextDouble() < (double)base.missChance - (double)base.missChance * addedPrecision)
			{
				actualDamage = -1;
			}
			else
			{
				base.Health -= actualDamage;
				base.setTrajectory(xTrajectory, yTrajectory);
				if (base.Health <= 0)
				{
					base.deathAnimation();
				}
				else
				{
					base.currentLocation.playSound("rockGolemHit");
				}
				base.currentLocation.playSound("hitEnemy");
			}
			return actualDamage;
		}*/

	}
}
