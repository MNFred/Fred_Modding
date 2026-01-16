using Dougie.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Dougie.cards
{
  internal sealed class Outbreak : Card, IDougieCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/Outbreak.png"));
      helper.Content.Cards.RegisterCard("Outbreak", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.DougieDeck.Deck,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Outbreak", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = ThisArt.Sprite,
        flippable = upgrade == Upgrade.A ? true : false,
        cost = upgrade == Upgrade.B ? 3 : 2
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new ASpawn{thing = new CellColony{targetPlayer = false}},
        new AMove{dir = -2, targetPlayer = true},
        new ASpawn{thing = new CellColony{targetPlayer = false}},
        ],
      Upgrade.B => [
        new ASpawn{thing = new CellColony{targetPlayer = false}},
        new AMove{dir = -1, targetPlayer = true},
        new ASpawn{thing = new CellColony{targetPlayer = false}},
        new AMove{dir = -1, targetPlayer = true},
        new ASpawn{thing = new CellColony{targetPlayer = false}},
      ],
      _ => [
        new ASpawn{thing = new CellColony{targetPlayer = false}},
        new AMove{dir = -2, targetPlayer = true},
        new ASpawn{thing = new CellColony{targetPlayer = false}},
      ],
    };
  }
}