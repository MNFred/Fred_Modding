using Fred.Jack.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Fred.Jack.cards
{
  internal sealed class HiJackControls : Card, IJackCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/background_SendMalware.png"));
      helper.Content.Cards.RegisterCard("HiJackControls", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.Jack_Deck.Deck,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B],
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "HiJackControls", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = ThisArt.Sprite,
        cost = upgrade == Upgrade.B ? 0 : 1,
        retain = upgrade == Upgrade.A ? true : false,
        exhaust = upgrade == Upgrade.B ? true : false
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new AStatus{
          status = Status.backwardsMissiles, statusAmount = 2, targetPlayer = false
        }
      ],
      Upgrade.B => [
        new AStatus{
          status = Status.backwardsMissiles, statusAmount = 3, targetPlayer = false
        }
      ],
      _ => [
        new AStatus{
          status = Status.backwardsMissiles, statusAmount = 2, targetPlayer = false
        }
      ],
    };
  }
}
