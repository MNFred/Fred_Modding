using Fred.Andromeda;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fred.Andromeda.cards
{
  internal sealed class LazyShift : Card, IAndromedaCard
  {
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      helper.Content.Cards.RegisterCard("LazyShift", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.AndromedaDeck.Deck,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "LazyShift", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        cost = 1
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new AStatus{status = ModEntry.Instance.PassiveGravitateStatus.Status, statusAmount = 2, targetPlayer = true},
        new ADrawCard{count = 2}
      ],
      Upgrade.B => [
        new AStatus{status = ModEntry.Instance.PassiveGravitateStatus.Status, statusAmount = 2, targetPlayer = true},
        new AStatus{status = Status.drawNextTurn, statusAmount = 1, targetPlayer = true},
      ],
      _ => [
        new AStatus{status = ModEntry.Instance.PassiveGravitateStatus.Status, statusAmount = 2, targetPlayer = true},
        new ADrawCard{count = 1}
      ],
    };
  }
}