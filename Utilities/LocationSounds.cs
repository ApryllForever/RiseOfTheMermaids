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
public class LocationSounds
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

    public Vector2 position;

    public int which;


    public LocationSounds(Vector2 location, int which)
        {
            IModHelper helper;
            this.position = location;
            this.which = which;
            //LocationSounds.babblingBrook.Play();

        }




    public static void InitShared()
    {
        if (Game1.soundBank != null)
        {
            if (LocationSounds.babblingBrook == null)
            {
                    LocationSounds.babblingBrook = Game1.soundBank.GetCue("babblingBrook");
                    LocationSounds.babblingBrook.Play();
                LocationSounds.babblingBrook.Pause();
            }
            if (LocationSounds.cracklingFire == null)
            {
                LocationSounds.cracklingFire = Game1.soundBank.GetCue("cracklingFire");
                LocationSounds.cracklingFire.Play();
                LocationSounds.cracklingFire.Pause();
            }
            if (LocationSounds.engine == null)
            {
                LocationSounds.engine = Game1.soundBank.GetCue("hiss");
                LocationSounds.engine.Play();
                LocationSounds.engine.Pause();
            }
            if (LocationSounds.cricket == null)
            {
                LocationSounds.cricket = Game1.soundBank.GetCue("cricketsAmbient");
                LocationSounds.cricket.Play();
                LocationSounds.cricket.Pause();
            }
        }
        LocationSounds.shortestDistanceForCue = new float[4];
    }

    public static void update(GameTime time)
    {
        if (LocationSounds.sounds.Count == 0)
        {
            return;
        }
        if (LocationSounds.volumeOverrideForLocChange < 1f)
        {
            LocationSounds.volumeOverrideForLocChange += (float)time.ElapsedGameTime.Milliseconds * 0.0003f;
        }
        LocationSounds.updateTimer -= time.ElapsedGameTime.Milliseconds;
        if (LocationSounds.updateTimer > 0)
        {
            return;
        }
        for (int j = 0; j < LocationSounds.shortestDistanceForCue.Length; j++)
        {
            LocationSounds.shortestDistanceForCue[j] = 9999999f;
        }
        Vector2 farmerPosition = Game1.player.getStandingPosition();
        foreach (KeyValuePair<Vector2, int> pair in LocationSounds.sounds)
        {
            float distance = Vector2.Distance(pair.Key, farmerPosition);
            if (LocationSounds.shortestDistanceForCue[pair.Value] > distance)
            {
                LocationSounds.shortestDistanceForCue[pair.Value] = distance;
            }
        }
        if (LocationSounds.volumeOverrideForLocChange >= 0f)
        {
            for (int i = 0; i < LocationSounds.shortestDistanceForCue.Length; i++)
            {
                if (LocationSounds.shortestDistanceForCue[i] <= (float)LocationSounds.farthestSoundDistance)
                {
                    float volume = Math.Min(LocationSounds.volumeOverrideForLocChange, Math.Min(1f, 1f - LocationSounds.shortestDistanceForCue[i] / (float)LocationSounds.farthestSoundDistance));
                    switch (i)
                    {
                        case 0:
                            if (LocationSounds.babblingBrook != null)
                            {
                                LocationSounds.babblingBrook.SetVariable("Volume", volume * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
                                LocationSounds.babblingBrook.Resume();
                            }
                            break;
                        case 1:
                            if (LocationSounds.cracklingFire != null)
                            {
                                LocationSounds.cracklingFire.SetVariable("Volume", volume * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
                                LocationSounds.cracklingFire.Resume();
                            }
                            break;
                        case 2:
                            if (LocationSounds.engine != null)
                            {
                                LocationSounds.engine.SetVariable("Volume", volume * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
                                LocationSounds.engine.Resume();
                            }
                            break;
                        case 3:
                            if (LocationSounds.cricket != null)
                            {
                                LocationSounds.cricket.SetVariable("Volume", volume * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
                                LocationSounds.cricket.Resume();
                            }
                            break;
                    }
                    continue;
                }
                switch (i)
                {
                    case 0:
                        if (LocationSounds.babblingBrook != null)
                        {
                            LocationSounds.babblingBrook.Pause();
                        }
                        break;
                    case 1:
                        if (LocationSounds.cracklingFire != null)
                        {
                            LocationSounds.cracklingFire.Pause();
                        }
                        break;
                    case 2:
                        if (LocationSounds.engine != null)
                        {
                            LocationSounds.engine.Pause();
                        }
                        break;
                    case 3:
                        if (LocationSounds.cricket != null)
                        {
                            LocationSounds.cricket.Pause();
                        }
                        break;
                }
            }
        }
        LocationSounds.updateTimer = 100;
    }

    public static void changeSpecificVariable(string variableName, float value, int whichSound)
    {
        if (whichSound == 2 && LocationSounds.engine != null)
        {
            LocationSounds.engine.SetVariable(variableName, value);
        }
    }

    public static void addSound(Vector2 tileLocation, int whichSound)
    {
        if (!LocationSounds.sounds.ContainsKey(tileLocation * 64f))
        {
            LocationSounds.sounds.Add(tileLocation * 64f, whichSound);
        }
    }

    public static void removeSound(Vector2 tileLocation)
    {
        if (!LocationSounds.sounds.ContainsKey(tileLocation * 64f))
        {
            return;
        }
        switch (LocationSounds.sounds[tileLocation * 64f])
        {
            case 0:
                if (LocationSounds.babblingBrook != null)
                {
                    LocationSounds.babblingBrook.Pause();
                }
                break;
            case 1:
                if (LocationSounds.cracklingFire != null)
                {
                    LocationSounds.cracklingFire.Pause();
                }
                break;
            case 2:
                if (LocationSounds.engine != null)
                {
                    LocationSounds.engine.Pause();
                }
                break;
            case 3:
                if (LocationSounds.cricket != null)
                {
                    LocationSounds.cricket.Pause();
                }
                break;
        }
        LocationSounds.sounds.Remove(tileLocation * 64f);
    }

    public static void onLocationLeave()
    {
        LocationSounds.sounds.Clear();
        LocationSounds.volumeOverrideForLocChange = -0.5f;
        if (LocationSounds.babblingBrook != null)
        {
            LocationSounds.babblingBrook.Pause();
        }
        if (LocationSounds.cracklingFire != null)
        {
            LocationSounds.cracklingFire.Pause();
        }
        if (LocationSounds.engine != null)
        {
            LocationSounds.engine.SetVariable("Frequency", 100f);
            LocationSounds.engine.Pause();
        }
        if (LocationSounds.cricket != null)
        {
            LocationSounds.cricket.Pause();
        }
    }
}

}