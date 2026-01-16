using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Reflection;

namespace Fred.Jack.Artifacts;
public class CoordinatedLaunch : Artifact, IJackArtifact
{
  public bool spawnedAlready = false;
  public int count = 3;
  private static Spr Phase0 = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/AdvancedFlight_0.png")).Sprite;
  private static Spr Phase1 = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/AdvancedFlight_1.png")).Sprite;
  private static Spr Phase2 = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/AdvancedFlight_2.png")).Sprite;
  private static Spr Phase3 = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/AdvancedFlight_3.png")).Sprite;
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
            Sprite = Phase3,
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
    public override Spr GetSprite()
    {
      switch(count)
      {
        case 0: return Phase0;
        case 1: return Phase1;
        case 2: return Phase2;
        case 3: return Phase3;
        default: return Phase3;
      }
    }
    public override void OnTurnStart(State state, Combat combat)
    {
      spawnedAlready = false;
      count = 0;
    }
    public override void OnPlayerSpawnSomething(State state, Combat combat, StuffBase thing)
    {
      count++;
      if(count >= 3)
      {
        if(!spawnedAlready)
        {
          combat.QueueImmediate(new AStatus{
            status = Status.droneShift,
            statusAmount = 1,
            targetPlayer = true
          });
          Pulse();
          spawnedAlready = true;
        }
      }
    }
}