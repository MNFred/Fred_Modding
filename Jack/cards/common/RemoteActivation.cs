using Fred.Jack.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fred.Jack.cards
{
  internal sealed class RemoteActivation : Card, IJackCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/background_SteelRecycle.png"));
      helper.Content.Cards.RegisterCard("RemoteActivation", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.Jack_Deck.Deck,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B],
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "RemoteActivation", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        description = ModEntry.Instance.Localizations.Localize(["card", "RemoteActivation", "description", upgrade.ToString()]),
        art = ThisArt.Sprite,
        retain = upgrade == Upgrade.A ? true : false,
        cost = 1,
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new ATriggerObject{upgradeB = false}
      ],
      Upgrade.B => [
        new ATriggerObject{upgradeB = true}
      ],
      _ => [
        new ATriggerObject{upgradeB = false}
      ],
    };
  }
  public class ATriggerObject : CardAction
  {
    public bool upgradeB;

    public override void Begin(G g, State s, Combat c)
    {
      foreach (StuffBase stuffBase in c.stuff.Values.ToList())
      {
        if (!upgradeB && c.otherShip.GetPartAtWorldX(stuffBase.x) != null)
          c.QueueImmediate(stuffBase.GetActions(s, c));
        if (upgradeB && s.ship.GetPartAtWorldX(stuffBase.x) != null)
          c.QueueImmediate(stuffBase.GetActions(s, c));
      }
    }
  }
}
