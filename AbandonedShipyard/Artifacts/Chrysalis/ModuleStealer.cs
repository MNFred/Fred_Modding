using Nickel;
using Nanoray.PluginManager;
using System.Reflection;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System;
using Nanoray.Shrike;
using Nanoray.Shrike.Harmony;
using System.Reflection.Emit;
using Microsoft.Extensions.Logging;

namespace Fred.AbandonedShipyard;

public class ModuleStealer : Artifact, IAbandonedArtifact
{
    public List<Tooltip> moduleTooltip = [];
    public bool TCicada1Module;
    public bool TFireballModule;
    public bool TStingerModule;
    public bool TPupaModule;
    public bool TMonarchModule;
    public bool TMineSweeperModule;
    public bool TStagModule;
    public bool TJumboModule;
    public bool TWizboModule;
    public bool TGnatModule;
    public bool TNeedlerModule;
    public bool TCicada2Module;
    public bool TBruiserModule;
    public bool TSolarModule;
    public bool TStardogModule;
    public bool TRailCannonModule;
    public bool TGoliathModule;
    public bool TCannoneerModule;
    public bool TSpikeJrModule;
    public bool TCicada3Module;
    public bool TReplicatorModule;
    public bool TScarabModule;
    public bool TIonRangerModule;
    public bool TFishModule;
    public bool TCrabModule;
    public bool TDrillModule;
    public bool TRustingModule;
    public bool TBuriedModule;
    public bool THopperModule;
    public bool TStriderModule;
    public bool TFireFlyModule;
    public bool TTuskerModule;
    public bool TGooseModule;
    public bool TInevitableModule;
    public bool TNotGoatModule;
    public bool TPupa2Module;
    public bool TOuroborosModule;
    public Artifact? moduleToGive = null;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("ModuleStealer", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.EventOnly],
                unremovable = true
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Chrysalis/ModuleStealer.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ModuleStealer", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ModuleStealer", "description"]).Localize,
        });
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(ArtifactReward), nameof(ArtifactReward.GetOffering)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(ArtifactReward_GetOffering_Postfix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Card), nameof(Card.GetActionsOverridden)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Card_GetActionsOverridden_Postfix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(AttackDrone), nameof(AttackDrone.GetActions)),
            transpiler: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(AttackDrone_GetActions_Transpiler))
        );
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return moduleTooltip;
    }
    public override void OnCombatEnd(State state)
    {
        moduleToGive = null;
        if (state.map.markers[state.map.currentLocation].contents is MapBattle contents)
        {
            switch (contents.ai!.Key())
            {
                case var enemy when enemy == new LightFighter().Key():
                    if (!TCicada1Module)
                        moduleToGive = new Cicada1Module();
                    break;
                case var enemy when enemy == new DrakePirate().Key():
                    if (!TFireballModule)
                        moduleToGive = new FireballModule();
                    break;
                case var enemy when enemy == new SimpleMissiler().Key():
                    if (!TStingerModule)
                        moduleToGive = new StingerModule();
                    break;
                case var enemy when enemy == new MediumFighter().Key():
                    if (!TPupaModule)
                        moduleToGive = new PupaModule();
                    break;
                case var enemy when enemy == new DroneDropperZ1().Key():
                    if (!TMonarchModule)
                        moduleToGive = new MonarchModule();
                    break;
                case var enemy when enemy == new DualDroneBaddie().Key():
                    if (!TMineSweeperModule)
                        moduleToGive = new MineSweeperModule();
                    break;
                case var enemy when enemy == new HeavyFighter().Key():
                    if (!TStagModule)
                        moduleToGive = new StagModule();
                    break;
                case var enemy when enemy == new WideCruiser().Key():
                    if (!TJumboModule)
                        moduleToGive = new JumboModule();
                    break;
                case var enemy when enemy == new Wizard().Key():
                    if (!TWizboModule)
                        moduleToGive = new WizboModule();
                    break;
                case var enemy when enemy == new LightScouter().Key():
                    if (!TGnatModule)
                        moduleToGive = new GnatModule();
                    break;
                case var enemy when enemy == new PaybackBruiserZ1().Key():
                    if (!TNeedlerModule)
                        moduleToGive = new NeedlerModule();
                    break;
                case var enemy when enemy == new LightFighterZone2().Key():
                    if (!TCicada2Module)
                        moduleToGive = new Cicada2Module();
                    break;
                case var enemy when enemy == new LockdownBruiser().Key():
                    if (!TBruiserModule)
                        moduleToGive = new BruiserModule();
                    break;
                case var enemy when enemy == new BinaryBaddie().Key():
                    if (!TSolarModule)
                        moduleToGive = new SolarModule();
                    break;
                case var enemy when enemy == new WideMissiler().Key():
                    if (!TStardogModule)
                        moduleToGive = new StardogModule();
                    break;
                case var enemy when enemy == new RailCannon().Key():
                    if (!TRailCannonModule)
                        moduleToGive = new RailCannonModule();
                    break;
                case var enemy when enemy == new GoliathDefender().Key():
                    if (!TGoliathModule)
                        moduleToGive = new GoliathModule();
                    break;
                case var enemy when enemy == new WideCruiserAlt().Key():
                    if (!TCannoneerModule)
                        moduleToGive = new CannoneerModule();
                    break;
                case var enemy when enemy == new OxygenLeakGuy().Key():
                    if (!TSpikeJrModule)
                        moduleToGive = new SpikeJrModule();
                    break;
                case var enemy when enemy == new LightFighterZone3().Key():
                    if (!TCicada3Module)
                        moduleToGive = new Cicada3Module();
                    break;
                case var enemy when enemy == new CannonCloner().Key():
                    if (!TReplicatorModule)
                        moduleToGive = new ReplicatorModule();
                    break;
                case var enemy when enemy == new MediumFighterZone3().Key():
                    if (!TScarabModule && !TPupa2Module)
                        moduleToGive = new ScarabModule();
                    break;
                case var enemy when enemy == new MediumAncient().Key():
                    if (!TIonRangerModule)
                        moduleToGive = new IonRangerModule();
                    break;
                case var enemy when enemy == new UnderwaterGuy().Key():
                    if (!TFishModule)
                        moduleToGive = new AquaticModule();
                    break;
                case var enemy when enemy == new CrabGuy().Key():
                    if (!TCrabModule)
                        moduleToGive = new CrabModule();
                    break;
                case var enemy when enemy == new AsteroidDriller().Key():
                    if (!TDrillModule)
                        moduleToGive = new DrillModule();
                    break;
                case var enemy when enemy == new RustingColossus().Key():
                    if (!TRustingModule)
                        moduleToGive = new RustingModule();
                    break;
                case var enemy when enemy == new StoneGuy().Key():
                    if (!TBuriedModule)
                        moduleToGive = new BuriedModule();
                    break;
                case var enemy when enemy == new GnatZ3().Key():
                    if (!THopperModule)
                        moduleToGive = new HopperModule();
                    break;
                case var enemy when enemy == ModEntry.Instance.Helper.Content.Enemies.LookupByUniqueName("TheJazMaster.EnemyPack::FollowerEnemy")!.UniqueName:
                    if (!TStriderModule)
                        moduleToGive = new StriderModule();
                    break;
                case var enemy when enemy == ModEntry.Instance.Helper.Content.Enemies.LookupByUniqueName("TheJazMaster.EnemyPack::FireflyEnemy")!.UniqueName:
                    if (!TFireFlyModule)
                        moduleToGive = new FireflyModule();
                    break;
                case var enemy when enemy == ModEntry.Instance.Helper.Content.Enemies.LookupByUniqueName("TheJazMaster.EnemyPack::BigGunsEnemy")!.UniqueName:
                    if (!TTuskerModule)
                        moduleToGive = new TuskenModule();
                    break;
                case var enemy when enemy == ModEntry.Instance.Helper.Content.Enemies.LookupByUniqueName("TheJazMaster.EnemyPack::GooseEnemy")!.UniqueName:
                    if (!TGooseModule)
                        moduleToGive = new GooseModule();
                    break;
                case var enemy when enemy == ModEntry.Instance.Helper.Content.Enemies.LookupByUniqueName("TheJazMaster.EnemyPack::TimestopEnemy")!.UniqueName:
                    if (!TInevitableModule)
                        moduleToGive = new InevitableModule();
                    break;
                case var enemy when enemy == ModEntry.Instance.Helper.Content.Enemies.LookupByUniqueName("TheJazMaster.EnemyPack::JupiterEnemy")!.UniqueName:
                    if (!TNotGoatModule)
                        moduleToGive = new NotGoatModule();
                    break;
                case var enemy when enemy == ModEntry.Instance.Helper.Content.Enemies.LookupByUniqueName("TheJazMaster.EnemyPack::PupaMk2Enemy")!.UniqueName:
                    if (!TPupa2Module && !TScarabModule)
                        moduleToGive = new Pupa2Module();
                    break;
                case var enemy when enemy == ModEntry.Instance.Helper.Content.Enemies.LookupByUniqueName("TheJazMaster.EnemyPack::OuroborosEnemy")!.UniqueName:
                    if (!TOuroborosModule)
                        moduleToGive = new OuroborosModule();
                    break;
            }
        }
        if (moduleToGive != null)
        {
            state.rewardsQueue.Add(new AArtifactOffering());
        }
    }
    public override void OnCombatStart(State state, Combat combat)
    {
        if (TGoliathModule)
        {
            combat.QueueImmediate(
                from cockpit in state.ship.parts.Select((Part part, int x) => new { part, x })
                where cockpit.part.type == PType.cockpit
                select new AClearObstacle()
                {
                    whereToDestroy = cockpit.x
                }
            );
        }
        if (TIonRangerModule)
        {
            combat.otherShip.overheatDamage += 1;
        }
        if (TCrabModule)
        {
            combat.QueueImmediate(new AStatus { status = Status.bubbleJuice, statusAmount = 4, targetPlayer = true });
        }
        if (TFireFlyModule)
        {
            combat.QueueImmediate(new AStatus { status = Status.serenity, statusAmount = 2, targetPlayer = true });
        }
        if (TInevitableModule)
        {
            combat.QueueImmediate(new AStatus { status = Status.timeStop, statusAmount = 2, targetPlayer = true });
        }
        if (TNotGoatModule)
        {
            combat.QueueImmediate(new AStatus { status = Status.droneShift, statusAmount = 2, targetPlayer = true });
        }
    }
    public override void OnPlayerDestroyDrone(State state, Combat combat){
        if (TDrillModule)
        {
            combat.QueueImmediate(new ADrawCard { count = 1 });
        }
    }
    public override void OnTurnEnd(State state, Combat combat){
        if (combat.turn % 3 == 0)
        {
            if (TBruiserModule)
            {
                combat.QueueImmediate(
                    from wing in state.ship.parts.Select((Part part, int x) => new { part, x })
                    where wing.part.flip == true && wing.part.active == true && wing.part.type == PType.wing
                    select new AAttack()
                    {
                        fromX = wing.x,
                        damage = 0,
                        multiCannonVolley = true,
                        status = Status.lockdown,
                        statusAmount = 1
                    }
                );
            }
        }
        if (TFishModule)
        {
            combat.QueueImmediate(
                from scaff in state.ship.parts.Select((Part part, int x) => new { part, x })
                where scaff.part.type == PType.empty
                select new AAttack()
                {
                    fromX = scaff.x,
                    damage = 1,
                    multiCannonVolley = true,
                }
            );
        }
    }
    public override void OnTurnStart(State state, Combat combat){
        if (combat.turn == 1 || combat.turn == 2)
        {
            if (TBuriedModule)
            {
                combat.QueueImmediate(new AStatus { status = Status.tempShield, statusAmount = 3, targetPlayer = true });
            }
        }
        if (combat.turn == 1)
        {
            if (TOuroborosModule)
            {
                combat.QueueImmediate(new OuroborosRecycle());
            }
        }
        if (TReplicatorModule)
            {
                combat.QueueImmediate(new AStatus { status = Status.shield, statusAmount = -1, targetPlayer = false });
            }
        if (combat.turn == 3)
        {
            if (TRustingModule)
            {
                combat.QueueImmediate(new AStatus { status = Status.corrode, statusAmount = 1, targetPlayer = false });
            }
        }
        if (combat.turn % 3 == 0)
        {
            if (TGnatModule)
            {
                combat.QueueImmediate(new AStatus { status = Status.autododgeRight, statusAmount = 1, targetPlayer = true });
            }
            if (TStriderModule)
            {
                combat.QueueImmediate(new AStatus { status = Status.hermes, statusAmount = 1, targetPlayer = true });
            }
            if (THopperModule)
            {
                combat.QueueImmediate(new AStatus { status = Status.autododgeRight, statusAmount = 2, targetPlayer = true });
            }
            if (TSpikeJrModule)
            {
                combat.QueueImmediate(
                    from wing in state.ship.parts.Select((Part part, int x) => new { part, x })
                    where wing.part.flip == false && wing.part.active == true && wing.part.type == PType.wing
                    select new ASpawn()
                    {
                        fromX = wing.x,
                        multiBayVolley = true,
                        thing = new Missile { missileType = MissileType.seeker, targetPlayer = false }
                    }
                );
            }
        }
        if (combat.turn == 2)
        {
            if (TCicada3Module)
            {
                combat.QueueImmediate(new AStatus { status = Status.overdrive, statusAmount = 1, targetPlayer = true });
            }
        }
        if (combat.turn % 2 == 0)
        {
            if (TNeedlerModule)
            {
                combat.QueueImmediate(new AStatus { status = Status.tempPayback, statusAmount = 1, targetPlayer = true });
            }
            if (TSolarModule)
            {
                combat.QueueImmediate(new AEnergy { changeAmount = -1 });
            }
        }
        if (combat.turn % 2 == 1)
        {
            if (TSolarModule)
            {
                combat.QueueImmediate(new AEnergy { changeAmount = 1 });
            }
        }
        if (combat.turn == 6)
        {
            if (TTuskerModule)
            {
                combat.QueueImmediate(new AStatus { status = Status.powerdrive, statusAmount = 2, targetPlayer = true });
            }
        }
    }
    public override int ModifyBaseMissileDamage(State state, Combat? combat, bool targetPlayer)
    {
        if (!targetPlayer)
            if (TMineSweeperModule)
                return 1;
            else return 0;
        else
        {
            if (TNotGoatModule)
            {
                return -1;
            }
            else return 0;
        }
    }
    private static int GetModifiedAttackDamage(int damage, AttackDrone drone)
    {
        if (MG.inst.g?.state is not { } state)
            return damage;
        if (drone.targetPlayer)
            return damage;

        var artifact = state.EnumerateAllArtifacts().OfType<ModuleStealer>().FirstOrDefault();
        if (artifact is null || !artifact.TMineSweeperModule)
            return damage;

        damage += 1;
        return damage;
    }

    private static IEnumerable<CodeInstruction> AttackDrone_GetActions_Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
    {
        try
        {
            return new SequenceBlockMatcher<CodeInstruction>(instructions)
                .Find(ILMatches.Call("AttackDamage"))
                .Insert(SequenceMatcherPastBoundsDirection.After, SequenceMatcherInsertionResultingBounds.IncludingInsertion, [
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(typeof(ModuleStealer), nameof(GetModifiedAttackDamage)))
                ])
                .AllElements();
        }
        catch (Exception ex)
        {
            ModEntry.Instance.Logger!.LogError("Could not patch method {Method} - {Mod} probably won't work.\nReason: {Exception}", originalMethod, "screams of the damned", ex);
            return instructions;
        }
    }
    private static void Card_GetActionsOverridden_Postfix(Card __instance, State s, ref List<CardAction> __result)
    {
        if (__instance == null)
            return;
        var artifact = s.EnumerateAllArtifacts().OfType<ModuleStealer>().FirstOrDefault();
        if (artifact == null)
            return;
        foreach (var baseAction in __result)
        {
            if (baseAction is AAttack attack)
            {
                if (attack.piercing == false)
                {
                    if (attack.status == null)
                    {
                        if (artifact.TFireballModule || artifact.TIonRangerModule)
                        {
                            attack.status = Status.heat;
                            attack.statusAmount = 1;
                        }
                    }
                }
            }
        }
    }
    private static void ArtifactReward_GetOffering_Postfix(State s, int count, ref List<Artifact> __result)
    {
        var artifact = s.EnumerateAllArtifacts().OfType<ModuleStealer>().FirstOrDefault();
        if (artifact == null)
            return;
        if (artifact.moduleToGive == null)
            return;
        __result.Clear();
        __result.Add(artifact.moduleToGive);
        artifact.moduleToGive = null;
    }
    public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount)
    {
        if (TPupaModule)
        {
            if (deck == Deck.trash)
            {
                combat.QueueImmediate(new AStatus { status = Status.tempShield, statusAmount = Math.Max(1, energyCost * 2), targetPlayer = true, timer = 0.2 });
            }
        }
        if (TScarabModule)
        {
            if (deck == Deck.trash)
            {
                combat.QueueImmediate(new AStatus { status = Status.tempShield, statusAmount = 1, targetPlayer = true, timer = 0.2 });
                combat.QueueImmediate(new AStatus { status = Status.overdrive, statusAmount = 1, targetPlayer = true, timer = 0.2 });
            }
        }
        if (TPupa2Module)
        {
            if (deck == Deck.trash)
            {
                combat.QueueImmediate(new ADrawCard{count = 1, timer = 0.2});
                combat.QueueImmediate(new AStatus { status = Status.overdrive, statusAmount = 1, targetPlayer = true, timer = 0.2 });
            }
        }
    }
    public override int ModifySpaceMineDamage(State state, Combat? combat, bool targetPlayer)
    {
        if (targetPlayer)
        {
            if (TNotGoatModule)
            {
                return -1;
            }
            else return 0;
        }
        else return 0;
    }
}
public class AClearObstacle : CardAction
{
    public int whereToDestroy;
    public override void Begin(G g, State s, Combat c)
    {
        foreach (StuffBase stuffBase in c.stuff.Values)
        {
            if (s.ship.GetPartAtWorldX(stuffBase.x) != null && s.ship.GetPartAtWorldX(stuffBase.x)!.type == PType.cockpit)
            {
                c.DestroyDroneAt(s, stuffBase.x, false);
            }
        }
        c.Queue(new ASpawn { thing = new SpaceMine(), fromX = whereToDestroy, multiBayVolley = true });
    }
}
public class OuroborosRecycle : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        var lastCard = c.hand.Last();
        lastCard.recycleOverride = true;
        lastCard.recycleOverrideIsPermanent = false;
    }
}