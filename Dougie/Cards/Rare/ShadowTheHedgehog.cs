using Dougie.Actions;
using Dougie.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Dougie.cards
{
  internal sealed class ShadowTheHedgehog : Card, IDougieCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/TheUltimateLifeform.png"));
      helper.Content.Cards.RegisterCard("ShadowTheHedgehog", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.DougieDeck.Deck,
          rarity = Rarity.rare,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "ShadowTheHedgehog", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = ThisArt.Sprite,
        cost = 3,
        exhaust = upgrade == Upgrade.A ? false : true
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new MutateAllA{timer = 0},
        new MutateAllD{timer = 0},
        new MutateAllX{timer = 0},
        new MutateAllF{timer = 0}
        ],
      Upgrade.B => [
        new MutateAllA{timer = 0},
        new MutateAllD{timer = 0},
        new MutateAllX{timer = 0},
        new MutateAllF{timer = 0},
        new AExoskeleton()
      ],
      _ => [
        new MutateAllA{timer = 0},
        new MutateAllD{timer = 0},
        new MutateAllX{timer = 0},
        new MutateAllF{timer = 0}
      ],
    };
  }
}