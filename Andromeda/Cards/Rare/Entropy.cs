using Fred.Andromeda;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fred.Andromeda.cards
{
  internal sealed class Entropy : Card, IAndromedaCard
  {
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      helper.Content.Cards.RegisterCard("Entropy", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.AndromedaDeck.Deck,
          rarity = Rarity.rare,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Entropy", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        exhaust = true,
        cost = upgrade == Upgrade.B ? 3 : 2,
        flippable = upgrade == Upgrade.A ? true : false
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new AMove{dir = 1, targetPlayer = true, isRandom = true},
        new AMove{dir = 2, targetPlayer = true},
        new AMove{dir = 3, targetPlayer = true, isRandom = true}
      ],
      Upgrade.B => [
        new AMove{dir = 1, targetPlayer = true, isRandom = true},
        new AMove{dir = 2, targetPlayer = true, isRandom = true},
        new AMove{dir = 3, targetPlayer = true, isRandom = true},
        new AMove{dir = 4, targetPlayer = true, isRandom = true}
      ],
      _ => [
        new AMove{dir = 1, targetPlayer = true, isRandom = true},
        new AMove{dir = 2, targetPlayer = true, isRandom = true},
        new AMove{dir = 3, targetPlayer = true, isRandom = true}
      ],
    };
  }
}