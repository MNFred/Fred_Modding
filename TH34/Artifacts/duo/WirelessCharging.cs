using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nanoray.Shrike;
using Nanoray.Shrike.Harmony;
using Nickel;

namespace Fred.TH34.Artifacts;
public class ArtifactWirelessCharging : Artifact, ITH34Artifact
{
    public bool buffDrones;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        if (ModEntry.Instance.DuoArtifactsApi is not { } api)
			return;
        helper.Content.Artifacts.RegisterArtifact("WirelessCharging", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/WirelessCharging.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "WirelessCharging", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "WirelessCharging", "description"]).Localize,
        });
        api.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.TH34_Deck.Deck, Deck.goat]);
        ModEntry.Instance.Harmony.Patch(
			original: AccessTools.DeclaredMethod(typeof(AttackDrone), nameof(AttackDrone.GetActions)),
			transpiler: new HarmonyMethod(typeof(ArtifactWirelessCharging), nameof(AttackDrone_GetActions_Transpiler))
		);

    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [..StatusMeta.GetTooltips(ModEntry.Instance.PlusChargeStatus.Status,1)];
    }
    public override void OnTurnStart(State state, Combat combat)
    {
        buffDrones = false;
    }
    public override void OnTurnEnd(State state, Combat combat)
    {
        if(state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)>0)
        {
            buffDrones = true;
        }
    }
    public override int ModifySpaceMineDamage(State state, Combat? combat, bool targetPlayer)
    {
        if(!targetPlayer)
            if(state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)>0)
                return 1;
            else return 0;
        else return 0;
    }
    public override int ModifyBaseMissileDamage(State state, Combat? combat, bool targetPlayer)
    {
        if(!targetPlayer)
            if(state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)>0)
                return 1;
            else return 0;
        else return 0;
    }
    private static int GetModifiedAttackDamage(int damage, AttackDrone drone)
	{
		if (MG.inst.g?.state is not { } state)
			return damage;
		if (drone.targetPlayer)
			return damage;

		var artifact = state.EnumerateAllArtifacts().OfType<ArtifactWirelessCharging>().FirstOrDefault();
		if (artifact is null || !artifact.buffDrones)
			return damage;

		artifact.Pulse();
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
					new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(typeof(ArtifactWirelessCharging), nameof(GetModifiedAttackDamage)))
				])
				.AllElements();
		}
		catch (Exception ex)
		{
			ModEntry.Instance.Logger!.LogError("Could not patch method {Method} - {Mod} probably won't work.\nReason: {Exception}", originalMethod, "screams of the damned", ex);
			return instructions;
		}
    }
}