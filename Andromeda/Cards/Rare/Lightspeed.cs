using Fred.Andromeda;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fred.Andromeda.cards
{
  internal sealed class Lightspeed : Card, IAndromedaCard
  {
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      helper.Content.Cards.RegisterCard("Lightspeed", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.AndromedaDeck.Deck,
          rarity = Rarity.rare,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Lightspeed", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        exhaust = true,
        cost = upgrade == Upgrade.A ? 3 : 4,
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new AStatus{status = Status.ace, statusAmount = 1, targetPlayer = true},
        new AStatus{status = Status.evade, statusAmount = 3, targetPlayer = true}
      ],
      Upgrade.B => [
        new AStatus{status = Status.ace, statusAmount = 1, targetPlayer = true},
        new AStatus{status = Status.autododgeLeft, statusAmount = 1, targetPlayer = true}
      ],
      _ => [
        new AStatus{status = Status.ace, statusAmount = 1, targetPlayer = true},
        new AStatus{status = Status.evade, statusAmount = 3, targetPlayer = true}
      ],
    };
  }
}