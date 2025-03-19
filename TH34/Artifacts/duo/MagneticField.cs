using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.TH34.Artifacts;
public class ArtifactMagneticField : Artifact, ITH34Artifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        if (ModEntry.Instance.DuoArtifactsApi is not { } api)
			return;
        helper.Content.Artifacts.RegisterArtifact("MagneticField", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/MagneticField.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "MagneticField", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "MagneticField", "description"]).Localize,
        });
        api.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.TH34_Deck.Deck, Deck.dizzy]);
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [..StatusMeta.GetTooltips(ModEntry.Instance.MinusChargeStatus.Status,1), 
        ..StatusMeta.GetTooltips(Status.maxShield,1)];
    }
    public override void AfterPlayerStatusAction(State state, Combat combat, Status status, AStatusMode mode, int statusAmount)
    {
        if(status == ModEntry.Instance.MinusChargeStatus.Status && mode == AStatusMode.Set)
        {
            combat.QueueImmediate(new AStatus{status = Status.maxShield, mode = AStatusMode.Set, statusAmount = state.ship.Get(ModEntry.Instance.MinusChargeStatus.Status)*2, targetPlayer = true, timer = 0});
            Pulse();
        }
        if(status == ModEntry.Instance.MinusChargeStatus.Status && mode == AStatusMode.Add)
        {
            combat.QueueImmediate(new AStatus{status = Status.maxShield, statusAmount = state.ship.Get(ModEntry.Instance.MinusChargeStatus.Status)*2, targetPlayer = true, timer = 0});
            Pulse();
        }
    }
}