using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.TH34.Artifacts;
public class ArtifactWireCutters : Artifact, ITH34Artifact
{
    public int turnCount = 0;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        if (ModEntry.Instance.DuoArtifactsApi is not { } api)
			return;
        helper.Content.Artifacts.RegisterArtifact("WireCutters", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/WireCutters.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "WireCutters", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "WireCutters", "description"]).Localize,
        });
        api.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.TH34_Deck.Deck, Deck.peri]);
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [..StatusMeta.GetTooltips(ModEntry.Instance.PlusChargeStatus.Status,1), 
        ..StatusMeta.GetTooltips(Status.powerdrive,1)];
    }
    public override int? GetDisplayNumber(State s)
    {
        return turnCount;
    }
    public override void OnTurnStart(State state, Combat combat)
    {
        if(state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)>0)
        {
            turnCount++;
        }else{ turnCount = 0; }
        if(turnCount >= 5)
        {
            combat.QueueImmediate(new AStatus{status = Status.powerdrive, targetPlayer = true, statusAmount = state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status), timer = 0.6});
            combat.QueueImmediate(new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, statusAmount = 0, mode = AStatusMode.Set, targetPlayer = true, timer = 0.6});
            turnCount = 0;
        }
    }
}