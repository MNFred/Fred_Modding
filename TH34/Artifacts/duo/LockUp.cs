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
        if (ModEntry.Instance.TuckerApi is not { } TuckerApi)
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
        api.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.TH34_Deck.Deck, TuckerApi.TuckerDeck]);
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
                return 2;
            }else return 0;
        }else return 0;
    }
    public bool IsEvadeActionEnabled(IKokoroApi.IV2.IEvadeHookApi.IHook.IIsEvadeActionEnabledArgs args)
    {
        if (args.State.EnumerateAllArtifacts().FirstOrDefault(a => a is ArtifactLockUp) is not { } artifact)
			return true;
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
}