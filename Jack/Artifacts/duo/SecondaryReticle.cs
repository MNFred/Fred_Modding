using System;
using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.Jack.Artifacts;
public class SecondaryReticle : Artifact, IJackArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      if (ModEntry.Instance.DuoArtifactsApi is not { } api)
			  return;
      helper.Content.Artifacts.RegisterArtifact("SecondaryReticle", new()
      {
          ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
          Meta = new()
          {
              owner = ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
              pools = [ArtifactPool.Common],
          },
          Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/duo_jack_peri.png")).Sprite,
          Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "SecondaryReticle", "name"]).Localize,
          Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "SecondaryReticle", "description"]).Localize,
      });
      api.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.Jack_Deck.Deck, Deck.peri]);
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
      return [..StatusMeta.GetTooltips(Status.overdrive,1),
      ..StatusMeta.GetTooltips(ModEntry.Instance.LockOnStatus.Status,1)];
    }
    public override int ModifyBaseMissileDamage(State state, Combat? combat, bool targetPlayer)
    {
      if(!targetPlayer)
        return state.ship.Get(Status.overdrive)/2;
      else return 0;
    }
}
