using Nickel;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using System;
using System.Collections.Generic;
using System.Linq;
using Fred.AbandonedShipyard.cards;

namespace Fred.AbandonedShipyard;
public sealed class ModEntry : SimpleMod
{
    internal Harmony Harmony { get; }
    internal static ModEntry Instance { get; private set; } = null!;
    internal IKokoroApi KokoroApi { get; }
    internal IEssentialsApi? EssentialsApi { get; private set; }
    internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }
    internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations { get; }
    //Foundry
    internal IShipEntry Foundry_Ship { get; }
    //Corsair
    internal IShipEntry Corsair_Ship { get; }
    //Vanguard
    internal IShipEntry Vanguard_Ship { get; }
    //
    internal IShipEntry Chrysalis_Ship { get; }
    internal ISpriteEntry Chrysalis_Right_Wing { get; }
    internal IPartEntry ChrysEmpty { get; }
    internal static IReadOnlyList<Type> Abandoned_Starter_Artifacts { get; } = [
        typeof(HeartOfFoundry),
        typeof(AutoAssembly),
        typeof(CorsairEngines),
        typeof(LastStand),
        typeof(ModuleStealer)
    ];
    internal static IReadOnlyList<Type> Module_Artifacts { get; } = [
        typeof(EmptyModule),
        typeof(Cicada1Module),
        typeof(FireballModule),
        typeof(StingerModule),
        typeof(PupaModule),
        typeof(MonarchModule),
        typeof(MineSweeperModule),
        typeof(StagModule),
        typeof(JumboModule),
        typeof(WizboModule),
        typeof(GnatModule),
        typeof(NeedlerModule),
        typeof(Cicada2Module),
        typeof(BruiserModule),
        typeof(SolarModule),
        typeof(StardogModule),
        typeof(RailCannonModule),
        typeof(GoliathModule),
        typeof(CannoneerModule),
        typeof(SpikeJrModule),
        typeof(Cicada3Module),
        typeof(ReplicatorModule),
        typeof(ScarabModule),
        typeof(IonRangerModule),
        typeof(AquaticModule),
        typeof(CrabModule),
        typeof(DrillModule),
        typeof(RustingModule),
        typeof(BuriedModule),
        typeof(HopperModule)
    ];
    internal static IReadOnlyList<Type> Abandoned_Boss_Artifacts { get; } = [
        typeof(CorsairDrive),
        typeof(DefenseProtocol)
    ];
    internal static IReadOnlyList<Type> Abandoned_Cards { get; } = [
        typeof(BasicMissileCard),
        typeof(BasicDroneCard)
    ];
    internal static IEnumerable<Type> Abandoned_AllCards
        => Abandoned_Cards;
    internal static IEnumerable<Type> Abandoned_AllArtifacts
        => Abandoned_Starter_Artifacts
        .Concat(Module_Artifacts)
        .Concat(Abandoned_Boss_Artifacts);
    public ModEntry(IPluginPackage<IModManifest> package, IModHelper helper, ILogger logger) : base(package, helper, logger)
    {
        Instance = this;
        Harmony = new(package.Manifest.UniqueName);
        Harmony.PatchAll();
        KokoroApi = helper.ModRegistry.GetApi<IKokoroApi>("Shockah.Kokoro")!;
        CustomTTGlossary.ApplyPatches(Harmony);
        AnyLocalizations = new JsonLocalizationProvider(
            tokenExtractor: new SimpleLocalizationTokenExtractor(),
            localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/{locale}.json").OpenRead()
        );
        Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
            new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(AnyLocalizations)
        );
        CustomTTGlossary.ApplyPatches(Harmony);
        this.AnyLocalizations = new JsonLocalizationProvider(
			tokenExtractor: new SimpleLocalizationTokenExtractor(),
			localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/{locale}.json").OpenRead()
		);
		this.Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
			new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(this.AnyLocalizations)
		);
        Chrysalis_Right_Wing = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Chrysalis/Chrysalis_RightWing.png"));
        foreach (var ArtifactType in Abandoned_AllArtifacts)
            AccessTools.DeclaredMethod(ArtifactType, nameof(IAbandonedArtifact.Register))?.Invoke(null, [package,helper]);
        foreach (var CardType in Abandoned_AllCards)
            AccessTools.DeclaredMethod(CardType, nameof(IAbandonedCard.Register))?.Invoke(null, [package,helper]);
        //Foundry Parts
        var FoundryWing = helper.Content.Ships.RegisterPart("Foundry.Wing", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Foundry/Foundry_Wing.png")).Sprite
        });
        var FoundryCockpit = helper.Content.Ships.RegisterPart("Foundry.Cockpit", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Foundry/Foundry_Cockpit.png")).Sprite
        });
        var FoundryMissile = helper.Content.Ships.RegisterPart("Foundry.Missile", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Foundry/Foundry_Missiles.png")).Sprite
        });
        var FoundryReactor = helper.Content.Ships.RegisterPart("Foundry.Special", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Foundry/Foundry_Comms.png")).Sprite
        });
        var FoundryCannon = helper.Content.Ships.RegisterPart("Foundry.Cannon", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Foundry/Foundry_Cannon.png")).Sprite
        });
        var FoundryChassis = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Foundry/Foundry_Chassis.png")).Sprite;
        //The Corsair
        var CorsairWing = helper.Content.Ships.RegisterPart("Corsair.WingL", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Corsair/Corsair_Wing.png")).Sprite
        });
        var CorsairMissile = helper.Content.Ships.RegisterPart("Corsair.Missile", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Corsair/Corsair_Missiles.png")).Sprite
        });
        var CorsairCockpit = helper.Content.Ships.RegisterPart("Corsair.Cockpit", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Corsair/Corsair_Cockpit.png")).Sprite
        });
        var CorsairCannon = helper.Content.Ships.RegisterPart("Corsair.Cannon", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Corsair/Corsair_Cannon.png")).Sprite
        });
        var CorsairScaffold = helper.Content.Ships.RegisterPart("Corsair.Empty", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Corsair/Corsair_Scaffold.png")).Sprite
        });
        var CorsairChassis = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Corsair/Corsair_Chassis.png")).Sprite;
        //The Vanguard
        var VanWing = helper.Content.Ships.RegisterPart("Van.Wing", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Vanguard/Vanguard_Wing.png")).Sprite
        });
        var VanCockpit = helper.Content.Ships.RegisterPart("Van.Cockpit", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Vanguard/Vanguard_Cockpit.png")).Sprite
        });
        var VanCannon = helper.Content.Ships.RegisterPart("Van.Cannon", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Vanguard/Vanguard_Cannon.png")).Sprite
        });
        var VanMissile = helper.Content.Ships.RegisterPart("Van.Missile", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Vanguard/Vanguard_Missiles.png")).Sprite
        });
        var VanSpecial = helper.Content.Ships.RegisterPart("Van.Special", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Vanguard/Vanguard_Special.png")).Sprite
        });
        var VanChassis = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Vanguard/Vanguard_Chassis.png")).Sprite;
        //Chrysalis
        var ChrysCockpit = helper.Content.Ships.RegisterPart("Chrys.Cockpit", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Chrysalis/Chrysalis_Cockpit.png")).Sprite
        });
        var ChrysCannon1 = helper.Content.Ships.RegisterPart("Chrys.Cannon", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Chrysalis/Chrysalis_Cannon.png")).Sprite
        });
        var ChrysCannon2 = helper.Content.Ships.RegisterPart("Chrys.Cannon2", new PartConfiguration()
        {
            DisabledSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Chrysalis/Chrysalis_Cannon2_Broken.png")).Sprite,
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Chrysalis/Chrysalis_Cannon2_Fixed.png")).Sprite
        });
        var ChrysWing = helper.Content.Ships.RegisterPart("Chrys.WingR", new PartConfiguration()
        {
            DisabledSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Chrysalis/Chrysalis_Wing.png")).Sprite,
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Chrysalis/Chrysalis_RightWing.png")).Sprite
        });
        var ChrysWing2 = helper.Content.Ships.RegisterPart("Chrys.WingL", new PartConfiguration()
        {
            DisabledSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Chrysalis/Chrysalis_Wing2.png")).Sprite,
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Chrysalis/Chrysalis_LeftWing.png")).Sprite
        });
        var ChrysMissile = helper.Content.Ships.RegisterPart("Chrys.Missile", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Chrysalis/Chrysalis_Missile.png")).Sprite
        });
        var ChrysMissile2 = helper.Content.Ships.RegisterPart("Chrys.Missile2", new PartConfiguration()
        {
            DisabledSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Chrysalis/Chrysalis_Missile2_Broken.png")).Sprite,
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Chrysalis/Chrysalis_Missile2_Fixed.png")).Sprite
        });
        ChrysEmpty = helper.Content.Ships.RegisterPart("Chrys.Empty", new PartConfiguration()
        {
            Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Chrysalis/Chrysalis_Empty.png")).Sprite
        });
        var ChrysChassis = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Chrysalis/Chrysalis_Chassis.png")).Sprite;
        //The Foundry
        Foundry_Ship = helper.Content.Ships.RegisterShip("Foundry", new ShipConfiguration()
        {
            Ship = new StarterShip()
            {
                ship = new Ship()
                {
                    hull = 17,
                    hullMax = 19,
                    shieldMaxBase = 3,
                    parts =
                    {
                        new Part
                        {
                            type = PType.wing,
                            skin = FoundryWing.UniqueName
                        },
                        new Part
                        {
                            type = PType.cockpit,
                            skin = FoundryCockpit.UniqueName
                        },
                        new Part
                        {
                            type = PType.comms,
                            skin = FoundryReactor.UniqueName,
                            damageModifier = PDamMod.weak
                        },
                        new Part
                        {
                            type = PType.missiles,
                            skin = FoundryMissile.UniqueName
                        },
                        new Part
                        {
                            type = PType.cannon,
                            skin = FoundryCannon.UniqueName
                        },
                        new Part
                        {
                            flip = true,
                            type = PType.wing,
                            skin = FoundryWing.UniqueName
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
                    new HeartOfFoundry(),
                    new AutoAssembly()
                }
            },
            ExclusiveArtifactTypes = new HashSet<Type>()
            {
                typeof(HeartOfFoundry),
                typeof(AutoAssembly)
            },
            UnderChassisSprite = FoundryChassis,
            Name = AnyLocalizations.Bind(["ship", "Foundry", "name"]).Localize,
            Description = AnyLocalizations.Bind(["ship", "Foundry", "description"]).Localize
        });
        //The Corsair
        Corsair_Ship = helper.Content.Ships.RegisterShip("Corsair", new ShipConfiguration()
        {
            Ship = new StarterShip()
            {
                ship = new Ship()
                {
                    hull = 9,
                    hullMax = 9,
                    shieldMaxBase = 3,
                    parts =
                    {
                        new Part
                        {
                            type = PType.wing,
                            skin = CorsairWing.UniqueName,
                            damageModifier = PDamMod.armor
                        },
                        new Part
                        {
                            type = PType.wing,
                            skin = CorsairWing.UniqueName,
                            flip = true,
                        },
                        new Part
                        {
                            type = PType.empty,
                            skin = CorsairScaffold.UniqueName,
                        },
                        new Part
                        {
                            type = PType.missiles,
                            skin = CorsairMissile.UniqueName,
                        },
                        new Part
                        {
                            type = PType.cockpit,
                            skin = CorsairCockpit.UniqueName,
                        },
                        new Part
                        {
                            type = PType.cannon,
                            skin = CorsairCannon.UniqueName,
                        },
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
                    new CorsairEngines()
                }
            },
            ExclusiveArtifactTypes = new HashSet<Type>()
            {
                typeof(CorsairEngines),
                typeof(CorsairDrive)
            },
            UnderChassisSprite = CorsairChassis,
            Name = AnyLocalizations.Bind(["ship", "Corsair", "name"]).Localize,
            Description = AnyLocalizations.Bind(["ship", "Corsair", "description"]).Localize
        });
        //Vanguard
        Vanguard_Ship = helper.Content.Ships.RegisterShip("Vanguard", new ShipConfiguration()
        {
            Ship = new StarterShip()
            {
                ship = new Ship()
                {
                    hull = 13,
                    hullMax = 13,
                    shieldMaxBase = 5,
                    parts =
                    {
                        new Part
                        {
                            type = PType.wing,
                            skin = VanWing.UniqueName
                        },
                        new Part
                        {
                            type = PType.special,
                            skin = VanSpecial.UniqueName,
                            damageModifier = PDamMod.armor
                        },
                        new Part
                        {
                            type = PType.cockpit,
                            skin = VanCockpit.UniqueName
                        },
                        new Part
                        {
                            type = PType.missiles,
                            skin = VanMissile.UniqueName
                        },
                        new Part
                        {
                            type = PType.cannon,
                            skin = VanCannon.UniqueName
                        },
                        new Part
                        {
                            type = PType.special,
                            skin = VanSpecial.UniqueName,
                            damageModifier = PDamMod.armor,
                            flip = true
                        },
                        new Part
                        {
                            type = PType.wing,
                            skin = VanWing.UniqueName,
                            flip = true
                        },
                    }
                },
                cards =
                {
                    new BasicShieldColorless(),
                    new BasicShieldColorless(),
                    new BasicShieldColorless(),
                    new CannonColorless(),
                },
                artifacts =
                {
                    new ShieldPrep(),
                    new LastStand()
                }
            },
            ExclusiveArtifactTypes = new HashSet<Type>()
            {
                typeof(LastStand),
                typeof(DefenseProtocol)
            },
            UnderChassisSprite = VanChassis,
            Name = AnyLocalizations.Bind(["ship", "Vanguard", "name"]).Localize,
            Description = AnyLocalizations.Bind(["ship", "Vanguard", "description"]).Localize
        });
        //Chrysalis
        Chrysalis_Ship = helper.Content.Ships.RegisterShip("Chrysalis", new ShipConfiguration()
        {
            Ship = new StarterShip()
            {
                ship = new Ship()
                {
                    hull = 13,
                    hullMax = 13,
                    shieldMaxBase = 2,
                    parts =
                    {
                        new Part
                        {
                            type = PType.wing,
                            skin = ChrysWing2.UniqueName,
                            flip = false,
                            active = false
                        },
                        new Part
                        {
                            type = PType.cannon,
                            skin = ChrysCannon1.UniqueName
                        },
                        new Part
                        {
                            type = PType.missiles,
                            skin = ChrysMissile2.UniqueName,
                            active = false
                        },
                        new Part
                        {
                            type = PType.cockpit,
                            skin = ChrysCockpit.UniqueName,
                        },
                        new Part
                        {
                            type = PType.missiles,
                            skin = ChrysMissile.UniqueName,
                        },
                        new Part
                        {
                            type = PType.cannon,
                            skin = ChrysCannon2.UniqueName,
                            active = false
                        },
                        new Part
                        {
                            type = PType.wing,
                            skin = ChrysWing.UniqueName,
                            active = false,
                            flip = true
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
                    new ModuleStealer()
                }
            },
            ExclusiveArtifactTypes = new HashSet<Type>()
            {
                typeof(ModuleStealer)
            },
            UnderChassisSprite = ChrysChassis,
            Name = AnyLocalizations.Bind(["ship", "Chrysalis", "name"]).Localize,
            Description = AnyLocalizations.Bind(["ship", "Chrysalis", "description"]).Localize
        });
    }
    
}
