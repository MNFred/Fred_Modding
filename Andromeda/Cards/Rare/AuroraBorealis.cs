using Fred.Andromeda;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fred.Andromeda.cards
{
  internal sealed class AuroraBorealis : Card, IAndromedaCard
  {
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      helper.Content.Cards.RegisterCard("AuroraBorealis", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.AndromedaDeck.Deck,
          rarity = Rarity.rare,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "AuroraBorealis", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        exhaust = true,
        cost = upgrade == Upgrade.B ? 1 : 2,
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new AStunShip{targetPlayer = false},
        new AMove{targetPlayer = false, dir = -2},
      ],
      Upgrade.B => [
        new AStatus{status = Status.overdrive, statusAmount = -1, targetPlayer = false},
        new AMove{targetPlayer = false, dir = -2},
        new AEndTurn()
      ],
      _ => [
        new AStunShip{targetPlayer = false},
        new AMove{targetPlayer = false, dir = -2},
        new AEndTurn()
      ],
    };
  }
}