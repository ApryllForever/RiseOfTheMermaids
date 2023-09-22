using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace RestStopLocations
{
    public class CustomTokens
    {
        internal static IModHelper Helper;
        private readonly IManifest ModManifest;
       

        public CustomTokens(IMod mod)
        {
            Helper = mod.Helper;
            ModManifest = mod.ModManifest;
        }

        public void RegisterTokens()
        {
            /*
            var cp = ExternalAPIs.CP;

            cp.RegisterToken(this.ModManifest, "QuestCompleted", () =>
            {
               
                if (SaveGame.loaded?.player != null || Context.IsWorldReady)
                {
                    var CompleteQuest = Game1.player.checkForQuestComplete(Game1.player,);
                    if (CompleteQuest != null)
                    {
                       
                    }
                }
               
                return null;
            });

            

            cp.RegisterToken(this.ModManifest, "FoxbloomDay", () => {
                int? randomseed = (int?)(Game1.stats?.daysPlayed ?? SaveGame.loaded?.stats?.daysPlayed);
                if (randomseed is not null)
                {   //Seed the random with a seed that only changes every 28 days
                    Random random = new Random((int)Game1.uniqueIDForThisGame + ((randomseed.Value - 1) / 28));
                    FoxbloomDay = random.Next(1, 5) * 7;
                    return new[] { FoxbloomDay.ToString() };
                }
                return null; //return null for an unready token.
            });

            */

           
        }



       

    }
}