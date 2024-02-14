

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley.BellsAndWhistles;
using StardewValley.Enchantments;
using StardewValley.Extensions;
using StardewValley.GameData.WildTrees;
using StardewValley.Internal;
using StardewValley.ItemTypeDefinitions;
using StardewValley.Locations;
using StardewValley.Logging;
using StardewValley.Tools;
using xTile.Dimensions;
using StardewValley.TerrainFeatures;
using StardewValley;
using StardewModdingAPI;
using Object = StardewValley.Object;

namespace RestStopLocations
{
    [XmlType("Mods_ApryllForever_RestStopLocations_Sequoia")]
    public class Sequoia : TerrainFeature
    {



        /// <remarks>The backing field for <see cref="M:StardewValley.TerrainFeatures.Tree.GetWildTreeDataDictionary" />.</remarks>
        protected static Dictionary<string, WildTreeData> _WildTreeData;

        /// <summary>The backing field for <see cref="M:StardewValley.TerrainFeatures.Tree.GetWildTreeSeedLookup" />.</summary>
        protected static Dictionary<string, string> _WildTreeSeedLookup;

        public const float chanceForDailySeed = 0.05f;

        public const float shakeRate = (float)Math.PI / 200f;

        public const float shakeDecayRate = 0.00306796166f;

        public const int minWoodDebrisForFallenTree = 12;

        public const int minWoodDebrisForStump = 5;

        public const int startingHealth = 10;

        public const int leafFallRate = 3;

        /// <summary>The oak tree type ID in <c>Data/WildTrees</c>.</summary>
        public const string bushyTree = "1";

        /// <summary>The maple tree type ID in <c>Data/WildTrees</c>.</summary>
        public const string leafyTree = "2";

        /// <summary>The pine tree type ID in <c>Data/WildTrees</c>.</summary>
        public const string pineTree = "3";

        public const string winterTree1 = "4";

        public const string winterTree2 = "5";

        /// <summary>The palm tree type ID (valley variant) in <c>Data/WildTrees</c>.</summary>
        public const string palmTree = "6";

        /// <summary>The mushroom tree type ID in <c>Data/WildTrees</c>.</summary>
        public const string mushroomTree = "7";

        /// <summary>The mahogany tree type ID in <c>Data/WildTrees</c>.</summary>
        public const string mahoganyTree = "8";

        /// <summary>The palm tree type ID (Ginger Island variant) in <c>Data/WildTrees</c>.</summary>
        public const string palmTree2 = "9";

        public const int seedStage = 0;

        public const int sproutStage = 1;

        public const int saplingStage = 2;

        public const int bushStage = 3;

        public const int treeStage = 5;

        /// <summary>The texture for the displayed tree sprites.</summary>
        [XmlIgnore]
        public Lazy<Texture2D> texture;

        /// <summary>The current season for the location containing the tree.</summary>
        protected Season? localSeason;

        [XmlElement("growthStage")]
        public readonly NetInt growthStage = new NetInt();

        [XmlElement("treeType")]
        public readonly NetString treeType = new NetString();

        [XmlElement("health")]
        public readonly NetFloat health = new NetFloat();

        [XmlElement("flipped")]
        public readonly NetBool flipped = new NetBool();

        [XmlElement("stump")]
        public readonly NetBool stump = new NetBool();

        [XmlElement("tapped")]
        public readonly NetBool tapped = new NetBool();

        [XmlElement("hasSeed")]
        public readonly NetBool hasSeed = new NetBool();

        [XmlIgnore]
        public readonly NetBool wasShakenToday = new NetBool();

        [XmlElement("fertilized")]
        public readonly NetBool fertilized = new NetBool();

        [XmlIgnore]
        public readonly NetBool shakeLeft = new NetBool().Interpolated(interpolate: false, wait: false);

        [XmlIgnore]
        private readonly NetBool falling = new NetBool();

        [XmlElement("destroy")]
        private readonly NetBool destroy = new NetBool();

        private float shakeRotation;

        private float maxShake;

        private float alpha = 1f;

        private List<Leaf> leaves = new List<Leaf>();

        [XmlElement("lastPlayerToHit")]
        private readonly NetLong lastPlayerToHit = new NetLong();

        private float shakeTimer;

        public static Microsoft.Xna.Framework.Rectangle treeTopSourceRect = new Microsoft.Xna.Framework.Rectangle(0, 0, 48, 96);

        public static Microsoft.Xna.Framework.Rectangle stumpSourceRect = new Microsoft.Xna.Framework.Rectangle(32, 96, 16, 32);

        public static Microsoft.Xna.Framework.Rectangle shadowSourceRect = new Microsoft.Xna.Framework.Rectangle(663, 1011, 41, 30);

        /// <summary>The asset name for the texture loaded by <see cref="F:StardewValley.TerrainFeatures.Tree.texture" />, if applicable.</summary>
        [XmlIgnore]
        public string TextureName { get; private set; }

        public Sequoia()
            : base(needsTick: true)
        {
            this.resetTexture();
        }

        public Sequoia(string id, int growthStage)
            : this()
        {
            this.growthStage.Value = growthStage;
            this.treeType.Value = id;
            if (this.treeType == "4")
            {
                this.treeType.Value = "1";
            }
            if (this.treeType == "5")
            {
                this.treeType.Value = "2";
            }
            this.flipped.Value = Game1.random.NextBool();
            this.health.Value = 10f;
        }

        public Sequoia(string id)
            : this()
        {
            this.treeType.Value = id;
            if (this.treeType == "4")
            {
                this.treeType.Value = "1";
            }
            if (this.treeType == "5")
            {
                this.treeType.Value = "2";
            }
            this.flipped.Value = Game1.random.NextBool();
            this.health.Value = 10f;
        }

        public override void initNetFields()
        {
            base.initNetFields();
            base.NetFields.AddField(this.growthStage, "growthStage").AddField(this.treeType, "treeType").AddField(this.health, "health")
                .AddField(this.flipped, "flipped")
                .AddField(this.stump, "stump")
                .AddField(this.tapped, "tapped")
                .AddField(this.hasSeed, "hasSeed")
                .AddField(this.fertilized, "fertilized")
                .AddField(this.shakeLeft, "shakeLeft")
                .AddField(this.falling, "falling")
                .AddField(this.destroy, "destroy")
                .AddField(this.lastPlayerToHit, "lastPlayerToHit")
                .AddField(this.wasShakenToday, "wasShakenToday");
            this.treeType.fieldChangeVisibleEvent += delegate
            {
                this.CheckForNewTexture();
            };
        }

    

        /// <summary>Load the raw wild tree data from <c>Data/WildTrees</c>.</summary>
        /// <remarks>This generally shouldn't be called directly; most code should use <see cref="M:StardewValley.TerrainFeatures.Tree.GetWildTreeDataDictionary" /> or <see cref="M:StardewValley.TerrainFeatures.Tree.GetWildTreeSeedLookup" /> instead.</remarks>
      

     

        /// <summary>Reload the tree texture based on <see cref="F:StardewValley.GameData.WildTrees.WildTreeData.Textures" /> if a different texture would be selected now.</summary>
        public void CheckForNewTexture()
        {
            if (this.texture.IsValueCreated)
            {
                string textureName;
                textureName = this.ChooseTexture();
                if (textureName != null && textureName != this.TextureName)
                {
                    this.resetTexture();
                }
            }
        }

        /// <summary>Reset the tree texture, so it'll be reselected and reloaded next time it's accessed.</summary>
        public void resetTexture()
        {
            this.texture = new Lazy<Texture2D>(LoadTexture);
            Texture2D LoadTexture()
            {
                this.TextureName = this.ChooseTexture();
                if (this.TextureName == null)
                {
                    return null;
                }
                return Game1.content.Load<Texture2D>(this.TextureName);
            }
        }

        /// <summary>Get the tree's data from <c>Data/WildTrees</c>, if found.</summary>
        public WildTreeData GetData()
        {
            if (!Tree.TryGetData(this.treeType.Value, out var data))
            {
                return null;
            }
            return data;
        }

        /// <summary>Try to get a tree's data from <c>Data/WildTrees</c>.</summary>
        /// <param name="id">The tree type ID (i.e. the key in <c>Data/WildTrees</c>).</param>
        /// <param name="data">The tree data, if found.</param>
        /// <returns>Returns whether the tree data was found.</returns>
        public static bool TryGetData(string id, out WildTreeData data)
        {
            if (id == null)
            {
                data = null;
                return false;
            }
            return Tree.GetWildTreeDataDictionary().TryGetValue(id, out data);
        }

        /// <summary>Choose an applicable texture from <see cref="F:StardewValley.GameData.WildTrees.WildTreeData.Textures" />.</summary>
        protected string ChooseTexture()
        {
            WildTreeData data;
            data = this.GetData();
            if (data != null && data.Textures?.Count > 0)
            {
                foreach (WildTreeTextureData entry in data.Textures)
                {
                    if ((!entry.Season.HasValue || entry.Season == this.localSeason) && (entry.Condition == null || GameStateQuery.CheckConditions(entry.Condition, this.Location)))
                    {
                        return entry.Texture;
                    }
                }
                return data.Textures[0].Texture;
            }
            return null;
        }

        public override Microsoft.Xna.Framework.Rectangle getBoundingBox()
        {
            Vector2 tileLocation;
            tileLocation = this.Tile;
            return new Microsoft.Xna.Framework.Rectangle((int)tileLocation.X * 64, (int)tileLocation.Y * 64, 64, 64);
        }

        public override Microsoft.Xna.Framework.Rectangle getRenderBounds()
        {
            Vector2 tileLocation;
            tileLocation = this.Tile;
            if ((bool)this.stump || (int)this.growthStage < 5)
            {
                return new Microsoft.Xna.Framework.Rectangle((int)(tileLocation.X - 0f) * 64, (int)(tileLocation.Y - 1f) * 64, 64, 128);
            }
            return new Microsoft.Xna.Framework.Rectangle((int)(tileLocation.X - 1f) * 64, (int)(tileLocation.Y - 5f) * 64, 192, 448);
        }

        public override bool performUseAction(Vector2 tileLocation)
        {
            GameLocation location;
            location = this.Location;
            if (!this.tapped)
            {
                if (this.maxShake == 0f && !this.stump && (int)this.growthStage >= 3 && this.IsLeafy())
                {
                    location.localSound("leafrustle");
                }
                this.shake(tileLocation, doEvenIfStillShaking: false);
            }
            if (Game1.player.ActiveObject != null && Game1.player.ActiveObject.canBePlacedHere(location, tileLocation))
            {
                return false;
            }
            return true;
        }

        private int extraWoodCalculator(Vector2 tileLocation)
        {
            Random random;
            random = Utility.CreateRandom(Game1.uniqueIDForThisGame, Game1.stats.DaysPlayed, (double)tileLocation.X * 7.0, (double)tileLocation.Y * 11.0);
            int extraWood;
            extraWood = 0;
            if (random.NextDouble() < Game1.player.DailyLuck)
            {
                extraWood++;
            }
            if (random.NextDouble() < (double)Game1.player.ForagingLevel / 12.5)
            {
                extraWood++;
            }
            if (random.NextDouble() < (double)Game1.player.ForagingLevel / 12.5)
            {
                extraWood++;
            }
            if (random.NextDouble() < (double)Game1.player.LuckLevel / 25.0)
            {
                extraWood++;
            }
            return extraWood;
        }

        public override bool tickUpdate(GameTime time)
        {
            GameLocation location;
            location = this.Location;
            Season? season;
            season = this.localSeason;
            if (!season.HasValue)
            {
                this.setSeason();
                this.CheckForNewTexture();
            }
            if (this.shakeTimer > 0f)
            {
                this.shakeTimer -= time.ElapsedGameTime.Milliseconds;
            }
            if ((bool)this.destroy)
            {
                return true;
            }
            this.alpha = Math.Min(1f, this.alpha + 0.05f);
            Vector2 tileLocation;
            tileLocation = this.Tile;
            if ((int)this.growthStage >= 5 && !this.falling && !this.stump && Game1.player.GetBoundingBox().Intersects(new Microsoft.Xna.Framework.Rectangle(64 * ((int)tileLocation.X - 1), 64 * ((int)tileLocation.Y - 5), 192, 288)))
            {
                this.alpha = Math.Max(0.4f, this.alpha - 0.09f);
            }
            if (!this.falling)
            {
                if ((double)Math.Abs(this.shakeRotation) > Math.PI / 2.0 && this.leaves.Count <= 0 && this.health.Value <= 0f)
                {
                    return true;
                }
                if (this.maxShake > 0f)
                {
                    if ((bool)this.shakeLeft)
                    {
                        this.shakeRotation -= (((int)this.growthStage >= 5) ? 0.005235988f : ((float)Math.PI / 200f));
                        if (this.shakeRotation <= 0f - this.maxShake)
                        {
                            this.shakeLeft.Value = false;
                        }
                    }
                    else
                    {
                        this.shakeRotation += (((int)this.growthStage >= 5) ? 0.005235988f : ((float)Math.PI / 200f));
                        if (this.shakeRotation >= this.maxShake)
                        {
                            this.shakeLeft.Value = true;
                        }
                    }
                }
                if (this.maxShake > 0f)
                {
                    this.maxShake = Math.Max(0f, this.maxShake - (((int)this.growthStage >= 5) ? 0.00102265389f : 0.00306796166f));
                }
            }
            else
            {
                this.shakeRotation += (this.shakeLeft ? (0f - this.maxShake * this.maxShake) : (this.maxShake * this.maxShake));
                this.maxShake += 0.00153398083f;
                WildTreeData data;
                data = this.GetData();
                if (data != null && Game1.random.NextDouble() < 0.01 && this.IsLeafy())
                {
                    location.localSound("leafrustle");
                }
                if ((double)Math.Abs(this.shakeRotation) > Math.PI / 2.0)
                {
                    this.falling.Value = false;
                    this.maxShake = 0f;
                    if (data != null)
                    {
                        if (data.DropHardwoodOnLumberChop || data.DropWoodOnChop)
                        {
                            location.localSound("treethud");
                        }
                        if (this.IsLeafy())
                        {
                            int leavesToAdd;
                            leavesToAdd = Game1.random.Next(90, 120);
                            for (int j = 0; j < leavesToAdd; j++)
                            {
                                this.leaves.Add(new Leaf(new Vector2(Game1.random.Next((int)(tileLocation.X * 64f), (int)(tileLocation.X * 64f + 192f)) + (this.shakeLeft ? (-320) : 256), tileLocation.Y * 64f - 64f), (float)Game1.random.Next(-10, 10) / 100f, Game1.random.Next(4), (float)Game1.random.Next(10, 40) / 10f));
                            }
                        }
                        if (data.DropWoodOnChop)
                        {
                            Game1.createRadialDebris(location, 12, (int)tileLocation.X + (this.shakeLeft ? (-4) : 4), (int)tileLocation.Y, (int)((Game1.getFarmer(this.lastPlayerToHit.Value).professions.Contains(12) ? 1.25 : 1.0) * (double)(12 + this.extraWoodCalculator(tileLocation))), resource: true);
                            Game1.createRadialDebris(location, 12, (int)tileLocation.X + (this.shakeLeft ? (-4) : 4), (int)tileLocation.Y, (int)((Game1.getFarmer(this.lastPlayerToHit.Value).professions.Contains(12) ? 1.25 : 1.0) * (double)(12 + this.extraWoodCalculator(tileLocation))), resource: false);
                        }
                        Random r;
                        if (Game1.IsMultiplayer)
                        {
                            Game1.recentMultiplayerRandom = Utility.CreateRandom((double)tileLocation.X * 1000.0, tileLocation.Y);
                            r = Game1.recentMultiplayerRandom;
                        }
                        else
                        {
                            r = Utility.CreateRandom(Game1.uniqueIDForThisGame, Game1.stats.DaysPlayed, (double)tileLocation.X * 7.0, (double)tileLocation.Y * 11.0);
                        }
                        Farmer targetFarmer;
                        targetFarmer = Game1.getFarmer(this.lastPlayerToHit.Value);
                        if (data.DropWoodOnChop)
                        {
                            Game1.createMultipleObjectDebris("(O)92", (int)tileLocation.X + (this.shakeLeft ? (-4) : 4), (int)tileLocation.Y, 5, this.lastPlayerToHit.Value, location);
                        }
                        int numHardwood;
                        numHardwood = 0;
                        if (data.DropHardwoodOnLumberChop && targetFarmer != null)
                        {
                            while (targetFarmer.professions.Contains(14) && r.NextBool())
                            {
                                numHardwood++;
                            }
                        }
                        List<WildTreeChopItemData> chopItems;
                        chopItems = data.ChopItems;
                        if (chopItems != null && chopItems.Count > 0)
                        {
                            bool addedAdditionalHardwood;
                            addedAdditionalHardwood = false;
                            foreach (WildTreeChopItemData drop in data.ChopItems)
                            {
                                Item item;
                                item = this.TryGetDrop(drop, r, targetFarmer, "ChopItems", null, false);
                                if (item != null)
                                {
                                    if (drop.ItemId == "709")
                                    {
                                        numHardwood += item.Stack;
                                        addedAdditionalHardwood = true;
                                    }
                                    else
                                    {
                                        Game1.createMultipleItemDebris(item, new Vector2(tileLocation.X + (float)(this.shakeLeft ? (-4) : 4), tileLocation.Y) * 64f, -2, location);
                                    }
                                }
                            }
                            if (addedAdditionalHardwood && targetFarmer != null && targetFarmer.professions.Contains(14))
                            {
                                numHardwood += (int)((float)numHardwood * 0.25f + 0.9f);
                            }
                        }
                        if (numHardwood > 0)
                        {
                            Game1.createMultipleObjectDebris("(O)709", (int)tileLocation.X + (this.shakeLeft ? (-4) : 4), (int)tileLocation.Y, numHardwood, this.lastPlayerToHit.Value, location);
                        }
                        float seedOnChopChance;
                        seedOnChopChance = data.SeedOnChopChance;
                        if (Game1.getFarmer(this.lastPlayerToHit.Value).getEffectiveSkillLevel(2) >= 1 && data != null && data.SeedItemId != null && r.NextDouble() < (double)seedOnChopChance)
                        {
                            Game1.createMultipleObjectDebris(data.SeedItemId, (int)tileLocation.X + (this.shakeLeft ? (-4) : 4), (int)tileLocation.Y, r.Next(1, 3), this.lastPlayerToHit.Value, location);
                        }
                    }
                    if (this.health.Value == -100f)
                    {
                        return true;
                    }
                    if (this.health.Value <= 0f)
                    {
                        this.health.Value = -100f;
                    }
                }
            }
            for (int i = this.leaves.Count - 1; i >= 0; i--)
            {
                Leaf leaf;
                leaf = this.leaves[i];
                leaf.position.Y -= leaf.yVelocity - 3f;
                leaf.yVelocity = Math.Max(0f, leaf.yVelocity - 0.01f);
                leaf.rotation += leaf.rotationRate;
                if (leaf.position.Y >= tileLocation.Y * 64f + 64f)
                {
                    this.leaves.RemoveAt(i);
                }
            }
            return false;
        }

        /// <summary>Get a dropped item if its fields match.</summary>
        /// <param name="drop">The drop data.</param>
        /// <param name="r">The RNG to use for random checks.</param>
        /// <param name="targetFarmer">The player interacting with the tree.</param>
        /// <param name="fieldName">The field name to show in error messages if the drop is invalid.</param>
        /// <param name="formatItemId">Format the selected item ID before it's resolved.</param>
        /// <param name="isStump">Whether the tree is a stump, or <c>null</c> to use <see cref="F:StardewValley.TerrainFeatures.Tree.stump" />.</param>
        /// <returns>Returns the produced item (if any), else <c>null</c>.</returns>
        protected Item TryGetDrop(WildTreeItemData drop, Random r, Farmer targetFarmer, string fieldName, Func<string, string> formatItemId = null, bool? isStump = null)
        {
            if (!r.NextBool(drop.Chance))
            {
                return null;
            }
            if (drop.Season.HasValue && drop.Season != this.Location.GetSeason())
            {
                return null;
            }
            if (drop.Condition != null && !GameStateQuery.CheckConditions(drop.Condition, this.Location, targetFarmer, null, null, r))
            {
                return null;
            }
            if (drop is WildTreeChopItemData chopItemData && !chopItemData.IsValidForGrowthStage(this.growthStage.Value, isStump ?? this.stump.Value))
            {
                return null;
            }
            return ItemQueryResolver.TryResolveRandomItem(drop, new ItemQueryContext(this.Location, targetFarmer, r), avoidRepeat: false, null, formatItemId, null, delegate (string query, string error)
            {
            
                DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(57, 5);
                defaultInterpolatedStringHandler.AppendLiteral("Wild tree '");
                defaultInterpolatedStringHandler.AppendFormatted(this.treeType.Value);
                defaultInterpolatedStringHandler.AppendLiteral("' failed parsing item query '");
                defaultInterpolatedStringHandler.AppendFormatted(query);
                defaultInterpolatedStringHandler.AppendLiteral("' for ");
                defaultInterpolatedStringHandler.AppendFormatted(fieldName);
                defaultInterpolatedStringHandler.AppendLiteral(" entry '");
                defaultInterpolatedStringHandler.AppendFormatted(drop.Id);
                defaultInterpolatedStringHandler.AppendLiteral("': ");
                defaultInterpolatedStringHandler.AppendFormatted(error);
                
            });
        }

        public void shake(Vector2 tileLocation, bool doEvenIfStillShaking)
        {
            GameLocation location;
            location = this.Location;
            WildTreeData data;
            data = this.GetData();
            if ((this.maxShake == 0f || doEvenIfStillShaking) && (int)this.growthStage >= 3 && !this.stump)
            {
                this.shakeLeft.Value = (float)Game1.player.StandingPixel.X > (tileLocation.X + 0.5f) * 64f || (Game1.player.Tile.X == tileLocation.X && Game1.random.NextBool());
                this.maxShake = (float)(((int)this.growthStage >= 5) ? (Math.PI / 128.0) : (Math.PI / 64.0));
                if ((int)this.growthStage >= 5)
                {
                    if (Game1.random.NextDouble() < 0.66)
                    {
                        int numberOfLeaves2;
                        numberOfLeaves2 = Game1.random.Next(1, 6);
                        for (int j = 0; j < numberOfLeaves2; j++)
                        {
                            this.leaves.Add(new Leaf(new Vector2(Game1.random.Next((int)(tileLocation.X * 64f - 64f), (int)(tileLocation.X * 64f + 128f)), Game1.random.Next((int)(tileLocation.Y * 64f - 256f), (int)(tileLocation.Y * 64f - 192f))), (float)Game1.random.Next(-10, 10) / 100f, Game1.random.Next(4), (float)Game1.random.Next(5) / 10f));
                        }
                    }
                    if (Game1.random.NextDouble() < 0.01 && (this.localSeason == Season.Spring || this.localSeason == Season.Summer))
                    {
                        bool isIslandButterfly;
                        isIslandButterfly = this.Location.InIslandContext();
                        while (Game1.random.NextDouble() < 0.8)
                        {
                            location.addCritter(new Butterfly(location, new Vector2(tileLocation.X + (float)Game1.random.Next(1, 3), tileLocation.Y - 2f + (float)Game1.random.Next(-1, 2)), isIslandButterfly));
                        }
                    }
                    if ((bool)this.hasSeed && (Game1.IsMultiplayer || Game1.player.ForagingLevel >= 1))
                    {
                        bool dropDefaultSeed;
                        dropDefaultSeed = true;
                        if (data != null && data.SeedDropItems?.Count > 0)
                        {
                            foreach (WildTreeSeedDropItemData drop in data.SeedDropItems)
                            {
                                Item seed;
                                seed = this.TryGetDrop(drop, Game1.random, Game1.player, "SeedDropItems");
                                if (seed != null)
                                {
                                    Game1.createItemDebris(seed, new Vector2(tileLocation.X * 64f, (tileLocation.Y - 3f) * 64f), -1, location, Game1.player.StandingPixel.Y);
                                    if (!drop.ContinueOnDrop)
                                    {
                                        dropDefaultSeed = false;
                                        break;
                                    }
                                }
                            }
                        }
                        if (dropDefaultSeed && data != null)
                        {
                            Game1.createItemDebris(ItemRegistry.Create(data.SeedItemId), new Vector2(tileLocation.X * 64f, (tileLocation.Y - 3f) * 64f), -1, location, Game1.player.StandingPixel.Y);
                        }
                        if (Game1.random.NextBool() && Game1.player.team.SpecialOrderRuleActive("DROP_QI_BEANS"))
                        {
                            Game1.createObjectDebris("(O)890", (int)tileLocation.X, (int)tileLocation.Y - 3, ((int)tileLocation.Y + 1) * 64, 0, 1f, location);
                        }
                        this.hasSeed.Value = false;
                    }
                    if (this.wasShakenToday.Value)
                    {
                        return;
                    }
                    this.wasShakenToday.Value = true;
                    if (data?.ShakeItems == null)
                    {
                        return;
                    }
                    {
                        foreach (WildTreeItemData entry in data.ShakeItems)
                        {
                            Item item;
                            item = this.TryGetDrop(entry, Game1.random, Game1.player, "ShakeItems");
                            if (item != null)
                            {
                                Game1.createItemDebris(item, tileLocation * 64f, -2, this.Location);
                            }
                        }
                        return;
                    }
                }
                if (Game1.random.NextDouble() < 0.66)
                {
                    int numberOfLeaves;
                    numberOfLeaves = Game1.random.Next(1, 3);
                    for (int i = 0; i < numberOfLeaves; i++)
                    {
                        this.leaves.Add(new Leaf(new Vector2(Game1.random.Next((int)(tileLocation.X * 64f), (int)(tileLocation.X * 64f + 48f)), tileLocation.Y * 64f - 32f), (float)Game1.random.Next(-10, 10) / 100f, Game1.random.Next(4), (float)Game1.random.Next(30) / 10f));
                    }
                }
            }
            else if ((bool)this.stump)
            {
                this.shakeTimer = 100f;
            }
        }

        public override bool isPassable(Character c = null)
        {
            if (this.health.Value <= -99f || (int)this.growthStage == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>Get the maximum size the tree can grow in its current position.</summary>
        /// <param name="ignoreSeason">Whether to assume the tree is in-season.</param>
        public virtual int GetMaxSizeHere(bool ignoreSeason = false)
        {
            GameLocation location;
            location = this.Location;
            Vector2 tile;
            tile = this.Tile;
            if (this.GetData() == null)
            {
                return this.growthStage.Value;
            }
            if (location.IsNoSpawnTile(tile, "Tree"))
            {
                return this.growthStage.Value;
            }
            if (!ignoreSeason && !this.IsInSeason())
            {
                return this.growthStage.Value;
            }
            if (this.growthStage.Value == 0 && location.objects.ContainsKey(tile))
            {
                return 0;
            }
            if (this.IsGrowthBlockedByNearbyTree())
            {
                return 4;
            }
            return 5;
        }

        /// <summary>Get whether this tree is in-season for its current location, so it can grow if applicable.</summary>
        public bool IsInSeason()
        {
            if (this.localSeason == Season.Winter && !this.fertilized.Value && !this.Location.SeedsIgnoreSeasonsHere())
            {
                return this.GetData()?.GrowsInWinter ?? false;
            }
            return true;
        }

        /// <summary>Get whether growth is blocked because it's too close to another fully-grown tree.</summary>
        public bool IsGrowthBlockedByNearbyTree()
        {
            GameLocation location;
            location = this.Location;
            Vector2 tile;
            tile = this.Tile;
            Microsoft.Xna.Framework.Rectangle growthRect;
            growthRect = new Microsoft.Xna.Framework.Rectangle((int)((tile.X - 1f) * 64f), (int)((tile.Y - 1f) * 64f), 192, 192);
            foreach (KeyValuePair<Vector2, TerrainFeature> other in location.terrainFeatures.Pairs)
            {
                if (other.Key != tile && other.Value is Tree otherTree && (int)otherTree.growthStage >= 5 && otherTree.getBoundingBox().Intersects(growthRect))
                {
                    return true;
                }
            }
            return false;
        }

        public override void dayUpdate()
        {
            GameLocation environment;
            environment = this.Location;
            this.wasShakenToday.Value = false;
            this.setSeason();
            this.CheckForNewTexture();
            WildTreeData data;
            data = this.GetData();
            Vector2 tile;
            tile = this.Tile;
            if (this.health.Value <= -100f)
            {
                this.destroy.Value = true;
            }
            if (this.tapped.Value)
            {
                Object tile_object;
                tile_object = environment.getObjectAtTile((int)tile.X, (int)tile.Y);
                if (tile_object == null || !tile_object.IsTapper())
                {
                    this.tapped.Value = false;
                }
            }
            if (this.GetMaxSizeHere() > this.growthStage.Value)
            {
                float chance;
                chance = data?.GrowthChance ?? 0.2f;
                float fertilizedGrowthChance;
                fertilizedGrowthChance = data?.FertilizedGrowthChance ?? 1f;
                if (Game1.random.NextBool(chance) || (this.fertilized.Value && Game1.random.NextBool(fertilizedGrowthChance)))
                {
                    this.growthStage.Value++;
                }
            }
            if (this.localSeason == Season.Winter && data != null && data.IsStumpDuringWinter && !this.IsInSeason())
            {
                this.stump.Value = true;
            }
            else if (data != null && data.IsStumpDuringWinter && Game1.dayOfMonth <= 1 && Game1.IsSpring)
            {
                this.stump.Value = false;
                this.health.Value = 10f;
                this.shakeRotation = 0f;
            }
            if ((int)this.growthStage >= 5 && environment is Farm && Game1.random.NextDouble() < 0.15)
            {
                int xCoord;
                xCoord = Game1.random.Next(-3, 4) + (int)tile.X;
                int yCoord;
                yCoord = Game1.random.Next(-3, 4) + (int)tile.Y;
                Vector2 location;
                location = new Vector2(xCoord, yCoord);
                if (!environment.IsNoSpawnTile(location, "Tree") && environment.isTileLocationOpen(new Location(xCoord, yCoord)) && !environment.IsTileOccupiedBy(location) && !environment.isWaterTile(xCoord, yCoord) && environment.isTileOnMap(location))
                {
                    environment.terrainFeatures.Add(location, new Tree(this.treeType, 0));
                }
            }
            this.hasSeed.Value = data != null && data.SeedItemId != null && (int)this.growthStage >= 5 && Game1.random.NextBool(data.SeedChance);
        }

        public override void performPlayerEntryAction()
        {
            base.performPlayerEntryAction();
            this.setSeason();
            this.CheckForNewTexture();
        }

        /// <inheritdoc />
        public override bool seasonUpdate(bool onLoad)
        {
            this.loadSprite();
            return false;
        }

        public override bool isActionable()
        {
            if (!this.tapped)
            {
                return (int)this.growthStage >= 3;
            }
            return false;
        }

        public virtual bool IsLeafy()
        {
            WildTreeData data;
            data = this.GetData();
            if (data != null && data.IsLeafy)
            {
                if (!data.IsLeafyInWinter)
                {
                    return !this.Location.IsWinterHere();
                }
                return true;
            }
            return false;
        }

        /// <summary>Get the color of the cosmetic wood chips when chopping the tree.</summary>
        public Color? GetChopDebrisColor()
        {
            return this.GetChopDebrisColor(this.GetData());
        }

        /// <summary>Get the color of the cosmetic wood chips when chopping the tree.</summary>
        /// <param name="data">The wild tree data to read.</param>
        public Color? GetChopDebrisColor(WildTreeData data)
        {
            string rawColor;
            rawColor = data?.DebrisColor;
            if (rawColor == null)
            {
                return null;
            }
            if (!int.TryParse(rawColor, out var debrisType))
            {
                return Utility.StringToColor(rawColor);
            }
            return Debris.getColorForDebris(debrisType);
        }

        public override bool performToolAction(Tool t, int explosion, Vector2 tileLocation)
        {
            GameLocation location;
            location = this.Location ?? Game1.currentLocation;
            if (explosion > 0)
            {
                this.tapped.Value = false;
            }
            if ((bool)this.tapped)
            {
                return false;
            }
            if (this.health.Value <= -99f)
            {
                return false;
            }
            if ((int)this.growthStage >= 5)
            {
                if (t is Axe)
                {
                    location.playSound("axchop", tileLocation);
                    this.lastPlayerToHit.Value = t.getLastFarmerToUse().UniqueMultiplayerID;
                    location.debris.Add(new Debris(12, Game1.random.Next(1, 3), t.getLastFarmerToUse().GetToolLocation() + new Vector2(16f, 0f), t.getLastFarmerToUse().Position, 0, this.GetChopDebrisColor()));
                    if (!this.stump && t.getLastFarmerToUse() != null && location.HasUnlockedAreaSecretNotes(t.getLastFarmerToUse()) && Game1.random.NextDouble() < 0.005)
                    {
                        Object o;
                        o = location.tryToCreateUnseenSecretNote(t.getLastFarmerToUse());
                        if (o != null)
                        {
                            Game1.createItemDebris(o, new Vector2(tileLocation.X, tileLocation.Y - 3f) * 64f, -1, location, Game1.player.StandingPixel.Y - 32);
                        }
                    }
                }
                else if (explosion <= 0)
                {
                    return false;
                }
                this.shake(tileLocation, doEvenIfStillShaking: true);
                float damage;
                if (explosion > 0)
                {
                    damage = explosion;
                }
                else
                {
                    if (t == null)
                    {
                        return false;
                    }
                    damage = (int)t.upgradeLevel switch
                    {
                        0 => 1f,
                        1 => 1.25f,
                        2 => 1.67f,
                        3 => 2.5f,
                        4 => 5f,
                        _ => (int)t.upgradeLevel + 1,
                    };
                }
                if (t is Axe && t.hasEnchantmentOfType<ShavingEnchantment>() && Game1.random.NextDouble() <= (double)(damage / 5f))
                {
                    Debris d;
                    d = new Debris("388", new Vector2(tileLocation.X * 64f + 32f, (tileLocation.Y - 0.5f) * 64f + 32f), Game1.player.getStandingPosition());
                    d.Chunks[0].xVelocity.Value += (float)Game1.random.Next(-10, 11) / 10f;
                    d.chunkFinalYLevel = (int)(tileLocation.Y * 64f + 64f);
                    location.debris.Add(d);
                }
                this.health.Value -= damage;
                if (this.health.Value <= 0f && this.performTreeFall(t, explosion, tileLocation))
                {
                    return true;
                }
            }
            else if ((int)this.growthStage >= 3)
            {
                if (t != null && t.BaseName.Contains("Ax"))
                {
                    location.playSound("axchop", tileLocation);
                    if (this.IsLeafy())
                    {
                        location.playSound("leafrustle");
                    }
                    location.debris.Add(new Debris(12, Game1.random.Next((int)t.upgradeLevel * 2, (int)t.upgradeLevel * 4), t.getLastFarmerToUse().GetToolLocation() + new Vector2(16f, 0f), Utility.PointToVector2(t.getLastFarmerToUse().StandingPixel), 0));
                }
                else if (explosion <= 0)
                {
                    return false;
                }
                this.shake(tileLocation, doEvenIfStillShaking: true);
                float damage2;
                damage2 = 1f;
                damage2 = ((explosion > 0) ? ((float)explosion) : ((int)t.upgradeLevel switch
                {
                    0 => 2f,
                    1 => 2.5f,
                    2 => 3.34f,
                    3 => 5f,
                    4 => 10f,
                    _ => 10 + ((int)t.upgradeLevel - 4),
                }));
                this.health.Value -= damage2;
                if (this.health.Value <= 0f)
                {
                    this.performBushDestroy(tileLocation);
                    return true;
                }
            }
            else if ((int)this.growthStage >= 1)
            {
                if (explosion > 0)
                {
                    location.playSound("cut");
                    return true;
                }
                if (t != null && t.BaseName.Contains("Axe"))
                {
                    location.playSound("axchop", tileLocation);
                    Game1.createRadialDebris(location, 12, (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(10, 20), resource: false);
                }
                if (t is Axe || t is Pickaxe || t is Hoe || t is MeleeWeapon)
                {
                    location.playSound("cut");
                    this.performSproutDestroy(t, tileLocation);
                    return true;
                }
            }
            else
            {
                if (explosion > 0)
                {
                    return true;
                }
                if (t.BaseName.Contains("Axe") || t.BaseName.Contains("Pick") || t.BaseName.Contains("Hoe"))
                {
                    location.playSound("woodyHit", tileLocation);
                    location.playSound("axchop", tileLocation);
                    this.performSeedDestroy(t, tileLocation);
                    return true;
                }
            }
            return false;
        }

        public bool fertilize()
        {
            GameLocation location;
            location = this.Location;
            if ((int)this.growthStage >= 5)
            {
                Game1.showRedMessageUsingLoadString("Strings\\StringsFromCSFiles:TreeFertilizer1");
                location.playSound("cancel");
                return false;
            }
            if (this.fertilized.Value)
            {
                Game1.showRedMessageUsingLoadString("Strings\\StringsFromCSFiles:TreeFertilizer2");
                location.playSound("cancel");
                return false;
            }
            this.fertilized.Value = true;
            location.playSound("dirtyHit");
            return true;
        }

        public bool instantDestroy(Vector2 tileLocation)
        {
            if ((int)this.growthStage >= 5)
            {
                return this.performTreeFall(null, 0, tileLocation);
            }
            if ((int)this.growthStage >= 3)
            {
                this.performBushDestroy(tileLocation);
                return true;
            }
            if ((int)this.growthStage >= 1)
            {
                this.performSproutDestroy(null, tileLocation);
                return true;
            }
            this.performSeedDestroy(null, tileLocation);
            return true;
        }

        protected void performSeedDestroy(Tool t, Vector2 tileLocation)
        {
            GameLocation location;
            location = this.Location;
            Game1.Multiplayer.broadcastSprites(location, new TemporaryAnimatedSprite(17, tileLocation * 64f, Color.White));
            WildTreeData data;
            data = this.GetData();
            if (data != null && data.SeedItemId != null)
            {
                if (this.lastPlayerToHit.Value != 0L && Game1.getFarmer(this.lastPlayerToHit.Value).getEffectiveSkillLevel(2) >= 1)
                {
                    Game1.createMultipleObjectDebris(data.SeedItemId, (int)tileLocation.X, (int)tileLocation.Y, 1, t.getLastFarmerToUse().UniqueMultiplayerID, location);
                }
                else if (Game1.player.getEffectiveSkillLevel(2) >= 1)
                {
                    Game1.createMultipleObjectDebris(data.SeedItemId, (int)tileLocation.X, (int)tileLocation.Y, 1, t?.getLastFarmerToUse().UniqueMultiplayerID ?? Game1.player.UniqueMultiplayerID, location);
                }
            }
        }

        /// <summary>Update the attached tapper's held output.</summary>
        /// <param name="tapper">The attached tapper instance.</param>
        /// <param name="previousOutput">The previous item produced by the tapper, if any.</param>
        public void UpdateTapperProduct(Object tapper, Object previousOutput = null)
        {
            if (tapper == null)
            {
                return;
            }
            WildTreeData data;
            data = this.GetData();
            if (data == null)
            {
                return;
            }
            float timeMultiplier;
            timeMultiplier = 1f;
            foreach (string contextTag in tapper.GetContextTags())
            {
                if (contextTag.StartsWith("tapper_multiplier_") && float.TryParse(contextTag.Substring("tapper_multiplier_".Length), out var multiplier))
                {
                    timeMultiplier = 1f / multiplier;
                    break;
                }
            }
            Random random;
            random = Utility.CreateRandom(Game1.uniqueIDForThisGame, Game1.stats.DaysPlayed, 73137.0, (double)this.Tile.X * 9.0, (double)this.Tile.Y * 13.0);
            if (this.TryGetTapperOutput(data.TapItems, previousOutput?.ItemId, random, timeMultiplier, out var output, out var minutesUntilReady))
            {
                tapper.heldObject.Value = output;
                tapper.minutesUntilReady.Value = minutesUntilReady;
            }
        }

        /// <summary>Get a valid item that can be produced by the tree's current tapper.</summary>
        /// <param name="tapItems">The tap item data to choose from.</param>
        /// <param name="previousItemId">The previous item ID that was produced.</param>
        /// <param name="r">The RNG with which to randomize.</param>
        /// <param name="timeMultiplier">A multiplier to apply to the minutes until ready.</param>
        /// <param name="output">The possible tapper output.</param>
        /// <param name="minutesUntilReady">The number of minutes until the tapper would produce the output.</param>
        protected bool TryGetTapperOutput(List<WildTreeTapItemData> tapItems, string previousItemId, Random r, float timeMultiplier, out Object output, out int minutesUntilReady)
        {
            if (tapItems != null)
            {
                previousItemId = ((previousItemId != null) ? ItemRegistry.QualifyItemId(previousItemId) : null);
                foreach (WildTreeTapItemData tapData in tapItems)
                {
                    if (!GameStateQuery.CheckConditions(tapData.Condition, this.Location))
                    {
                        continue;
                    }
                    if (tapData.PreviousItemId != null)
                    {
                        bool found;
                        found = false;
                        foreach (string expectedPrevId in tapData.PreviousItemId)
                        {
                            found = (string.IsNullOrEmpty(expectedPrevId) ? (previousItemId == null) : string.Equals(previousItemId, ItemRegistry.QualifyItemId(expectedPrevId), StringComparison.OrdinalIgnoreCase));
                            if (found)
                            {
                                break;
                            }
                        }
                        if (!found)
                        {
                            continue;
                        }
                    }
                    if (tapData.Season.HasValue && tapData.Season != this.localSeason)
                    {
                        continue;
                    }
                    Farmer targetFarmer;
                    targetFarmer = Game1.getFarmer(this.lastPlayerToHit.Value);
                    Item item;
                    item = this.TryGetDrop(tapData, r, targetFarmer, "TapItems", (string id) => id.Replace("PREVIOUS_OUTPUT_ID", previousItemId));
                    if (item != null)
                    {
                        if (item is Object obj)
                        {
                            int daysUntilReady;
                            daysUntilReady = (int)Utility.ApplyQuantityModifiers(tapData.DaysUntilReady, tapData.DaysUntilReadyModifiers, tapData.DaysUntilReadyModifierMode, this.Location, Game1.player);
                            output = obj;
                            minutesUntilReady = Utility.CalculateMinutesUntilMorning(Game1.timeOfDay, (int)Math.Max(1.0, Math.Floor((float)daysUntilReady * timeMultiplier)));
                            return true;
                        }
                        IGameLogger log;
                       
                        DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(64, 2);
                        defaultInterpolatedStringHandler.AppendLiteral("Wild tree '");
                        defaultInterpolatedStringHandler.AppendFormatted(this.treeType.Value);
                        defaultInterpolatedStringHandler.AppendLiteral("' can't produce item '");
                        defaultInterpolatedStringHandler.AppendFormatted(item.ItemId);
                        defaultInterpolatedStringHandler.AppendLiteral("': must be an object-type item.");
                       
                    }
                }
                if (previousItemId != null)
                {
                    return this.TryGetTapperOutput(tapItems, null, r, timeMultiplier, out output, out minutesUntilReady);
                }
            }
            output = null;
            minutesUntilReady = 0;
            return false;
        }

        protected void performSproutDestroy(Tool t, Vector2 tileLocation)
        {
            GameLocation location;
            location = this.Location;
            Game1.createRadialDebris(location, 12, (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(10, 20), resource: false);
            if (t != null && t.BaseName.Contains("Axe") && Game1.recentMultiplayerRandom.NextDouble() < (double)((float)t.getLastFarmerToUse().ForagingLevel / 10f))
            {
                Game1.createDebris(12, (int)tileLocation.X, (int)tileLocation.Y, 1);
            }
            Game1.Multiplayer.broadcastSprites(location, new TemporaryAnimatedSprite(17, tileLocation * 64f, Color.White));
        }

        protected void performBushDestroy(Vector2 tileLocation)
        {
            GameLocation location;
            location = this.Location;
            WildTreeData data;
            data = this.GetData();
            if (data == null)
            {
                return;
            }
            Game1.createRadialDebris(location, 12, (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(20, 30), resource: false, -1, item: false, this.GetChopDebrisColor(data));
            if (data.DropWoodOnChop || data.DropHardwoodOnLumberChop)
            {
                Game1.createDebris(12, (int)tileLocation.X, (int)tileLocation.Y, (int)((Game1.getFarmer(this.lastPlayerToHit.Value).professions.Contains(12) ? 1.25 : 1.0) * 4.0), location);
            }
            List<WildTreeChopItemData> chopItems;
            chopItems = data.ChopItems;
            if (chopItems == null || chopItems.Count <= 0)
            {
                return;
            }
            Random r;
            if (Game1.IsMultiplayer)
            {
                Game1.recentMultiplayerRandom = Utility.CreateRandom((double)tileLocation.X * 1000.0, tileLocation.Y);
                r = Game1.recentMultiplayerRandom;
            }
            else
            {
                r = Utility.CreateRandom(Game1.uniqueIDForThisGame, Game1.stats.DaysPlayed, (double)tileLocation.X * 7.0, (double)tileLocation.Y * 11.0);
            }
            Farmer targetFarmer;
            targetFarmer = Game1.getFarmer(this.lastPlayerToHit.Value);
            foreach (WildTreeChopItemData drop in data.ChopItems)
            {
                Item item;
                item = this.TryGetDrop(drop, r, targetFarmer, "ChopItems");
                if (item != null)
                {
                    Game1.createMultipleItemDebris(item, tileLocation * 64f, -2, location);
                }
            }
        }

        protected bool performTreeFall(Tool t, int explosion, Vector2 tileLocation)
        {
            GameLocation location;
            location = this.Location;
            WildTreeData data;
            data = this.GetData();
            this.Location.objects.Remove(this.Tile);
            this.tapped.Value = false;
            if (!this.stump)
            {
                if (t != null || explosion > 0)
                {
                    location.playSound("treecrack");
                }
                this.stump.Value = true;
                this.health.Value = 5f;
                this.falling.Value = true;
                if (t != null && t.getLastFarmerToUse().IsLocalPlayer)
                {
                    t?.getLastFarmerToUse().gainExperience(2, 12);
                    if (t?.getLastFarmerToUse() == null)
                    {
                        this.shakeLeft.Value = true;
                    }
                    else
                    {
                        this.shakeLeft.Value = (float)t.getLastFarmerToUse().StandingPixel.X > (tileLocation.X + 0.5f) * 64f;
                    }
                }
            }
            else
            {
                if (t != null && this.health.Value != -100f && t.getLastFarmerToUse().IsLocalPlayer)
                {
                    t?.getLastFarmerToUse().gainExperience(2, 1);
                }
                this.health.Value = -100f;
                if (data != null)
                {
                    Game1.createRadialDebris(location, 12, (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(30, 40), resource: false, -1, item: false, this.GetChopDebrisColor(data));
                    Random r;
                    if (Game1.IsMultiplayer)
                    {
                        Game1.recentMultiplayerRandom = Utility.CreateRandom((double)tileLocation.X * 2000.0, tileLocation.Y);
                        r = Game1.recentMultiplayerRandom;
                    }
                    else
                    {
                        r = Utility.CreateRandom(Game1.uniqueIDForThisGame, Game1.stats.DaysPlayed, (double)tileLocation.X * 7.0, (double)tileLocation.Y * 11.0);
                    }
                    if (t?.getLastFarmerToUse() == null)
                    {
                        if (location.Equals(Game1.currentLocation))
                        {
                            Game1.createMultipleObjectDebris("(O)92", (int)tileLocation.X, (int)tileLocation.Y, 2, location);
                        }
                        else
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                Game1.createItemDebris(ItemRegistry.Create("(O)92"), tileLocation * 64f, 2, location);
                            }
                        }
                    }
                    else if (Game1.IsMultiplayer)
                    {
                        if (data.DropWoodOnChop)
                        {
                            Game1.createRadialDebris(location, 12, (int)tileLocation.X, (int)tileLocation.Y, (int)((Game1.getFarmer(this.lastPlayerToHit.Value).professions.Contains(12) ? 1.25 : 1.0) * 4.0), resource: true);
                        }
                        List<WildTreeChopItemData> chopItems;
                        chopItems = data.ChopItems;
                        if (chopItems != null && chopItems.Count > 0)
                        {
                            Farmer targetFarmer2;
                            targetFarmer2 = Game1.getFarmer(this.lastPlayerToHit.Value);
                            foreach (WildTreeChopItemData drop2 in data.ChopItems)
                            {
                                Item item2;
                                item2 = this.TryGetDrop(drop2, r, targetFarmer2, "ChopItems");
                                if (item2 != null)
                                {
                                    if (item2.QualifiedItemId == "(O)420" && tileLocation.X % 7f == 0f)
                                    {
                                        item2 = ItemRegistry.Create("(O)422", item2.Stack, item2.Quality);
                                    }
                                    Game1.createMultipleItemDebris(item2, tileLocation * 64f, -2, location);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (data.DropWoodOnChop)
                        {
                            Game1.createRadialDebris(location, 12, (int)tileLocation.X, (int)tileLocation.Y, (int)((Game1.getFarmer(this.lastPlayerToHit.Value).professions.Contains(12) ? 1.25 : 1.0) * (double)(5 + this.extraWoodCalculator(tileLocation))), resource: true);
                        }
                        List<WildTreeChopItemData> chopItems2;
                        chopItems2 = data.ChopItems;
                        if (chopItems2 != null && chopItems2.Count > 0)
                        {
                            Farmer targetFarmer;
                            targetFarmer = Game1.getFarmer(this.lastPlayerToHit.Value);
                            foreach (WildTreeChopItemData drop in data.ChopItems)
                            {
                                Item item;
                                item = this.TryGetDrop(drop, r, targetFarmer, "ChopItems");
                                if (item != null)
                                {
                                    if (item.QualifiedItemId == "(O)420" && tileLocation.X % 7f == 0f)
                                    {
                                        item = ItemRegistry.Create("(O)422", item.Stack, item.Quality);
                                    }
                                    Game1.createMultipleItemDebris(item, tileLocation * 64f, -2, location);
                                }
                            }
                        }
                    }
                    if (Game1.random.NextDouble() <= 0.25 && Game1.player.team.SpecialOrderRuleActive("DROP_QI_BEANS"))
                    {
                        Game1.createObjectDebris("(O)890", (int)tileLocation.X, (int)tileLocation.Y - 3, ((int)tileLocation.Y + 1) * 64, 0, 1f, location);
                    }
                    location.playSound("treethud");
                }
                if (!this.falling)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>Update the tree's season for the location it's planted in.</summary>
        protected void setSeason()
        {
            GameLocation location;
            location = this.Location;
            this.localSeason = ((!(location is Desert) && !(location is MineShaft)) ? Game1.GetSeasonForLocation(location) : Season.Spring);
        }

        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 positionOnScreen, Vector2 tileLocation, float scale, float layerDepth)
        {
            layerDepth += positionOnScreen.X / 100000f;
            if ((int)this.growthStage < 5)
            {
                Microsoft.Xna.Framework.Rectangle sourceRect;
                sourceRect = (int)this.growthStage switch
                {
                    0 => new Microsoft.Xna.Framework.Rectangle(32, 128, 16, 16),
                    1 => new Microsoft.Xna.Framework.Rectangle(0, 128, 16, 16),
                    2 => new Microsoft.Xna.Framework.Rectangle(16, 128, 16, 16),
                    _ => new Microsoft.Xna.Framework.Rectangle(0, 96, 16, 32),
                };
                spriteBatch.Draw(this.texture.Value, positionOnScreen - new Vector2(0f, (float)sourceRect.Height * scale), sourceRect, Color.White, 0f, Vector2.Zero, scale, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth + (positionOnScreen.Y + (float)sourceRect.Height * scale) / 20000f);
                return;
            }
            if (!this.falling)
            {
                spriteBatch.Draw(this.texture.Value, positionOnScreen + new Vector2(0f, -64f * scale), new Microsoft.Xna.Framework.Rectangle(32, 96, 16, 32), Color.White, 0f, Vector2.Zero, scale, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth + (positionOnScreen.Y + 448f * scale - 1f) / 20000f);
            }
            if (!this.stump || (bool)this.falling)
            {
                spriteBatch.Draw(this.texture.Value, positionOnScreen + new Vector2(-64f * scale, -320f * scale), new Microsoft.Xna.Framework.Rectangle(0, 0, 48, 96), Color.White, this.shakeRotation, Vector2.Zero, scale, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth + (positionOnScreen.Y + 448f * scale) / 20000f);
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            if (base.isTemporarilyInvisible)
            {
                return;
            }
            Vector2 tileLocation;
            tileLocation = this.Tile;
            float baseSortPosition;
            baseSortPosition = this.getBoundingBox().Bottom;
            if (this.texture.Value == null || !Tree.TryGetData(this.treeType.Value, out var data))
            {
                IItemDataDefinition itemType;
                itemType = ItemRegistry.RequireTypeDefinition("(O)");
                spriteBatch.Draw(itemType.GetErrorTexture(), Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * 64f + ((this.shakeTimer > 0f) ? ((float)Math.Sin(Math.PI * 2.0 / (double)this.shakeTimer) * 3f) : 0f), tileLocation.Y * 64f)), itemType.GetErrorSourceRect(), Color.White * this.alpha, 0f, Vector2.Zero, 4f, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (baseSortPosition + 1f) / 10000f);
                return;
            }
            if ((int)this.growthStage < 5)
            {
                Microsoft.Xna.Framework.Rectangle sourceRect;
                sourceRect = (int)this.growthStage switch
                {
                    0 => new Microsoft.Xna.Framework.Rectangle(32, 128, 16, 16),
                    1 => new Microsoft.Xna.Framework.Rectangle(0, 128, 16, 16),
                    2 => new Microsoft.Xna.Framework.Rectangle(16, 128, 16, 16),
                    _ => new Microsoft.Xna.Framework.Rectangle(0, 96, 16, 32),
                };
                spriteBatch.Draw(this.texture.Value, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * 64f + 32f, tileLocation.Y * 64f - (float)(sourceRect.Height * 4 - 64) + (float)(((int)this.growthStage >= 3) ? 128 : 64))), sourceRect, this.fertilized ? Color.HotPink : Color.White, this.shakeRotation, new Vector2(8f, ((int)this.growthStage >= 3) ? 32 : 16), 4f, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, ((int)this.growthStage == 0) ? 0.0001f : (baseSortPosition / 10000f));
            }
            else
            {
                if (!this.stump || (bool)this.falling)
                {
                    spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * 64f - 51f, tileLocation.Y * 64f - 16f)), Tree.shadowSourceRect, Color.White * ((float)Math.PI / 2f - Math.Abs(this.shakeRotation)), 0f, Vector2.Zero, 4f, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1E-06f);
                    Microsoft.Xna.Framework.Rectangle source_rect;
                    source_rect = Tree.treeTopSourceRect;
                    if ((data.UseAlternateSpriteWhenSeedReady && this.hasSeed.Value) || (data.UseAlternateSpriteWhenNotShaken && !this.wasShakenToday.Value))
                    {
                        source_rect.X = 48;
                    }
                    else
                    {
                        source_rect.X = 0;
                    }
                    spriteBatch.Draw(this.texture.Value, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * 64f + 32f, tileLocation.Y * 64f + 64f)), source_rect, Color.White * this.alpha, this.shakeRotation, new Vector2(24f, 96f), 4f, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (baseSortPosition + 2f) / 10000f - tileLocation.X / 1000000f);
                }
                if (this.health.Value >= 1f || (!this.falling && this.health.Value > -99f))
                {
                    spriteBatch.Draw(this.texture.Value, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * 64f + ((this.shakeTimer > 0f) ? ((float)Math.Sin(Math.PI * 2.0 / (double)this.shakeTimer) * 3f) : 0f), tileLocation.Y * 64f - 64f)), Tree.stumpSourceRect, Color.White * this.alpha, 0f, Vector2.Zero, 4f, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, baseSortPosition / 10000f);
                }
                if ((bool)this.stump && this.health.Value < 4f && this.health.Value > -99f)
                {
                    spriteBatch.Draw(this.texture.Value, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * 64f + ((this.shakeTimer > 0f) ? ((float)Math.Sin(Math.PI * 2.0 / (double)this.shakeTimer) * 3f) : 0f), tileLocation.Y * 64f)), new Microsoft.Xna.Framework.Rectangle(Math.Min(2, (int)(3f - this.health.Value)) * 16, 144, 16, 16), Color.White * this.alpha, 0f, Vector2.Zero, 4f, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (baseSortPosition + 1f) / 10000f);
                }
            }
            foreach (Leaf i in this.leaves)
            {
                spriteBatch.Draw(this.texture.Value, Game1.GlobalToLocal(Game1.viewport, i.position), new Microsoft.Xna.Framework.Rectangle(16 + i.type % 2 * 8, 112 + i.type / 2 * 8, 8, 8), Color.White, i.rotation, Vector2.Zero, 4f, SpriteEffects.None, baseSortPosition / 10000f + 0.01f);
            }
        }




    }
}

