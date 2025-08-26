using Nickel;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using System;
using System.Collections.Generic;
using System.Linq;
using FMOD;
using Nickel.Common;
using FredAndRadience.Radiant_Shipyard.DialogueAdditions;
using FredAndRadience.Radiant_Shipyard.features;

namespace FredAndRadience.Radiant_Shipyard;
public sealed class ModEntry : SimpleMod
{
    internal Harmony Harmony { get; }
    internal static ModEntry Instance { get; private set; } = null!;
    internal IKokoroApi KokoroApi { get; }
    internal IEssentialsApi? EssentialsApi { get; private set; }
    internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }
    internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations { get; }
    //Uranus
    internal IShipEntry Uranus_Ship { get; }
    internal IDeckEntry Uranus_Deck { get; }
    internal ISpriteEntry Uranus_Frame { get; }
    internal ISpriteEntry Uranus_Background { get; }
    internal ISpriteEntry Uranus_NukeActive { get; }
    internal IPartEntry Uranus_Scaffold { get; }
    internal IPartEntry UranusWingL { get; }
    internal IPartEntry UranusWingR { get; }
    internal ISpriteEntry Uranus_Remnant_Right { get; }
    internal ISpriteEntry Uranus_Remnant_Middle_L { get; }
    internal ISpriteEntry Uranus_Remnant_Middle_R { get; }
    internal ISpriteEntry Uranus_Remnant_Left { get; }
    internal ISpriteEntry Uranus_Nuke_Icon { get; }
    //Cerberus
    internal IShipEntry Cerberus_Ship { get; }
    internal IPartEntry CerberusCannonInActive { get; }
    internal IPartEntry CerberusCannon0 { get; }
    internal IPartEntry CerberusCannon1 { get; }
    internal IPartEntry CerberusCannon2 { get; }
    internal IPartEntry CerberusCannon3 { get; }
    internal IPartEntry CerberusCannon4 { get; }
    internal IPartEntry CerberusCannon5 { get; }
    internal IPartEntry CerberusCannon6 { get; }
    //Venus
    internal IShipEntry Venus_Ship { get; }
    internal IPartEntry VenusCannon { get; }
    internal IPartEntry VenusMissiles { get; }
    //Mercury
    internal IShipEntry Mercury_Ship { get; }
    internal IDeckEntry Mercury_Deck { get; }
    internal ISpriteEntry Mercury_CardBackground { get; }
    internal ISpriteEntry Mercury_CardFrame { get; }
    internal IPartEntry MercuryCannon { get; }
    internal IPartEntry MercuryMissile { get; }
    //Hades
    internal IShipEntry Hades_Ship { get; }
    internal IDeckEntry Hades_Deck { get; }
    internal IStatusEntry Elec_Charge { get; }
    internal ISpriteEntry Hades_CardBackground { get; }
    internal ISpriteEntry Hades_CardFrame { get; }
    internal IPartEntry Hades_UnChargedCannon { get; }
    internal IPartEntry Hades_ChargedCannon { get; }
    internal IPartEntry Hades_InActiveCannon { get; }
    //Changeling
    internal IShipEntry Changeling_Ship { get; }
    internal IDeckEntry Changeling_Deck { get; }
    internal ISpriteEntry Changeling_CardBackground { get; }
    internal ISpriteEntry Changeling_CardFrame { get; }
    internal IPartEntry Changeling_Cannon { get; }
    internal IPartEntry Changeling_Comms { get; }
    internal IPartEntry ChangelingMissiles { get; }
    internal static IReadOnlyList<Type> Harbour_Cards { get; } = [
        typeof(CardIllegalOrdnance),
        typeof(CardShipPartShuffle),
        typeof(CardMercuryDriver),
        typeof(CardChargeCannon)
    ];
    internal static IEnumerable<Type> Ships_AllCard_Types
        => Harbour_Cards;
    internal static IReadOnlyList<Type> Harbour_Common_Artifacts { get; } = [
        typeof(ArtifactUranusCore),
        typeof(ArtifactCerberusHead),
        typeof(ArtifactVenusCore),
        typeof(ArtifactMercuryCore),
        typeof(ArtifactHadesCannon),
        typeof(ArtifactChangelingCore)
    ];
    internal static IReadOnlyList<Type> Harbour_Boss_Artifacts { get; } = [
        typeof(ArtifactUranusCore2),
        typeof(ArtifactCerberusArmor),
        typeof(ArtifactVenusCore2),
        typeof(ArtifactMercuryCore2),
        typeof(ArtifactTartarusRage),
        typeof(ArtifactRenovations)
    ];
    internal static IEnumerable<Type> Harbour_AllArtifacts
        => Harbour_Common_Artifacts
        .Concat(Harbour_Boss_Artifacts);
    public ModEntry(IPluginPackage<IModManifest> package, IModHelper helper, ILogger logger) : base(package, helper, logger)
    {
        Instance = this;
        Harmony = new(package.Manifest.UniqueName);
        Harmony.PatchAll();
        Harmony.Patch(
			original: typeof(MG).GetMethod("DrawLoadingScreen", AccessTools.all),
			prefix: new HarmonyMethod(typeof(StoryVarsAdditions), nameof(StoryVarsAdditions.DrawLoadingScreen_Prefix)),
			postfix: new HarmonyMethod(typeof(StoryVarsAdditions), nameof(StoryVarsAdditions.DrawLoadingScreen_Postfix))
		);
        KokoroApi = helper.ModRegistry.GetApi<IKokoroApi>("Shockah.Kokoro")!;
        _ = new ElecChargeManager();
        CustomTTGlossary.ApplyPatches(Harmony);
        AnyLocalizations = new JsonLocalizationProvider(
            tokenExtractor: new SimpleLocalizationTokenExtractor(),
            localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/{locale}.json").OpenRead()
        );
        Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
            new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(AnyLocalizations)
        );
        //Uranus
        Uranus_Background = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Uranus/card_BG_Illegal_Ordnance.png"));
        Uranus_Frame = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Uranus/card_border_Uranus.png"));
        Uranus_NukeActive = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Uranus/nuke.png"));
        Uranus_Remnant_Middle_L = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Uranus/Uranus_wing_remnant_L_middle.png"));
        Uranus_Remnant_Middle_R = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Uranus/Uranus_wing_remnant_R_middle.png"));
        Uranus_Remnant_Right = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Uranus/Uranus_wing_remnant_side.png"));
        Uranus_Remnant_Left = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Uranus/Uranus_wing_remnant_side2.png"));
        Uranus_Nuke_Icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Uranus/Uranus_Nuke_Icon.png"));
        //Mercury
        Mercury_CardBackground = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Mercury/card_BG_Mercury_Driver.png"));
        Mercury_CardFrame = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Mercury/card_border_Mercury.png"));
        //Hades
        Hades_CardBackground = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Hades/HadesCardBG.png"));
        Hades_CardFrame = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Hades/HadesCardBorder.png"));
        //Changeling
        Changeling_CardBackground = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Changeling/card_BG_Shuffle_Parts.png"));
        Changeling_CardFrame = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Changeling/card_border_Changeling.png"));
        //Uranus deck
        Uranus_Deck = helper.Content.Decks.RegisterDeck("UranusDeck", new DeckConfiguration()
        {
            Definition = new DeckDef()
            {
                color = new Color("bfbfbf"),
                titleColor = new Color("ffffff"),
            },
            DefaultCardArt = Uranus_Background.Sprite,
            BorderSprite = Uranus_Frame.Sprite,
            Name = AnyLocalizations.Bind(["ship", "Uranus", "name"]).Localize,
        });
        //Mercury Deck
        Mercury_Deck = helper.Content.Decks.RegisterDeck("MercuryDeck", new DeckConfiguration()
        {
            Definition = new DeckDef()
            {
                color = new Color("ff988c"),
                titleColor = new Color("000000"),
            },
            DefaultCardArt = Mercury_CardBackground.Sprite,
            BorderSprite = Mercury_CardFrame.Sprite,
            Name = AnyLocalizations.Bind(["ship", "Mercury", "name"]).Localize,
        });
        //Hades Deck
        Hades_Deck = helper.Content.Decks.RegisterDeck("HadesDeck", new DeckConfiguration()
        {
            Definition = new DeckDef()
            {
                color = new Color("00C4EB"),
                titleColor = new Color("000000"),
            },
            DefaultCardArt = Hades_CardBackground.Sprite,
            BorderSprite = Hades_CardFrame.Sprite,
            Name = AnyLocalizations.Bind(["ship", "Hades", "name"]).Localize,
        });
        //Hades Status
        Elec_Charge = helper.Content.Statuses.RegisterStatus("E_Charge", new()
        {
            Definition = new()
            {
                icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Hades/icon_Hades_Charge.png")).Sprite,
                color = new Color("54beff"),
                isGood = true
            },
            Name = this.AnyLocalizations.Bind(["status", "E_Charge", "name"]).Localize,
            Description = this.AnyLocalizations.Bind(["status", "E_Charge", "description"]).Localize  
        });
        //Changeling Deck
        Changeling_Deck = helper.Content.Decks.RegisterDeck("ChangelingDeck", new DeckConfiguration()
        {
            Definition = new DeckDef()
            {
                color = new Color("7c48a9"),
                titleColor = new Color("000000"),
            },
            DefaultCardArt = Changeling_CardBackground.Sprite,
            BorderSprite = Changeling_CardFrame.Sprite,
            Name = AnyLocalizations.Bind(["ship", "Changeling", "name"]).Localize,
        });
        CustomTTGlossary.ApplyPatches(Harmony);
        this.AnyLocalizations = new JsonLocalizationProvider(
			tokenExtractor: new SimpleLocalizationTokenExtractor(),
			localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/{locale}.json").OpenRead()
		);
		this.Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
			new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(this.AnyLocalizations)
		);
        foreach (var CardType in Ships_AllCard_Types)
            AccessTools.DeclaredMethod(CardType, nameof(IRadiantCard.Register))?.Invoke(null, [package,helper]);
        foreach (var ArtifactType in Harbour_AllArtifacts)
            AccessTools.DeclaredMethod(ArtifactType, nameof(IRadiantArtifact.Register))?.Invoke(null, [package,helper]);
        //Uranus Parts
        var UranusCockpit = helper.Content.Ships.RegisterPart("Uranus.Cockpit", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Uranus/Uranus_cockpit.png")).Sprite
        });
        var UranusCannon = helper.Content.Ships.RegisterPart("Uranus.Cannon", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Uranus/Uranus_cannon.png")).Sprite
        });
        var UranusMissiles = helper.Content.Ships.RegisterPart("Uranus.Missiles", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Uranus/Uranus_missiles.png")).Sprite
        });
        Uranus_Scaffold = helper.Content.Ships.RegisterPart("Uranus.Scaffold", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Uranus/Uranus_scaffold.png")).Sprite
        });
        UranusWingL = helper.Content.Ships.RegisterPart("Uranus.WingL", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Uranus/Uranus_wing_L.png")).Sprite
        });
        UranusWingR = helper.Content.Ships.RegisterPart("Uranus.WingR", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Uranus/Uranus_wing_R.png")).Sprite
        });
        var UranusChasis = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Uranus/Uranus_chassis.png")).Sprite;
        //Cerberus Parts
        var CerberusCockpit = helper.Content.Ships.RegisterPart("Cerberus.Cockpit", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cerberus/Cerberus_cockpit.png")).Sprite
        });
        CerberusCannonInActive = helper.Content.Ships.RegisterPart("Cerberus.CannonI", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cerberus/C_inactive_cannon.png")).Sprite,
            DisabledSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cerberus/C_inactive_cannon_re.png")).Sprite,
        });
        CerberusCannon0 = helper.Content.Ships.RegisterPart("Cerberus.Cannon0", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cerberus/C_active_cannon_0.png")).Sprite,
        });
        CerberusCannon1 = helper.Content.Ships.RegisterPart("Cerberus.Cannon1", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cerberus/C_active_cannon_I.png")).Sprite,
        });
        CerberusCannon2 = helper.Content.Ships.RegisterPart("Cerberus.Cannon2", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cerberus/C_active_cannon_II.png")).Sprite,
        });
        CerberusCannon3 = helper.Content.Ships.RegisterPart("Cerberus.Cannon3", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cerberus/C_active_cannon_III.png")).Sprite,
        });
        CerberusCannon4 = helper.Content.Ships.RegisterPart("Cerberus.Cannon4", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cerberus/C_active_cannon_IV.png")).Sprite,
        });
        CerberusCannon5 = helper.Content.Ships.RegisterPart("Cerberus.Cannon5", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cerberus/C_active_cannon_V.png")).Sprite,
        });
        CerberusCannon6 = helper.Content.Ships.RegisterPart("Cerberus.Cannon6", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cerberus/C_active_cannon_VI.png")).Sprite,
        });
        var CerberusMissiles = helper.Content.Ships.RegisterPart("Cerberus.Missiles", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cerberus/Cerberus_missiles.png")).Sprite,
        });
        var CerberusScaffold = helper.Content.Ships.RegisterPart("Cerberus.Scaffold", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cerberus/Cerberus_scaffold.png")).Sprite,
        });
        var CerberusChasis = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Cerberus/Cerberus_chassis.png")).Sprite;
        //Venus Parts
        var VenusCockpit = helper.Content.Ships.RegisterPart("Venus.Cockpit", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Venus/Venus_cockpit.png")).Sprite
        });
        VenusCannon = helper.Content.Ships.RegisterPart("Venus.Cannon", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Venus/Venus_cannon.png")).Sprite
        });
        VenusMissiles = helper.Content.Ships.RegisterPart("Venus.Missiles", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Venus/Venus_missiles.png")).Sprite
        });
        var VenusWing = helper.Content.Ships.RegisterPart("Venus.Wing", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Venus/Venus_wing.png")).Sprite
        });
        var VenusChasis = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Venus/Venus_chassis.png")).Sprite;
        //Mercury Parts
        var MercuryCockpit = helper.Content.Ships.RegisterPart("Mercury.Cockpit", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Mercury/Mercury_cockpit.png")).Sprite
        });
        MercuryCannon = helper.Content.Ships.RegisterPart("Mercury.Cannon", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Mercury/Mercury_cannon_active.png")).Sprite,
            DisabledSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Mercury/Mercury_cannon_inactive.png")).Sprite,
        });
        MercuryMissile = helper.Content.Ships.RegisterPart("Mercury.Missiles", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Mercury/Mercury_missiles_active.png")).Sprite,
            DisabledSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Mercury/Mercury_missiles_inactive.png")).Sprite,
        });
        var MercuryChasis = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Mercury/Mercury_chassis.png")).Sprite;
        //Hades Parts
        var HadesCockpit = helper.Content.Ships.RegisterPart("Hades.Cockpit", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Hades/Hades_cockpit.png")).Sprite
        });
        var HadesWing = helper.Content.Ships.RegisterPart("Hades.Wing", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Hades/Hades_wing.png")).Sprite
        });
        var HadesMissiles = helper.Content.Ships.RegisterPart("Hades.Missiles", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Hades/Hades_missiles.png")).Sprite
        });
        Hades_UnChargedCannon = helper.Content.Ships.RegisterPart("Hades.CannonU", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Hades/Hades_cannon_active_uncharged.png")).Sprite
        });
        var HadesScaffold = helper.Content.Ships.RegisterPart("Hades.Empty", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Hades/Hades_scaffold.png")).Sprite
        });
        Hades_ChargedCannon = helper.Content.Ships.RegisterPart("Hades.CannonC", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Hades/Hades_cannon_active_charged.png")).Sprite
        });
        Hades_InActiveCannon = helper.Content.Ships.RegisterPart("Hades.CannonI", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Hades/Hades_cannon_inactive.png")).Sprite
        });
        var HadesChasis = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Hades/Hades_chassis.png")).Sprite;
        //Changeling Parts
        var ChangelingCockpit = helper.Content.Ships.RegisterPart("Changeling.Cockpit", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Changeling/Changeling_cockpit.png")).Sprite
        });
        ChangelingMissiles = helper.Content.Ships.RegisterPart("Changeling.Missiles", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Changeling/Changeling_missiles.png")).Sprite
        });
        var ChangelingWing = helper.Content.Ships.RegisterPart("Changeling.Wing", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Changeling/Changeling_wing.png")).Sprite
        });
        Changeling_Cannon = helper.Content.Ships.RegisterPart("Changeling.Cannon", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Changeling/Changeling_cannon.png")).Sprite
        });
        Changeling_Comms = helper.Content.Ships.RegisterPart("Changeling.Comms", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Changeling/Changeling_comms.png")).Sprite
        });
        var ChangelingScaffold = helper.Content.Ships.RegisterPart("Changeling.Empty", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Changeling/Changeling_scaffold.png")).Sprite
        });
        var ChangelingChasis = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Changeling/Changeling_chassis_B.png")).Sprite;
        //Uranus
        Uranus_Ship = helper.Content.Ships.RegisterShip("Uranus", new ShipConfiguration()
        {
            Ship = new StarterShip()
            {
                ship = new Ship()
                {
                    hull = 6,
                    hullMax = 6,
                    shieldMaxBase = 6,
                    parts =
                    {
                        new Part
                        {
                            type = PType.cockpit,
                            skin = UranusCockpit.UniqueName
                        },
                        new Part
                        {
                            type = PType.wing,
                            skin = UranusWingL.UniqueName,
                            key = "LeftComp"
                        },
                        new Part
                        {
                            type = PType.cannon,
                            skin = UranusCannon.UniqueName
                        },
                        new Part
                        {
                            type = PType.wing,
                            skin = UranusWingR.UniqueName,
                            key = "RightComp"
                        },
                        new Part
                        {
                            type = PType.missiles,
                            skin = UranusMissiles.UniqueName
                        }
                    }
                },
                cards =
                {
                    new BasicShieldColorless(),
                    new DodgeColorless(),
                    new CannonColorless(),
                    new CannonColorless()
                },
                artifacts =
                {
                    new ShieldPrep(),
                    new ArtifactUranusCore()
                }
            },
            ExclusiveArtifactTypes = new HashSet<Type>()
            {
                typeof(ArtifactUranusCore),
                typeof(ArtifactUranusCore2)
            },
            UnderChassisSprite = UranusChasis,
            Name = AnyLocalizations.Bind(["ship", "Uranus", "name"]).Localize,
            Description = AnyLocalizations.Bind(["ship", "Uranus", "description"]).Localize
        });
        //Cerberus
        Cerberus_Ship = helper.Content.Ships.RegisterShip("Cerberus", new ShipConfiguration()
        {
            Ship = new StarterShip()
            {
                ship = new Ship()
                {
                    hull = 9,
                    hullMax = 9,
                    shieldMaxBase = 4,
                    parts =
                    {
                        new Part
                        {
                            type = PType.cannon,
                            active = false,
                            skin = CerberusCannonInActive.UniqueName,
                            uuid = 1,
                            key = "CerbLeft"
                        },
                        new Part
                        {
                            type = PType.cockpit,
                            skin = CerberusCockpit.UniqueName,
                        },
                        new Part
                        {
                            type = PType.empty,
                            skin = CerberusScaffold.UniqueName,
                            flip = true
                        },
                        new Part
                        {
                            type = PType.cannon,
                            active = true,
                            skin = CerberusCannon0.UniqueName,
                            uuid = 2,
                            key = "CerbMiddle"
                        },
                        new Part
                        {
                            type = PType.empty,
                            skin = CerberusScaffold.UniqueName,
                        },
                        new Part
                        {
                            type = PType.missiles,
                            skin = CerberusMissiles.UniqueName,
                        },
                        new Part
                        {
                            type = PType.cannon,
                            active = false,
                            skin = CerberusCannonInActive.UniqueName,
                            uuid = 3,
                            key = "CerbRight"
                        },
                    }
                },
                cards =
                {
                    new BasicShieldColorless(),
                    new DodgeColorless(),
                    new CannonColorless(),
                    new CannonColorless(),
                    new BasicSpacer()
                },
                artifacts =
                {
                    new ShieldPrep(),
                    new ArtifactCerberusHead()
                }
            },
            ExclusiveArtifactTypes = new HashSet<Type>()
            {
                typeof(ArtifactCerberusHead),
                typeof(ArtifactCerberusArmor)
            },
            UnderChassisSprite = CerberusChasis,
            Name = AnyLocalizations.Bind(["ship", "Cerberus", "name"]).Localize,
            Description = AnyLocalizations.Bind(["ship", "Cerberus", "description"]).Localize
        });
        //Venus
        Venus_Ship = helper.Content.Ships.RegisterShip("Venus", new ShipConfiguration()
        {
            Ship = new StarterShip()
            {
                ship = new Ship()
                {
                    hull = 9,
                    hullMax = 9,
                    shieldMaxBase = 4,
                    parts =
                    {
                        new Part
                        {
                            type = PType.missiles,
                            skin = VenusMissiles.UniqueName,
                            key = "LeftPart"
                        },
                        new Part
                        {
                            type = PType.wing,
                            skin = VenusWing.UniqueName,
                            flip = true,
                            key = "VenusLeftWing"
                        },
                        new Part
                        {
                            type = PType.cockpit,
                            skin = VenusCockpit.UniqueName,
                            key = "Venus_Cockpit"
                        },
                        new Part
                        {
                            type = PType.wing,
                            skin = VenusWing.UniqueName,
                            key = "VenusRightWing"
                        },
                        new Part
                        {
                            type = PType.missiles,
                            skin = VenusMissiles.UniqueName,
                            key = "RightPart"
                        },
                    }
                },
                cards =
                {
                    new BasicShieldColorless(),
                    new DodgeColorless(),
                    new CannonColorless(),
                    new CannonColorless(),
                    new BasicSpacer()
                },
                artifacts =
                {
                    new ShieldPrep(),
                    new ArtifactVenusCore()
                }
            },
            ExclusiveArtifactTypes = new HashSet<Type>()
            {
                typeof(ArtifactVenusCore),
                typeof(ArtifactVenusCore2)
            },
            UnderChassisSprite = VenusChasis,
            Name = AnyLocalizations.Bind(["ship", "Venus", "name"]).Localize,
            Description = AnyLocalizations.Bind(["ship", "Venus", "description"]).Localize
        });
        //Mercury
        Mercury_Ship = helper.Content.Ships.RegisterShip("Mercury", new ShipConfiguration()
        {
            Ship = new StarterShip()
            {
                ship = new Ship()
                {
                    hull = 12,
                    hullMax = 12,
                    shieldMaxBase = 4,
                    parts =
                    {
                        new Part
                        {
                            type = PType.missiles,
                            skin = MercuryMissile.UniqueName,
                            active = false,
                            key = "LeftMissile"
                        },
                        new Part
                        {
                            type = PType.cannon,
                            skin = MercuryCannon.UniqueName,
                            flip = true,
                            active = false,
                            key = "LeftCannon"
                        },
                        new Part
                        {
                            type = PType.cockpit,
                            skin = MercuryCockpit.UniqueName,
                        },
                        new Part
                        {
                            type = PType.cannon,
                            skin = MercuryCannon.UniqueName,
                            active = false,
                            key = "RightCannon"
                        },
                        new Part
                        {
                            type = PType.missiles,
                            skin = MercuryMissile.UniqueName,
                            active = false,
                            key = "RightMissile"
                        },
                    }
                },
                cards =
                {
                    new BasicShieldColorless(),
                    new DodgeColorless(),
                    new CannonColorless(),
                    new CannonColorless(),
                },
                artifacts =
                {
                    new ShieldPrep(),
                    new ArtifactMercuryCore()
                }
            },
            ExclusiveArtifactTypes = new HashSet<Type>()
            {
                typeof(ArtifactMercuryCore),
                typeof(ArtifactMercuryCore2)
            },
            UnderChassisSprite = MercuryChasis,
            Name = AnyLocalizations.Bind(["ship", "Mercury", "name"]).Localize,
            Description = AnyLocalizations.Bind(["ship", "Mercury", "description"]).Localize
        });
        //Hades
        Hades_Ship = helper.Content.Ships.RegisterShip("Hades", new ShipConfiguration()
        {
            Ship = new StarterShip()
            {
                ship = new Ship()
                {
                    hull = 10,
                    hullMax = 10,
                    shieldMaxBase = 3,
                    parts =
                    {
                        new Part
                        {
                            type = PType.wing,
                            skin = HadesWing.UniqueName,
                            flip = true
                        },
                        new Part
                        {
                            type = PType.cannon,
                            skin = Hades_UnChargedCannon.UniqueName,
                            key = "HadesRailGun"
                        },
                        new Part
                        {
                            type = PType.cockpit,
                            skin = HadesCockpit.UniqueName
                        },
                        new Part
                        {
                            type = PType.empty,
                            skin = HadesScaffold.UniqueName
                        },
                        new Part
                        {
                            type = PType.missiles,
                            skin = HadesMissiles.UniqueName,
                        },
                        new Part
                        {
                            type = PType.wing,
                            skin = HadesWing.UniqueName
                        }
                    }
                },
                cards =
                {
                    new BasicShieldColorless(),
                    new BasicShieldColorless(),
                    new DodgeColorless(),
                    new CannonColorless(),
                },
                artifacts =
                {
                    new ShieldPrep(),
                    new ArtifactHadesCannon()
                }
            },
            ExclusiveArtifactTypes = new HashSet<Type>()
            {
                typeof(ArtifactHadesCannon),
                typeof(ArtifactTartarusRage)
            },
            UnderChassisSprite = HadesChasis,
            Name = AnyLocalizations.Bind(["ship", "Hades", "name"]).Localize,
            Description = AnyLocalizations.Bind(["ship", "Hades", "description"]).Localize
        });
        //Changeling
        Changeling_Ship = helper.Content.Ships.RegisterShip("Changeling", new ShipConfiguration()
        {
            Ship = new StarterShip()
            {
                ship = new Ship()
                {
                    hull = 16,
                    hullMax = 16,
                    shieldMaxBase = 4,
                    parts =
                    {
                        new Part
                        {
                            type = PType.wing,
                            skin = ChangelingWing.UniqueName,
                            damageModifier = PDamMod.armor,
                            key = "ChangelingLeftWing"
                        },
                        new Part
                        {
                            type = PType.cockpit,
                            skin = ChangelingCockpit.UniqueName,
                            key = "ChangelingCockpit"
                        },
                        new Part
                        {
                            type = PType.cannon,
                            skin = Changeling_Cannon.UniqueName,
                            key = "ChangelingCannon1"
                        },
                        new Part 
                        {
                            type = PType.empty,
                            skin = ChangelingScaffold.UniqueName
                        },
                        new Part
                        {
                            type = PType.missiles,
                            skin = ChangelingMissiles.UniqueName,
                            key = "ChangelingMissiles"
                        },
                        new Part
                        {
                            type = PType.comms,
                            damageModifier = PDamMod.weak,
                            skin = Changeling_Comms.UniqueName,
                            key = "ChangelingComms1"
                        },
                        new Part
                        {
                            type = PType.wing,
                            damageModifier = PDamMod.armor,
                            key = "ChangelingRightWing",
                            flip = true,
                            skin = ChangelingWing.UniqueName
                        }
                    }
                },
                cards =
                {
                    new BasicShieldColorless(),
                    new DodgeColorless(),
                    new CannonColorless(),
                    new CannonColorless(),
                },
                artifacts =
                {
                    new ShieldPrep(),
                    new ArtifactChangelingCore()
                }
            },
            ExclusiveArtifactTypes = new HashSet<Type>()
            {
                typeof(ArtifactChangelingCore),
                typeof(ArtifactRenovations)
            },
            UnderChassisSprite = ChangelingChasis,
            Name = AnyLocalizations.Bind(["ship", "Changeling", "name"]).Localize,
            Description = AnyLocalizations.Bind(["ship", "Changeling", "description"]).Localize
        });
    }
    
}
