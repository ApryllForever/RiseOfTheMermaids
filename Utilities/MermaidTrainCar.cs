using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley.Objects;
using System;
using StardewValley.Network;
using StardewValley.BellsAndWhistles;
using StardewValley;
using RestStopLocations.Game.Locations.Sapphire;
using StardewModdingAPI;
using System.Xml.Serialization;

namespace RestStopLocations
{
	[XmlType("Mods_ApryllForever_RestStopLocations_MermaidTrainCar")]
	public class MermaidTrainCar : INetObject<NetFields>
	{
		//public static IModHelper Helper { get; set; }

		public const int spotsForTopFeatures = 6;

		public const double chanceForTopFeature = 0.2;

		public const int engine = 3;

		public const int passengerCar = 2;

		public const int coalCar = 1;

		public const int plainCar = 0;

		public const int coal = 0;

		public const int metal = 1;

		public const int wood = 2;

		public const int compartments = 3;

		public const int grass = 4;

		public const int hay = 5;

		public const int bricks = 6;

		public const int rocks = 7;

		public const int packages = 8;

		public const int presents = 9;

		public readonly NetInt frontDecal = new NetInt();

		public readonly NetInt carType = new NetInt();

		public readonly NetInt resourceType = new NetInt();

		public readonly NetInt loaded = new NetInt();

		public readonly NetArray<int, NetInt> topFeatures = new NetArray<int, NetInt>(6);

		public readonly NetBool alternateCar = new NetBool();

		public readonly NetColor color = new NetColor();

		Texture2D mermaidTexture;



		public NetFields NetFields
		{
			get;
		} = new NetFields("MermaidTrainCar");


		public MermaidTrainCar()
		{
			initNetFields();
		}

		public MermaidTrainCar(Random random, int carType, int frontDecal, Color color, int resourceType = 0, int loaded = 0)
			: this()
		{
			this.carType.Value = carType;
			this.frontDecal.Value = frontDecal;
			this.color.Value = color;
			this.resourceType.Value = resourceType;
			this.loaded.Value = loaded;
			if (carType != 0 && carType != 1)
			{
				this.color.Value = Color.White;
			}
			if (carType == 0 && !color.Equals(Color.DimGray))
			{
				for (int i = 0; i < topFeatures.Count; i++)
				{
					if (random.NextDouble() < 0.2)
					{
						topFeatures[i] = random.Next(2);
					}
					else
					{
						topFeatures[i] = -1;
					}
				}
			}
			if (carType == 2 && random.NextDouble() < 0.5)
			{
				alternateCar.Value = true;
			}

			Texture2D mermaidTexture = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\MermaidTexture");

			this.mermaidTexture = mermaidTexture;
		}

		private void initNetFields()
		{
			NetFields.AddField(this.frontDecal).AddField(carType).AddField(resourceType).AddField(loaded).AddField(this.topFeatures).AddField(this.alternateCar).AddField(this.color);
		}

		public void draw(SpriteBatch b, Vector2 globalPosition, float wheelRotation)
		{
			b.Draw(mermaidTexture, Game1.GlobalToLocal(Game1.viewport, globalPosition), new Rectangle(192 + (int)carType.Value * 128, 512 - (alternateCar ? 64 : 0), 128, 57), this.color.Value, 0f, Vector2.Zero, 4f, SpriteEffects.None, (globalPosition.Y + 256f) / 10000f);
			b.Draw(mermaidTexture, Game1.GlobalToLocal(Game1.viewport, globalPosition + new Vector2(0f, 228f)), new Rectangle(192 + (int)carType.Value * 128, 569, 128, 7), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (globalPosition.Y + 256f) / 10000f);
			if ((int)carType.Value == 1)
			{
				b.Draw(mermaidTexture, Game1.GlobalToLocal(Game1.viewport, globalPosition), new Rectangle(448 + (int)resourceType.Value * 128 % 256, 576 + (int)resourceType.Value / 2 * 32, 128, 32), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (globalPosition.Y + 260f) / 10000f);
				if ((int)loaded.Value > 0 && Game1.random.NextDouble() < 0.02 && globalPosition.X > 320f && globalPosition.X < (float)(Game1.currentLocation.map.DisplayWidth - 256))
				{
					loaded.Value--;
						string debrisId;
					debrisId = null;
                    switch ((int)this.resourceType)
                    {
                        case 0:
                            debrisId = "(O)382";
                            break;
                        case 1:
                            debrisId = ((this.color.R > this.color.G) ? "(O)378" : ((this.color.G > this.color.B) ? "(O)380" : ((this.color.B > this.color.R) ? "(O)384" : "(O)378")));
                            break;
                        case 7:
                            debrisId = (Game1.season.Equals("Winter") ? "(O)536" : ((Game1.stats.DaysPlayed > 120 && this.color.R > this.color.G) ? "(O)537" : "(O)535"));
                            break;
                        case 2:
                            debrisId = "(O)388";
                            break;
                        case 6:
                            debrisId = "(O)390";
                            break;
                    }
                    if (debrisId != null)
                    {
                        Game1.createObjectDebris(debrisId, (int)globalPosition.X / 64 + 2, (int)globalPosition.Y / 64, (int)(globalPosition.Y + 320f));
                    }
                    if (Game1.random.NextDouble() < 0.01)
                    {
                        Game1.createItemDebris(ItemRegistry.Create("(B)806"), new Vector2((int)globalPosition.X + 128, (int)globalPosition.Y), (int)(globalPosition.Y + 320f));
                    }
                }
			}
			if ((int)carType.Value == 0)
			{
				for (int i = 0; i < topFeatures.Count; i += 64)
				{
					if (topFeatures[i] != -1)
					{
						b.Draw(mermaidTexture, Game1.GlobalToLocal(Game1.viewport, globalPosition + new Vector2(64 + i, 20f)), new Rectangle(192, 608 + topFeatures[i] * 16, 16, 16), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (globalPosition.Y + 260f) / 10000f);
					}
				}
			}
			if ((int)frontDecal.Value != -1 && ((int)carType.Value == 0 || (int)carType.Value == 1))
			{
				//Game1.mouseCursors
				b.Draw(mermaidTexture, Game1.GlobalToLocal(Game1.viewport, globalPosition + new Vector2(192f, 92f)), new Rectangle(224 + (int)frontDecal * 32 % 224, 576 + (int)frontDecal * 32 / 224 * 32, 32, 32), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (globalPosition.Y + 260f) / 10000f);
			}
			if ((int)carType.Value == 3)
			{
				Vector2 backWheel = Game1.GlobalToLocal(Game1.viewport, globalPosition + new Vector2(72f, 208f));
				Vector2 frontWheel = Game1.GlobalToLocal(Game1.viewport, globalPosition + new Vector2(316f, 208f));
				b.Draw(mermaidTexture, backWheel, new Rectangle(192, 576, 20, 20), Color.White, wheelRotation, new Vector2(10f, 10f), 4f, SpriteEffects.None, (globalPosition.Y + 260f) / 10000f);
				b.Draw(mermaidTexture, Game1.GlobalToLocal(Game1.viewport, globalPosition + new Vector2(228f, 208f)), new Rectangle(192, 576, 20, 20), Color.White, wheelRotation, new Vector2(10f, 10f), 4f, SpriteEffects.None, (globalPosition.Y + 260f) / 10000f);
				b.Draw(mermaidTexture, frontWheel, new Rectangle(192, 576, 20, 20), Color.White, wheelRotation, new Vector2(10f, 10f), 4f, SpriteEffects.None, (globalPosition.Y + 260f) / 10000f);
				int startX = (int)((double)(backWheel.X + 4f) + 24.0 * Math.Cos(wheelRotation));
				int startY = (int)((double)(backWheel.Y + 4f) + 24.0 * Math.Sin(wheelRotation));
				int endX = (int)((double)(frontWheel.X + 4f) + 24.0 * Math.Cos(wheelRotation));
				int endY = (int)((double)(frontWheel.Y + 4f) + 24.0 * Math.Sin(wheelRotation));
				Utility.drawLineWithScreenCoordinates(startX, startY, endX, endY, b, new Color(112, 98, 92), (globalPosition.Y + 264f) / 10000f);
				Utility.drawLineWithScreenCoordinates(startX, startY + 2, endX, endY + 2, b, new Color(112, 98, 92), (globalPosition.Y + 264f) / 10000f);
				Utility.drawLineWithScreenCoordinates(startX, startY + 4, endX, endY + 4, b, new Color(53, 46, 43), (globalPosition.Y + 264f) / 10000f);
				Utility.drawLineWithScreenCoordinates(startX, startY + 6, endX, endY + 6, b, new Color(53, 46, 43), (globalPosition.Y + 264f) / 10000f);
				b.Draw(mermaidTexture, new Vector2(startX - 8, startY - 8), new Rectangle(192, 640, 24, 24), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (globalPosition.Y + 268f) / 10000f);
				b.Draw(mermaidTexture, new Vector2(endX - 8, endY - 8), new Rectangle(192, 640, 24, 24), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (globalPosition.Y + 268f) / 10000f);
			}
		}
	}
}