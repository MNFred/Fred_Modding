using Nickel;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using System;
using System.Collections.Generic;
using System.Linq;
using FMOD;
using Nickel.Common;
using Fred.TH34.features;
using Fred.TH34.cards;
using Fred.TH34.Artifacts;
using Nanoray.Shrike;
using System.Reflection.Emit;
using Nanoray.Shrike.Harmony;

namespace Fred.TH34;
public sealed class ModEntry : SimpleMod
{
    internal IMoreDifficultiesApi? MoreDifficultiesApi { get; }
    internal Harmony Harmony { get; }
    internal static ModEntry Instance { get; private set; } = null!;
    internal IKokoroApi KokoroApi { get; }
    internal IEssentialsApi? EssentialsApi { get; private set; }
    internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }
    internal ApiImplementation Api { get; private set; } = null!;
    internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations { get; }
    internal IDeckEntry TH34_Deck { get; }
    internal ISpriteEntry TH34_CardBackground { get; }
    internal ISpriteEntry TH34_CardBorder { get; }
    internal ISpriteEntry TH34_Panel { get; }
    internal ISpriteEntry TH34_neutral_0 { get; }
    internal ISpriteEntry TH34_neutral_1 { get; }
    internal ISpriteEntry TH34_neutral_2 { get; }
    internal ISpriteEntry TH34_neutral_3 { get; }
    internal ISpriteEntry TH34_squint_0 { get; }
    internal ISpriteEntry TH34_squint_1 { get; }
    internal ISpriteEntry TH34_squint_2 { get; }
    internal ISpriteEntry TH34_squint_3 { get; }
    internal ISpriteEntry TH34_gameover { get; }
    internal ISpriteEntry TH34_mini { get; }
    internal IStatusEntry MinusChargeStatus { get; }
    internal IStatusEntry PlusChargeStatus { get; }
    internal IStatusEntry RefractoryStatus { get; }
    internal IStatusEntry OptimizeStatus { get; }
    internal IStatusEntry OptimizeBStatus { get; }
    internal static IReadOnlyList<Type> TH34_Common_Cards { get; } = [
        typeof(CardTurboMode),
        typeof(CardIdleMode),
        typeof(CardAmpUp),
        typeof(CardTagOut),
        typeof(CardThermoResistor),
        typeof(CardFactoryReset),
        typeof(CardBackgroundTask),
        typeof(CardThetaProtocol),
        typeof(CardFragShot)
    ];
    internal static IReadOnlyList<Type> TH34_Uncommon_Cards { get; } = [
        typeof(CardCounterBalance),
        typeof(CardSafeguard),
        typeof(CardAnode),
        typeof(CardAnion),
        typeof(CardCleverTech),
        typeof(CardPulseCannon),
        typeof(CardKillSwitch)
    ];
    internal static IReadOnlyList<Type> TH34_Rare_Cards { get; } = [
        typeof(CardOptimize),
        typeof(CardReboot),
        typeof(CardManifold),
        typeof(CardDemonCore),
        typeof(CardUnload)
    ];
    internal static IEnumerable<Type> TH34_AllCard_Types
        => TH34_Common_Cards
        .Concat(TH34_Uncommon_Cards)
        .Concat(TH34_Rare_Cards);
    internal static IReadOnlyList<Type> TH34_Common_Artifacts { get; } = [
        typeof(ArtifactSilverCoils),
        typeof(ArtifactCodeBlue),
        typeof(ArtifactThermalPump),
        typeof(ArtifactElectronDonor)
    ];
    internal static IReadOnlyList<Type> TH34_Boss_Artifacts { get; } = [
        typeof(ArtifactMechanicalHeart),
        typeof(ArtifactIonBattery)
    ];
    internal static IEnumerable<Type> TH34_AllArtifacts
        => TH34_Common_Artifacts
        .Concat(TH34_Boss_Artifacts);
    public ModEntry(IPluginPackage<IModManifest> package, IModHelper helper, ILogger logger) : base(package, helper, logger)
    {
        Instance = this;
        Harmony = new(package.Manifest.UniqueName);
        Api = new();
        KokoroApi = helper.ModRegistry.GetApi<IKokoroApi>("Shockah.Kokoro")!;
        _ = new AMinusChargeManager();
        _ = new APlusChargeManager();
        _ = new ARefractoryManager();
        _ = new AOptimizeManager();
        _ = new AOptimizeBManager();
        Harmony.PatchAll();
        helper.Events.OnModLoadPhaseFinished += (_, phase) =>
		{
			if (phase != ModLoadPhase.AfterDbInit)
				return;

			EssentialsApi = helper.ModRegistry.GetApi<IEssentialsApi>("Nickel.Essentials");
		};
        CustomTTGlossary.ApplyPatches(Harmony);
        AnyLocalizations = new JsonLocalizationProvider(
            tokenExtractor: new SimpleLocalizationTokenExtractor(),
            localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/{locale}.json").OpenRead()
        );
        Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
            new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(AnyLocalizations)
        );
        TH34_CardBackground = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/TH34_DefaultArt.png"));
        TH34_CardBorder = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/CardBorder.png"));
        TH34_neutral_0 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/TH-34Neutral_0.png"));
        TH34_neutral_1 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/TH-34Neutral_1.png"));
        TH34_neutral_2 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/TH-34Neutral_2.png"));
        TH34_neutral_3 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/TH-34Neutral_3.png"));
        TH34_squint_0 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/TH-34Squint_0.png"));
        TH34_squint_1 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/TH-34Squint_1.png"));
        TH34_squint_2 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/TH-34Squint_2.png"));
        TH34_squint_3 = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/TH-34Squint_3.png"));
        TH34_gameover = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/TH-34Defeat.png"));
        TH34_mini = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/MiniTH-34.png"));
        TH34_Panel = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/characters/TH-34Panel.png"));

        TH34_Deck = helper.Content.Decks.RegisterDeck("TH34Deck", new DeckConfiguration()
        {
            Definition = new DeckDef()
            {
                color = new Color("aaaaaa"),
                titleColor = new Color("000000"),
            },
            DefaultCardArt = TH34_CardBackground.Sprite,
            BorderSprite = TH34_CardBorder.Sprite,
            Name = AnyLocalizations.Bind(["character", "TH34", "name"]).Localize,
        });
        MinusChargeStatus = helper.Content.Statuses.RegisterStatus("MinusCharge", new()
        {
            Definition = new()
            {
                icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/MinusCharge.png")).Sprite,
                color = new Color("00c1ff"),
                isGood = true
            },
            Name = this.AnyLocalizations.Bind(["status", "MinusCharge", "name"]).Localize,
            Description = this.AnyLocalizations.Bind(["status", "MinusCharge", "description"]).Localize
            
        });
        PlusChargeStatus = helper.Content.Statuses.RegisterStatus("PlusCharge", new()
        {
            Definition = new()
            {
                icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/PlusCharge.png")).Sprite,
                color = new Color("ff1f00"),
                isGood = true
            },
            Name = this.AnyLocalizations.Bind(["status", "PlusCharge", "name"]).Localize,
            Description = this.AnyLocalizations.Bind(["status", "PlusCharge", "description"]).Localize
            
        });
        RefractoryStatus = helper.Content.Statuses.RegisterStatus("Refractory", new()
        {
            Definition = new()
            {
                icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/Refractory.png")).Sprite,
                color = new Color("eb8400"),
                isGood = true
            },
            Name = this.AnyLocalizations.Bind(["status", "Refractory", "name"]).Localize,
            Description = this.AnyLocalizations.Bind(["status", "Refractory", "description"]).Localize
            
        });
        OptimizeStatus = helper.Content.Statuses.RegisterStatus("Optimize", new()
        {
            Definition = new()
            {
                icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/OptimizeStatus.png")).Sprite,
                color = new Color("00e0eb"),
                isGood = true
            },
            Name = this.AnyLocalizations.Bind(["status", "Optimize", "name"]).Localize,
            Description = this.AnyLocalizations.Bind(["status", "Optimize", "description"]).Localize
            
        });
        OptimizeBStatus = helper.Content.Statuses.RegisterStatus("OptimizeB", new()
        {
            Definition = new()
            {
                icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/icons/OptimizeBStatus.png")).Sprite,
                color = new Color("eb3900"),
                isGood = true
            },
            Name = this.AnyLocalizations.Bind(["status", "OptimizeB", "name"]).Localize,
            Description = this.AnyLocalizations.Bind(["status", "OptimizeB", "description"]).Localize
            
        });
        helper.Content.Characters.RegisterCharacterAnimation(new CharacterAnimationConfiguration()
        {
            Deck = TH34_Deck.Deck,
            LoopTag = "neutral",
            Frames = new[]
            {
                TH34_neutral_0.Sprite,
                TH34_neutral_1.Sprite,
                TH34_neutral_2.Sprite,
                TH34_neutral_3.Sprite
            }
        });
        helper.Content.Characters.RegisterCharacterAnimation(new CharacterAnimationConfiguration()
        {
            Deck = TH34_Deck.Deck,
            LoopTag = "mini",
            Frames = new[]
            {
                TH34_mini.Sprite
            }
        });
        helper.Content.Characters.RegisterCharacterAnimation(new CharacterAnimationConfiguration()
        {
            Deck = TH34_Deck.Deck,
            LoopTag = "squint",
            Frames = new[]
            {
                TH34_squint_0.Sprite,
                TH34_squint_1.Sprite,
                TH34_squint_2.Sprite,
                TH34_squint_3.Sprite
            }
        });
        helper.Content.Characters.RegisterCharacterAnimation(new CharacterAnimationConfiguration()
        {
            Deck = TH34_Deck.Deck,
            LoopTag = "gameover",
            Frames = new[]
            {
                TH34_gameover.Sprite
            }
        });
        CustomTTGlossary.ApplyPatches(Harmony);
        this.AnyLocalizations = new JsonLocalizationProvider(
			tokenExtractor: new SimpleLocalizationTokenExtractor(),
			localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/{locale}.json").OpenRead()
		);
		this.Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
			new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(this.AnyLocalizations)
		);
        helper.Content.Characters.RegisterCharacter("TH34", new CharacterConfiguration()
        {
            Deck = TH34_Deck.Deck,
            Description = AnyLocalizations.Bind(["character", "TH34", "description"]).Localize,
            BorderSprite = TH34_Panel.Sprite,
            StarterCardTypes = [
                typeof(CardIdleMode),
                typeof(CardTurboMode)
            ],
        });
        foreach (var CardType in TH34_AllCard_Types)
            AccessTools.DeclaredMethod(CardType, nameof(ITH34Card.Register))?.Invoke(null, [package,helper]);
        foreach (var ArtifactType in TH34_AllArtifacts)
            AccessTools.DeclaredMethod(ArtifactType, nameof(ITH34Artifact.Register))?.Invoke(null, [package,helper]);
        helper.ModRegistry.GetApi<IMoreDifficultiesApi>("TheJazMaster.MoreDifficulties", new SemanticVersion(1, 6, 3))?.RegisterAltStarters(
			deck: TH34_Deck.Deck,
			starterDeck: new StarterDeck
			{
				cards = [
                    new CardTagOut(),
					new CardAmpUp()
				],
			}
		);
    }
    public override object? GetApi(IModManifest requestingMod)
		=> new ApiImplementation();
}
