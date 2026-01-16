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
  internal sealed class Hypertrophy : Card, IDougieCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/Hypertrophy.png"));
      helper.Content.Cards.RegisterCard("Hypertrophy", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.DougieDeck.Deck,
          rarity = Rarity.uncommon,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Hypertrophy", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = ThisArt.Sprite,
        retain = upgrade == Upgrade.A ? true : false,
        cost = 1
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new MutateAllF{timer = 0}
        ],
      Upgrade.B => [
        new MutateAllF{timer = 0},
        new AStatus{status = Status.droneShift, statusAmount = 1, targetPlayer = true}
      ],
      _ => [
        new MutateAllF{timer = 0}
      ],
    };
  }
}