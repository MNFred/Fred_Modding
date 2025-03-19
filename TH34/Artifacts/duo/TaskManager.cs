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
public class ArtifactTaskManager : Artifact, ITH34Artifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        if (ModEntry.Instance.DuoArtifactsApi is not { } api)
			return;
        helper.Content.Artifacts.RegisterArtifact("TaskManager", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/TaskManager.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "TaskManager", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "TaskManager", "description"]).Localize,
        });
        api.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.TH34_Deck.Deck, Deck.hacker]);
        ModEntry.Instance.Harmony.Patch(
			original: AccessTools.DeclaredMethod(typeof(Card), nameof(Card.GetDataWithOverrides)),
			postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Card_GetDataWithOverrides_Postfix))
		);
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [..StatusMeta.GetTooltips(ModEntry.Instance.MinusChargeStatus.Status,1), new TTGlossary("cardtrait.exhaust")];
    }
    private static void Card_GetDataWithOverrides_Postfix(Card __instance, ref CardData __result, State state)
	{
        if (state.IsOutsideRun())
            return;
		if (state.EnumerateAllArtifacts().FirstOrDefault(a => a is ArtifactTaskManager) is not { } artifact)
			return;

		if(__instance.GetData(state).exhaust == true)
        {
            if(state.ship.Get(ModEntry.Instance.MinusChargeStatus.Status)>0)
                __result.cost -= 1;
        }
	}
}