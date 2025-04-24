using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Reflection;

namespace Fred.Jack.Artifacts;
public class CoordinatedLaunch : Artifact, IJackArtifact
{
  public bool spawnedAlready = false;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("CoordinatedLaunch", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.Jack_Deck.Deck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/AdvancedFlight.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "CoordinatedLaunch", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "CoordinatedLaunch", "description"]).Localize,
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
      spawnedAlready = false;
    }
    public override void OnPlayerSpawnSomething(State state, Combat combat, StuffBase thing)
    {
      if(!spawnedAlready)
      {
        combat.QueueImmediate(new AStatus{
          status = Status.droneShift,
          statusAmount = 1,
          targetPlayer = true
        });
        spawnedAlready = true;
      }
    }
}