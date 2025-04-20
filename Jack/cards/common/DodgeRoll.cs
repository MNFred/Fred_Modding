using Fred.Jack.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fred.Jack.cards
{
  internal sealed class DodgeRoll : Card, IJackCard
  {
    private static ISpriteEntry MainArt = null!;

    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      MainArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/background_DodgeRoll.png"));
      helper.Content.Cards.RegisterCard("DodgeRoll", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.Jack_Deck.Deck,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "DodgeRoll", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = MainArt.Sprite,
        flippable = upgrade == Upgrade.A ? true : false,
        recycle = upgrade == Upgrade.B ? true : false,
        cost = 1
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new AMove{
          dir = -1,
          targetPlayer = true
        },
        new AStatus{
          status = Status.tempShield,
          statusAmount = 2,
          targetPlayer = true
        }
      ],
      Upgrade.B => [
        new AMove{
          dir = -1,
          targetPlayer = true
        },
        new AStatus{
          status = Status.tempShield,
          statusAmount = 2,
          targetPlayer = true
        }
      ],
      _ => [
        new AMove{
          dir = -1,
          targetPlayer = true
        },
        new AStatus{
          status = Status.tempShield,
          statusAmount = 2,
          targetPlayer = true
        }
      ],
    };
  }
}
