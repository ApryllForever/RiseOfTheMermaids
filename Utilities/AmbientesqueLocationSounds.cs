using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley.BellsAndWhistles;
using StardewModdingAPI;
using StardewValley;

namespace RestStopLocations.Utilities
{

//[InstanceStatics]
public static class AmbientesqueLocationSounds
    {
    public const int sound_babblingBrook = 0;

    public const int sound_cracklingFire = 1;

    public const int sound_engine = 2;

    public const int sound_cricket = 3;

    public const int numberOfSounds = 4;

    public const float doNotPlay = 9999999f;

    private static Dictionary<Vector2, int> sounds = new Dictionary<Vector2, int>();

    private static int updateTimer = 100;

    private static int farthestSoundDistance = 1024;

    private static float[] shortestDistanceForCue;

    private static ICue babblingBrook;

    private static ICue cracklingFire;

    private static ICue engine;

    private static ICue cricket;

    private static float volumeOverrideForLocChange;

    public static void InitShared()
    {
        if (Game1.soundBank != null)
        {
            if (AmbientesqueLocationSounds.babblingBrook == null)
            {
                    AmbientesqueLocationSounds.babblingBrook = Game1.soundBank.GetCue("babblingBrook");
                    AmbientesqueLocationSounds.babblingBrook.Play();
                AmbientesqueLocationSounds.babblingBrook.Pause();
            }
            if (AmbientesqueLocationSounds.cracklingFire == null)
            {
                AmbientesqueLocationSounds.cracklingFire = Game1.soundBank.GetCue("cracklingFire");
                AmbientesqueLocationSounds.cracklingFire.Play();
                AmbientesqueLocationSounds.cracklingFire.Pause();
            }
            if (AmbientesqueLocationSounds.engine == null)
            {
                AmbientesqueLocationSounds.engine = Game1.soundBank.GetCue("rainsound");
                AmbientesqueLocationSounds.engine.Play();
                AmbientesqueLocationSounds.engine.Pause();
            }
            if (AmbientesqueLocationSounds.cricket == null)
            {
                AmbientesqueLocationSounds.cricket = Game1.soundBank.GetCue("cricketsAmbient");
                AmbientesqueLocationSounds.cricket.Play();
                AmbientesqueLocationSounds.cricket.Pause();
            }
        }
        AmbientesqueLocationSounds.shortestDistanceForCue = new float[4];
    }

    public static void update(GameTime time)
    {
        if (AmbientesqueLocationSounds.sounds.Count == 0)
        {
            return;
        }
        if (AmbientesqueLocationSounds.volumeOverrideForLocChange < 1f)
        {
            AmbientesqueLocationSounds.volumeOverrideForLocChange += (float)time.ElapsedGameTime.Milliseconds * 0.0003f;
        }
        AmbientesqueLocationSounds.updateTimer -= time.ElapsedGameTime.Milliseconds;
        if (AmbientesqueLocationSounds.updateTimer > 0)
        {
            return;
        }
        for (int j = 0; j < AmbientesqueLocationSounds.shortestDistanceForCue.Length; j++)
        {
            AmbientesqueLocationSounds.shortestDistanceForCue[j] = 9999999f;
        }
        Vector2 farmerPosition = Game1.player.getStandingPosition();
        foreach (KeyValuePair<Vector2, int> pair in AmbientesqueLocationSounds.sounds)
        {
            float distance = Vector2.Distance(pair.Key, farmerPosition);
            if (AmbientesqueLocationSounds.shortestDistanceForCue[pair.Value] > distance)
            {
                AmbientesqueLocationSounds.shortestDistanceForCue[pair.Value] = distance;
            }
        }
        if (AmbientesqueLocationSounds.volumeOverrideForLocChange >= 0f)
        {
            for (int i = 0; i < AmbientesqueLocationSounds.shortestDistanceForCue.Length; i++)
            {
                if (AmbientesqueLocationSounds.shortestDistanceForCue[i] <= (float)AmbientesqueLocationSounds.farthestSoundDistance)
                {
                    float volume = Math.Min(AmbientesqueLocationSounds.volumeOverrideForLocChange, Math.Min(1f, 1f - AmbientesqueLocationSounds.shortestDistanceForCue[i] / (float)AmbientesqueLocationSounds.farthestSoundDistance));
                    switch (i)
                    {
                        case 0:
                            if (AmbientesqueLocationSounds.babblingBrook != null)
                            {
                                AmbientesqueLocationSounds.babblingBrook.SetVariable("Volume", volume * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
                                AmbientesqueLocationSounds.babblingBrook.Resume();
                            }
                            break;
                        case 1:
                            if (AmbientesqueLocationSounds.cracklingFire != null)
                            {
                                AmbientesqueLocationSounds.cracklingFire.SetVariable("Volume", volume * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
                                AmbientesqueLocationSounds.cracklingFire.Resume();
                            }
                            break;
                        case 2:
                            if (AmbientesqueLocationSounds.engine != null)
                            {
                                AmbientesqueLocationSounds.engine.SetVariable("Volume", volume * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
                                AmbientesqueLocationSounds.engine.Resume();
                            }
                            break;
                        case 3:
                            if (AmbientesqueLocationSounds.cricket != null)
                            {
                                AmbientesqueLocationSounds.cricket.SetVariable("Volume", volume * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
                                AmbientesqueLocationSounds.cricket.Resume();
                            }
                            break;
                    }
                    continue;
                }
                switch (i)
                {
                    case 0:
                        if (AmbientesqueLocationSounds.babblingBrook != null)
                        {
                            AmbientesqueLocationSounds.babblingBrook.Pause();
                        }
                        break;
                    case 1:
                        if (AmbientesqueLocationSounds.cracklingFire != null)
                        {
                            AmbientesqueLocationSounds.cracklingFire.Pause();
                        }
                        break;
                    case 2:
                        if (AmbientesqueLocationSounds.engine != null)
                        {
                            AmbientesqueLocationSounds.engine.Pause();
                        }
                        break;
                    case 3:
                        if (AmbientesqueLocationSounds.cricket != null)
                        {
                            AmbientesqueLocationSounds.cricket.Pause();
                        }
                        break;
                }
            }
        }
        AmbientesqueLocationSounds.updateTimer = 100;
    }

    public static void changeSpecificVariable(string variableName, float value, int whichSound)
    {
        if (whichSound == 2 && AmbientesqueLocationSounds.engine != null)
        {
            AmbientesqueLocationSounds.engine.SetVariable(variableName, value);
        }
    }

    public static void addSound(Vector2 tileLocation, int whichSound)
    {
        if (!AmbientesqueLocationSounds.sounds.ContainsKey(tileLocation * 64f))
        {
            AmbientesqueLocationSounds.sounds.Add(tileLocation * 64f, whichSound);
        }
    }

    public static void removeSound(Vector2 tileLocation)
    {
        if (!AmbientesqueLocationSounds.sounds.ContainsKey(tileLocation * 64f))
        {
            return;
        }
        switch (AmbientesqueLocationSounds.sounds[tileLocation * 64f])
        {
            case 0:
                if (AmbientesqueLocationSounds.babblingBrook != null)
                {
                    AmbientesqueLocationSounds.babblingBrook.Pause();
                }
                break;
            case 1:
                if (AmbientesqueLocationSounds.cracklingFire != null)
                {
                    AmbientesqueLocationSounds.cracklingFire.Pause();
                }
                break;
            case 2:
                if (AmbientesqueLocationSounds.engine != null)
                {
                    AmbientesqueLocationSounds.engine.Pause();
                }
                break;
            case 3:
                if (AmbientesqueLocationSounds.cricket != null)
                {
                    AmbientesqueLocationSounds.cricket.Pause();
                }
                break;
        }
        AmbientesqueLocationSounds.sounds.Remove(tileLocation * 64f);
    }

    public static void onLocationLeave()
    {
        AmbientesqueLocationSounds.sounds.Clear();
        AmbientesqueLocationSounds.volumeOverrideForLocChange = -0.5f;
        if (AmbientesqueLocationSounds.babblingBrook != null)
        {
            AmbientesqueLocationSounds.babblingBrook.Pause();
        }
        if (AmbientesqueLocationSounds.cracklingFire != null)
        {
            AmbientesqueLocationSounds.cracklingFire.Pause();
        }
        if (AmbientesqueLocationSounds.engine != null)
        {
            AmbientesqueLocationSounds.engine.SetVariable("Frequency", 100f);
            AmbientesqueLocationSounds.engine.Pause();
        }
        if (AmbientesqueLocationSounds.cricket != null)
        {
            AmbientesqueLocationSounds.cricket.Pause();
        }
    }
}

}