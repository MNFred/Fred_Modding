using Fred.Andromeda;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fred.Andromeda.cards
{
  internal sealed class LawOfMotion : Card, IAndromedaCard
  {
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      helper.Content.Cards.RegisterCard("LawOfMotion", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.AndromedaDeck.Deck,
          rarity = Rarity.uncommon,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "LawOfMotion", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        retain = upgrade == Upgrade.A ? true : false,
        cost = 1
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new AVariableHint{status = Status.evade},
        new AStatus{status = ModEntry.Instance.PassiveGravitateStatus.Status, statusAmount = s.ship.Get(Status.evade), targetPlayer = false, xHint = 1},
        new AStatus{status = Status.evade, statusAmount = -2, targetPlayer = true}
      ],
      Upgrade.B => [
        new AVariableHint{status = Status.evade},
        new AStatus{status = ModEntry.Instance.PassiveGravitateStatus.Status, statusAmount = s.ship.Get(Status.evade), targetPlayer = false, xHint = 1},
        new AStatus{status = Status.evade, mode = AStatusMode.Set, statusAmount = 0, targetPlayer = true}
      ],
      _ => [
        new AVariableHint{status = Status.evade},
        new AStatus{status = ModEntry.Instance.PassiveGravitateStatus.Status, statusAmount = s.ship.Get(Status.evade), targetPlayer = false, xHint = 1},
        new AStatus{status = Status.evade, mode = AStatusMode.Set, statusAmount = 0, targetPlayer = true}
      ],
    };
  }
}