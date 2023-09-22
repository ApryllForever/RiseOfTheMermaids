/*
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using xTile.ObjectModel;
using StardewValley;

namespace RestStopLocations
{
	public class MermaidDoorDictionary : NetPointDictionary<bool, MermaidDoor>
	{
		public struct MermaidDoorCollection : IEnumerable<MermaidDoor>, IEnumerable
		{
			public struct Enumerator : IEnumerator<MermaidDoor>, IDisposable, IEnumerator
			{
				private readonly MermaidDoorDictionary _dict;

				private Dictionary<Point, MermaidDoor>.Enumerator _enumerator;

				private MermaidDoor _current;

				private bool _done;

				public MermaidDoor Current => _current;

				object IEnumerator.Current
				{
					get
					{
						if (_done)
						{
							throw new InvalidOperationException();
						}
						return _current;
					}
				}

				public Enumerator(MermaidDoorDictionary dict)
				{
					_dict = dict;
					_enumerator = _dict.FieldDict.GetEnumerator();
					_current = null;
					_done = false;
				}

				public bool MoveNext()
				{
					if (_enumerator.MoveNext())
					{
						KeyValuePair<Point, MermaidDoor> pair = _enumerator.Current;
						_current = pair.Value;
						_current.Location = _dict.location;
						_current.Position = pair.Key;
						return true;
					}
					_done = true;
					_current = null;
					return false;
				}

				public void Dispose()
				{
				}

				void IEnumerator.Reset()
				{
					_enumerator = _dict.FieldDict.GetEnumerator();
					_current = null;
					_done = false;
				}
			}

			private MermaidDoorDictionary _dict;

			public MermaidDoorCollection(MermaidDoorDictionary dict)
			{
				_dict = dict;
			}

			public Enumerator GetEnumerator()
			{
				return new Enumerator(_dict);
			}

			IEnumerator<MermaidDoor> IEnumerable<MermaidDoor>.GetEnumerator()
			{
				return new Enumerator(_dict);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return new Enumerator(_dict);
			}
		}

		private GameLocation location;

		public MermaidDoorCollection MermaidDoors => new MermaidDoorCollection(this);

		public MermaidDoorDictionary(GameLocation location)
		{
			this.location = location;
		}

		protected override void setFieldValue(MermaidDoor mermaiddoor, Point position, bool open)
		{
			mermaiddoor.Location = location;
			mermaiddoor.Position = position;
			base.setFieldValue(mermaiddoor, position, open);
		}

		public void ResetSharedState()
		{
			if (location.Map.Properties.TryGetValue("MermaidDoors", out PropertyValue mermaiddoorsValue) && mermaiddoorsValue != null)
			{
				string[] mermaiddoorsParams = mermaiddoorsValue.ToString().Split(' ');
				for (int i = 0; i < mermaiddoorsParams.Length; i += 4)
				{
					int x = Convert.ToInt32(mermaiddoorsParams[i]);
					int y = Convert.ToInt32(mermaiddoorsParams[i + 1]);
					base[new Point(x, y)] = false;
				}
			}
		}

		public void ResetLocalState()
		{
			if ( !location.Map.Properties.TryGetValue("MermaidDoors", out PropertyValue mermaiddoorsValue) || mermaiddoorsValue == null)
			{
				return;
			}
			string[] mermaiddoorsParams = mermaiddoorsValue.ToString().Split(' ');
			for (int i = 0; i < mermaiddoorsParams.Length; i += 4)
			{
				int x = Convert.ToInt32(mermaiddoorsParams[i]);
				int y = Convert.ToInt32(mermaiddoorsParams[i + 1]);
				Point mermaiddoorPoint = new Point(x, y);
				if (ContainsKey(mermaiddoorPoint))
				{
					MermaidDoor mermaidDoor = base.FieldDict[mermaiddoorPoint];
					mermaidDoor.Location = location;
					mermaidDoor.Position = mermaiddoorPoint;
					mermaidDoor.ResetLocalState();
				}
			}
		}

		public void MakeMapModifications()
		{
			foreach (MermaidDoor mermaiddoor in MermaidDoors)
			{
				mermaiddoor.ApplyMapModifications();
			}
		}

		public void CleanUpLocalState()
		{
			foreach (MermaidDoor mermaiddoor in MermaidDoors)
			{
				mermaiddoor.CleanUpLocalState();
			}
		}

		public void Update(GameTime time)
		{
			foreach (MermaidDoor mermaiddoor in MermaidDoors)
			{
				mermaiddoor.Update(time);
			}
		}

		public void Draw(SpriteBatch b)
		{
			foreach (MermaidDoor mermaiddoor in MermaidDoors)
			{
				mermaiddoor.Draw(b);
			}
		}
	}
} */