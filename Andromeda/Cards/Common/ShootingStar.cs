using Fred.Andromeda;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fred.Andromeda.cards
{
  internal sealed class ShootingStar : Card, IAndromedaCard
  {
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      helper.Content.Cards.RegisterCard("ShootingStar", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.AndromedaDeck.Deck,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "ShootingStar", "name"]).Localize,
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
        new AAttack{damage = GetDmg(s, 2)},
        new AStatus{status = ModEntry.Instance.ForcefullGravitate.Status, statusAmount = 2, targetPlayer = false}
      ],
      Upgrade.B => [
        new AAttack{damage = GetDmg(s, 0), stunEnemy = true},
        new AStatus{status = ModEntry.Instance.ForcefullGravitate.Status, statusAmount = 2, targetPlayer = false}
      ],
      _ => [
        new AAttack{damage = GetDmg(s, 1)},
        new AStatus{status = ModEntry.Instance.ForcefullGravitate.Status, statusAmount = 2, targetPlayer = false}
      ],
    };
  }
}