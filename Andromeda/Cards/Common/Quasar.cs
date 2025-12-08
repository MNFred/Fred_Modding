using Fred.Andromeda;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fred.Andromeda.cards
{
  internal sealed class Quasar : Card, IAndromedaCard
  {
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      helper.Content.Cards.RegisterCard("Quasar", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.AndromedaDeck.Deck,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Quasar", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        exhaust = upgrade == Upgrade.B ? true : false,
        cost = upgrade == Upgrade.B ? 0 : 2,
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new ASpawn{thing = new SpaceMine(), offset = -1},
        new ASpawn{thing = new Asteroid()},
        new ASpawn{thing = new SpaceMine(), offset = 1}
      ],
      Upgrade.B => [
        new ASpawn{thing = new SpaceMine(), offset = -1},
        new ASpawn{thing = new SpaceMine(), offset = 1}
      ],
      _ => [
        new ASpawn{thing = new SpaceMine(), offset = -1},
        new ASpawn{thing = new SpaceMine(), offset = 1}
      ],
    };
  }
}