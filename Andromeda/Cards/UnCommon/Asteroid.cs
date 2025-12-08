using Fred.Andromeda;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fred.Andromeda.cards
{
  internal sealed class AsteroidCard : Card, IAndromedaCard
  {
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      helper.Content.Cards.RegisterCard("Asteroid", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.AndromedaDeck.Deck,
          rarity = Rarity.uncommon,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Asteroid", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        cost = 2
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new AAttack{damage = GetDmg(s, 1), moveEnemy = -1},
        new AAttack{damage = GetDmg(s, 1), moveEnemy = -1},
        new AAttack{damage = GetDmg(s, 1), moveEnemy = -2},
      ],
      Upgrade.B => [
        new AAttack{damage = GetDmg(s, 1), moveEnemy = -3},
        new AAttack{damage = GetDmg(s, 1), moveEnemy = -1, stunEnemy = true},
      ],
      _ => [
        new AAttack{damage = GetDmg(s, 1), moveEnemy = -1},
        new AAttack{damage = GetDmg(s, 1), moveEnemy = -2}
      ],
    };
  }
}