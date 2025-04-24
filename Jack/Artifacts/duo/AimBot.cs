using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.Jack.Artifacts;
public class AimBot : Artifact, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      if (ModEntry.Instance.DuoArtifactsApi is not { } api)
			  return;
      helper.Content.Artifacts.RegisterArtifact("AimBot", new()
      {
          ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
          Meta = new()
          {
              owner = ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
              pools = [ArtifactPool.Common],
          },
          Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/duo_jack_hacker.png")).Sprite,
          Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "AimBot", "name"]).Localize,
          Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "AimBot", "description"]).Localize,
      });
      api.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.Jack_Deck.Deck, Deck.hacker]);
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
      return [..StatusMeta.GetTooltips(ModEntry.Instance.LockOnStatus.Status,1),
      ..StatusMeta.GetTooltips(ModEntry.Instance.ALockOnStatus.Status,1)];
    }
    public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount)
    {
      if(card.GetData(state).exhaust)
      {
        combat.Queue(new AStatus{status = ModEntry.Instance.LockOnStatus.Status, statusAmount = 1, targetPlayer = false});
        Pulse();
      }
      if(card.GetData(state).singleUse)
      {
        combat.Queue(new AStatus{status = ModEntry.Instance.ALockOnStatus.Status, statusAmount = 1, targetPlayer = false});
        Pulse();
      }
    }
}
