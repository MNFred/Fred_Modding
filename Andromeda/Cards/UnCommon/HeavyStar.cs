using Fred.Andromeda;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fred.Andromeda.cards
{
  internal sealed class HeavyStar : Card, IAndromedaCard
  {
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      helper.Content.Cards.RegisterCard("HeavyStar", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.AndromedaDeck.Deck,
          rarity = Rarity.uncommon,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "HeavyStar", "name"]).Localize,
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
        new AStatus{status = ModEntry.Instance.ForcefullGravitate.Status, statusAmount = 2, targetPlayer = false},
        new AAttack{damage = GetDmg(s, 0)},
        new AAttack{damage = GetDmg(s, 0)}
      ],
      Upgrade.B => [
        new AStatus{status = ModEntry.Instance.ForcefullGravitate.Status, statusAmount = 3, targetPlayer = false},
        new AAttack{damage = GetDmg(s, 0)}
      ],
      _ => [
        new AStatus{status = ModEntry.Instance.ForcefullGravitate.Status, statusAmount = 2, targetPlayer = false},
        new AAttack{damage = GetDmg(s, 0)}
      ],
    };
  }
}