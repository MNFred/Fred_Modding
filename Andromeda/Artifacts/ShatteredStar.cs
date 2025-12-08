using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Reflection;
using Fred.Andromeda.cards;

namespace Fred.Andromeda.Artifacts;
public class ShatteredStar : Artifact, IAndromedaArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("ShatteredStar", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.AndromedaDeck.Deck,
                pools = [ArtifactPool.Boss],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Artifact/ShatteredStar.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ShatteredStar", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ShatteredStar", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
      return [
        ..StatusMeta.GetTooltips(ModEntry.Instance.PassiveGravitateStatus.Status,1),
      ];
    }
    public override void OnTurnStart(State state, Combat combat)
    {
        if(combat.turn % 2 == 0)
        {
            combat.QueueImmediate(new AStatus{status = ModEntry.Instance.PassiveGravitateStatus.Status, statusAmount = 1, targetPlayer = false, timer = 0.0});
        }
    }
}