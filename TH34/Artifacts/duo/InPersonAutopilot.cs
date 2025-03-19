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
public class ArtifactInPersonAutopilot : Artifact, ITH34Artifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        if (ModEntry.Instance.DuoArtifactsApi is not { } api)
			return;
        helper.Content.Artifacts.RegisterArtifact("InPersonAutopilot", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/InPersonAutopilot.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "InPersonAutopilot", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "InPersonAutopilot", "description"]).Localize,
        });
        api.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.TH34_Deck.Deck, ModEntry.Instance.BucketApi!.BucketDeck]);
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [..StatusMeta.GetTooltips(ModEntry.Instance.MinusChargeStatus.Status,1), ..StatusMeta.GetTooltips(Status.autopilot,1)];
    }
    public override void OnTurnStart(State state, Combat combat)
    {
        if(state.ship.Get(ModEntry.Instance.MinusChargeStatus.Status)>0)
        {
            Pulse();
            combat.QueueImmediate(new AStatus{status = Status.autopilot, statusAmount = 1, targetPlayer = true, timer = 0});
        }
    }
}