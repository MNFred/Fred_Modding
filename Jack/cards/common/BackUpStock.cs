using Fred.Jack.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fred.Jack.cards
{
  internal sealed class BackUpMissile : Card, IJackCard
  {
    private static ISpriteEntry MainArt = null!;

    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      MainArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/background_FlareGun.png"));
      helper.Content.Cards.RegisterCard("BackUpMissile", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.Jack_Deck.Deck,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "BackUpMissile", "name"]).Localize,
      });
    }
    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = MainArt.Sprite,
        exhaust = true,
        description = ModEntry.Instance.Localizations.Localize(["card", "BackUpMissile", "description", upgrade.ToString()]),
        cost = 2
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new AAddCard{
          card = new BackUpMissileCard{upgrade = Upgrade.A},
          destination = CardDestination.Hand,
          amount = 1,
        }
      ],
      Upgrade.B => [
        new AAddCard{
          card = new BackUpMissileCard(),
          destination = CardDestination.Deck,
          amount = 2,
        },
      ],
      _ => [
        new AAddCard{
          card = new BackUpMissileCard(),
          destination = CardDestination.Hand,
          amount = 1,
        }
      ],
    };
  }
}
