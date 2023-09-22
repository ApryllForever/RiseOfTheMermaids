/*

using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using StardewModdingAPI.Events;
using Microsoft.Xna.Framework;
using StardewValley.Menus;
using SpaceCore.Events;

namespace RestStopLocations.Quests
{
    internal static class SpecialOrders
    {
        static IModHelper Helper;
        static IMonitor Monitor;
        internal static void Initialize(IMod ModInstance)
        {
            Helper = ModInstance.Helper;
            Monitor = ModInstance.Monitor;

            //Helper.Events.GameLoop.DayStarted += OnDayStarted;
            Helper.Events.GameLoop.OneSecondUpdateTicked += OnUpdate;
            SpaceEvents.OnEventFinished += OnEventFinished;
        }

        private static void OnUpdate(object sender, OneSecondUpdateTickedEventArgs e)
        {

            //Specific code to give message to player upon finishing crystals quest
            if (Game1.player.team.completedSpecialOrders.ContainsKey(MermaidConstants.SO_RedCRYSTAL) && Game1.player.team.completedSpecialOrders.ContainsKey(MermaidConstants.SO_OrangeCRYSTAL) && Game1.player.team.completedSpecialOrders.ContainsKey(MermaidConstants.SO_YellowCRYSTAL) && Game1.player.team.completedSpecialOrders.ContainsKey(MermaidConstants.SO_GreenCRYSTAL) && Game1.player.team.completedSpecialOrders.ContainsKey(MermaidConstants.SO_BlueCRYSTAL) && Game1.player.team.completedSpecialOrders.ContainsKey(MermaidConstants.SO_PurpleCRYSTAL) && Game1.player.team.completedSpecialOrders.ContainsKey(MermaidConstants.SO_GrayCRYSTAL) && Game1.player.mailReceived.Contains(MermaidConstants.M_CRYSTALS))
            {
                Game1.player.mailReceived.Remove(MermaidConstants.M_CRYSTALS);
                Game1.playSound("healSound");
                Game1.activeClickableMenu = new DialogueBox(Helper.Translation.Get("CompleteCrystalsNotif"));
            }
        }

        static void OnEventFinished(object sender, EventArgs e)
        {
            if (!Game1.player.IsMainPlayer)
                return;

            switch (Game1.CurrentEvent.id)
            {
                

                case MermaidConstants.E_MEETDAIA:
                    Game1.player.team.specialOrders.Add(SpecialOrder.GetSpecialOrder(MermaidConstants.SO_DAIAQUEST, null));
                    break;

                case MermaidConstants.E_SPIRITGH2:
                    Game1.player.team.specialOrders.Add(SpecialOrder.GetSpecialOrder(MermaidConstants.SO_HAUNTEDGH, null));
                    break;

                case MermaidConstants.E_PREUNSEAL:
                    Game1.player.team.specialOrders.Add(SpecialOrder.GetSpecialOrder(MermaidConstants.SO_RedCRYSTAL, null));
                    Game1.player.team.specialOrders.Add(SpecialOrder.GetSpecialOrder(MermaidConstants.SO_OrangeCRYSTAL, null));
                    Game1.player.team.specialOrders.Add(SpecialOrder.GetSpecialOrder(MermaidConstants.SO_YellowCRYSTAL, null));
                    Game1.player.team.specialOrders.Add(SpecialOrder.GetSpecialOrder(MermaidConstants.SO_GreenCRYSTAL, null));
                    Game1.player.team.specialOrders.Add(SpecialOrder.GetSpecialOrder(MermaidConstants.SO_BlueCRYSTAL, null));
                    Game1.player.team.specialOrders.Add(SpecialOrder.GetSpecialOrder(MermaidConstants.SO_PurpleCRYSTAL, null));
                    Game1.player.team.specialOrders.Add(SpecialOrder.GetSpecialOrder(MermaidConstants.SO_GrayCRYSTAL, null));
                    Game1.player.mailReceived.Add(MermaidConstants.M_CRYSTALS);
                    break;
            }
        }
    }
}


*/