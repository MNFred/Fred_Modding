using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Fred.Jack.Artifacts;
public class NeverMissile : Artifact, IJackArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("NeverMissile", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.Jack_Deck.Deck,
                pools = [ArtifactPool.Boss],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/UnPerfectAim.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "NeverMissile", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "NeverMissile", "description"]).Localize,
        });
    }
    public override void OnTurnEnd(State state, Combat combat)
    {
        combat.QueueImmediate(new SeekTarget{timer = 0.1});
    }
}
public class SeekTarget : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
      int missilesTurned = 2;
      foreach(StuffBase stuffBase in c.stuff.Values.ToList())
      {
        if(stuffBase is Missile missile && !missile.targetPlayer && missile.missileType != MissileType.seeker && missile.missileType != MissileType.shaker && missile.missileType != MissileType.punch)
        {
          if(c.otherShip.GetPartAtWorldX(missile.x) == null || c.otherShip.GetPartAtWorldX(missile.x)!.type == PType.empty)
          {
            if(missilesTurned != 0)
            {
              Missile missileSeeker = new Missile()
              {
                targetPlayer = missile.targetPlayer,
                x = missile.x,
                xLerped = missile.xLerped,
                bubbleShield = missile.bubbleShield,
                missileType = MissileType.seeker
              };
              c.stuff[missile.x] = missileSeeker;
              missilesTurned--;
            }
          }
        }
      }
    }
}