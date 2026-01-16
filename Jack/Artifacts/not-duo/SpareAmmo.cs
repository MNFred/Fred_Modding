using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Reflection;
using Fred.Jack.cards;

namespace Fred.Jack.Artifacts;
public class SpareAmmo : Artifact, IJackArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("SpareAmmo", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.Jack_Deck.Deck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/CarePack.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "SpareAmmo", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "SpareAmmo", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [
          new TTCard{
            card = new BackUpMissileCard{temporaryOverride = true}
          },
          ..StatusMeta.GetTooltips(Status.droneShift,1)
          ];
    }
    public override void OnTurnStart(State state, Combat combat)
    {
      if(combat.turn < 4)
      {
        combat.Queue(new AAmmoCheck());
      }
    }
}
public class AAmmoCheck : CardAction
{
  public bool cardExists;
    public override void Begin(G g, State s, Combat c)
    {
      cardExists = false;
      if(c.isPlayerTurn)
      {
        foreach(Card card in c.hand)
        {
          if(card.GetMeta().deck == ModEntry.Instance.Jack_Deck.Deck)
          {
            cardExists = true;
          }
        }
        if(!cardExists)
        {
          c.QueueImmediate(new AAddCard{
            card = new BackUpMissileCard{temporaryOverride = true},
            amount = 1,
            destination = CardDestination.Hand,
          });
          return;
        }
        if(cardExists)
        {
          c.QueueImmediate(new AStatus{
            status = Status.droneShift,
            statusAmount = 1,
            targetPlayer = true
          });
          return;
        }
      }
    }
}