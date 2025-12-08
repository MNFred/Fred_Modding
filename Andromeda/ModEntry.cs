using Andromeda.External;
using Andromeda.features;
using Fred.Andromeda.Artifacts;
using Fred.Andromeda.cards;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using Nickel.Common;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Fred.Andromeda;

internal class ModEntry : SimpleMod
{
    internal static ModEntry Instance { get; private set; } = null!;
    internal Harmony Harmony;
    internal IKokoroApi.IV2 KokoroApi;
    internal IDeckEntry AndromedaDeck;
    internal IStatusEntry PassiveGravitateStatus;
    internal IStatusEntry ForcefullGravitate;
    internal IStatusEntry HermesDash;
    internal ISpriteEntry PanelSprite;
    internal ISoundEntry AndroBabble { get; }
    internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }
    internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations { get; }

    private static List<Type> AndromedaCommonCards = [
        typeof(StardustCloud),
        typeof(PiercingStar),
        typeof(ShootingStar),
        typeof(LoneMoon),
        typeof(WindNudge),
        typeof(LazyShift),
        typeof(Comet),
        typeof(IronStar),
        typeof(Quasar)
    ];
    private static List<Type> AndromedaUnCommonCards = [
        typeof(HermesDash),
        typeof(SaturnRing),
        typeof(LawOfMotion),
        typeof(HeavyStar),
        typeof(GravitationalPull),
        typeof(AsteroidCard),
        typeof(ViolentWind)
    ];
    private static List<Type> AndromedaRareCards = [
        typeof(Lithobraking),
        typeof(Entropy),
        typeof(AuroraBorealis),
        typeof(Meteor),
        typeof(Lightspeed)
    ];
    private static List<Type> AndromedaSpecialCards = [
        typeof(AndromedaEXE),
        typeof(Polarity)
    ];
    private static IEnumerable<Type> AndromedaCardTypes =
        AndromedaCommonCards
        .Concat(AndromedaUnCommonCards)
        .Concat(AndromedaRareCards)
        .Concat(AndromedaSpecialCards);

    private static List<Type> AndromedaCommonArtifacts = [
        typeof(ReversedPolarity),
        typeof(PocketTelescope),
        typeof(Spaghettification)
    ];
    private static List<Type> AndromedaBossArtifacts = [
        typeof(ShatteredStar)
    ];
    private static IEnumerable<Type> AndromedaArtifactTypes =
        AndromedaCommonArtifacts
        .Concat(AndromedaBossArtifacts);

    private static IEnumerable<Type> AllRegisterableTypes =
        AndromedaCardTypes
        .Concat(AndromedaArtifactTypes);

    public ModEntry(IPluginPackage<IModManifest> package, IModHelper helper, ILogger logger) : base(package, helper, logger)
    {
        Instance = this;
        Harmony = new Harmony("Fred.Andromeda");
        KokoroApi = helper.ModRegistry.GetApi<IKokoroApi>("Shockah.Kokoro")!.V2;

        AnyLocalizations = new JsonLocalizationProvider(
            tokenExtractor: new SimpleLocalizationTokenExtractor(),
            localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/{locale}.json").OpenRead()
        );
        Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
            new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(AnyLocalizations)
        );
        PanelSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/CharacterFrame_Andromeda.png"));
        AndroBabble = helper.Content.Audio.RegisterSound(package.PackageRoot.GetRelativeFile("assets/andromedaBabble.wav"));
        AndromedaDeck = helper.Content.Decks.RegisterDeck("AndromedaDeck", new DeckConfiguration
        {
            Definition = new DeckDef
            {
                color = new Color("0009AB"),

                titleColor = new Color("000000")
            },
            DefaultCardArt = RegisterSprite(package, "assets/Card/DefaultBackGround_Andromeda.png").Sprite,
            BorderSprite = RegisterSprite(package, "assets/Andromeda_Frame.png").Sprite,
            Name = AnyLocalizations.Bind(["character", "name"]).Localize
        });
        helper.Content.Characters.V2.RegisterPlayableCharacter("Andromeda", new PlayableCharacterConfigurationV2()
        {
            Deck = AndromedaDeck.Deck,
            Description = AnyLocalizations.Bind(["character", "description"]).Localize,
            BorderSprite = PanelSprite.Sprite,
            Babble = new CharacterBabbleConfiguration()
            {
                Sound = AndroBabble
            },
            SoloStarters = new()
            {
                cards = [
                    new PiercingStar(),
                    new StardustCloud(),
                    new ShootingStar(),
                    new LoneMoon(),
                    new DodgeColorless(),
                    new CannonColorless()
                ]
            },
            Starters = new StarterDeck
            {
                cards = [
                    new PiercingStar(),
                    new StardustCloud()
                ]
            },
            ExeCardType = typeof(AndromedaEXE)
        });

        foreach (var type in AllRegisterableTypes)
            AccessTools.DeclaredMethod(type, nameof(IRegisterable.Register))?.Invoke(null, [package, helper]);
        
        RegisterAnimation(package, "neutral", "assets/Animation/Andromeda_Neutral_", 4);
        RegisterAnimation(package, "squint", "assets/Animation/Andromeda_Squint_", 4);
        Instance.Helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2
        {
            CharacterType = AndromedaDeck.Deck.Key(),
            LoopTag = "gameover",
            Frames = [
                RegisterSprite(package, "assets/Animation/Andromeda_GameOver_0.png").Sprite,
            ]
        });
        Instance.Helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2
        {
            CharacterType = AndromedaDeck.Deck.Key(),
            LoopTag = "mini",
            Frames = [
                RegisterSprite(package, "assets/Animation/Andromeda_Mini.png").Sprite,
            ]
        });

        PassiveGravitateStatus = helper.Content.Statuses.RegisterStatus("PassiveGravitate", new StatusConfiguration
        {
            Definition = new StatusDef
            {
                isGood = true,
                color = new Color("0053C7"),
                icon = RegisterSprite(package, "assets/PassiveGravitate.png").Sprite
            },
            Name = AnyLocalizations.Bind(["status", "PassiveGravitate", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "PassiveGravitate", "desc"]).Localize
        });
        ForcefullGravitate = helper.Content.Statuses.RegisterStatus("ForcefullGravitate", new StatusConfiguration
        {
            Definition = new StatusDef
            {
                isGood = true,
                color = new Color("0053C7"),
                icon = RegisterSprite(package, "assets/ForcefullGravitate.png").Sprite
            },
            Name = AnyLocalizations.Bind(["status", "ForcefullGravitate", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "ForcefullGravitate", "desc"]).Localize
        });
        HermesDash = helper.Content.Statuses.RegisterStatus("HermesDash", new StatusConfiguration
        {
            Definition = new StatusDef
            {
                isGood = true,
                color = new Color("0053C7"),
                icon = RegisterSprite(package, "assets/HermesDash.png").Sprite
            },
            Name = AnyLocalizations.Bind(["status", "HermesDash", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "HermesDash", "desc"]).Localize
        });
        _ = new HermesDashManager();
        _ = new PassiveGravitateManager();
        _ = new ForcefullGravitateManager();
        _ = new NegativeOverdriveManager();

        helper.ModRegistry.GetApi<IMoreDifficultiesApi>("TheJazMaster.MoreDifficulties", new SemanticVersion(1, 3, 0))?.RegisterAltStarters(
			deck: AndromedaDeck.Deck,
			starterDeck: new StarterDeck
			{
				cards = [
					new ShootingStar(),
					new LoneMoon(),
				]
			}
		);

    }
    public static ISpriteEntry RegisterSprite(IPluginPackage<IModManifest> package, string dir)
    {
        return Instance.Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile(dir));
    }

    public static void RegisterAnimation(IPluginPackage<IModManifest> package, string tag, string dir, int frames)
    {
        Instance.Helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2
        {
            CharacterType = Instance.AndromedaDeck.Deck.Key(),
            LoopTag = tag,
            Frames = Enumerable.Range(0, frames)
                .Select(i => RegisterSprite(package, dir + i + ".png").Sprite)
                .ToImmutableList()
        });
    }
}

