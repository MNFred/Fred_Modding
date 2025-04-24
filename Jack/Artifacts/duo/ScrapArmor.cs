using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.Jack.Artifacts;
public class ScrapArmor : Artifact, IJackArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      if (ModEntry.Instance.DuoArtifactsApi is not { } api)
			  return;
      helper.Content.Artifacts.RegisterArtifact("ScrapArmor", new()
      {
          ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
          Meta = new()
          {
              owner = ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
              pools = [ArtifactPool.Common],
          },
          Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/duo_jack_dizzy.png")).Sprite,
          Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ScrapArmor", "name"]).Localize,
          Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ScrapArmor", "description"]).Localize,
      });
      api.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.Jack_Deck.Deck, Deck.dizzy]);
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
      return [..StatusMeta.GetTooltips(Status.shield,1),
      ..StatusMeta.GetTooltips(Status.tempShield,1),
      ..StatusMeta.GetTooltips(Status.maxShield,1)];
    }
    public override void OnPlayerSpawnSomething(State state, Combat combat, StuffBase thing)
    {
        switch(thing)
        {
          case Missile:
            combat.QueueImmediate(new AStatus{status = Status.tempShield, statusAmount = 1, targetPlayer = true});
          break;
          case Asteroid:
            combat.QueueImmediate(new AStatus{status = Status.shield, statusAmount = 1, targetPlayer = true});
          break;
          default:
            combat.QueueImmediate(new AStatus{status = Status.maxShield, statusAmount = 1, targetPlayer = true});
          break;
        }
    }
}
