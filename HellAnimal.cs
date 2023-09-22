/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

using Microsoft.Xna.Framework;

using Netcode;

using StardewValley;
using StardewValley.Buildings;
using StardewValley.Objects;


namespace RestStopLocations
{
   public enum HellAnimalType
    {
        WhiteCow,
        BrownCow,
        VoidChicken,
        BrownChicken,
        WhiteChicken,
        Duck,
        Goat,
        Sheep,
        Pig,
        Rabbit,
        Dinosaur,
        Ostrich
    }

    [XmlType("Mods_ApryllForever_RestStopLocations_HellAnimal")]
    public class HellAnimal : FarmAnimal
    {
        public readonly NetEnum<HellAnimalType> hellType = new();

        public static string GetVanillaTypeFromHellType(HellAnimalType type)
        {
            switch (type)
            {
                case HellAnimalType.WhiteCow:
                    return "White Cow";
                case HellAnimalType.BrownCow:
                    return "Brown Cow";
                case HellAnimalType.VoidChicken:
                    return "Void Chicken";
                case HellAnimalType.BrownChicken:
                    return "Brown Chicken";
                case HellAnimalType.WhiteChicken:
                    return "White Chicken";
                case HellAnimalType.Duck:
                    return "Duck";
                case HellAnimalType.Goat:
                    return "Goat";
                case HellAnimalType.Sheep:
                    return "Sheep";
                case HellAnimalType.Pig:
                    return "Pig";
                case HellAnimalType.Rabbit:
                    return "Rabbit";
                case HellAnimalType.Dinosaur:
                    return "Dinosaur";
                case HellAnimalType.Ostrich:
                    return "Ostrich";
            }

            throw new ArgumentException("Invalid HellAnimal type");
        }

        public HellAnimal() { }
        public HellAnimal(HellAnimalType type, Vector2 pos, long id)
        : base(GetVanillaTypeFromHellType(type), id, 0)
        {
            hellType.Value = type;
            position.Value = pos;
            age.Value = ageWhenMature.Value;

            switch (type)
            {
                case HellAnimalType.WhiteCow:
                    displayType = "White Cow";
                    break;
                case HellAnimalType.BrownCow:
                    displayType = "Brown Cow";
                    break;
                case HellAnimalType.VoidChicken:
                    displayType = "Void Chicken";
                    break;
                case HellAnimalType.BrownChicken:
                    displayType = "Brown Chicken";
                    break;
                case HellAnimalType.WhiteChicken:
                    displayType = "White Chicken";
                    break;
                case HellAnimalType.Duck:
                    displayType = "Duck";
                    break;
                case HellAnimalType.Goat:
                    displayType = "Goat";
                    break;
                case HellAnimalType.Sheep:
                    displayType = "Sheep";
                    break;
                case HellAnimalType.Pig:
                    displayType = "Pig ";
                    break;
                case HellAnimalType.Rabbit:
                    displayType = "Rabbit";
                    break;
                case HellAnimalType.Dinosaur:
                    displayType = "Dinosaur";
                    break;
                case HellAnimalType.Ostrich:
                    displayType = "Ostrich";
                    break;
            }
            reloadData();
        }

        protected override void initNetFields()
        {
            base.initNetFields();
            NetFields.AddFields(hellType);
        }

        public override void reloadData()
        {
            base.reloadData();
            /*switch (hellType.Value)
            {
                case HellAnimalType.WhiteCow:
                    Sprite = new AnimatedSprite(Mod.instance.Helper.ModContent.GetInternalAssetName("assets/animals/White Cow.png").BaseName, 0, 32, 32);
                    break;
                case HellAnimalType.VoidChicken:
                    Sprite = new AnimatedSprite(Mod.instance.Helper.ModContent.GetInternalAssetName("assets/animals/Void Chicken.png").BaseName, 0, 16, 16);
                    break;
            }*/

/*
            fullnessDrain.Value *= 0;
            happinessDrain.Value *= 0;
        }

        public override bool CanHavePregnancy()
        {
            return hellType.Value == HellAnimalType.BrownCow;

        }

        public override bool updateWhenCurrentLocation(GameTime time, GameLocation location)
        {
            //SpaceShared.Log.Debug( displayName + " full " + fullness.Value + " " + FarmAnimal.NumPathfindingThisTick );
            if (!Game1.IsClient)
            {
                // Eat more aggressively since they can't go home to eat
                if (!this.isSwimming.Value && location.IsOutdoors && this.fullness.Value < 195 && Game1.random.NextDouble() < 0.1 && FarmAnimal.NumPathfindingThisTick < FarmAnimal.MaxPathfindingPerTick)
                {
                    FarmAnimal.NumPathfindingThisTick++;
                    base.controller = new PathFindController(this, location, grassEndPointFunction, -1, eraseOldPathController: false, behaviorAfterFindingGrassPatch, 200, Point.Zero);
                    this._followTarget = null;
                    this._followTargetPosition = null;
                }
            }
            return base.updateWhenCurrentLocation(time, location);
        }

        public override void updateWhenNotCurrentLocation(Building currentBuilding, GameTime time, GameLocation environment)
        {
            base.updateWhenNotCurrentLocation(currentBuilding, time, environment);
            //SpaceShared.Log.Debug( displayName + " full " + fullness.Value+" "+FarmAnimal.NumPathfindingThisTick );
            if (!Game1.IsClient)
            {
                // Eat more aggressively since they can't go home to eat
                if (!this.isSwimming.Value && environment.IsOutdoors && this.fullness.Value < 195 && Game1.random.NextDouble() < 0.1 && FarmAnimal.NumPathfindingThisTick < FarmAnimal.MaxPathfindingPerTick)
                {
                    FarmAnimal.NumPathfindingThisTick++;
                    base.controller = new PathFindController(this, environment, grassEndPointFunction, -1, eraseOldPathController: false, behaviorAfterFindingGrassPatch, 200, Point.Zero);
                    this._followTarget = null;
                    this._followTargetPosition = null;
                }
            }
        }

        public void ActualDayUpdate(GameLocation loc)
        {
            if (this.daysOwned.Value < 0)
            {
                this.daysOwned.Value = this.age.Value;
            }
            this.StopAllActions();
            this.health.Value = 3;
            this.daysSinceLastLay.Value++;
            if (!this.wasPet.Value && !this.wasAutoPet.Value)
            {
                this.friendshipTowardFarmer.Value = Math.Max(0, (int)this.friendshipTowardFarmer.Value - (10 - (int)this.friendshipTowardFarmer.Value / 200));
                this.happiness.Value = (byte)Math.Max(0, this.happiness.Value - this.happinessDrain.Value * 5);
            }
            this.wasPet.Value = false;
            this.wasAutoPet.Value = false;
            this.daysOwned.Value++;
            Random r = new Random((int)this.myID.Value / 2 + (int)Game1.stats.DaysPlayed);
            if (this.fullness.Value > 200 || r.NextDouble() < (double)(this.fullness.Value - 30) / 170.0)
            {
                this.age.Value++;
                this.happiness.Value = (byte)Math.Min(255, this.happiness.Value + this.happinessDrain.Value * 2);
            }
            if (this.fullness.Value < 200)
            {
                this.happiness.Value = (byte)Math.Max(0, this.happiness.Value - 100);
                this.friendshipTowardFarmer.Value = Math.Max(0, this.friendshipTowardFarmer.Value - 20);
            }
            bool produceToday = this.daysSinceLastLay.Value >= this.daysToLay.Value - ((this.type.Value.Equals("Sheep") && Game1.getFarmer(this.ownerID.Value).professions.Contains(3)) ? 1 : 0) && r.NextDouble() < (double)(int)this.fullness.Value / 200.0 && r.NextDouble() < (double)(int)this.happiness.Value / 70.0;
            int whichProduce;
            if (!produceToday || this.age.Value < this.ageWhenMature.Value)
            {
                whichProduce = -1;
            }
            else
            {
                whichProduce = this.defaultProduceIndex.Value;
                if (r.NextDouble() < this.happiness.Value / 150.0)
                {
                    float happinessModifier = ((this.happiness.Value > 200) ? ((float)(int)this.happiness.Value * 1.5f) : ((float)((this.happiness.Value <= 100) ? (this.happiness.Value - 100) : 0)));
                    this.daysSinceLastLay.Value = 0;
                    Game1.stats.ChickenEggsLayed++;
                    double chanceForQuality = this.friendshipTowardFarmer.Value / 1000f - (1f - this.happiness.Value / 225f);
                    if ((!this.isCoopDweller() && Game1.getFarmer(this.ownerID.Value).professions.Contains(3)) || (this.isCoopDweller() && Game1.getFarmer(this.ownerID.Value).professions.Contains(2)))
                    {
                        chanceForQuality += 0.33;
                    }
                    if (chanceForQuality >= 0.95 && r.NextDouble() < chanceForQuality / 2.0)
                    {
                        this.produceQuality.Value = 4;
                    }
                    else if (r.NextDouble() < chanceForQuality / 2.0)
                    {
                        this.produceQuality.Value = 2;
                    }
                    else if (r.NextDouble() < chanceForQuality)
                    {
                        this.produceQuality.Value = 1;
                    }
                    else
                    {
                        this.produceQuality.Value = 0;
                    }
                }
            }
            if (this.harvestType.Value == 1 && produceToday)
            {
                this.currentProduce.Value = whichProduce;
                whichProduce = -1;
            }
            if (whichProduce != -1  )
            {
                bool spawn_object = true;
               
                
            }
            //if ( !wasLeftOutLastNight )
            {
                if (this.fullness.Value < 30)
                {
                    this.moodMessage.Value = 4;
                }
                else if (this.happiness.Value < 30)
                {
                    this.moodMessage.Value = 3;
                }
                else if (this.happiness.Value < 200)
                {
                    this.moodMessage.Value = 2;
                }
                else
                {
                    this.moodMessage.Value = 1;
                }
            }
            if (Game1.timeOfDay < 1700)
            {
                this.fullness.Value = (byte)Math.Max(0, this.fullness.Value - this.fullnessDrain.Value * (1700 - Game1.timeOfDay) / 100);
            }
            this.fullness.Value = 0;
            if (Utility.isFestivalDay(Game1.dayOfMonth, Game1.currentSeason))
            {
                this.fullness.Value = 250;
            }
           
        }
    }
} */