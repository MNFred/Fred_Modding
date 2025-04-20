using Fred.Jack.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fred.Jack.cards
{
  internal sealed class OnMyMark : Card, IJackCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/background_GunpowderDivert.png"));
      helper.Content.Cards.RegisterCard("OnMyMark", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.Jack_Deck.Deck,
          rarity = Rarity.uncommon,
          upgradesTo = [Upgrade.A, Upgrade.B],
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "OnMyMark", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = ThisArt.Sprite,
        retain = true,
        exhaust = true,
        cost = upgrade == Upgrade.A ? 0 : 1,
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new AStatus{
          status = ModEntry.Instance.ALockOnStatus.Status,
          statusAmount = 1,
          targetPlayer = false
        },
        new AStatus{
          status = ModEntry.Instance.MidrowHaltStatus.Status,
          statusAmount = 1,
          targetPlayer = true
        }
      ],
      Upgrade.B => [
        new AStatus{
          status = ModEntry.Instance.ALockOnStatus.Status,
          statusAmount = 2,
          targetPlayer = false
        },
        new AStatus{
          status = ModEntry.Instance.MidrowHaltStatus.Status,
          statusAmount = 2,
          targetPlayer = true
        }
      ],
      _ => [
        new AStatus{
          status = ModEntry.Instance.ALockOnStatus.Status,
          statusAmount = 1,
          targetPlayer = false
        },
        new AStatus{
          status = ModEntry.Instance.MidrowHaltStatus.Status,
          statusAmount = 1,
          targetPlayer = true
        }
      ],
    };
  }
}
