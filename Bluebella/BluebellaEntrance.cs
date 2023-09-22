﻿using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using xTile.Dimensions;
using StardewValley.Locations;
using RestStopLocations.Game.Locations;

namespace RestStopLocations.Game.Locations
{
    [XmlType("Mods_ApryllForever_RestStopLocations_BluebellaAnteroom")]
    public class BluebellaAnteroom : HellLocation
    {

        public BluebellaAnteroom() { }
        public BluebellaAnteroom(IModContentHelper content)
        : base(content, "BluebellaAnteroom", "BluebellaAnteroom")
        {
        }

        static string EnterDungeon = "Do you wish to enter this forboding gate?";
        protected override void resetLocalState()
        {
            base.resetLocalState();

            if (Game1.player.Tile.Y == 1)
            {
                Game1.player.Position = new Vector2(25.5f * Game1.tileSize, 19 * Game1.tileSize);
            }
        }

        public override bool performAction(string action, Farmer who, Location tileLocation)
        {
            if (action == "RSBluebella")
            {
                createQuestionDialogue(EnterDungeon, createYesNoResponses(), "HellDungeonEntrance");
            }
           
            return base.performAction(action, who, tileLocation);
        }

        public override bool answerDialogue(Response answer)
        {
            if (lastQuestionKey != null && afterQuestion == null)
            {
                string qa = lastQuestionKey.Split(' ')[0] + "_" + answer.responseKey;
                switch (qa)
                {
                    case "HellDungeonEntrance_Yes":
                        performTouchAction("MagicWarp " + "BluebellaDungeon0 31 50", Game1.player.Tile);
                        return true;
                }
            }

            return base.answerDialogue(answer);
        }
    }
}