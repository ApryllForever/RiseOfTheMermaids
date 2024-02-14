using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using SpaceCore.Events;
using SpaceCore.Interface;
using SpaceShared;
using SpaceShared.APIs;
using StardewModdingAPI;
using StardewModdingAPI.Enums;
using StardewModdingAPI.Events;
using StardewValley;
using xTile;
using xTile.Dimensions;
using xTile.Layers;
using xTile.ObjectModel;
using xTile.Tiles;
using Object = StardewValley.Object;
using RestStopLocations.Game.Locations;
using RestStopLocations.Bluebella;
using System.Reflection;
using RestStopLocations.Game.Locations.Sapphire;
using RestStopLocations.Utilities;
using System.Threading;
using UtilitiesStuff;
using static StardewValley.Minigames.CraneGame;
using StardewValley.Menus;
using RestStopLocations.VirtualProperties;
using RestStopLocations.Patches;
//using RestStopLocations.Quests;


namespace RestStopLocations
{
    public class Mod : StardewModdingAPI.Mod
    {
        public static Mod instance;

        internal static IMonitor ModMonitor { get; set; }

        internal static IModHelper ModHelper { get; set; }

        public static List<Vector2> coolinatedLava = new List<Vector2>();

        //IMonitor monitor;

        private static Multiplayer multiplayer;

        public MermaidTrain MermaidTrain;
       
        //public static Texture2D mermaidCursor = Mod.instance.Helper.ModContent.Load<Texture2D>("Assets/M.png");

        public override void Entry(IModHelper helper)
        {

            instance = this;
            Assets.Load(helper.ModContent);
            Helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            Helper.Events.GameLoop.TimeChanged += OnTimeChanged;
            Helper.Events.GameLoop.DayStarted += OnDayStarted;
            Helper.Events.Specialized.LoadStageChanged += OnLoadStageChanged;
            Helper.Events.GameLoop.ReturnedToTitle += OnReturnedToTitle;
            Helper.Events.Player.Warped += OnWarped;
            Helper.Events.Content.AssetRequested += OnAssetRequested;
            Helper.Events.Input.ButtonPressed += OnButtonPressed;
            Helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            Helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
            Helper.Events.GameLoop.DayEnding += OnDayEnding;
            Helper.ConsoleCommands.Add("Mermaid_Enlightenment", "Gives you the Enlightenment.", OnKeyCommand);

            SpaceEvents.OnEventFinished += OnEventFinished;
            SpaceEvents.AfterGiftGiven += OnGiftGiven;
            SpaceEvents.BeforeGiftGiven += AforeGiftGiven;

            FireflySpawner.Enable(helper, Monitor);
            Items.Enable(helper, Monitor);

            //QuestController.Initialize(this);
            AmbientishLocationSounds.Initialize();
            AmbientesqueLocationSounds.InitShared();

            BluebellaDungeon.Monitor = this.Monitor;
            HellDungeon.Monitor = this.Monitor;
            SapphireSprings.Monitor = this.Monitor;
            BigTree.Monitor = this.Monitor;
         

            //SoundEffect mainMusic = SoundEffect.FromFile(Path.Combine(Helper.DirectoryPath, "assets", "SongLost.wav"));
            //Game1.soundBank.AddCue(new CueDefinition("SongLost", mainMusic, 2, loop: true));
            CueDefinition myCueDefinition = new CueDefinition();
            myCueDefinition.name = "SongLost";
            myCueDefinition.instanceLimit = 1;
            myCueDefinition.limitBehavior = CueDefinition.LimitBehavior.ReplaceOldest;
            SoundEffect audio;
            string filePathCombined = Path.Combine(this.Helper.DirectoryPath, "assets", "SongLost.wav");
            using (var stream = new System.IO.FileStream(filePathCombined, System.IO.FileMode.Open))
            {
                audio = SoundEffect.FromStream(stream);
            }
            myCueDefinition.SetSound(audio, Game1.audioEngine.GetCategoryIndex("Music"), true);

            var harmony = new Harmony(ModManifest.UniqueID);

            harmony.PatchAll();
            //harmony.Patch(AccessTools.Method("StardewModdingAPI.Framework.SGame:DrawImpl"), transpiler: new HarmonyMethod(typeof(Patches.Game1CatchLightingRenderPatch).GetMethod("Transpiler")));

            var Game1_multiplayer = this.Helper.Reflection.GetField<Multiplayer>(typeof(Game1), "multiplayer").GetValue();
            multiplayer = Game1_multiplayer;

            harmony.Patch(
             original: AccessTools.Method(typeof(Game1), nameof(Game1.getCharacterFromName), new Type[] { typeof(string), typeof(bool) }),
             prefix: new HarmonyMethod(typeof(SequoiaPatches), nameof(SequoiaPatches.getCharacterFromName_Prefix))
            );

            //Code Esca allowed me to use, 7/19/23 Discord Moddding Server
            // HarmonyPatch_BedPlacement.ApplyPatch(harmony, Monitor);
        }

      

        private void OnAssetRequested(object? sender, AssetRequestedEventArgs e)
        {
            /*
             if (e.Name.IsEquivalentTo("Assets/Maps/SovaraCanyon"))
        {
            e.Edit(asset =>
            {
                IAssetDataForMap editor = asset.AsMap();
                Map map = editor.Data;

               
            });
        }*/

        }
            //=> Assets.ApplyEdits(e);

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            Mod.ModHelper = Helper;
            Mod.ModMonitor = Monitor;

            UnderworldField.Initialize(instance,Helper, Monitor);

            //TileActionHandler.Initialize(Helper);

            AmbientishLocationSounds.Initialize();
            AmbientesqueLocationSounds.InitShared();


            //Bluebella.Initialize(Helper);
            //ExternalAPIs.Initialize(Helper);
            var sc = Helper.ModRegistry.GetApi<ISpaceCoreApi>("spacechase0.SpaceCore");
            var JA = Helper.ModRegistry.GetApi<IJsonAssetsApi>("spacechase0.JsonAssets");
            sc.RegisterSerializerType(typeof(HellLocation));
            sc.RegisterSerializerType(typeof(SapphireLocation));
            sc.RegisterSerializerType(typeof(RestStopLocation));
            sc.RegisterSerializerType(typeof(AspenLocation));
            sc.RegisterSerializerType(typeof(EmeraldLocation));
            sc.RegisterSerializerType(typeof(DungeonEntrance));
            sc.RegisterSerializerType(typeof(SapphireSprings));
            sc.RegisterSerializerType(typeof(SapphireForest));
            sc.RegisterSerializerType(typeof(SapphireSchoolHouse));
            sc.RegisterSerializerType(typeof(SapphireVolcano));
            sc.RegisterSerializerType(typeof(HellDungeon));
            sc.RegisterSerializerType(typeof(SaltyTail));
            sc.RegisterSerializerType(typeof(BluebellaDungeon));
            sc.RegisterSerializerType(typeof(BluebellaAnteroom));
            sc.RegisterSerializerType(typeof(MoranaHome));
            sc.RegisterSerializerType(typeof(MermaidIslandBlacksmith));
            sc.RegisterSerializerType(typeof(SapphireHospital));
            sc.RegisterSerializerType(typeof(SapphireMarket));
            sc.RegisterSerializerType(typeof(RestStop));
            sc.RegisterSerializerType(typeof(SouthRestStop));
            sc.RegisterSerializerType(typeof(AspenCanyon));
            sc.RegisterSerializerType(typeof(AspenMountainTop));
            sc.RegisterSerializerType(typeof(JunkBeach));
            sc.RegisterSerializerType(typeof(SapphireSubway));
            sc.RegisterSerializerType(typeof(SapphireFlorist));
            //sc.RegisterSerializerType(typeof(ATrain));
            sc.RegisterSerializerType(typeof(MermaidTrain));
            sc.RegisterSerializerType(typeof(SapphireFarmHouse));
            sc.RegisterSerializerType(typeof(ForestMermaidHouse));
            sc.RegisterSerializerType(typeof(UnderworldField));
            sc.RegisterSerializerType(typeof(SovaraCanyon));
            sc.RegisterSerializerType(typeof(BeyondDark));
            sc.RegisterSerializerType(typeof(RealmofSpiritsWinter));
            sc.RegisterSerializerType(typeof(EmeraldForestShrine));
            sc.RegisterSerializerType(typeof(BigTree));
            sc.RegisterSerializerType(typeof(Sequoia));

            sc.RegisterSerializerType(typeof(PearlEanchantment));
            //sc.RegisterSerializerType(typeof(BugRing));

            SpaceEvents.AddWalletItems += AddWalletItems;
            sc.RegisterCustomProperty(typeof(FarmerTeam), "hasEnlightenment", typeof(NetBool), AccessTools.Method(typeof(FarmerTeam_Enlightenment), nameof(FarmerTeam_Enlightenment.hasEnlightenment)), AccessTools.Method(typeof(FarmerTeam_Enlightenment), nameof(FarmerTeam_Enlightenment.set_hasEnlightenment)));

        }

        private void OnKeyCommand(string cmd, string[] args)
        {
            Game1.player.team.hasEnlightenment().Value = true;
        }



        private void OnTimeChanged(object sender, TimeChangedEventArgs e)
        {
            HellDungeon.UpdateLevels10Minutes(e.NewTime);
            BluebellaDungeon.UpdateLevels10Minutes(e.NewTime);
        }

        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            HellDungeon.ClearAllLevels();
            BluebellaDungeon.ClearAllLevels();
            // SouthRestStop.plantGenerating();
        }

        private void OnLoadStageChanged(object sender, LoadStageChangedEventArgs e)
        {
            if (e.NewStage == LoadStage.CreatedInitialLocations || e.NewStage == LoadStage.SaveAddedLocations)
            {
                Game1.locations.Add(new DungeonEntrance(Helper.ModContent));
                Game1.locations.Add(new BluebellaAnteroom(Helper.ModContent));
                Game1.locations.Add(new MoranaHome(Helper.ModContent));
                Game1.locations.Add(new SapphireSprings(Helper.ModContent));
                Game1.locations.Add(new SapphireSchoolHouse(Helper.ModContent));
                Game1.locations.Add(new SapphireForest(Helper.ModContent));
                Game1.locations.Add(new SaltyTail(Helper.ModContent));
                Game1.locations.Add(new MermaidIslandBlacksmith(Helper.ModContent));
                Game1.locations.Add(new SapphireHospital(Helper.ModContent));
                Game1.locations.Add(new SapphireMarket(Helper.ModContent));
                Game1.locations.Add(new RestStop(Helper.ModContent));
                Game1.locations.Add(new SouthRestStop(Helper.ModContent));
                Game1.locations.Add(new AspenCanyon(Helper.ModContent));
                Game1.locations.Add(new AspenMountainTop(Helper.ModContent));
                Game1.locations.Add(new JunkBeach(Helper.ModContent));
                Game1.locations.Add(new SapphireSubway(Helper.ModContent));
                Game1.locations.Add(new SapphireFlorist(Helper.ModContent));
                Game1.locations.Add(new SapphireFarmHouse(Helper.ModContent));
                Game1.locations.Add(new ForestMermaidHouse(Helper.ModContent));
                Game1.locations.Add(new UnderworldField(Helper.ModContent));
                Game1.locations.Add(new SovaraCanyon(Helper.ModContent));
                Game1.locations.Add(new BeyondDark(Helper.ModContent));
                Game1.locations.Add(new RealmofSpiritsWinter(Helper.ModContent));
                Game1.locations.Add(new EmeraldForestShrine(Helper.ModContent));
            }
        }

        private void OnDayEnding(object sender, DayEndingEventArgs e)
        {
            {
                Helper.GameContent.InvalidateCache("Maps\\Custom_UnderworldField");
                //UnderworldField.LavaTileClear();

                foreach (Vector2 tile in coolinatedLava)
                {
                    int tileX = new int();
                    int tileY = new int();

                    var butt = Game1.game1.xTileContent.Load<Map>(Mod.instance.Helper.ModContent.GetInternalAssetName("assets/maps/UnderworldField.tmx").BaseName);


                    Layer layer = butt.GetLayer("Buildings");
                    layer.Tiles[tileX, tileY] = new StaticTile(layer: layer, tileSheet: butt.GetTileSheet("z_UnderworldTile"), tileIndex: 1193, blendMode: BlendMode.Alpha);
                }

                coolinatedLava.Clear();
            }

            
        }
            private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;
        }


        private void OnReturnedToTitle(object sender, ReturnedToTitleEventArgs e)
        {
            HellDungeon.ClearAllLevels();
            BluebellaDungeon.ClearAllLevels();
            SapphireVolcano.ClearAllLevels();

            AmbientesqueLocationSounds.onLocationLeave();
        }

        private void OnWarped(object sender, WarpedEventArgs e)
        {
            Vector2 tileLocation = new Vector2(113, 7);
            Vector2 tileLocation2 = new Vector2(13, 7);
            if (e.OldLocation is SapphireForest )
                    AmbientishLocationSounds.removeSound(tileLocation);
                    AmbientesqueLocationSounds.removeSound(tileLocation2);

            if (e.NewLocation is SapphireForest)
                AmbientishLocationSounds.addSound(tileLocation, 2);
                AmbientesqueLocationSounds.addSound(tileLocation2, 2);


            // if (e.OldLocation is HellLocation || e.NewLocation is HellLocation)
            //   Helper.GameContent.InvalidateCache("TerrainFeatures/hoeDirt");

                // if (e.NewLocation?.NameOrUniqueName == "Mine")
                // {
                //     e.NewLocation.setMapTile(43, 10, 173, "Buildings", "Warp 21 39 Custom_MoranaHall", 1);
                // }
        }

        private void OnUpdateTicked(object sender, EventArgs e)
        {
            GameTime time = new GameTime();
            // Vector2 butt = new Vector2(113, 7);
            //AmbientishLocationSounds.addSound(butt, 2);

            if (Game1.currentLocation is SapphireForest)
                AmbientishLocationSounds.update(time);

        }


            private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

        }

        private void AddWalletItems(object sender, EventArgs e)
        {
            var page = sender as NewSkillsPage;
            if (Game1.player.team.hasEnlightenment().Value)
                page.specialItems.Add(new ClickableTextureComponent(
                    name: "", bounds: new Microsoft.Xna.Framework.Rectangle(-1, -1, 16 * Game1.pixelZoom, 16 * Game1.pixelZoom),
                    label: null, hoverText: "Enlightenment",
                    texture: Assets.Enlightenment, sourceRect: new Microsoft.Xna.Framework.Rectangle(0, 0, 16, 16), scale: 4f, drawShadow: true));
        }


        static void OnEventFinished(object sender, EventArgs e)
        {
            switch (Game1.CurrentEvent.id)
            {
                case "17333004":
                   
                    Game1.player.addQuest("1733305");
                    Game1.screenOverlayTempSprites.AddRange(Utility.sparkleWithinArea(new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), 500, Color.Indigo, 10, 2000));

                

                    break;

            }
        }
        public static void AforeGiftGiven(object sender, EventArgsBeforeReceiveObject e)
        {

            if (sender != Game1.player)
                return;
            var Allya = Game1.getCharacterFromName("Alla");

            if (e.Npc.Equals(Allya))
            {
                // if(e.Gift.Equals(342))
                if (e.Gift.ParentSheetIndex == 458)
                {
                    if (Game1.player.eventsSeen.Contains("Mermaid.Alla8HeartEvent"))
                     return;
                    Allya.CurrentDialogue.Push(new Dialogue(Allya, "Strings\\StringsFromCSFiles:Alla.Bouquet.cs.3962"));
                    Game1.drawDialogue(Allya);
                    Friendship friendship = Game1.player.friendshipData["Alla"];
                    if (!friendship.IsDating())
                    {
                        friendship.Status = FriendshipStatus.Friendly;
                       
                    }
                     e.Cancel = true;

                }
            }
        }

        static void OnGiftGiven(object sender, EventArgsGiftGiven e)
        {
            var Game1_multiplayer = instance.Helper.Reflection.GetField<Multiplayer>(typeof(Game1), "multiplayer").GetValue();
            multiplayer = Game1_multiplayer;
            if (sender != Game1.player)
                return;
            var Oksie = Game1.getCharacterFromName("Oksana");
            var Vi = Game1.getCharacterFromName("Vika");
            var Abbi = Game1.getCharacterFromName("Abigail");


            if (e.Npc.Equals(Oksie))
            {
                // if(e.Gift.Equals(342))
                if (e.Gift.ParentSheetIndex == 342)
                {
                    Game1.DrawDialogue(Game1.getCharacterFromName("Oksana"), "Do I look pregnant to you? Are you fat shaming me?$3#$b#Never mind!!! Fuck You, take this jar up your ass!!!$5");
                    Game1.playSound("clubSmash");
                    Game1.player.health = 0;
                }

                else if (e.Gift.Category == StardewValley.Object.flowersCategory)
                {

                    DelayedAction.playSoundAfterDelay("secret1", 3000);
                    Game1.DrawDialogue(Game1.getCharacterFromName("Oksana"), "Awwwww... I love flowers so so so much!!!$8#$b#Here's a kiss, to say  thank you in the most mermaid way possible!$11",null);


                    bool flip = (e.Npc.Sprite.currentFrame == 32 && Game1.player.FacingDirection == 3) || (e.Npc.Sprite.currentFrame == 34 && Game1.player.FacingDirection == 1);
                    int kissyFrame = 32;
                    int delay = (Game1.player.movementPause = (100));
                    Game1.player.Sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>
                        {
                            new FarmerSprite.AnimationFrame(kissyFrame, delay, secondaryArm: false, flip, haltMe, behaviorAtEndOfFrame: true)
                        });
                    multiplayer.broadcastSprites(Game1.player.currentLocation, new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Microsoft.Xna.Framework.Rectangle(211, 428, 7, 6), 2000f, 1, 0, new Vector2(Game1.player.TilePoint.X, Game1.player.TilePoint.Y) * 64f + new Vector2(16f, -64f), flicker: false, flipped: false, 1f, 0f, Color.White, 4f, 0f, 0f, 0f)
                  
                    
                    {
                        motion = new Vector2(0f, -0.5f),
                        alphaFade = 0.01f
                    });

                    Game1.playSound("dwop");


                    e.Npc.Sprite.UpdateSourceRect();


                    //PerformKiss(3);
                    int playerFaceDirection = 0;
                    if ((e.Npc.Sprite.CurrentFrame == 4 || e.Npc.Sprite.CurrentFrame == 0))
                    {
                        playerFaceDirection = 1;
                    }
                    else if ((e.Npc.Sprite.CurrentFrame == 8 || e.Npc.Sprite.CurrentFrame == 12))
                    {
                        playerFaceDirection = 3;
                    }

                    Game1.player.PerformKiss(playerFaceDirection);

                }
            }
            if (e.Npc.Equals(Vi))
            {
                // if(e.Gift.Equals(342))
                if (e.Gift.ParentSheetIndex == 342)
                {
                    Game1.DrawDialogue(e.Npc, "I love these!!! How did you know?");
                    e.Npc.doEmote(20);

                }

                if (e.Gift.ParentSheetIndex == 272)
                {
                    Game1.DrawDialogue(e.Npc, "Wwwwwwhat are you trying to say?!?!?!$2#$b#Doesn't matter! FUCK YOU!!!$3");
                    Game1.playSound("thunder");
                    Game1.player.health = 0;
                }

                if (e.Gift.ParentSheetIndex == 394)
                {
                    Game1.DrawDialogue(e.Npc, "Ahhhhhhhh!!!! My favorite shell!!! You deserve a worthy reward!$5#$b#You get to see my boobs!!!$7");
                    Game1.playSound("stardrop");
                    Game1.screenOverlayTempSprites.AddRange(Utility.sparkleWithinArea(new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), 500, Color.Indigo, 10, 2000));
                    Game1.player.health = Game1.player.maxHealth;
                    Game1.player.stamina = Game1.player.maxStamina.Value;
                    Game1.player.team.sharedDailyLuck.Value = 0.37;
                    e.Npc.doEmote(20);

                }

            }
            if (e.Npc.Equals(Abbi))
            {
                if (e.Gift.ParentSheetIndex == 109)
                {
                    Game1.DrawDialogue(e.Npc, "*GASP*A SWORD!?!?!?! For me??????$7#$b#Thank you Thank You THANK YOU!!!$1");
                    multiplayer.broadcastSprites(Game1.player.currentLocation, new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Microsoft.Xna.Framework.Rectangle(211, 428, 7, 6), 2000f, 1, 0, new Vector2(Game1.player.TilePoint.X, Game1.player.TilePoint.Y) * 64f + new Vector2(16f, -64f), flicker: false, flipped: false, 1f, 0f, Color.White, 4f, 0f, 0f, 0f));
                }
                if (e.Gift.ParentSheetIndex == 614)
                {
                    Game1.DrawDialogue(e.Npc, "Does my hair look green today? Did you mistake me for my mom or something?$3");
                }
                if (e.Gift.ParentSheetIndex == 196)
                {
                    Game1.DrawDialogue(e.Npc, "This is just nasty. Are you implying something??? Did you drink the jerk juice this morning or something???$5");
                    Game1.player.health = 0;
                }
                if (e.Gift.ParentSheetIndex == 200)
                {
                    Game1.DrawDialogue(e.Npc, "What gives you the right to judge my life choices??? NOTHING!!!$5");
                    Game1.player.health = 0;
                }
            }






        }
                public static void haltMe(Farmer who)
        {
            Game1.player.Halt();
        }




        /*
        public static void PerformKiss(int facingDirection)
        {
      
            if (!Game1.eventUp && !Game1.player.UsingTool && (!Game1.player.IsLocalPlayer || Game1.activeClickableMenu == null) && !Game1.player.isRidingHorse() && !Game1.player.IsSitting() && !Game1.player.IsEmoting && Game1.player.CanMove)
            {
                Game1.player.CanMove = false;
                Game1.player.FarmerSprite.PauseForSingleAnimation = false;
                Game1.player.faceDirection(facingDirection);
                Game1.player.FarmerSprite.animateOnce(new List<FarmerSprite.AnimationFrame>
            {
                new FarmerSprite.AnimationFrame(101, 1000, 0, secondaryArm: false, Game1.player.FacingDirection == 3),
                new FarmerSprite.AnimationFrame(6, 1, secondaryArm: false, Game1.player.FacingDirection == 3)
            }.ToArray());
            }
        } */





    }
}