using Dougie.Artifacts;
using Dougie.cards;
using Dougie.features;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using Nickel.Common;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using static Dougie.Actions.CellHarvest;
using static Dougie.IKokoroApi.IV2.IActionCostsApi;

namespace Dougie;

internal class ModEntry : SimpleMod
{
    internal static ModEntry Instance { get; private set; } = null!;
    internal Harmony Harmony;
    internal IKokoroApi.IV2 KokoroApi;
    internal IDeckEntry DougieDeck;
    internal ISpriteEntry PanelSprite;
    internal ISpriteEntry Cell_Blank_Sprite;
    internal ISpriteEntry Cell_X_Sprite;
    internal ISpriteEntry Cell_D_Sprite;
    internal ISpriteEntry Cell_F_Sprite;
    internal ISpriteEntry Cell_A_Sprite;
    internal ISpriteEntry Cell_XD_Sprite;
    internal ISpriteEntry Cell_XF_Sprite;
    internal ISpriteEntry Cell_FA_Sprite;
    internal ISpriteEntry Cell_DA_Sprite;
    internal ISpriteEntry Cell_XA_Sprite;
    internal ISpriteEntry Cell_DF_Sprite;
    internal ISpriteEntry Cell_XDF_Sprite;
    internal ISpriteEntry Cell_XDA_Sprite;
    internal ISpriteEntry Cell_XFA_Sprite;
    internal ISpriteEntry Cell_DFA_Sprite;
    internal ISpriteEntry Cell_XDFA_Sprite;
    internal ISpriteEntry CostSatisfiedIcon;
    internal ISpriteEntry CostUnsatisfiedIcon;
    internal ISpriteEntry AMutationIcon;
    internal ISpriteEntry DMutationIcon;
    internal ISpriteEntry FMutationIcon;
    internal ISpriteEntry XMutationIcon;
    internal ISpriteEntry MarkedIcon;
    internal ISpriteEntry CellColonyIcon;
    internal ICardTraitEntry SymbioticTrait;
    internal ISpriteEntry SymbioticIcon;
    internal IModSoundEntry HarvestSound1 { get; private set; } = null!;
    internal IModSoundEntry HarvestSound2 { get; private set; } = null!;
    internal IModSoundEntry HarvestSound3 { get; private set; } = null!;
    internal IModSoundEntry MutationSound1 { get; private set; } = null!;
    internal IModSoundEntry MutationSound2 { get; private set; } = null!;
    internal IModSoundEntry MutationSound3 { get; private set; } = null!;
    internal IModSoundEntry MutationSound4 { get; private set; } = null!;
    internal IStatusEntry CryptobiosisStatus;
    internal IStatusEntry ApoptosisStatus;
    internal ISpriteEntry BMutationIcon;
    internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }
    internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations { get; }

    private static List<Type> DougieCommonCards = [
        typeof(CellColonyCard),
        typeof(Bioweapon),
        typeof(Outbreak),
        typeof(KeratinSpike),
        typeof(Rebirth),
        typeof(Pressurize),
        typeof(Consume),
        typeof(Fatten),
        typeof(Biofuel)
    ];
    private static List<Type> DougieUnCommonCards = [
        typeof(Mutate),
        typeof(Evolve),
        typeof(FeedTheHorde),
        typeof(Hypertrophy),
        typeof(Engorge),
        typeof(LethalInjection),
        typeof(Clone)
    ];
    private static List<Type> DougieRareCards = [
        typeof(RSelection),
        typeof(Cryptobiosis),
        typeof(Apoptosis),
        typeof(Exoskeleton),
        typeof(ShadowTheHedgehog)
    ];
    private static List<Type> DougieSpecialCards = [
        typeof(MeatGrinder),
        typeof(DougieEXE)
    ];
    private static IEnumerable<Type> DougieCardTypes =
        DougieCommonCards
        .Concat(DougieUnCommonCards)
        .Concat(DougieRareCards)
        .Concat(DougieSpecialCards);

    private static List<Type> DougieCommonArtifacts = [
        typeof(ExtendoGrip),
        typeof(GrowthHormones),
        typeof(AggressionGenes),
        typeof(Polydactyly),
        typeof(FLOState)
    ];
    private static List<Type> DougieBossArtifacts = [
        typeof(HotDog),
        typeof(Parasitoid)
    ];
    private static IEnumerable<Type> DougieArtifactTypes =
        DougieCommonArtifacts
        .Concat(DougieBossArtifacts);

    private static IEnumerable<Type> AllRegisterableTypes =
        DougieCardTypes
        .Concat(DougieArtifactTypes);

    public ModEntry(IPluginPackage<IModManifest> package, IModHelper helper, ILogger logger) : base(package, helper, logger)
    {
        Instance = this;
        Harmony = new Harmony("Dougie");
        KokoroApi = helper.ModRegistry.GetApi<IKokoroApi>("Shockah.Kokoro")!.V2;

        AnyLocalizations = new JsonLocalizationProvider(
            tokenExtractor: new SimpleLocalizationTokenExtractor(),
            localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/{locale}.json").OpenRead()
        );
        Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
            new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(AnyLocalizations)
        );
        PanelSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Animation/DougiePanel.png"));
        Cell_Blank_Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cells/Cell_Blank.png"));
        Cell_A_Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cells/Cell_A.png"));
        Cell_D_Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cells/Cell_D.png"));
        Cell_DA_Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cells/Cell_DA.png"));
        Cell_DF_Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cells/Cell_DF.png"));
        Cell_DFA_Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cells/Cell_DFA.png"));
        Cell_F_Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cells/Cell_F.png"));
        Cell_FA_Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cells/Cell_FA.png"));
        Cell_X_Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cells/Cell_X.png"));
        Cell_XA_Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cells/Cell_XA.png"));
        Cell_XD_Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cells/Cell_XD.png"));
        Cell_XDA_Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cells/Cell_XDA.png"));
        Cell_XDF_Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cells/Cell_XDF.png"));
        Cell_XDFA_Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cells/Cell_XDFA.png"));
        Cell_XF_Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cells/Cell_XF.png"));
        Cell_XFA_Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cells/Cell_XFA.png"));
        CostSatisfiedIcon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/CellHarvestOn.png"));
        CostUnsatisfiedIcon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/CellHarvestOff.png"));
        AMutationIcon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/AMutation.png"));
        DMutationIcon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/DMutation.png"));
        FMutationIcon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/FMutation.png"));
        XMutationIcon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/XMutation.png"));
        MarkedIcon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/iconMarkedForDeath.png"));
        CellColonyIcon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/CellColony.png"));
        SymbioticIcon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Symbiotic.png"));
        BMutationIcon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/BMutation.png"));

        HarvestSound1 = ModEntry.Instance.Helper.Content.Audio.RegisterSound(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/sounds/Harvest_1.mp3"));
        HarvestSound2 = ModEntry.Instance.Helper.Content.Audio.RegisterSound(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/sounds/Harvest_2.mp3"));
        HarvestSound3 = ModEntry.Instance.Helper.Content.Audio.RegisterSound(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/sounds/Harvest_3.mp3"));
        MutationSound1 = ModEntry.Instance.Helper.Content.Audio.RegisterSound(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/sounds/Mutation_1.mp3"));
        MutationSound2 = ModEntry.Instance.Helper.Content.Audio.RegisterSound(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/sounds/Mutation_2.mp3"));
        MutationSound3 = ModEntry.Instance.Helper.Content.Audio.RegisterSound(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/sounds/Mutation_3.mp3"));
        MutationSound4 = ModEntry.Instance.Helper.Content.Audio.RegisterSound(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/sounds/Mutation_4.mp3"));
        DougieDeck = helper.Content.Decks.RegisterDeck("DougieDeck", new DeckConfiguration
        {
            Definition = new DeckDef
            {
                color = new Color("CC6641"),

                titleColor = new Color("000000")
            },
            DefaultCardArt = RegisterSprite(package, "assets/Card/DougieDefaultArt.png").Sprite,
            BorderSprite = RegisterSprite(package, "assets/Card/CardBorder.png").Sprite,
            Name = AnyLocalizations.Bind(["character", "name"]).Localize
        });
        SymbioticTrait = helper.Content.Cards.RegisterTrait("Symbiotic", new()
		{
			Name = this.AnyLocalizations.Bind(["trait", "Symbiotic", "name"]).Localize,
			Icon = (state, card) => SymbioticIcon.Sprite,
			Tooltips = (state, card) => [
				new GlossaryTooltip($"trait.{Instance.Package.Manifest.UniqueName}::Symbiotic")
				{
					Icon = SymbioticIcon.Sprite,
					TitleColor = Colors.cardtrait,
					Title = Localizations.Localize(["trait", "Symbiotic", "name"]),
					Description = Localizations.Localize(["trait", "Symbiotic", "description"])
				},
                new GlossaryTooltip($"{Instance.Package.Manifest.UniqueName}::Midrow::Cell")
                {
                    Icon = CellColonyIcon.Sprite,
                    TitleColor = Colors.midrow,
                    Title = Localizations.Localize(["midrow", "Cell", "name"]),
                    Description = string.Format("Will block 1 shot before being destroyed.")
                },
			]
		});
        CryptobiosisStatus = helper.Content.Statuses.RegisterStatus("Cryptobiosis", new()
        {
            Definition = new()
            {
                icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cryptobiosis.png")).Sprite,
                color = new Color("CC6641"),
                isGood = true
            },
            Name = AnyLocalizations.Bind(["status", "Cryptobiosis", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "Cryptobiosis", "description"]).Localize
        });
        ApoptosisStatus = helper.Content.Statuses.RegisterStatus("Apoptosis", new()
        {
            Definition = new()
            {
                icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Apoptosis.png")).Sprite,
                color = new Color("CC6641"),
                isGood = true
            },
            Name = AnyLocalizations.Bind(["status", "Apoptosis", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "Apoptosis", "description"]).Localize
        });
        Instance.KokoroApi!.ActionCosts.RegisterResourceCostIcon(new CellResource(), CostSatisfiedIcon.Sprite, CostUnsatisfiedIcon.Sprite);
        helper.Content.Characters.V2.RegisterPlayableCharacter("Dougie", new PlayableCharacterConfigurationV2()
        {
            Deck = DougieDeck.Deck,
            Description = AnyLocalizations.Bind(["character", "description"]).Localize,
            BorderSprite = PanelSprite.Sprite,
            SoloStarters = new()
            {
                cards = [
                    
                ]
            },
            Starters = new StarterDeck
            {
                cards = [
                    new CellColonyCard(),
                    new Bioweapon()
                ]
            },
            ExeCardType = typeof(DougieEXE)
        });

        foreach (var type in AllRegisterableTypes)
            AccessTools.DeclaredMethod(type, nameof(IRegisterable.Register))?.Invoke(null, [package, helper]);
        
        RegisterAnimation(package, "neutral", "assets/Animation/DougieNeutral_", 5);
        RegisterAnimation(package, "squint", "assets/Animation/DougieSquint_", 5);
        Instance.Helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2
        {
            CharacterType = DougieDeck.Deck.Key(),
            LoopTag = "gameover",
            Frames = [
                RegisterSprite(package, "assets/Animation/DougieDed.png").Sprite,
            ]
        });
        Instance.Helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2
        {
            CharacterType = DougieDeck.Deck.Key(),
            LoopTag = "mini",
            Frames = [
                RegisterSprite(package, "assets/Animation/MiniDougie.png").Sprite,
            ]
        });
        _ = new CryptobiosisManager();
        _ = new ApoptosisManager();
        _ = new SymbioticManager();
        _ = new CellResource();

        helper.ModRegistry.GetApi<IMoreDifficultiesApi>("TheJazMaster.MoreDifficulties", new SemanticVersion(1, 3, 0))?.RegisterAltStarters(
			deck: DougieDeck.Deck,
			starterDeck: new StarterDeck
			{
				cards = [
					new Outbreak(),
                    new KeratinSpike()
				]
			}
		);
        Instance.Harmony!.Patch(
                original: AccessTools.DeclaredMethod(typeof(Combat), nameof(Combat.IsVisible)),
                postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Combat_IsVisible_Postfix))
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
            CharacterType = Instance.DougieDeck.Deck.Key(),
            LoopTag = tag,
            Frames = Enumerable.Range(0, frames)
                .Select(i => RegisterSprite(package, dir + i + ".png").Sprite)
                .ToImmutableList()
        });
    }
    private static void Combat_IsVisible_Postfix(Combat __instance, ref bool __result)
    {
        if (__instance.routeOverride is ActionRoute)
            __result = true;
    }
    
}

