using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewValley.BellsAndWhistles;
using StardewValley;
using StardewModdingAPI;


namespace RestStopLocations

{ 


public class AmbientishLocationSounds 
{
	public const int sound_babblingBrook = 0;

	public const int sound_cracklingFire = 1;

	public const int sound_engine = 2;

	public const int sound_cricket = 3;

	public const int numberOfSounds = 4;

	//public const float doNotPlay = 9999999f;

	private static Dictionary<Vector2, int> sounds = new Dictionary<Vector2, int>();

	private static int updateTimer = 100;

	private static int farthestSoundDistance = 1024;

	private static float[] shortestDistanceForCue;

	private static ICue babblingBrook;

	private static ICue cracklingFire;

	private static ICue engine;

	private static ICue cricket;

	private static float volumeOverrideForLocChange;

		private static IModHelper Helper { get; set; }

		internal static void Initialize()
		{
			IModHelper helper;

          
			if (Game1.soundBank != null)
			{
				if (AmbientishLocationSounds.babblingBrook == null)
				{
					AmbientishLocationSounds.babblingBrook = Game1.soundBank.GetCue("rain");
					AmbientishLocationSounds.babblingBrook.Play();
					AmbientishLocationSounds.babblingBrook.Pause();
				}
				if (AmbientishLocationSounds.cracklingFire == null)
				{
					AmbientishLocationSounds.cracklingFire = Game1.soundBank.GetCue("doorCreak");
					AmbientishLocationSounds.cracklingFire.Play();
					AmbientishLocationSounds.cracklingFire.Pause();
				}
				if (AmbientishLocationSounds.engine == null)
				{
					AmbientishLocationSounds.engine = Game1.soundBank.GetCue("powerup"); //powerup
					AmbientishLocationSounds.engine.Play();
					AmbientishLocationSounds.engine.Pause();
				}
				if (AmbientishLocationSounds.cricket == null)
				{
					AmbientishLocationSounds.cricket = Game1.soundBank.GetCue("cavedrip");
					AmbientishLocationSounds.cricket.Play();
					AmbientishLocationSounds.cricket.Pause();
				}
			}
			AmbientishLocationSounds.shortestDistanceForCue = new float[4];


		}

	/*	public static void InitShared()
	{
		if (Game1.soundBank != null)
		{
			if (AmbientishLocationSounds.babblingBrook == null)
			{
					AmbientishLocationSounds.babblingBrook = Game1.soundBank.GetCue("rain");
					AmbientishLocationSounds.babblingBrook.Play();
					AmbientishLocationSounds.babblingBrook.Pause();
			}
			if (AmbientishLocationSounds.cracklingFire == null)
			{
					AmbientishLocationSounds.cracklingFire = Game1.soundBank.GetCue("doorCreak");
					AmbientishLocationSounds.cracklingFire.Play();
					AmbientishLocationSounds.cracklingFire.Pause();
			}
			if (AmbientishLocationSounds.engine == null)
			{
					AmbientishLocationSounds.engine = Game1.soundBank.GetCue("powerup");
					AmbientishLocationSounds.engine.Play();
					AmbientishLocationSounds.engine.Pause();
			}
			if (AmbientishLocationSounds.cricket == null)
			{
					AmbientishLocationSounds.cricket = Game1.soundBank.GetCue("cavedrip");
					AmbientishLocationSounds.cricket.Play();
					AmbientishLocationSounds.cricket.Pause();
			}
		}
			AmbientishLocationSounds.shortestDistanceForCue = new float[4];
	}   */

	public static void update(GameTime time)
	{
			
		if (AmbientishLocationSounds.sounds.Count == 0)
		{
			return;
		}
		if (AmbientishLocationSounds.volumeOverrideForLocChange < 1f)
		{
				AmbientishLocationSounds.volumeOverrideForLocChange += (float)time.ElapsedGameTime.Milliseconds * 0.0003f;
		}
			AmbientishLocationSounds.updateTimer -= time.ElapsedGameTime.Milliseconds;
		if (AmbientishLocationSounds.updateTimer > 0)
		{
			return;
		}
		for (int j = 0; j < AmbientishLocationSounds.shortestDistanceForCue.Length; j++)
		{
				AmbientishLocationSounds.shortestDistanceForCue[j] = 9999999f;
		}
		Vector2 farmerPosition = Game1.player.getStandingPosition();
		foreach (KeyValuePair<Vector2, int> pair in AmbientishLocationSounds.sounds)
		{
			float distance = Vector2.Distance(pair.Key, farmerPosition);
			if (AmbientishLocationSounds.shortestDistanceForCue[pair.Value] > distance)
			{
					AmbientishLocationSounds.shortestDistanceForCue[pair.Value] = distance;
			}
		}
		if (AmbientishLocationSounds.volumeOverrideForLocChange >= 0f)
		{
			for (int i = 0; i < AmbientishLocationSounds.shortestDistanceForCue.Length; i++)
			{
				if (AmbientishLocationSounds.shortestDistanceForCue[i] <= (float)AmbientishLocationSounds.farthestSoundDistance)
				{
					float volume = Math.Min(AmbientishLocationSounds.volumeOverrideForLocChange, Math.Min(1f, 1f - AmbientishLocationSounds.shortestDistanceForCue[i] / (float)AmbientishLocationSounds.farthestSoundDistance));
					switch (i)
					{
						case 0:
							if (AmbientishLocationSounds.babblingBrook != null)
							{
									AmbientishLocationSounds.babblingBrook.SetVariable("Volume", volume * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
									AmbientishLocationSounds.babblingBrook.Resume();
							}
							break;
						case 1:
							if (AmbientishLocationSounds.cracklingFire != null)
							{
									AmbientishLocationSounds.cracklingFire.SetVariable("Volume", volume * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
									AmbientishLocationSounds.cracklingFire.Resume();
							}
							break;
						case 2:
							if (AmbientishLocationSounds.engine != null)
							{
									AmbientishLocationSounds.engine.SetVariable("Volume", volume * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
									AmbientishLocationSounds.engine.Resume();
							}
							break;
						case 3:
							if (AmbientishLocationSounds.cricket != null)
							{
									AmbientishLocationSounds.cricket.SetVariable("Volume", volume * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
									AmbientishLocationSounds.cricket.Resume();
							}
							break;
					}
					continue;
				}
				switch (i)
				{
					case 0:
						if (AmbientishLocationSounds.babblingBrook != null)
						{
								AmbientishLocationSounds.babblingBrook.Pause();
						}
						break;
					case 1:
						if (AmbientishLocationSounds.cracklingFire != null)
						{
								AmbientishLocationSounds.cracklingFire.Pause();
						}
						break;
					case 2:
						if (AmbientishLocationSounds.engine != null)
						{
								AmbientishLocationSounds.engine.Pause();
						}
						break;
					case 3:
						if (AmbientishLocationSounds.cricket != null)
						{
								AmbientishLocationSounds.cricket.Pause();
						}
						break;
				}
			}
		}
			AmbientishLocationSounds.updateTimer = 100;
	}

	public static void changeSpecificVariable(string variableName, float value, int whichSound)
	{
		if (whichSound == 2 && AmbientishLocationSounds.engine != null)
		{
				AmbientishLocationSounds.engine.SetVariable(variableName, value);
		}
	}

	public static void addSound(Vector2 tileLocation, int whichSound)
	{
		if (!AmbientishLocationSounds.sounds.ContainsKey(tileLocation * 64f))
		{
				AmbientishLocationSounds.sounds.Add(tileLocation * 64f, whichSound);
		}
	}

	public static void removeSound(Vector2 tileLocation)
	{
		if (!AmbientishLocationSounds.sounds.ContainsKey(tileLocation * 64f))
		{
			return;
		}
		switch (AmbientishLocationSounds.sounds[tileLocation * 64f])
		{
			case 0:
				if (AmbientishLocationSounds.babblingBrook != null)
				{
						AmbientishLocationSounds.babblingBrook.Pause();
				}
				break;
			case 1:
				if (AmbientishLocationSounds.cracklingFire != null)
				{
						AmbientishLocationSounds.cracklingFire.Pause();
				}
				break;
			case 2:
				if (AmbientishLocationSounds.engine != null)
				{
						AmbientishLocationSounds.engine.Pause();
				}
				break;
			case 3:
				if (AmbientishLocationSounds.cricket != null)
				{
						AmbientishLocationSounds.cricket.Pause();
				}
				break;
		}
			AmbientishLocationSounds.sounds.Remove(tileLocation * 64f);
	}

	public static void onLocationLeave()
	{
			AmbientishLocationSounds.sounds.Clear();
			AmbientishLocationSounds.volumeOverrideForLocChange = -0.5f;
		if (AmbientishLocationSounds.babblingBrook != null)
		{
				AmbientishLocationSounds.babblingBrook.Pause();
		}
		if (AmbientishLocationSounds.cracklingFire != null)
		{
				AmbientishLocationSounds.cracklingFire.Pause();
		}
		if (AmbientishLocationSounds.engine != null)
		{
				AmbientishLocationSounds.engine.SetVariable("Frequency", 100f);
				AmbientishLocationSounds.engine.Pause();
		}
		if (AmbientishLocationSounds.cricket != null)
		{
				AmbientishLocationSounds.cricket.Pause();
		}
	}
}

}