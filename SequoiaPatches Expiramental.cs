using StardewValley.TerrainFeatures;
using StardewValley;
using StardewValley.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using xTile.Dimensions;
using HarmonyLib;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.ItemTypeDefinitions;
using StardewValley.Tools;
using StardewModdingAPI;
using StardewValley.Characters;
using Netcode;
using Force.DeepCloner;
using xTile.Tiles;
using xTile.Layers;

namespace RestStopLocations
{
    public static class SequoiaPatchesMarkII  //Code given to me by Bungus, who is in the Main Stardew Discord, on the day 2/13/24
    {
        public static IModHelper Helper;
        public static void Initialize(IModHelper helper)
        {
            Helper = helper;
        }

        // add custom seed logic (after adding via CP) by patching canPlantThisSeedHere
        public static void canPlantThisSeedHere_Postfix(ref bool __result, string itemId)
        {
            if (itemId == "ApryllForever.RiseMermaids_SequoiaSeed") { __result = true; }
        }

        /*
         * Don't need this one, I already have the Patch
         * 
        public static bool getBoundingBox_Prefix(Tree __instance, ref Microsoft.Xna.Framework.Rectangle __result)
        {
            // ignore vanilla trees
            if (int.TryParse(__instance.treeType.Value, out _))
                return true;

            if (__instance.treeType.Value == "Mermaid.Sequoia")
            {
                Vector2 tileLocation = __instance.Tile;
                if ((int)__instance.growthStage < 4)
                {
                    __result = new Microsoft.Xna.Framework.Rectangle((int)(tileLocation.X) * 64, (int)(tileLocation.Y) * 64, 128, 128);
                    return false;
                }
                else
                    __result = new Microsoft.Xna.Framework.Rectangle((int)(tileLocation.X) * 64, (int)(tileLocation.Y) * 64, 128, 128);
                return false;
            }
            return true;
        }
        */


        public static bool getRenderBounds_Prefix(Tree __instance, ref Microsoft.Xna.Framework.Rectangle __result)
        {
            // ignore vanilla trees
            if (int.TryParse(__instance.treeType.Value, out _))
                return true;

            if (__instance.treeType.Value != "Mermaid.Sequoia")
                return true;

                if (__instance.treeType.Value == "Mermaid.Sequoia")
            {
                Vector2 tileLocation = __instance.Tile;
                if ((bool)__instance.stump.Value || (int)__instance.growthStage.Value < 4)
                {
                    __result = new Microsoft.Xna.Framework.Rectangle((int)(tileLocation.X - 0f) * 64, (int)(tileLocation.Y - 1f) * 64, 64, 64);
                    return false;
                }

                if ((bool)__instance.stump.Value || (int)__instance.growthStage.Value < 5)
                {
                    __result = new Microsoft.Xna.Framework.Rectangle((int)(tileLocation.X - 0f) * 64, (int)(tileLocation.Y - 1f) * 64, 64, 128);
                    return false;
                }
                __result = new Microsoft.Xna.Framework.Rectangle((int)(tileLocation.X - 3f) * 64, (int)(tileLocation.Y - 5f) * 64, 384, 448);
                return false;
            }
            return true;
        }

        public static void isCollidingPosition_Postfix(GameLocation __instance, ref bool __result, Microsoft.Xna.Framework.Rectangle position)
        {
            Microsoft.Xna.Framework.Rectangle farmerBounds = Game1.player.GetBoundingBox();

            foreach (KeyValuePair<Vector2, TerrainFeature> feature in __instance.terrainFeatures.Pairs)
            {
                if (feature.Value is Tree tree)
                {
                    Microsoft.Xna.Framework.Rectangle treeBounds = tree.getBoundingBox();
                    if (treeBounds.Intersects(position) && !treeBounds.Intersects(farmerBounds))
                    {
                        __result = true;
                    }
                }
            }
        }
        public static bool draw_Prefix(Tree __instance, SpriteBatch spriteBatch)
        {
            var treeTopSourceRect = new Microsoft.Xna.Framework.Rectangle(0, 0, 144, 224);
            var stumpSourceRect = new Microsoft.Xna.Framework.Rectangle(96, 224, 48, 48);
            var shadowSourceRect = new Microsoft.Xna.Framework.Rectangle(663, 1011, 41, 30);

            // get reflected values
            float shakeTimer = Helper.Reflection.GetField<float>(__instance, "shakeTimer").GetValue();
            float alpha = Helper.Reflection.GetField<float>(__instance, "alpha").GetValue();
            float shakeRotation = Helper.Reflection.GetField<float>(__instance, "shakeRotation").GetValue();
            List<Leaf> leaves = Helper.Reflection.GetField<List<Leaf>>(__instance, "leaves").GetValue();
            bool falling = (bool)Helper.Reflection.GetField<NetBool>(__instance, "falling").GetValue().Value;

            // ignore vanilla trees
            if (int.TryParse(__instance.treeType.Value, out _))
                return true;

            if (__instance.treeType.Value == "Mermaid.Sequoia")
            {
                Vector2 tileLocation = __instance.Tile;
                float baseSortPosition = __instance.getBoundingBox().Bottom;

                // can't find tree data or texture
                if (__instance.texture.Value == null || !Tree.TryGetData(__instance.treeType.Value, out var data))
                {
                    IItemDataDefinition itemType = ItemRegistry.RequireTypeDefinition("(O)");
                    spriteBatch.Draw(
                        itemType.GetErrorTexture(),
                        Game1.GlobalToLocal(Game1.viewport, new Vector2(
                            tileLocation.X * 64f + ((shakeTimer > 0f) ? ((float)Math.Sin(Math.PI * 2.0 / (double)shakeTimer) * 3f) : 0f),
                            tileLocation.Y * 64f)),
                        itemType.GetErrorSourceRect(),
                        Color.White * alpha,
                        0f,
                        Vector2.Zero,
                        4f,
                        __instance.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                        (baseSortPosition + 1f) / 10000f);
                    return false;
                }
                if ((int)__instance.growthStage < 4)
                {
                    Microsoft.Xna.Framework.Rectangle sourceRect = (long)__instance.growthStage switch
                    {
                        0L => new Microsoft.Xna.Framework.Rectangle(80, 272, 16, 16),
                        1L => new Microsoft.Xna.Framework.Rectangle(48, 272, 16, 16),
                        2L => new Microsoft.Xna.Framework.Rectangle(64, 256, 16, 32),
                        3L => new Microsoft.Xna.Framework.Rectangle(0, 224, 16, 48),
                       // 4L => new Microsoft.Xna.Framework.Rectangle(144, 64, 48, 176),
                    };
                    // draw immature tree
                    spriteBatch.Draw(
                        __instance.texture.Value,
                        Game1.GlobalToLocal(Game1.viewport, new Vector2(
                            tileLocation.X * 64f + 32f,
                            tileLocation.Y * 64f - (float)(sourceRect.Height * 4 - 64) + (float)(((int)__instance.growthStage >= 2) ? 128 : 64))),
                        sourceRect,
                        __instance.fertilized ? Color.HotPink : Color.White,
                        shakeRotation,
                        new Vector2(8f, ((int)__instance.growthStage >= 2) ? 32 : 16),
                        4f,
                        __instance.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                        ((int)__instance.growthStage == 0) ? 0.0001f : (baseSortPosition / 10000f));
                }




              else if ((int)__instance.growthStage < 5)
                {
                    Microsoft.Xna.Framework.Rectangle sourceRect = (long)__instance.growthStage switch
                    {
                      
                        4L => new Microsoft.Xna.Framework.Rectangle(144, 64, 48, 176),
                    };
                    // draw immature tree
                    spriteBatch.Draw(
                        __instance.texture.Value,
                        Game1.GlobalToLocal(Game1.viewport, new Vector2(
                            tileLocation.X * 64f + 32f,
                            tileLocation.Y * 64f - (float)(sourceRect.Height * 4 - 64) + (float)(((int)__instance.growthStage >= 3) ? 128 : 64))),
                        sourceRect,
                        __instance.fertilized ? Color.HotPink : Color.White,
                        shakeRotation,
                        new Vector2(8f, ((int)__instance.growthStage >= 3) ? 32 : 16),
                        4f,
                        __instance.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                        ((int)__instance.growthStage == 0) ? 0.0001f : (baseSortPosition / 10000f));
                }

                else
                {
                    if (!__instance.stump || (bool)falling)
                    {
                        // draw shadow
                        spriteBatch.Draw(
                            Game1.mouseCursors,
                            Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * 64f - 51f, tileLocation.Y * 64f - 16f)),
                            shadowSourceRect,
                            Color.White * ((float)Math.PI / 2f - Math.Abs(shakeRotation)),
                            0f,
                            Vector2.Zero,
                            4f,
                            __instance.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                            1E-06f
                            );

                        //This is for alt appearance if seed in tree

                        //if ((data.UseAlternateSpriteWhenSeedReady && __instance.hasSeed.Value) || (data.UseAlternateSpriteWhenNotShaken && !__instance.wasShakenToday.Value))
                        //{
                         //   treeTopSourceRect.X = 48;
                       // }
                        //else


                        {
                            treeTopSourceRect.X = 0;
                        }
                        // draw tree top
                        spriteBatch.Draw(
                            __instance.texture.Value,
                            Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * 64f - 32f, tileLocation.Y * 64f + 64f)),
                            treeTopSourceRect,
                            Color.White * alpha,
                            shakeRotation,
                            new Vector2(24f, 96f),
                            4f, __instance.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                            (baseSortPosition + 2f) / 10000f - tileLocation.X / 1000000f);
                    }
                    if (__instance.health.Value >= 1f || (!falling && __instance.health.Value > -99f))
                    {
                        // draw tree stump
                        spriteBatch.Draw(
                            __instance.texture.Value,
                            Game1.GlobalToLocal(Game1.viewport, new Vector2(
                                tileLocation.X * 64f + ((shakeTimer > 0f) ? ((float)Math.Sin(Math.PI * 2.0 / (double)shakeTimer) * 3f) : 0f),
                                tileLocation.Y * 64f - 64f)),
                            stumpSourceRect,
                            Color.White * alpha,
                            0f,
                            Vector2.Zero,
                            4f,
                            __instance.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                            baseSortPosition / 10000f);
                    }
                    if ((bool)__instance.stump && __instance.health.Value < 4f && __instance.health.Value > -99f)
                    {
                        spriteBatch.Draw(
                            __instance.texture.Value,
                            Game1.GlobalToLocal(Game1.viewport, new Vector2(
                                tileLocation.X * 64f + ((shakeTimer > 0f) ? ((float)Math.Sin(Math.PI * 2.0 / (double)shakeTimer) * 3f) : 0f),
                                tileLocation.Y * 64f)),
                            new Microsoft.Xna.Framework.Rectangle(Math.Min(2, (int)(3f - __instance.health.Value)) * 96, 224, 16 * 3, 16 * 3),
                            Color.White * alpha,
                            0f,
                            Vector2.Zero,
                            4f,
                            __instance.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                            (baseSortPosition + 1f) / 10000f);
                    }
                }
                foreach (Leaf i in leaves)
                {
                    spriteBatch.Draw(
                        __instance.texture.Value,
                        Game1.GlobalToLocal(Game1.viewport, i.position),
                        new Microsoft.Xna.Framework.Rectangle(16 + i.type % 2 * 8, 256 + i.type / 2 * 8, 8, 8),
                        Color.White,
                        i.rotation,
                        Vector2.Zero,
                        4f,
                        SpriteEffects.None,
                        baseSortPosition / 10000f + 0.01f);
                }
                return false;
            }
            return true;
        }

        /*
         * 
         * Don't Know what the fuck this one is even for. No one axes my Redwoods.
         * 
         * 
        public static void DoFunction_Postfix(Axe __instance, GameLocation location, int x, int y, int power, Farmer who)
        {
            int tileX = x / 64;
            int tileY = y / 64;
            Microsoft.Xna.Framework.Rectangle tileRect = new Microsoft.Xna.Framework.Rectangle(tileX * 64, tileY * 64, 64, 64);
            Vector2 tile = new Vector2(tileX, tileY);

            foreach (KeyValuePair<Vector2, TerrainFeature> feature in location.terrainFeatures.Pairs)
            {
                if (feature.Value is Tree tree)
                {
                    Microsoft.Xna.Framework.Rectangle treeBounds = tree.getBoundingBox();
                    if (treeBounds.Intersects(tileRect) && tree.performToolAction(__instance, 0, tile))
                    {
                        location.terrainFeatures.Remove(feature.Key);
                    }
                }
            }
        }
        */

        public static void removeObjectsAndSpawned_Postfix(GameLocation __instance, int x, int y, int width, int height)
        {
            Microsoft.Xna.Framework.Rectangle pixelArea = new Microsoft.Xna.Framework.Rectangle(x * 64, y * 64, width * 64, height * 64);
            int maxX = x + width - 1;
            int maxY = y + height - 1;
            for (int curY = y; curY <= maxY; curY++)
            {
                for (int curX = x; curX <= maxX; curX++)
                {
                    Vector2 tile = new Vector2(curX, curY);
                    __instance.terrainFeatures.Remove(tile);
                    __instance.objects.Remove(tile);
                }
            }

            foreach (KeyValuePair<Vector2, TerrainFeature> feature in __instance.terrainFeatures.Pairs)
            {
                if (feature.Value is Tree tree)
                {
                    Microsoft.Xna.Framework.Rectangle treeBounds = tree.getBoundingBox();
                    if (treeBounds.Intersects(pixelArea))
                    {
                        __instance.terrainFeatures.Remove(feature.Key);
                    }
                }
            }
        }
    }


    [HarmonyPatch(typeof(Tree), nameof(Tree.getBoundingBox))]
    public static class SequoiaPatch
    {
        public static bool Prefix(Tree __instance, ref Microsoft.Xna.Framework.Rectangle __result)
        {
            if (int.TryParse(__instance.treeType.Value, out _)) // ignore vanilla trees
                return true;
            // WildTreeData data = __instance.GetData();

            // if (data != null && data.CustomFields.ContainsValue("MermaidSequoia"))
            if (__instance.treeType.Value == "Mermaid.Sequoia")
            {
                Vector2 tileLocation = __instance.Tile;
                if ((int)__instance.growthStage < 4)
                {
                    __result = new Microsoft.Xna.Framework.Rectangle((int)tileLocation.X * 64, (int)tileLocation.Y * 64, 64, 64);
                    return false;
                }
                else
                    //Game1.player.isColliding(Game1.player.currentLocation,tileLocation);
                    __result = new Microsoft.Xna.Framework.Rectangle((int)(tileLocation.X - 1f) * 64, (int)(tileLocation.Y - 1f) * 64, 192, 192);
                return false;
            }
            return true;
        }
    }



}