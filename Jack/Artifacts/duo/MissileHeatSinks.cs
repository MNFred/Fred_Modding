using System.Collections.Generic;
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace Fred.Jack.Artifacts;
public class MissileHeatSinks : Artifact, IJackArtifact
{
  public bool active = false;
  public int launchedCount = 0;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      if (ModEntry.Instance.DuoArtifactsApi is not { } api)
			  return;
      helper.Content.Artifacts.RegisterArtifact("MissileHeatSinks", new()
      {
          ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
          Meta = new()
          {
              owner = ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
              pools = [ArtifactPool.Common],
          },
          Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/duo_jack_eunice.png")).Sprite,
          Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "MissileHeatSinks", "name"]).Localize,
          Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "MissileHeatSinks", "description"]).Localize,
      });
      api.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.Jack_Deck.Deck, Deck.eunice]);
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
      return [..StatusMeta.GetTooltips(Status.heat,3)];
    }
    public override void OnCombatStart(State state, Combat combat)
    {
        launchedCount = 0;
    }
    public override void OnPlayerSpawnSomething(State state, Combat combat, StuffBase thing)
    {
        if(thing is Missile)
        {
          launchedCount++;
          if(launchedCount >= 3)
          {
            combat.QueueImmediate(new AStatus{status = Status.heat, statusAmount = -1, targetPlayer = true});
            launchedCount = 0;
          }
        }
    }
    public override void OnTurnStart(State state, Combat combat)
    {
        active = false;
    }
    public override void OnTurnEnd(State state, Combat combat)
    {
        if(state.ship.Get(Status.heat)>=3)
        {
          active = true;
        }
    }
    public override int ModifyBaseMissileDamage(State state, Combat? combat, bool targetPlayer)
    {
        if(active || state.ship.Get(Status.heat)>=3)
        {
          return 1;
        }else return 0;
    }
}
