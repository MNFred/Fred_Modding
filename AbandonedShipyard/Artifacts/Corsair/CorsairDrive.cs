using Nickel;
using Nanoray.PluginManager;
using System.Reflection;
using System.Linq;
using HarmonyLib;
using System.Collections.Generic;

namespace Fred.AbandonedShipyard;
public class CorsairDrive : Artifact, IAbandonedArtifact
{
    public bool activateEngines = true;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("CorsairDrive", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.Boss],
                unremovable = true
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Corsair/CorsairDrive.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "CorsairDrive", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "CorsairDrive", "description"]).Localize,
        });
        ModEntry.Instance.Harmony.Patch(
			original: AccessTools.DeclaredMethod(typeof(AMove), nameof(AMove.Begin)),
			prefix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(AMove_Begin_Prefix)),
			postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(AMove_Begin_Postfix))
		);
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [..StatusMeta.GetTooltips(Status.evade,1)];
    }
    public override void OnReceiveArtifact(State state)
    {
        state.ship.baseEnergy += 1;
        state.GetCurrentQueue().QueueImmediate(new ALoseArtifact { artifactType = new CorsairEngines().Key() });
    }
    public override void OnCombatStart(State state, Combat combat)
    {
        activateEngines = true;
    }
    public override void OnTurnStart(State state, Combat combat)
    {
        if(state.ship.Get(Status.evade)==0)
        {
            combat.QueueImmediate(new AStatus{status = Status.evade, targetPlayer = true, statusAmount = 1});
        }
        if(activateEngines == false)
        {
            Pulse();
            combat.QueueImmediate(new AHurt{hurtAmount = 1, hurtShieldsFirst = false, targetPlayer = true});
        }
        activateEngines = false;
    }
    private static void AMove_Begin_Prefix(AMove __instance, State s, Combat c, out int __state)
	{
		var ship = __instance.targetPlayer ? s.ship : c.otherShip;
		__state = ship.x;
	}

	private static void AMove_Begin_Postfix(AMove __instance, State s, Combat c, in int __state)
	{
		if (!__instance.targetPlayer)
			return;
		
		var ship = __instance.targetPlayer ? s.ship : c.otherShip;
		if (ship.x == __state)
			return;
		if (s.EnumerateAllArtifacts().OfType<CorsairDrive>().FirstOrDefault() is not { } artifact)
			return;

		artifact.activateEngines = true;
	}
}