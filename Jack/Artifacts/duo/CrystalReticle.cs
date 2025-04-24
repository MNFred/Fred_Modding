using System.Collections.Generic;
using System.Reflection;
using Fred.Jack.Midrow;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.Jack.Artifacts;
public class CrystalReticle : Artifact, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      if (ModEntry.Instance.DuoArtifactsApi is not { } api)
			  return;
      helper.Content.Artifacts.RegisterArtifact("CrystalReticle", new()
      {
          ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
          Meta = new()
          {
              owner = ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
              pools = [ArtifactPool.Common],
          },
          Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/duo_jack_shard.png")).Sprite,
          Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "CrystalReticle", "name"]).Localize,
          Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "CrystalReticle", "description"]).Localize,
      });
      api.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.Jack_Deck.Deck, Deck.shard]);
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
      return [..StatusMeta.GetTooltips(Status.shard,3), ..StatusMeta.GetTooltips(ModEntry.Instance.LockOnStatus.Status,1)];
    }
    public override void OnTurnEnd(State state, Combat combat)
    {
      if(state.ship.Get(Status.shard)>0)
      {
        Pulse();
        combat.QueueImmediate(new AStatus{status = ModEntry.Instance.LockOnStatus.Status, statusAmount = state.ship.Get(Status.shard), targetPlayer = false});
        combat.QueueImmediate(new AStatus{status = Status.shard, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set});
      }
    }
}
