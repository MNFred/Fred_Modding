using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Reflection;
using Fred.Andromeda.cards;

namespace Fred.Andromeda.Artifacts;
public class Spaghettification : Artifact, IAndromedaArtifact
{
  public bool attackedThisTurn = false;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("Spaghettification", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.AndromedaDeck.Deck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Artifact/Spaghettification.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Spaghettification", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Spaghettification", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
      return [
        ..StatusMeta.GetTooltips(ModEntry.Instance.ForcefullGravitate.Status,1)
      ];
    }
    public override void OnTurnEnd(State state, Combat combat)
    {
        if(!attackedThisTurn)
        {
            Pulse();
            combat.QueueImmediate(new AStatus{status = ModEntry.Instance.ForcefullGravitate.Status, statusAmount = 1, targetPlayer = false, timer = 0.0});
        }
    }
    public override void OnTurnStart(State state, Combat combat)
    {
        attackedThisTurn = false;
    }
    public override void OnPlayerAttack(State state, Combat combat)
    {
        attackedThisTurn = true;
    }
}