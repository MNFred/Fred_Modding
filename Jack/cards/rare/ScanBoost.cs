using Fred.Jack.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fred.Jack.cards
{
  internal sealed class ScanBoostCard : Card, IJackCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/background_ScanBoost.png"));
      helper.Content.Cards.RegisterCard("ScanBoostCard", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.Jack_Deck.Deck,
          rarity = Rarity.rare,
          upgradesTo = [Upgrade.A, Upgrade.B],
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "ScanBoostCard", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = ThisArt.Sprite,
        exhaust = true,
        buoyant = upgrade == Upgrade.A ? true : false,
        cost = 1,
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new AStatus{
          status = ModEntry.Instance.ScanBoostStatus.Status,
          statusAmount = 1,
          targetPlayer = true
        }
      ],
      Upgrade.B => [
        new AStatus{
          status = ModEntry.Instance.ScanBoostStatus.Status,
          statusAmount = 1,
          targetPlayer = true
        },
        new AStatus{
          status = Status.droneShift,
          statusAmount = 2,
          targetPlayer = true
        }
      ],
      _ => [
        new AStatus{
          status = ModEntry.Instance.ScanBoostStatus.Status,
          statusAmount = 1,
          targetPlayer = true
        }
      ],
    };
  }
}
