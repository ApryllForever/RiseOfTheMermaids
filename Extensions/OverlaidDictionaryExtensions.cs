using Microsoft.Xna.Framework;
using Netcode;
using StardewValley.Network;
using SObject = StardewValley.Object;

namespace RestStopLocations.Extensions
{
    internal static class OverlaidDictionaryExtensions
    {
        public static bool TryAdd(this OverlaidDictionary objects, Vector2 key, SObject obj)
        {
            if (!objects.ContainsKey(key)) 
            {
                objects.Add(key, obj); 
                return true;
            }

            return false;
        }

        public static bool TryAdd<T, TField>(this NetVector2Dictionary<T, TField> objects, Vector2 key, T value) where TField : NetField<T, TField>, new()
        {
            if (!objects.ContainsKey(key))
            {
                objects.Add(key, value);
                return true;
            }

            return false;
        }
    }
}
