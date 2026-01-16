using Fred.Jack.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fred.Jack.cards
{
  internal sealed class RadarAmp : Card, IJackCard
  {
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      helper.Content.Cards.RegisterCard("RadarAmp", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.Jack_Deck.Deck,
          rarity = Rarity.rare,
          upgradesTo = [Upgrade.A, Upgrade.B],
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "RadarAmp", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        cost = 1,
        exhaust = upgrade == Upgrade.B ? true : false,
        retain = upgrade == Upgrade.A ? true : false
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new AVariableHint{
          status = Status.droneShift
        },
        new AStatus{
          status = ModEntry.Instance.LockOnStatus.Status,
          statusAmount = Math.Max(0, s.ship.Get(Status.droneShift)),
          xHint = 1,
          targetPlayer = false
        },
        new AStatus{
          status = Status.droneShift,
          statusAmount = 0,
          mode = AStatusMode.Set,
          targetPlayer = true
        }
      ],
      Upgrade.B => [
        new AVariableHint{
          status = Status.droneShift
        },
        new AStatus{
          status = ModEntry.Instance.ALockOnStatus.Status,
          statusAmount = Math.Max(0, s.ship.Get(Status.droneShift)),
          xHint = 1,
          targetPlayer = false
        },
        new AStatus{
          status = Status.droneShift,
          statusAmount = 0,
          mode = AStatusMode.Set,
          targetPlayer = true
        }
      ],
      _ => [
        new AVariableHint{
          status = Status.droneShift
        },
        new AStatus{
          status = ModEntry.Instance.LockOnStatus.Status,
          statusAmount = Math.Max(0, s.ship.Get(Status.droneShift)),
          xHint = 1,
          targetPlayer = false
        },
        new AStatus{
          status = Status.droneShift,
          statusAmount = 0,
          mode = AStatusMode.Set,
          targetPlayer = true
        }
      ],
    };
  }
}
