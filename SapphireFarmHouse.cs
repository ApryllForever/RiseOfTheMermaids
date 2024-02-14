using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley.Objects;
using xTile.Dimensions;
using StardewValley.Locations;
using StardewValley;
using StardewModdingAPI;
using RestStopLocations.Game.Locations;


namespace RestStopLocations
{
	[XmlType("Mods_ApryllForever_RestStopLocations_SapphireFarmHouse")]
	public class SapphireFarmHouse : SapphireDecoratableLocation
	{

		[XmlElement("fridge")]
		public readonly NetRef<Chest> fridge = new NetRef<Chest>(new Chest(playerChest: true));

		public Point fridgePosition;

		public NetBool visited = new NetBool(value: false);

		public SapphireFarmHouse()
		{
		}

		//content.GetInternalAssetName("assets/maps/SapphireFarmHouse.tmx").BaseName, "Custom_SapphireFarmHouse" + mapName

		


		public SapphireFarmHouse(IModContentHelper content)
			: base(content, "SapphireFarmHouse", "SapphireFarmHouse")
		{
            this.fridge.Value.Location = this;
            base.furniture.Add(ItemRegistry.Create<Furniture>("(F)1798").SetPlacement(12, 8));
            base.furniture.Add(ItemRegistry.Create<Furniture>("(F)1614").SetPlacement(3, 1));
            base.furniture.Add(ItemRegistry.Create<Furniture>("(F)1614").SetPlacement(8, 1));
            base.furniture.Add(ItemRegistry.Create<Furniture>("(F)1614").SetPlacement(20, 1));
            base.furniture.Add(ItemRegistry.Create<Furniture>("(F)1614").SetPlacement(25, 1));
            base.furniture.Add(ItemRegistry.Create<Furniture>("(F)1294").SetPlacement(1, 4));
            base.furniture.Add(ItemRegistry.Create<Furniture>("(F)1294").SetPlacement(10, 4));
            base.furniture.Add(ItemRegistry.Create<Furniture>("(F)1294").SetPlacement(18, 4));
            base.furniture.Add(ItemRegistry.Create<Furniture>("(F)1294").SetPlacement(28, 4));
            base.furniture.Add(ItemRegistry.Create<Furniture>("(F)1742").SetPlacement(20, 4));
            Furniture f;
            f = ItemRegistry.Create<Furniture>("(F)1755").SetPlacement(14, 9);
            base.furniture.Add(f);
			this.ReadWallpaperAndFloorTileData();
			/*
			base.setWallpaper(88, 0, persist: true);
			this.setFloor(23, 0, persist: true);
			base.setWallpaper(88, 1, persist: true);
			this.setFloor(48, 1, persist: true);
			base.setWallpaper(87, 2, persist: true);
			this.setFloor(52, 2, persist: true);
			base.setWallpaper(87, 3, persist: true);
			this.setFloor(23, 3, persist: true);
			base.setWallpaper(87, 4, persist: true); */
			this.fridgePosition = default(Point);
		}

		public override void TransferDataFromSavedLocation(GameLocation l)
		{
			this.fridge.Value = (l as SapphireFarmHouse).fridge.Value;
			this.visited.Value = (l as SapphireFarmHouse).visited.Value;
			base.TransferDataFromSavedLocation(l);
		}

		public override bool CanFreePlaceFurniture()
		{
			return true;
		}

        public override void UpdateWhenCurrentLocation(GameTime time)
        {
            base.UpdateWhenCurrentLocation(time);
            this.fridge.Value.updateWhenCurrentLocation(time);
        }
        public override void DayUpdate(int dayOfMonth)
		{
			base.DayUpdate(dayOfMonth);
		}

		/*
        [Obsolete]
        public override List<Microsoft.Xna.Framework.Rectangle> getWalls()
		{
			return new List<Microsoft.Xna.Framework.Rectangle>
		{
			new Microsoft.Xna.Framework.Rectangle(1, 1, 10, 3),
			new Microsoft.Xna.Framework.Rectangle(18, 1, 11, 3),
			new Microsoft.Xna.Framework.Rectangle(12, 5, 5, 2),
			new Microsoft.Xna.Framework.Rectangle(17, 9, 2, 2),
			new Microsoft.Xna.Framework.Rectangle(21, 9, 8, 2)
		};
		}*/

		protected override void resetLocalState()
		{
			base.resetLocalState();
			if (!this.visited.Value)
			{
				this.visited.Value = true;
			}
			bool found_fridge = false;
			for (int x = 0; x < base.map.GetLayer("Buildings").LayerWidth; x++)
			{
				for (int y = 0; y < base.map.GetLayer("Buildings").LayerHeight; y++)
				{
					if (base.map.GetLayer("Buildings").Tiles[x, y] != null && base.map.GetLayer("Buildings").Tiles[x, y].TileIndex == 258)
					{
						this.fridgePosition = new Point(x, y);
						found_fridge = true;
						break;
					}
				}
				if (found_fridge)
				{
					break;
				}
			}
		}

		public override List<Microsoft.Xna.Framework.Rectangle> getFloors()
		{
			return new List<Microsoft.Xna.Framework.Rectangle>
		{
			new Microsoft.Xna.Framework.Rectangle(1, 3, 11, 12),
			new Microsoft.Xna.Framework.Rectangle(11, 7, 6, 9),
			new Microsoft.Xna.Framework.Rectangle(18, 3, 11, 6),
			new Microsoft.Xna.Framework.Rectangle(17, 11, 12, 6)
		};
		}


        public override bool CanPlaceThisFurnitureHere(Furniture furniture)
        {
            if (furniture == null)
            {
                return false;
            }

            if (furniture.furniture_type.Value == 15 && (Game1.player.currentLocation is SaltyTail))
            {
                return true;
            }

            int placementRestriction = furniture.placementRestriction;
            if ((placementRestriction == 0 || placementRestriction == 2))
            {
                return true;
            }

            if ((placementRestriction == 1 || placementRestriction == 2))
            {
                return true;
            }

            return false;
        }
        protected override void initNetFields()
		{
			base.initNetFields();
			base.NetFields.AddField(this.fridge).AddField(this.visited);
			this.visited.InterpolationEnabled = false;
			this.visited.fieldChangeVisibleEvent += delegate
			{
				this.InitializeBeds();
			};
		
		
	}


        public virtual void InitializeBeds()
        {
            if (!Game1.IsMasterGame || Game1.gameMode == 6 || !this.visited.Value)
            {
                return;
            }
            int player_count;
            player_count = 0;
            foreach (Farmer allFarmer in Game1.getAllFarmers())
            {
                _ = allFarmer;
                player_count++;
            }
            string bedId;
            bedId = "2176";
            base.furniture.Add(new BedFurniture(bedId, new Vector2(22f, 3f)));
            player_count--;
            if (player_count > 0)
            {
                base.furniture.Add(new BedFurniture(bedId, new Vector2(26f, 3f)));
                player_count--;
            }
            for (int i = 0; i < Math.Min(6, player_count); i++)
            {
                int x;
                x = 3;
                int y;
                y = 3;
                if (i % 2 == 0)
                {
                    x += 4;
                }
                y += i / 2 * 4;
                base.furniture.Add(new BedFurniture(bedId, new Vector2(x, y)));
            }
        }

        public override void drawAboveFrontLayer(SpriteBatch b)
		{
			base.drawAboveFrontLayer(b);
			if (this.fridge.Value.mutex.IsLocked())
			{
				b.Draw(Game1.mouseCursors2, Game1.GlobalToLocal(Game1.viewport, new Vector2(this.fridgePosition.X, this.fridgePosition.Y - 1) * 64f), new Microsoft.Xna.Framework.Rectangle(0, 192, 16, 32), Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, (float)((this.fridgePosition.Y + 1) * 64 + 1) / 10000f);
			}
		}

		public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
		{
			if (base.map.GetLayer("Buildings").Tiles[tileLocation] != null && base.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex == 258)
			{
				this.fridge.Value.fridge.Value = true;
				this.fridge.Value.checkForAction(who);
				return true;
			}
			return base.checkAction(tileLocation, viewport, who);
		}
	}
}