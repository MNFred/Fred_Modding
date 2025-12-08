using Fred.Andromeda;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fred.Andromeda.cards
{
  internal sealed class LoneMoon : Card, IAndromedaCard
  {
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      helper.Content.Cards.RegisterCard("LoneMoon", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.AndromedaDeck.Deck,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "LoneMoon", "name"]).Localize,
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
        new ASpawn{thing = new Asteroid{bubbleShield = true}, offset = -1},
        new AStatus{status= Status.evade, statusAmount = 1, targetPlayer = true}
      ],
      Upgrade.B => [
        new ASpawn{thing = new Asteroid(), offset = -1},
        new AStatus{status= Status.droneShift, statusAmount = 2, targetPlayer = true},
        new AStatus{status= Status.evade, statusAmount = 1, targetPlayer = true}
      ],
      _ => [
        new ASpawn{thing = new Asteroid(), offset = -1},
        new AStatus{status= Status.evade, statusAmount = 1, targetPlayer = true}
      ],
    };
  }
}