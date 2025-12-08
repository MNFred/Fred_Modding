using Fred.Andromeda;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fred.Andromeda.cards
{
  internal sealed class PiercingStar : Card, IAndromedaCard
  {
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      helper.Content.Cards.RegisterCard("PiercingStar", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.AndromedaDeck.Deck,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "PiercingStar", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        flippable = upgrade == Upgrade.A ? true : false,
        cost = 1
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new AAttack{damage = GetDmg(s, 1), piercing = true},
        new AMove{targetPlayer = true, dir = -1}
      ],
      Upgrade.B => [
        new AAttack{damage = GetDmg(s, 1), piercing = true},
        new AAttack{damage = GetDmg(s, 1)},
        new AMove{targetPlayer = true, dir = -1}
      ],
      _ => [
        new AAttack{damage = GetDmg(s, 1), piercing = true},
        new AMove{targetPlayer = true, dir = -1}
      ],
    };
  }
}