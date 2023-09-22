using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using xTile.Dimensions;
using StardewValley.Locations;
using RestStopLocations.Game.Locations;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardewValley.Network;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using xTile;
using xTile.Layers;
using xTile.ObjectModel;
using xTile.Tiles;
using SObject = StardewValley.Object;

namespace RestStopLocations.Game.Locations
{
    [XmlType("Mods_ApryllForever_RestStopLocations_MermaidIslandBlacksmith")]
    public class MermaidIslandBlacksmith : HellLocation
    {

		static string ProcessGeodes = "Do you wish to process geodes, beautiful adventurer?";

		public string potraitPersonDialogue;
		public NPC portraitPerson;

		public MermaidIslandBlacksmith() { }
        public MermaidIslandBlacksmith(IModContentHelper content)
        : base(content, "MermaidIslandBlacksmith", "MermaidIslandBlacksmith")
        {
        }

		public override bool performAction(string action, Farmer who, Location tileLocation)
		{
			if (action == "Mermaid")
			{

				Game1.activeClickableMenu = new IslandGeodeMenu();


			}

			if (action == "MermaidBS")
			{

				createQuestionDialogue(ProcessGeodes, createYesNoResponses(), "GeodeBusting");



			}
			if (action == "RS.WeaponStore")
			{
				
				{
					/*
					string shopWords = "Hey Lovely! Buy my stuff! You will feel better about yourself!!!";
					portraitPerson = Game1.getCharacterFromName("MermaidLilac");


					Random r = new Random((int)(Game1.stats.DaysPlayed + 898 + Game1.uniqueIDForThisGame));
					Dictionary<ISalable, int[]> stock = new Dictionary<ISalable, int[]>();
					stock.Add(new Boots(853), new int[4]
					{
						0,
						2147483647,
						848,
						100
					});
					Utility.AddStock(stock, new SObject(Vector2.Zero, 286, int.MaxValue), 25);
					Utility.AddStock(stock, new SObject(Vector2.Zero, 287, int.MaxValue), 100);
					Utility.AddStock(stock, new SObject(Vector2.Zero, 288, int.MaxValue), 200);
					Utility.AddStock(stock, new SObject(Vector2.Zero, 773, int.MaxValue), 150);
					Utility.AddStock(stock, new SObject(Vector2.Zero, 265, int.MaxValue), 100);
					Utility.AddStock(stock, new SObject(Vector2.Zero, 886, int.MaxValue), 1000);
					Utility.AddStock(stock, new SObject(Vector2.Zero, 486, int.MaxValue), 100);
					Utility.AddStock(stock, new Clothing(1297), 100);
					if (r.NextDouble() < 0.5)
					{
						Utility.AddStock(stock, new SObject(Vector2.Zero, 909, 30), 2000);
					}
					else
					{
						Utility.AddStock(stock, new SObject(Vector2.Zero, 499, 1), 3000);
					}
					if (r.NextDouble() < 0.25)
					{
						Utility.AddStock(stock, new Clothing(1028), 100);
					}


					Game1.activeClickableMenu = new ShopMenu(stock, 0, "BluebellaElfShop", null, null, "MermaidLilac")
					{
						portraitPerson = Game1.getCharacterFromName("MermaidLilac")
					};
					potraitPersonDialogue = Game1.parseText(shopWords, Game1.dialogueFont, 304);
					portraitPerson = Game1.objectDialoguePortraitPerson;


					*/
				}
					

				return true;



			}
			return base.performAction(action, who, tileLocation);
		}


        protected override void resetLocalState()
        {
            base.resetLocalState();

            
        }

     

        public override bool answerDialogue(Response answer)
        {
            if (lastQuestionKey != null && afterQuestion == null)
            {
                string qa = lastQuestionKey.Split(' ')[0] + "_" + answer.responseKey;
                switch (qa)
                {		
					case "GeodeBusting_Yes":
						Game1.activeClickableMenu = new IslandGeodeMenu();
						return true;

				}
            }

            return base.answerDialogue(answer);
        }
    }
}