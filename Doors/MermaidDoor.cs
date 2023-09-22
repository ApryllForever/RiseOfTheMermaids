/*

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using System.IO;
using xTile;
using xTile.Dimensions;
using xTile.Layers;
using xTile.ObjectModel;
using xTile.Tiles;
using StardewValley;

namespace RestStopLocations
{
	public class MermaidDoor : NetField<bool, MermaidDoor>
	{
		public GameLocation Location;

		public Point Position;

		public TemporaryAnimatedSprite Sprite;

		public Tile Tile;

		public MermaidDoor()
		{
		}

		public MermaidDoor(GameLocation location, Point position)
			: this()
		{
			Location = location;
			Position = position;
		}

		public override void Set(bool newValue)
		{
			if (newValue != value)
			{
				cleanSet(newValue);
				MarkDirty();
			}
		}

		protected override void ReadDelta(BinaryReader reader, NetVersion version)
		{
			bool newValue = reader.ReadBoolean();
			if (version.IsPriorityOver(ChangeVersion))
			{
				setInterpolationTarget(newValue);
			}
		}

		protected override void WriteDelta(BinaryWriter writer)
		{
			writer.Write(targetValue);
		}

		public void ResetLocalState()
		{
			int x = Position.X;
			int y = Position.Y;
			Location mermaiddoorLocation = new Location(x, y);
			Layer buildingsLayer = Location.Map.GetLayer("Buildings");
			Layer backLayer = Location.Map.GetLayer("Back");
			if (Tile == null)
			{
				Tile = buildingsLayer.Tiles[mermaiddoorLocation];
			}
			if (Tile == null)
			{
				return;
			}
			if (Tile.Properties.TryGetValue("Action", out PropertyValue mermaiddoorAction) && mermaiddoorAction != null && mermaiddoorAction.ToString().Contains("MermaidDoor") && mermaiddoorAction.ToString().Split(' ').Length > 1 && backLayer.Tiles[mermaiddoorLocation] != null && !backLayer.Tiles[mermaiddoorLocation].Properties.ContainsKey("TouchAction"))
			{
				backLayer.Tiles[mermaiddoorLocation].Properties.Add("TouchAction", new PropertyValue("MermaidDoor " + mermaiddoorAction.ToString().Substring(mermaiddoorAction.ToString().IndexOf(' ') + 1)));
			}
			Microsoft.Xna.Framework.Rectangle sourceRect = default(Microsoft.Xna.Framework.Rectangle);
			bool flip = false;
			switch (Tile.TileIndex)
			{
				case 824:
					sourceRect = new Microsoft.Xna.Framework.Rectangle(640, 144, 16, 48);
					break;
				case 825:
					sourceRect = new Microsoft.Xna.Framework.Rectangle(640, 144, 16, 48);
					flip = true;
					break;
				case 838:
					sourceRect = new Microsoft.Xna.Framework.Rectangle(576, 144, 16, 48);
					if (x == 10 && y == 5)
					{
						flip = true;
					}
					break;
				case 120:
					sourceRect = new Microsoft.Xna.Framework.Rectangle(512, 144, 16, 48);
					break;
			}
			Sprite = new TemporaryAnimatedSprite("LooseSprites\\Cursors", sourceRect, 100f, 4, 1, new Vector2(x, y - 2) * 64f, flicker: false, flip, (float)((y + 1) * 64 - 12) / 10000f, 0f, Color.White, 4f, 0f, 0f, 0f)
			{
				holdLastFrame = true,
				paused = true
			};
			if (base.Value)
			{
				Sprite.paused = false;
				Sprite.resetEnd();
			}
		}

		public virtual void ApplyMapModifications()
		{
			if (base.Value)
			{
				openMermaidDoorTiles();
			}
			else
			{
				closeMermaidDoorTiles();
			}
		}

		public void CleanUpLocalState()
		{
			closeMermaidDoorTiles();
		}

		private void closeMermaidDoorSprite()
		{
			Sprite.reset();
			Sprite.paused = true;
		}

		private void openMermaidDoorSprite()
		{
			Sprite.paused = false;
		}

		private void openMermaidDoorTiles()
		{
			Location.setTileProperty(Position.X, Position.Y, "Back", "TemporaryBarrier", "T");
			Location.removeTile(Position.X, Position.Y, "Buildings");
			DelayedAction.functionAfterDelay(delegate
			{
				Location.removeTileProperty(Position.X, Position.Y, "Back", "TemporaryBarrier");
			}, 400);
			Location.removeTile(Position.X, Position.Y - 1, "Front");
			Location.removeTile(Position.X, Position.Y - 2, "Front");
		}

		private void closeMermaidDoorTiles()
		{
			Location mermaiddoorLocation = new Location(Position.X, Position.Y);
			Map map = Location.Map;
			if (map != null && Tile != null)
			{
				map.GetLayer("Buildings").Tiles[mermaiddoorLocation] = Tile;
				Location.removeTileProperty(Position.X, Position.Y, "Back", "TemporaryBarrier");
				mermaiddoorLocation.Y--;
				map.GetLayer("Front").Tiles[mermaiddoorLocation] = new StaticTile(map.GetLayer("Front"), Tile.TileSheet, BlendMode.Alpha, Tile.TileIndex - Tile.TileSheet.SheetWidth);
				mermaiddoorLocation.Y--;
				map.GetLayer("Front").Tiles[mermaiddoorLocation] = new StaticTile(map.GetLayer("Front"), Tile.TileSheet, BlendMode.Alpha, Tile.TileIndex - Tile.TileSheet.SheetWidth * 2);
			}
		}

		public void Update(GameTime time)
		{
			if (Sprite != null)
			{
				if (base.Value && Sprite.paused)
				{
					openMermaidDoorSprite();
					openMermaidDoorTiles();
				}
				else if (!base.Value && !Sprite.paused)
				{
					closeMermaidDoorSprite();
					closeMermaidDoorTiles();
				}
				Sprite.update(time);
			}
		}

		public void Draw(SpriteBatch b)
		{
			if (Sprite != null)
			{
				Sprite.draw(b);
			}
		}
	}
} */