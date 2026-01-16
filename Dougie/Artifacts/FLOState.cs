using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Reflection;
using Dougie.cards;
using System.Linq;

namespace Dougie.Artifacts;
public class FLOState : Artifact, IDougieArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("FLOState", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.DougieDeck.Deck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Artifact/FLOState.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "FLOState", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "FLOState", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
      return [
        ..StatusMeta.GetTooltips(Status.droneShift,1)
      ];
    }
    public override void OnTurnStart(State state, Combat combat)
    {
        if(combat.turn % 3 == 0)
        {
            Pulse();
            combat.QueueImmediate(new AStatus{status = Status.droneShift, statusAmount = 1, targetPlayer = true, timer = 0});
        }
    }
}