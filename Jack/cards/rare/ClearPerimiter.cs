using Fred.Jack.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fred.Jack.cards
{
  internal sealed class ClearPerimiter : Card, IJackCard
  {
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      helper.Content.Cards.RegisterCard("ClearPerimiter", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.Jack_Deck.Deck,
          rarity = Rarity.rare,
          upgradesTo = [Upgrade.A, Upgrade.B],
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "ClearPerimiter", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        description = ModEntry.Instance.Localizations.Localize(["card", "ClearPerimiter", "description", upgrade.ToString()]),
        cost = upgrade == Upgrade.A ? 0 : 1,
        retain = true,
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new AClearPerimiter{upgradeB = false}
      ],
      Upgrade.B => [
        new AClearPerimiter{upgradeB = true}
      ],
      _ => [
        new AClearPerimiter{upgradeB = false}
      ],
    };
  }
  public class AClearPerimiter : CardAction
  {
    public bool upgradeB;
    public int objectCount;
    public override void Begin(G g, State s, Combat c)
    {
      objectCount = 0;
      foreach (StuffBase stuffBase in c.stuff.Values)
      {
        if (s.ship.GetPartAtWorldX(stuffBase.x) != null)
        {
          if(!upgradeB)
            c.DestroyDroneAt(s, stuffBase.x, true);
          if(upgradeB)
            objectCount++;
        }
      }
      if(upgradeB)
        c.QueueImmediate(new AStatus{status = Status.droneShift, statusAmount = objectCount, targetPlayer = true});
    }
  }
}
