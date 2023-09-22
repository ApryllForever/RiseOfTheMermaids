/*

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley.Network;
using StardewValley.BellsAndWhistles;
using StardewValley;
using System.Xml.Serialization;
using StardewModdingAPI;

namespace RestStopLocations


{
	[XmlType("Mods_ApryllForever_RestStopLocations_ATrain")]
	public class ATrain :Train
	{
		static IModHelper Helper;

		

		private TemporaryAnimatedSprite whistleSteam;

		



		public ATrain(GameLocation location)
		{
			IModHelper helper = ATrain.Helper;


			initNetFields();
			Random r = new Random();
			if (r.NextDouble() < 0.1)
			{
				type.Value = 3;
			}
			else if (r.NextDouble() < 0.1)
			{
				type.Value = 1;
			}
			else if (r.NextDouble() < 0.1)
			{
				type.Value = 2;
			}
			else if (r.NextDouble() < 0.05)
			{
				type.Value = 5;
			}
			else if (Game1.currentSeason.ToLower().Equals("winter") && r.NextDouble() < 0.2)
			{
				type.Value = 6;
			}
			else
			{
				type.Value = 0;
			}
			int numCars = r.Next(8, 25);
			if (r.NextDouble() < 0.1)
			{
				numCars *= 2;
			}
			speed = 0.2f;
			smokeTimer = speed * 2000f;
			Color color = Color.White;
			double chanceForPassengerCar = 1.0;
			double chanceForCoalCar = 1.0;
			switch ((int)type.Value)
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
					speed = 0.4f;
					break;
				case 2:
					chanceForPassengerCar = 0.0;
					chanceForCoalCar = 0.7;
					break;
				case 5:
					chanceForCoalCar = 0.0;
					chanceForPassengerCar = 0.0;
					color = Color.MediumBlue;
					speed = 0.4f;
					break;
				case 6:
					chanceForPassengerCar = 0.0;
					chanceForCoalCar = 1.0;
					color = Color.Red;
					break;
			}
			cars.Add(new TrainCar(location,r, 3, -1, Color.White));
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
				if ((int)type.Value == 1)
				{
					frontDecal = 2;
				}
				else if ((int)type.Value == 5)
				{
					frontDecal = 1;
				}
				else if ((int)type.Value == 6)
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
					if ((int)type.Value == 6)
					{
						resourceType = 9;
					}
				}
				cars.Add(new TrainCar(location,r, whichCar, frontDecal, carColor, resourceType, r.Next(4, 10)));
			}
		}

		private void initNetFields()
		{
            this.NetFields.SetOwner(this).AddField(this.cars, "cars").AddField(this.type, "type")
                .AddField(this.position.NetFields, "position.NetFields");
        }

		private void Position_fieldChangeEvent(NetFloat field, float oldValue, float newValue)
		{
			Console.WriteLine("ChangeedD: " + newValue);
		}

		private void Position_fieldChangeVisibleEvent(NetFloat field, float oldValue, float newValue)
		{
			Console.WriteLine("newVal: " + newValue);
		}

		new public Rectangle getBoundingBox()
		{
			return new Rectangle(-cars.Count * 128 * 4 + (int)position.X, 2720, cars.Count * 128 * 4, 128);
		}

	}
}


*/