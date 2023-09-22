/*                                                  This is a weird Text Junk which Bing wrote me. Bing Sucks.
using System;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Monsters;
using StardewModdingAPI;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using StardewModdingAPI.Enums;
using StardewModdingAPI.Events;

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
using xTile;
using xTile.Dimensions;
using xTile.Layers;
using xTile.ObjectModel;
using xTile.Tiles;
using Object = StardewValley.Object;
using RestStopLocations.Game.Locations;
using RestStopLocations.Bluebella;
using System.Reflection;
using RestStopLocations.Game.Locations.Sapphire;
using StardewValley.Projectiles;

namespace RestStopLocations
{
    // A custom monster that shoots fireballs at the player
    public class FireballMonster : Monster
    {
        // The time between each fireball attack in milliseconds
        private const int FireballCooldown = 3000;

        // The speed of the fireball projectile
        private const float FireballSpeed = 8f;

        // The damage of the fireball projectile
        private const int FireballDamage = 15;

        // The radius of the fireball explosion
        private const float FireballRadius = 64f;

        // The sound of the fireball explosion
        private const string FireballSound = "explosion";

        // The texture of the fireball projectile
        private readonly Texture2D FireballTexture;

        // The timer for the fireball attack
        private int FireballTimer;

        // The constructor for the fireball monster
        public FireballMonster()
            : base("Fireball Monster", new Vector2(0, 0))
        {
            // Set the health, damage, defense and experience of the monster
            this.Health = 100;
            this.DamageToFarmer = 10;
            this.resilience.Value = 5;
            this.ExperienceGained = 50;

            // Set the movement speed and animation frames of the monster
            this.Speed = 3;
            this.Sprite.SpriteWidth = 16;
            this.Sprite.SpriteHeight = 16;
            this.Sprite.LoadTexture("Characters\\Monsters\\Fireball Monster");
            this.Sprite.currentFrame = 0;
            this.Sprite.loop = true;

            // Load the texture of the fireball projectile from the mod folder
            //this.FireballTexture = Mod.instance.Helper.ModContent.GetInternalAssetName ("assets/fireball.png");

            // Initialize the timer for the fireball attack
            this.FireballTimer = FireballCooldown;
        }

        // The update method for the fireball monster
        public override void update(GameTime time, GameLocation location)
        {
            base.update(time, location);

            // Update the timer for the fireball attack
            this.FireballTimer -= time.ElapsedGameTime.Milliseconds;

            // If the timer reaches zero, shoot a fireball at the player
            if (this.FireballTimer <= 0)
            {
                this.ShootFireball(location);
                this.FireballTimer = FireballCooldown;
            }
        }

        // The method to shoot a fireball at the player
        private void ShootFireball(GameLocation location)
        {
            // Get the position and direction of the fireball
            Vector2 position = this.getStandingPosition();
            Vector2 direction = Utility.getVelocityTowardPlayer(this.GetBoundingBox().Center, FireballSpeed, Game1.player);

            // Create a new projectile with the fireball texture, position, direction and damage
            //Projectile fireball = new Projectile(FireballTexture, position, direction.X, direction.Y, 0, FireballDamage, FireballSound, "", false, false);

            // Set the collision action of the fireball to create an explosion effect and damage nearby entities
            fireball.collisionAction = (GameLocation loc, int xTile, int yTile, Farmer who) =>
            {
                // Create an explosion effect at the collision position
                loc.temporarySprites.Add(new TemporaryAnimatedSprite(362, (float)Game1.random.Next(30, 90), 6, 1, new Vector2(xTile * 64 + 32 - FireballRadius / 2f, yTile * 64 + 32 - FireballRadius / 2f), false));

                // Get all the entities within the radius of the explosion
                Rectangle explosionArea = new Rectangle(xTile * 64 - (int)FireballRadius / 2 + 32, yTile * 64 - (int)FireballRadius / 2 + 32, (int)FireballRadius, (int)FireballRadius);
                foreach (var character in loc.getCharacters())
                {
                    if (character.GetBoundingBox().Intersects(explosionArea))
                    {
                        // If the entity is a monster, damage it with half of the fireball damage
                        if (character is Monster monster)
                        {
                            monster.takeDamage(FireballDamage / 2, (int)direction.X * -1 / Math.Max(1, Math.Abs((int)direction.X)), (int)direction.Y * -1 / Math.Max(1, Math.Abs((int)direction.Y)), false, 0.0);
                        }
                        // If the entity is the player, damage it with the full fireball damage
                        else if (character is Farmer farmer)
                        {
                            farmer.takeDamage(FireballDamage, true, null);
                        }
                    }
                }
            };

            // Add the fireball to the location
            location.projectiles.Add(fireball);
        }
    }
}
*/