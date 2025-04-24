using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.Jack.Artifacts;
public class ObjectScanner : Artifact, IJackArtifact
{
  public int objectCount = 0;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      if (ModEntry.Instance.DuoArtifactsApi is not { } api)
			  return;
      helper.Content.Artifacts.RegisterArtifact("ObjectScanner", new()
      {
          ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
          Meta = new()
          {
              owner = ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
              pools = [ArtifactPool.Common],
          },
          Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/duo_jack_riggs.png")).Sprite,
          Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ObjectScanner", "name"]).Localize,
          Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ObjectScanner", "description"]).Localize,
      });
      api.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.Jack_Deck.Deck, Deck.riggs]);
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
      return [..StatusMeta.GetTooltips(Status.drawNextTurn,1)];
    }
    public override void OnTurnEnd(State state, Combat combat)
    {
      foreach(StuffBase stuff in combat.stuff.Values)
      {
        if(stuff != null)
        {
          objectCount++;
        }
      }
      if(objectCount>3)
        objectCount = 3;
      combat.QueueImmediate(new AStatus{status = Status.drawNextTurn, statusAmount = objectCount, targetPlayer = true});
      objectCount = 0;
    }
}
