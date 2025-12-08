using Fred.Andromeda;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fred.Andromeda.cards
{
  internal sealed class WindNudge : Card, IAndromedaCard
  {
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      helper.Content.Cards.RegisterCard("WindNudge", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.AndromedaDeck.Deck,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "WindNudge", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        flippable = upgrade == Upgrade.A ? true : false,
        cost = 2
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new AMove{dir = -2, targetPlayer = false},
        new AStatus{status = Status.tempShield, statusAmount = 1, targetPlayer = true}
      ],
      Upgrade.B => [
        new AMove{dir = -2, targetPlayer = false},
        new AAttack{damage = 1, moveEnemy = -2},
        new AStatus{status = Status.tempShield, statusAmount = 1, targetPlayer = true}
      ],
      _ => [
        new AMove{dir = -2, targetPlayer = false},
        new AStatus{status = Status.tempShield, statusAmount = 1, targetPlayer = true}
      ],
    };
  }
}