/*

using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using System;


namespace RestStopLocations
{
    
    public class OksanaResponse : Mod
    {
        internal static IMonitor ModMonitor { get; private set; }
        internal static IReflectionHelper Reflection { get; private set; }
       
        public override void Entry(IModHelper helper)
        {
            Reflection = helper.Reflection;
         

            var harmony = new Harmony(this.ModManifest.UniqueID);

            harmony.Patch(
                original: AccessTools.Method(typeof(NPC), nameof(NPC.receiveGift)),
                postfix: new HarmonyMethod(this.GetType(), nameof(PatchAfterGiftGiven))
            );

        }

        private static void PatchAfterGiftGiven(NPC __instance, StardewValley.Object o)
        {
            try
            {
                var Oksie = Game1.getCharacterFromName("Oksana");

                if (__instance == Oksie)
                {
                    if (o.ParentSheetIndex == 342)
                    {
                        CancelCurrentDialogue(__instance);
                        Game1.drawDialogue(__instance, "Do I look pregnant to you? Are you fat shaming me?$3#$b#Never mind!!! Fuck You, take this jar up your ass!!!$5");
                    }
                    else if (o.Category == StardewValley.Object.flowersCategory)
                        CancelCurrentDialogue(__instance);
                        Game1.drawDialogue(__instance, "Awwwww... I love flowers so so so much!!!$8#$b#Here's a kiss, to say  thank you in the most mermaid way possible!$11");
                } 
            }
            catch (Exception ex)
            {
                ModMonitor.Log($"An error happened in the Oksie Pickle Rage Patch: {ex.Message}", LogLevel.Error);
                ModMonitor.Log(ex.ToString());
            }
        }


        public static void CancelCurrentDialogue(NPC speaker)
        {
            if (Game1.activeClickableMenu is DialogueBox dialogueBox && Game1.currentSpeaker == speaker)
            {
                
                if (speaker.CurrentDialogue.Count > 0
                    && GetDialogueBoxDialogue(dialogueBox) == speaker.CurrentDialogue.Peek())
                {
                    speaker.CurrentDialogue.Pop();
                }

                Game1.dialogueUp = false;
                Game1.currentSpeaker = null;
                Game1.activeClickableMenu = null;
            }
        }

        private static Dialogue GetDialogueBoxDialogue(DialogueBox dialogueBox)
        {
            return OksanaResponse.Reflection
                .GetField<Dialogue>(dialogueBox, "characterDialogue")
                .GetValue();
        }

    }
}
*/