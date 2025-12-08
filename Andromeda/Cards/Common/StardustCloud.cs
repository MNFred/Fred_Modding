using Fred.Andromeda;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fred.Andromeda.cards
{
  internal sealed class StardustCloud : Card, IAndromedaCard
  {
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      helper.Content.Cards.RegisterCard("StardustCloud", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.AndromedaDeck.Deck,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "StardustCloud", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        cost = upgrade == Upgrade.B ? 2 : 1
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new AStatus{status = Status.evade, statusAmount = 1, targetPlayer = true},
        new AStatus{status = ModEntry.Instance.PassiveGravitateStatus.Status, statusAmount = 2, targetPlayer = false}
      ],
      Upgrade.B => [
        new AStatus{status = Status.evade, statusAmount = 1, targetPlayer = true},
        new AMove{dir = 1, targetPlayer = true},
        new AStatus{status = ModEntry.Instance.PassiveGravitateStatus.Status, statusAmount = 2, targetPlayer = false}
      ],
      _ => [
        new AStatus{status = Status.evade, statusAmount = 1, targetPlayer = true},
        new AStatus{status = ModEntry.Instance.PassiveGravitateStatus.Status, statusAmount = 1, targetPlayer = false}
      ],
    };
  }
}