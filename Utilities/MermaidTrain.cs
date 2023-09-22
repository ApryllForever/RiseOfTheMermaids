using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley.Network;
using StardewValley.BellsAndWhistles;
using StardewValley;
using RestStopLocations.Game.Locations.Sapphire;
using StardewModdingAPI;
using System.Xml.Serialization;

namespace RestStopLocations
{
	[XmlType("Mods_ApryllForever_RestStopLocations_MermaidTrain")]
	public class MermaidTrain : INetObject<NetFields>
	{
		static IModHelper Helper { get; set; }

		public const int minCars = 8;

		public const int maxCars = 24;

		public const double chanceForLongTrain = 0.1;

		public const int randomTrain = 0;

		public const int jojaTrain = 1;

		public const int coalTrain = 2;

		public const int passengerTrain = 3;

		public const int uniformColorPlainTrain = 4;

		public const int prisonTrain = 5;

		public const int christmasTrain = 6;

		public readonly NetObjectList<MermaidTrainCar> mermaidcars = new NetObjectList<MermaidTrainCar>();

		public readonly NetInt type = new NetInt();

		public readonly NetPosition position = new NetPosition();

		public float speed;

		public float wheelRotation;

		public float smokeTimer;

		private TemporaryAnimatedSprite whistleSteam;

		public NetFields NetFields { get; } = new NetFields("MermaidTrain");

		
		public MermaidTrain(/*Mod*/)
		{
			//MermaidTrain.Helper = Mod.Helper;
			this.initNetFields();
			Random r = new Random();
			if (r.NextDouble() < 0.1)
			{
				this.type.Value = 3;
			}
			else if (r.NextDouble() < 0.1)
			{
				this.type.Value = 1;
			}
			else if (r.NextDouble() < 0.1)
			{
				this.type.Value = 2;
			}
			else if (r.NextDouble() < 0.05)
			{
				this.type.Value = 5;
			}
			else if (Game1.currentSeason.ToLower().Equals("winter") && r.NextDouble() < 0.2)
			{
				this.type.Value = 6;
			}
			else
			{
				this.type.Value = 0;
			}
			int numCars = r.Next(8, 25);
			if (r.NextDouble() < 0.1)
			{
				numCars *= 2;
			}
			this.speed = 0.2f;
			this.smokeTimer = this.speed * 2000f;
			Color color = Color.White;
			double chanceForPassengerCar = 1.0;
			double chanceForCoalCar = 1.0;
			switch ((int)this.type.Value)
			{
				case 0:
					chanceForPassengerCar = 0.2;
					chanceForCoalCar = 0.2;
					break;
				case 1:
					chanceForPassengerCar = 0.0;
					chanceForCoalCar = 0.0;
					color = Color.DimGray;
					break;
				case 3:
					chanceForPassengerCar = 1.0;
					chanceForCoalCar = 0.0;
					this.speed = 0.4f;
					break;
				case 2:
					chanceForPassengerCar = 0.0;
					chanceForCoalCar = 0.7;
					break;
				case 5:
					chanceForCoalCar = 0.0;
					chanceForPassengerCar = 0.0;
					color = Color.MediumBlue;
					this.speed = 0.4f;
					break;
				case 6:
					chanceForPassengerCar = 0.0;
					chanceForCoalCar = 1.0;
					color = Color.Red;
					break;
			}
			this.mermaidcars.Add(new MermaidTrainCar(r, 3, -1, Color.White));
			for (int i = 1; i < numCars; i++)
			{
				int whichCar = 0;
				if (r.NextDouble() < chanceForPassengerCar)
				{
					whichCar = 2;
				}
				else if (r.NextDouble() < chanceForCoalCar)
				{
					whichCar = 1;
				}
				Color carColor = color;
				if (color.Equals(Color.White))
				{
					bool redTint = false;
					bool greenTint = false;
					bool blueTint = false;
					switch (r.Next(3))
					{
						case 0:
							redTint = true;
							break;
						case 1:
							greenTint = true;
							break;
						case 2:
							blueTint = true;
							break;
					}
					carColor = new Color(r.Next((!redTint) ? 100 : 0, 250), r.Next((!greenTint) ? 100 : 0, 250), r.Next((!blueTint) ? 100 : 0, 250));
				}
				int frontDecal = -1;
				if ((int)this.type.Value == 1)
				{
					frontDecal = 2;
				}
				else if ((int)this.type.Value == 5)
				{
					frontDecal = 1;
				}
				else if ((int)this.type.Value == 6)
				{
					frontDecal = -1;
				}
				else if (r.NextDouble() < 0.3)
				{
					frontDecal = r.Next(35);
				}
				int resourceType = 0;
				if (whichCar == 1)
				{
					resourceType = r.Next(9);
					if ((int)this.type.Value == 6)
					{
						resourceType = 9;
					}
				}
				this.mermaidcars.Add(new MermaidTrainCar(r, whichCar, frontDecal, carColor, resourceType, r.Next(4, 10)));
			}
		}

		private void initNetFields()
		{
			this.NetFields.AddField(this.mermaidcars).AddField(this.type).AddField(this.position.NetFields);
		}

		private void Position_fieldChangeEvent(NetFloat field, float oldValue, float newValue)
		{
			Console.WriteLine("ChangeedD: " + newValue);
		}

		private void Position_fieldChangeVisibleEvent(NetFloat field, float oldValue, float newValue)
		{
			Console.WriteLine("newVal: " + newValue);
		}

		public Rectangle getBoundingBox()
		{
			return new Rectangle(-this.mermaidcars.Count * 128 * 4 + (int)this.position.X, 1250, this.mermaidcars.Count * 128 * 4, 128);
		}

		public bool Update(GameTime time, GameLocation location)
		{


			if (Game1.IsMasterGame)
			{
				this.position.X += (float)time.ElapsedGameTime.Milliseconds * this.speed;
			}
			this.wheelRotation += (float)time.ElapsedGameTime.Milliseconds * ((float)Math.PI / 256f);
			this.wheelRotation %= (float)Math.PI * 2f;
			if (!Game1.eventUp && location.Equals(Game1.currentLocation))
			{
				Farmer player = Game1.player;
				if (player.GetBoundingBox().Intersects(this.getBoundingBox()))
				{
					player.xVelocity = 8f;
					player.yVelocity = (float)(this.getBoundingBox().Center.Y - player.GetBoundingBox().Center.Y) / 4f;
					player.takeDamage(20, overrideParry: true, null);
					if (player.UsingTool)
					{
						Game1.playSound("clank");
					}
				}
			}
			if (Game1.random.NextDouble() < 0.001 && location.Equals(Game1.currentLocation) && Game1.currentLocation is SapphireSubway)
			{
				Game1.playSound("trainWhistle");
				this.whistleSteam = new TemporaryAnimatedSprite(27, new Vector2(this.position.X - 250f, 1088f), Color.White, 8, flipped: false, 100f, 0, 64, 1f, 64);
			}
			if (this.whistleSteam != null)
			{
				this.whistleSteam.Position = new Vector2(this.position.X - 258f, 1058f);
				if (this.whistleSteam.update(time))
				{
					this.whistleSteam = null;
				}
			}
			this.smokeTimer -= time.ElapsedGameTime.Milliseconds;
			if (this.smokeTimer <= 0f && Game1.currentLocation is SapphireSubway)
			{
				Game1.player.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(25, new Vector2(this.position.X - 170f, 1020f), Color.White, 8, flipped: false, 100f, 0, 64, 1f, 128));
				this.smokeTimer = this.speed * 2000f;
			}
			if (this.position.X > (float)(this.mermaidcars.Count * 128 * 4 + 4480))
			{
				return true;
			}
			return false;
		}

		public void draw(SpriteBatch b)
		{
			for (int i = 0; i < this.mermaidcars.Count; i++)
			{
				this.
					mermaidcars[i].draw(b, new Vector2(this.position.X - (float)((i + 1) * 512), 1120f), this.wheelRotation);
			}
			if (this.whistleSteam != null)
			{
				this.whistleSteam.draw(b);
			}

		}
	}
}