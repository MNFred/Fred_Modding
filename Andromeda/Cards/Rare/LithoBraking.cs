using Fred.Andromeda;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fred.Andromeda.cards
{
  internal sealed class Lithobraking : Card, IAndromedaCard
  {
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      helper.Content.Cards.RegisterCard("Lithobraking", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.AndromedaDeck.Deck,
          rarity = Rarity.rare,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Lithobraking", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        description = "Align the enemy with you, they take damage based on distance." + ((state.route is Combat combat) ? $"(<c=redd>{Math.Abs(state.ship.x - combat.otherShip.x)}</c>)" : "(<c=redd>0</c>)"),
        exhaust = true,
        retain = upgrade == Upgrade.B ? true : false,
        cost = upgrade == Upgrade.A ? 0 : 1
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new LithoBreak()
      ],
      Upgrade.B => [
        new LithoBreak()
      ],
      _ => [
        new LithoBreak()
      ],
    };
  }
}
public class LithoBreak : CardAction
{
    private int distance = 0;
    public override void Begin(G g, State s, Combat c)
    {
        distance = Math.Abs(s.ship.x - c.otherShip.x);
        c.otherShip.x = s.ship.x;
        if(distance > 0)
        {
            c.otherShip.shake = 2.0;
            c.Queue(new AHurt{hurtAmount = distance, targetPlayer = false, hurtShieldsFirst = true});
        }
    }
}