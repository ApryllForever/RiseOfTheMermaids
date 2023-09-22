/*

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using SpaceShared;
using StardewValley;
//using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
//using RestStopLocations;


namespace JsonAssets
{
    public interface IJsonAssetsApi
    {
        void LoadAssets(string path);

        int GetObjectId(string name);
        int GetCropId(string name);
        int GetFruitTreeId(string name);
        int GetBigCraftableId(string name);
        int GetHatId(string name);
        int GetWeaponId(string name);
        int GetClothingId(string name);

        IDictionary<string, int> GetAllObjectIds();
        IDictionary<string, int> GetAllCropIds();
        IDictionary<string, int> GetAllFruitTreeIds();
        IDictionary<string, int> GetAllBigCraftableIds();
        IDictionary<string, int> GetAllHatIds();
        IDictionary<string, int> GetAllWeaponIds();
        IDictionary<string, int> GetAllClothingIds();


        event EventHandler ItemsRegistered;
        event EventHandler IdsAssigned;
        event EventHandler AddedItemsToShop;
        event EventHandler IdsFixed;

        bool FixIdsInItem(Item item);
        void FixIdsInItemList(List<Item> items);
        void FixIdsInLocation(GameLocation location);

        bool TryGetCustomSprite(object entity, out Texture2D texture, out Rectangle sourceRect);
        bool TryGetCustomSpriteSheet(object entity, out Texture2D texture, out Rectangle sourceRect);
    }
}

*/