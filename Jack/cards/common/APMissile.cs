using Fred.Jack.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fred.Jack.cards
{
  internal sealed class CardAPMissile : Card, IJackCard
  {
    private static ISpriteEntry MainArt = null!;

    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      MainArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/background_APMissile_Main.png"));
      helper.Content.Cards.RegisterCard("ApMissileCard", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.Jack_Deck.Deck,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "APMissileCard", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = MainArt.Sprite,
        cost = upgrade == Upgrade.B ? 2 : 1
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new ASpawn{
          thing = new APRocket{targetPlayer = false}
        },
        new AStatus{status = Status.droneShift, statusAmount = 1, targetPlayer = true}
        ],
      Upgrade.B => [
        new ASpawn{
          thing = new APRocket{targetPlayer = false}
        },
        new ASpawn{
          thing = new APRocket{targetPlayer = false},
          offset = 1
        },
      ],
      _ => [
        new ASpawn{
          thing = new APRocket{targetPlayer = false}
        },
      ],
    };
  }
}
