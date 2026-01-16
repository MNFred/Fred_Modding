using Dougie.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Dougie.cards
{
  internal sealed class CellColonyCard : Card, IDougieCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/CellColony.png"));
      helper.Content.Cards.RegisterCard("CellColonyCard", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.DougieDeck.Deck,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "CellColonyCard", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = ThisArt.Sprite,
        cost = 1
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new ASpawn{thing = new CellColony{targetPlayer = false}},
        new AStatus{status = Status.droneShift, statusAmount = 2, targetPlayer = true}
        ],
      Upgrade.B => [
        new ASpawn{thing = new CellColony{targetPlayer = false, bubbleShield = true}},
        new AStatus{status = Status.droneShift, statusAmount = 1, targetPlayer = true}
      ],
      _ => [
        new ASpawn{thing = new CellColony{targetPlayer = false}},
        new AStatus{status = Status.droneShift, statusAmount = 1, targetPlayer = true}
      ],
    };
  }
}