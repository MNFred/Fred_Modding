using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Xna.Framework.Graphics;
using HarmonyLib;
using System;
using Microsoft.Extensions.Logging;
using System.Reflection.Emit;
using Nanoray.Shrike;
using Nanoray.Shrike.Harmony;
using FSPRO;
using static Fred.TH34.IKokoroApi.IV2.IEvadeHookApi.IEvadePostcondition;

namespace Fred.TH34.Artifacts;
public class ArtifactLockUp : Artifact, ITH34Artifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        if (ModEntry.Instance.DuoArtifactsApi is not { } api)
			return;
        helper.Content.Artifacts.RegisterArtifact("LockUp", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/LockUp.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "LockUp", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "LockUp", "description"]).Localize,
        });
        api.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.TH34_Deck.Deck, ModEntry.Instance.TuckerApi!.TuckerDeck]);
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(AMove), nameof(AMove.Begin)),
            transpiler: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(AMove_Begin_Transpiler))
        );
        ModEntry.Instance.Harmony.Patch(
			original: AccessTools.DeclaredMethod(typeof(Card), nameof(Card.GetActionsOverridden)),
			postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Card_GetActionsOverridden_Postfix))
		);
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [..StatusMeta.GetTooltips(ModEntry.Instance.MinusChargeStatus.Status,1)];
    }
    public override int ModifyBaseDamage(int baseDamage, Card? card, State state, Combat? combat, bool fromPlayer)
    {
        if(state.ship.Get(ModEntry.Instance.MinusChargeStatus.Status)>0)
        {
            if(fromPlayer == true)
            {
                return 1;
            }else return 0;
        }else return 0;
    }
    public static IEnumerable<CodeInstruction> AMove_Begin_Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase method)
    {
        try
        {
            return new SequenceBlockMatcher<CodeInstruction>(instructions)
                .Find([
                    ILMatches.Ldarg(3),
                    ILMatches.Ldfld("hand"),
                    ILMatches.Call("OfType"),
                    ILMatches.Call("Any"),
                    ILMatches.Brfalse.GetBranchTarget(out var branchTarget)
                ])
                .Insert(SequenceMatcherPastBoundsDirection.After, SequenceMatcherInsertionResultingBounds.IncludingInsertion, [
                    new CodeInstruction(OpCodes.Ldarg_2),
                    new CodeInstruction(OpCodes.Ldarg_3),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(typeof(ArtifactLockUp), nameof(AMove_Begin_Transpiler_Sub))),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.DeclaredField(typeof(AMove), nameof(AMove.fromEvade))),
                    new CodeInstruction(OpCodes.Brfalse, branchTarget.Value)
                ])
                .Find([
                    ILMatches.Ldloc<Ship>(method),
                    ILMatches.LdcI4((int)Status.lockdown),
                    ILMatches.Call("Get"),
                    ILMatches.LdcI4(0),
                    ILMatches.Ble.GetBranchTarget(out branchTarget)
                ])
                .Insert(SequenceMatcherPastBoundsDirection.After, SequenceMatcherInsertionResultingBounds.IncludingInsertion, [
                    new CodeInstruction(OpCodes.Ldarg_2),
                    new CodeInstruction(OpCodes.Ldarg_3),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(typeof(ArtifactLockUp), nameof(AMove_Begin_Transpiler_Sub))),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.DeclaredField(typeof(AMove), nameof(AMove.fromEvade))),
                    new CodeInstruction(OpCodes.Brfalse, branchTarget.Value)
                ])
                .Find([
                    ILMatches.Ldloc<Ship>(method),
                    ILMatches.LdcI4((int)Status.engineStall),
                    ILMatches.Call("Get"),
                    ILMatches.LdcI4(0),
                    ILMatches.Ble.GetBranchTarget(out branchTarget)
                ])
                .Insert(SequenceMatcherPastBoundsDirection.After, SequenceMatcherInsertionResultingBounds.IncludingInsertion, [
                    new CodeInstruction(OpCodes.Ldarg_2),
                    new CodeInstruction(OpCodes.Ldarg_3),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(typeof(ArtifactLockUp), nameof(AMove_Begin_Transpiler_Sub))),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.DeclaredField(typeof(AMove), nameof(AMove.fromEvade))),
                    new CodeInstruction(OpCodes.Brfalse, branchTarget.Value)
                ])
                .AllElements();
        }
        catch(Exception ex)
        {
            ModEntry.Instance.Logger!.LogError("Could not patch method {method} - {Mod} probably won't work. \nReason: {Exception}", method, "Hell is full", ex);
            return instructions;
        }
    }
    public bool IsEvadeActionEnabled(IKokoroApi.IV2.IEvadeHookApi.IHook.IIsEvadeActionEnabledArgs args)
    {
        if(args.State.ship.Get(ModEntry.Instance.MinusChargeStatus.Status)>0)
        {
            return false;
        }else return true;
    }
    private static void Card_GetActionsOverridden_Postfix(Card __instance, State s, ref List<CardAction> __result)
	{
		if (s.ship.Get(ModEntry.Instance.MinusChargeStatus.Status)<=0)
			return;

		if (s.EnumerateAllArtifacts().FirstOrDefault(a => a is ArtifactLockUp) is not { } artifact)
			return;

		foreach (var baseAction in __result)
		{
			if (baseAction is AMove move)
			{
                move.dir = 0;
			}
		}
	}
    public static void AMove_Begin_Transpiler_Sub(State s, Combat c, AMove move)
    {
        var card = ModEntry.Instance.KokoroApi.V2.ActionInfo.GetSourceCard(s, move);
        if(card == null)
            return;
        if(move.fromEvade == false)
        {
            if(s.ship.Get(ModEntry.Instance.MinusChargeStatus.Status)>0)
            {
                move.dir = 0;
                Audio.Play(Event.Status_PowerDown);
                s.ship.shake += 1;
                if(s.ship.Get(Status.engineStall)>0) s.ship.Add(Status.engineStall, -1);
            }
        }
    }
}