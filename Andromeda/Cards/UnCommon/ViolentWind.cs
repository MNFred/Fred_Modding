using Fred.Andromeda;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fred.Andromeda.cards
{
  internal sealed class ViolentWind : Card, IAndromedaCard
  {
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      helper.Content.Cards.RegisterCard("ViolentWind", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.AndromedaDeck.Deck,
          rarity = Rarity.uncommon,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "ViolentWind", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        exhaust = true,
        cost = upgrade == Upgrade.A ? 1 : 2
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new AStatus{status = ModEntry.Instance.PassiveGravitateStatus.Status, statusAmount = 3, targetPlayer = false},
        new AStatus{status = ModEntry.Instance.ForcefullGravitate.Status, statusAmount = 3, targetPlayer = false}
      ],
      Upgrade.B => [
        new AStatus{status = ModEntry.Instance.PassiveGravitateStatus.Status, statusAmount = 5, targetPlayer = false},
        new AStatus{status = ModEntry.Instance.ForcefullGravitate.Status, statusAmount = 5, targetPlayer = false}
      ],
      _ => [
        new AStatus{status = ModEntry.Instance.PassiveGravitateStatus.Status, statusAmount = 3, targetPlayer = false},
        new AStatus{status = ModEntry.Instance.ForcefullGravitate.Status, statusAmount = 3, targetPlayer = false}
      ],
    };
  }
}