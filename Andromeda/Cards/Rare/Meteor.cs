using Fred.Andromeda;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fred.Andromeda.cards
{
  internal sealed class Meteor : Card, IAndromedaCard
  {
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      helper.Content.Cards.RegisterCard("Meteor", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.AndromedaDeck.Deck,
          rarity = Rarity.rare,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Meteor", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        exhaust = true,
        cost = 3,
        flippable = upgrade == Upgrade.A ? true : false
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new AAttack{damage = GetDmg(s, 4), moveEnemy = -5}
      ],
      Upgrade.B => [
        new AAttack{damage = GetDmg(s, 4), moveEnemy = -5, piercing = true}
      ],
      _ => [
        new AAttack{damage = GetDmg(s, 4), moveEnemy = -5}
      ],
    };
  }
}