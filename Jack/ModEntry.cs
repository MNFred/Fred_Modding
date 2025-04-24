using Nickel;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using System;
using System.Collections.Generic;
using System.Linq;
using Nickel.Common;
using Fred.Jack.features;
using Fred.Jack.cards;
using Fred.Jack.DialogueAdditions;
using Fred.Jack.Midrow;
using Fred.Jack.Artifacts;

namespace Fred.Jack;
public sealed class ModEntry : SimpleMod
{
    internal IMoreDifficultiesApi? MoreDifficultiesApi { get; }
    internal Harmony Harmony { get; }
    internal static ModEntry Instance { get; set; } = null!;
    internal IKokoroApi KokoroApi { get; }
    internal Settings Settings { get; set; } = new Settings();
    internal IEssentialsApi? EssentialsApi { get; set; }
    internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }
    internal IDynaApi? DynaApi { get; set; }
    internal ITyAndSashaApi? TyApi { get; set; }
    internal ApiImplementation Api { get; set; }
    internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations { get; }
    internal IDuoArtifactsApi? DuoArtifactsApi { get; set; }
    internal ISpriteEntry Jack_Copper_CardBackground { get; }
    internal ISpriteEntry Jack_Copper_CardFrame { get; }
    internal ISpriteEntry Jack_Copper_Panel { get; }
    internal ISpriteEntry JaCk_Neutral_0 { get; }
    internal ISpriteEntry JaCk_Neutral_1 { get; }
    internal ISpriteEntry JaCk_Shock_0 { get; }
    internal ISpriteEntry JaCk_Shock_1 { get; }
    internal ISpriteEntry JaCk_Gameover_0 { get; }
    internal ISpriteEntry JaCk_Question_0 { get; }
    internal ISpriteEntry JaCk_Question_1 { get; }
    internal ISpriteEntry JaCk_Mini_0 { get; }
    internal ISpriteEntry JaCk_Squint_0 { get; }
    internal ISpriteEntry JaCk_Squint_1 { get; }
    internal ISpriteEntry JaCk_Mimir_0 { get; }
    internal ISpriteEntry JaCk_Salute_0 { get; }
    internal ISpriteEntry JaCk_Salute_1 { get; }
    internal ISpriteEntry JaCk_Confuddled_0 { get; }
    internal ISpriteEntry JaCk_Confuddled_1 { get; }
    internal ISpriteEntry JaCk_Serious_0 { get; }
    internal ISpriteEntry JaCk_Serious_1 { get; }
    internal ISpriteEntry JaCk_Happy_0 { get; }
    internal ISpriteEntry JaCk_Happy_1 { get; }
    internal IDeckEntry Jack_Deck { get; }
    internal IStatusEntry LoseDroneshiftNextStatus { get; }
    internal IStatusEntry ScanBoostStatus { get; }
    internal IStatusEntry LockOnStatus { get; }
    internal ISpriteEntry BlankRocket_Icon { get; }
    internal ISpriteEntry BlankRocket_Sprite { get; }
    internal ISpriteEntry APRocket_Icon { get; }
    internal ISpriteEntry APRocket_Sprite { get; }
    internal ISpriteEntry BalisticRocket_Sprite { get; }
    internal ISpriteEntry ClusterRocket_Sprite { get; }
    internal ISpriteEntry ClusterRocket_Icon { get; }
    internal ISpriteEntry BalisticRocket_Icon { get; }
    internal ISpriteEntry MiniMissile { get; }
    internal ISpriteEntry MiniMissile_Icon { get; }
    internal ISpriteEntry ClearWay_Icon { get; }
    internal ISpriteEntry RandoRocketIcon { get; }
    internal IStatusEntry ALockOnStatus { get; }
    internal ISpriteEntry DOME_Sprite1 { get; }
    internal ISpriteEntry DOME_Sprite2 { get; }
    internal ISpriteEntry DOME_Sprite3 { get; }
    internal ISpriteEntry DOME_Icon { get; }
    internal ISpriteEntry TargeterIcon { get; }
    internal ISoundEntry JackBabble { get; }
    internal IStatusEntry MidrowHaltStatus { get; }
    internal static IReadOnlyList<Type> EventTypes { get; } = [
      typeof(JackHullEvent)
    ];
    internal static IReadOnlyList<Type> Jack_Common_Cards { get; } = [
        typeof(CardAPMissile),
        typeof(DodgeRoll),
        typeof(BackUpMissile),
        typeof(MissileThrow),
        typeof(BlankMissileCard),
        typeof(HiJackControls),
        typeof(TargetPractice),
        typeof(RadarOverclock),
        typeof(RemoteActivation),
    ];
    internal static IReadOnlyList<Type> Jack_Uncommon_Cards { get; } = [
        typeof(HeavyShift),
        typeof(MissileSalvo),
        typeof(MissileBarrage),
        typeof(RapidReload),
        typeof(OnMyMark),
        typeof(BalisticMissileCard),
        typeof(CorrosonMissileCard)
    ];
    internal static IReadOnlyList<Type> Jack_Rare_Cards { get; } = [
        typeof(ScanBoostCard),
        typeof(SteadyAim),
        typeof(ClusterMissileCard),
        typeof(ClearPerimiter),
        typeof(RadarAmp)
    ];
    internal static IReadOnlyList<Type> Jack_Special_Cards { get; } = [
        typeof(BackUpMissileCard),
        typeof(PrecisionCard),
        typeof(JackEXE)
    ];
    internal static IEnumerable<Type> Jack_AllCard_Types
        => Jack_Common_Cards
        .Concat(Jack_Uncommon_Cards)
        .Concat(Jack_Rare_Cards)
        .Concat(Jack_Special_Cards);
    internal static IReadOnlyList<Type> Jack_Common_Artifacts { get; } = [
        typeof(SpareAmmo),
        typeof(TripleTag),
        typeof(PreloadedBays),
        typeof(CoordinatedLaunch)
    ];
    internal static IReadOnlyList<Type> Jack_Boss_Artifacts { get; } = [
        typeof(NeverMissile)
    ];
    internal static IEnumerable<Type> Jack_AllArtifacts
        => Jack_Common_Artifacts
        .Concat(Jack_Boss_Artifacts);

    internal static IReadOnlyList<Type> Jack_Duo_Artifacts { get; } = [
        typeof(DOMEArtifact),
        typeof(HeavyImpact),
        typeof(CrystalReticle),
        typeof(ObjectScanner),
        typeof(ScrapArmor),
        typeof(SecondaryReticle),
        typeof(AimBot),
        typeof(SimulatedReinforcement),
        typeof(MissileHeatSinks),
        typeof(ToyMissile)
    ];
    internal static IEnumerable<Type> RegisterableTypes { get; }
		= [
			.. EventTypes,
            typeof(CardDOME)
		];
    internal static readonly IEnumerable<Type> LateRegisterableTypes
		= Jack_Duo_Artifacts;
    public ModEntry(IPluginPackage<IModManifest> package, IModHelper helper, ILogger logger) : base(package, helper, logger)
    {
        Instance = this;
        Harmony = new(package.Manifest.UniqueName);
        Api = new();
        KokoroApi = helper.ModRegistry.GetApi<IKokoroApi>("Shockah.Kokoro")!;
        
        Harmony.PatchAll();
        helper.Events.OnModLoadPhaseFinished += (_, phase) =>
		{
			if (phase != ModLoadPhase.AfterDbInit)
				return;
			EssentialsApi = helper.ModRegistry.GetApi<IEssentialsApi>("Nickel.Essentials");
            DynaApi = helper.ModRegistry.GetApi<IDynaApi>("Shockah.Dyna");
            TyApi = helper.ModRegistry.GetApi<ITyAndSashaApi>("TheJazMaster.TyAndSasha");
            DuoArtifactsApi = helper.ModRegistry.GetApi<IDuoArtifactsApi>("Shockah.DuoArtifacts");
            helper.ModRegistry.AwaitApi<IModSettingsApi>(
			"Nickel.ModSettings",
			api => api.RegisterModSettings(api.MakeList([
				api.MakeProfileSelector(
					() => package.Manifest.DisplayName ?? package.Manifest.UniqueName,
					Settings.ProfileBased
				),
				api.MakeCheckbox(
					() => "Jack Event",
					() => Settings.ProfileBased.Current.JackEventEnabled,
					(_, _, value) => Settings.ProfileBased.Current.JackEventEnabled = value
				).SetTooltips(() => [
					new GlossaryTooltip($"settings.{package.Manifest.UniqueName}::{nameof(ProfileSettings.JackEventEnabled)}")
					{
						TitleColor = Colors.textBold,
						Title = "Jack Event",
						Description = "Uncheck to disable event involving Jack from appearing in zone 2 and 3."
					}
				]),
			]).SubscribeToOnMenuClose(_ =>
			{
				helper.Storage.SaveJson(helper.Storage.GetMainStorageFile("json"), Settings);
				UpdateSettings();
			}))
		);
        foreach (var registerableType in LateRegisterableTypes)
				AccessTools.DeclaredMethod(registerableType, nameof(IRegisterable.Register))?.Invoke(null, [package, helper]);
        foreach (var type in RegisterableTypes)
			AccessTools.DeclaredMethod(type, nameof(IRegisterable.Register))?.Invoke(null, [package, helper]);
		};
        helper.Events.OnSaveLoaded += (_, _) => UpdateSettings();
        Harmony.Patch(
			original: typeof(MG).GetMethod("DrawLoadingScreen", AccessTools.all),
			prefix: new HarmonyMethod(typeof(StoryVarsAdditions), nameof(StoryVarsAdditions.DrawLoadingScreen_Prefix)),
			postfix: new HarmonyMethod(typeof(StoryVarsAdditions), nameof(StoryVarsAdditions.DrawLoadingScreen_Postfix))
		);
        CustomTTGlossary.ApplyPatches(Harmony);
        AnyLocalizations = new JsonLocalizationProvider(
            tokenExtractor: new SimpleLocalizationTokenExtractor(),
            localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/{locale}.json").OpenRead()
        );
        Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
            new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(AnyLocalizations)
        );
        _ = new LoseDroneShiftManager();
        _ = new LockOnManager();
        _ = new ScanBoostManager();
        _ = new ALockOnManager();
        _ = new MidrowHaltManager();
        Jack_Copper_CardBackground = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/Jack_Copper_cardbackground.png"));
        Jack_Copper_CardFrame = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/Jack_Copper_cardframe.png"));
        Jack_Copper_Panel = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/Jack_Copper_panel.png"));
        JaCk_Neutral_0 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/JaCk_neutral_0.png"));
        JaCk_Neutral_1 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/JaCk_neutral_1.png"));
        JaCk_Mini_0 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/JaCk_mini_0.png"));
        JaCk_Question_0 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/JaCk_question_0.png"));
        JaCk_Question_1 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/JaCk_question_1.png"));
        JaCk_Squint_0 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/JaCk_squint_0.png"));
        JaCk_Squint_1 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/JaCk_squint_1.png"));
        JaCk_Gameover_0 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/JaCk_gameover_0.png"));
        JaCk_Mimir_0 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/JaCk_mimir_0.png"));
        JaCk_Salute_0 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/JaCk_salute_0.png"));
        JaCk_Salute_1 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/JaCk_salute_1.png"));
        JaCk_Happy_0 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/JaCk_happy_0.png"));
        JaCk_Happy_1 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/JaCk_happy_1.png"));
        JaCk_Confuddled_0 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/JaCk_confuddled_0.png"));
        JaCk_Confuddled_1 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/JaCk_confuddled_1.png"));
        JaCk_Serious_0 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/JaCk_serious_0.png"));
        JaCk_Serious_1 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/JaCk_serious_1.png"));
        JaCk_Shock_0 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/JaCk_shocked_0.png"));
        JaCk_Shock_1 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/JaCk_shocked_1.png"));
        BlankRocket_Icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/blankrocket_icon.png"));
        BlankRocket_Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/rockets/blank_rocket.png"));
        MiniMissile = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/rockets/mini_missile.png"));
        MiniMissile_Icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/mini_missile_icon.png"));
        APRocket_Icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/aprocket_icon.png"));
        APRocket_Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/rockets/ap_rocket.png"));
        BalisticRocket_Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/rockets/balistic_rocket.png"));
        BalisticRocket_Icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/balisticrocket_icon.png"));
        ClearWay_Icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/clearway_icon.png"));
        ClusterRocket_Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/rockets/cluster_rocket.png"));
        ClusterRocket_Icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/clusterrocket_icon.png"));
        RandoRocketIcon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/randoRocket_icon.png"));
        DOME_Icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/DOME_icon.png"));
        DOME_Sprite1 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/rockets/DOME_sprite1.png"));
        DOME_Sprite2 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/rockets/DOME_sprite2.png"));
        DOME_Sprite3 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/rockets/DOME_sprite3.png"));
        TargeterIcon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/lockonicon2.png"));
        JackBabble = helper.Content.Audio.RegisterSound(package.PackageRoot.GetRelativeFile("assets/sounds/Jack_Babble_Sound.wav"));

        Jack_Deck = helper.Content.Decks.RegisterDeck("JackDeck", new DeckConfiguration()
        {
            Definition = new DeckDef()
            {
                color = new Color("0cd85d"),
                titleColor = new Color("000000"),
            },
            DefaultCardArt = Jack_Copper_CardBackground.Sprite,
            BorderSprite = Jack_Copper_CardFrame.Sprite,
            Name = AnyLocalizations.Bind(["character", "Jack", "name"]).Localize,
        });
        ScanBoostStatus = helper.Content.Statuses.RegisterStatus("ScanBoost", new()
        {
            Definition = new()
            {
                icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/ScanBoost_icon.png")).Sprite,
                color = new Color("0cd85d"),
                isGood = true
            },
            Name = AnyLocalizations.Bind(["status", "ScanBoost", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "ScanBoost", "description"]).Localize
        });
        LockOnStatus = helper.Content.Statuses.RegisterStatus("LockOn", new()
        {
            Definition = new()
            {
                icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/lockonicon.png")).Sprite,
                color = new Color("0cd85d"),
                isGood = false
            },
            Name = this.AnyLocalizations.Bind(["status", "LockOn", "name"]).Localize,
            Description = this.AnyLocalizations.Bind(["status", "LockOn", "description"]).Localize
        });
        ALockOnStatus = helper.Content.Statuses.RegisterStatus("ALockOn", new()
        {
            Definition = new()
            {
                icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/Alockonicon.png")).Sprite,
                color = new Color("0cd85d"),
                isGood = false
            },
            Name = this.AnyLocalizations.Bind(["status", "ALockOn", "name"]).Localize,
            Description = this.AnyLocalizations.Bind(["status", "ALockOn", "description"]).Localize
        });
        LoseDroneshiftNextStatus = helper.Content.Statuses.RegisterStatus("LoseDroneNext", new()
        {
            Definition = new()
            {
                icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/LoseDroneShift.png")).Sprite,
                color = new Color("0cd85d"),
                isGood = false
            },
            Name = this.AnyLocalizations.Bind(["status", "LoseDroneNext", "name"]).Localize,
            Description = this.AnyLocalizations.Bind(["status", "LoseDroneNext", "description"]).Localize
        });
        MidrowHaltStatus = helper.Content.Statuses.RegisterStatus("MidrowHalt", new()
        {
            Definition = new()
            {
                icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/MidrowHaltIcon.png")).Sprite,
                color = new Color("0cd85d"),
                isGood = false
            },
            Name = this.AnyLocalizations.Bind(["status", "MidrowHalt", "name"]).Localize,
            Description = this.AnyLocalizations.Bind(["status", "MidrowHalt", "description"]).Localize
        });
        helper.Content.Characters.RegisterCharacterAnimation(new CharacterAnimationConfiguration()
        {
            Deck = Jack_Deck.Deck,
            LoopTag = "neutral",
            Frames = new[]
            {
                JaCk_Neutral_0.Sprite,
                JaCk_Neutral_1.Sprite,
                JaCk_Neutral_0.Sprite
            }
        });
        helper.Content.Characters.RegisterCharacterAnimation(new CharacterAnimationConfiguration()
        {
            Deck = Jack_Deck.Deck,
            LoopTag = "mini",
            Frames = new[]
            {
                JaCk_Mini_0.Sprite
            }
        });
        helper.Content.Characters.RegisterCharacterAnimation(new CharacterAnimationConfiguration()
        {
            Deck = Jack_Deck.Deck,
            LoopTag = "squint",
            Frames = new[]
            {
                JaCk_Squint_0.Sprite,
                JaCk_Squint_1.Sprite,
                JaCk_Squint_0.Sprite
            }
        });
        helper.Content.Characters.RegisterCharacterAnimation(new CharacterAnimationConfiguration()
        {
            Deck = Jack_Deck.Deck,
            LoopTag = "gameover",
            Frames = new[]
            {
                JaCk_Gameover_0.Sprite
            }
        });
        helper.Content.Characters.RegisterCharacterAnimation(new CharacterAnimationConfiguration()
        {
            Deck = Jack_Deck.Deck,
            LoopTag = "confused",
            Frames = new[]
            {
                JaCk_Confuddled_0.Sprite,
                JaCk_Confuddled_1.Sprite,
                JaCk_Confuddled_0.Sprite,
            }
        });
        helper.Content.Characters.RegisterCharacterAnimation(new CharacterAnimationConfiguration()
        {
            Deck = Jack_Deck.Deck,
            LoopTag = "happy",
            Frames = new[]
            {
                JaCk_Happy_0.Sprite,
                JaCk_Happy_1.Sprite,
                JaCk_Happy_0.Sprite,
            }
        });
        helper.Content.Characters.RegisterCharacterAnimation(new CharacterAnimationConfiguration()
        {
            Deck = Jack_Deck.Deck,
            LoopTag = "question",
            Frames = new[]
            {
                JaCk_Question_0.Sprite,
                JaCk_Question_1.Sprite,
                JaCk_Question_0.Sprite,
            }
        });
        helper.Content.Characters.RegisterCharacterAnimation(new CharacterAnimationConfiguration()
        {
            Deck = Jack_Deck.Deck,
            LoopTag = "salute",
            Frames = new[]
            {
                JaCk_Salute_0.Sprite,
                JaCk_Salute_1.Sprite,
                JaCk_Salute_0.Sprite,
            }
        });
        helper.Content.Characters.RegisterCharacterAnimation(new CharacterAnimationConfiguration()
        {
            Deck = Jack_Deck.Deck,
            LoopTag = "serious",
            Frames = new[]
            {
                JaCk_Serious_0.Sprite,
                JaCk_Serious_1.Sprite,
                JaCk_Serious_0.Sprite,
            }
        });
        helper.Content.Characters.RegisterCharacterAnimation(new CharacterAnimationConfiguration()
        {
            Deck = Jack_Deck.Deck,
            LoopTag = "shock",
            Frames = new[]
            {
                JaCk_Shock_0.Sprite,
                JaCk_Shock_1.Sprite,
                JaCk_Shock_0.Sprite,
            }
        });
        helper.Content.Characters.RegisterCharacterAnimation(new CharacterAnimationConfiguration()
        {
            Deck = Jack_Deck.Deck,
            LoopTag = "sleep",
            Frames = new[]
            {
                JaCk_Mimir_0.Sprite,
            }
        });
        CustomTTGlossary.ApplyPatches(Harmony);
        AnyLocalizations = new JsonLocalizationProvider(
			tokenExtractor: new SimpleLocalizationTokenExtractor(),
			localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/{locale}.json").OpenRead()
		);
		Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
			new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(this.AnyLocalizations)
		);
        helper.Content.Characters.V2.RegisterPlayableCharacter("Jack", new PlayableCharacterConfigurationV2()
        {
            Deck = Jack_Deck.Deck,
            Description = AnyLocalizations.Bind(["character", "Jack", "description"]).Localize,
            BorderSprite = Jack_Copper_Panel.Sprite,
            Babble = new CharacterBabbleConfiguration()
            {
                Sound = JackBabble
            },
            SoloStarters = new()
            {
                cards = [
                    new MissileThrow(),
                    new DodgeRoll(),
                    new BlankMissileCard(),
                    new CardAPMissile(),
                    new HiJackControls(),
                    new RadarOverclock()
                ]
            },
            Starters = new StarterDeck
            {
                cards = [
                    new MissileThrow(),
                    new DodgeRoll()
                ]
            },
            ExeCardType = typeof(JackEXE)
        });
        helper.Content.Characters.V2.RegisterNonPlayableCharacter("JackE", new NonPlayableCharacterConfigurationV2()
        {
            Babble = new CharacterBabbleConfiguration()
            {
                Sound = JackBabble
            },
            CharacterType = "JackE",
            Name = AnyLocalizations.Bind(["eventGuy", "JackE", "name"]).Localize,
            NeutralAnimation = new CharacterAnimationConfigurationV2()
            {
                CharacterType = "JackE",
                LoopTag = "neutral",
                Frames = new[]
                {
                    JaCk_Neutral_0.Sprite,
                    JaCk_Neutral_1.Sprite,
                    JaCk_Neutral_0.Sprite
                }
            },
        });
        foreach (var CardType in Jack_AllCard_Types)
            AccessTools.DeclaredMethod(CardType, nameof(IJackCard.Register))?.Invoke(null, [package,helper]);
        foreach (var ArtifactType in Jack_AllArtifacts)
            AccessTools.DeclaredMethod(ArtifactType, nameof(IJackArtifact.Register))?.Invoke(null, [package,helper]);
        helper.ModRegistry.GetApi<IMoreDifficultiesApi>("TheJazMaster.MoreDifficulties", new SemanticVersion(1, 6, 3))?.RegisterAltStarters(
			deck: Jack_Deck.Deck,
			starterDeck: new StarterDeck
			{
				cards = [
                    new BlankMissileCard(),
                    new TargetPractice()
				],
			}
		);
    }
    private void UpdateSettings()
	{
		foreach (var type in RegisterableTypes)
			AccessTools.DeclaredMethod(type, nameof(IRegisterable.UpdateSettings))?.Invoke(null, [Package, Helper, Settings.ProfileBased.Current]);
	}
    public override object? GetApi(IModManifest requestingMod)
		=> new ApiImplementation();
}
