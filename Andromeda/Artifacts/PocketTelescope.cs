using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Reflection;
using Fred.Andromeda.cards;

namespace Fred.Andromeda.Artifacts;
public class PocketTelescope : Artifact, IAndromedaArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("PocketTelescope", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.AndromedaDeck.Deck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Artifact/PocketTelescope.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "PocketTelescope", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "PocketTelescope", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
      return [
        ..StatusMeta.GetTooltips(Status.evade,1)
      ];
    }
    public override void OnTurnEnd(State state, Combat combat)
    {
        foreach(Part part in combat.otherShip.parts)
        {
            foreach(Part part1 in state.ship.parts)
            {
                if(combat.otherShip.GetWorldXOfPart(part.key!) == state.ship.GetWorldXOfPart(part1.key!))
                {
                    return;
                }
            }
        }
        combat.QueueImmediate(new AStatus{status = Status.evade, statusAmount = 1, targetPlayer = true, timer = 0.0});
        Pulse();
    }
}