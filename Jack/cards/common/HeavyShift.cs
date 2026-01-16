using Fred.Jack.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fred.Jack.cards
{
  internal sealed class HeavyShift : Card, IJackCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/background_RemoteActive.png"));
      helper.Content.Cards.RegisterCard("HeavyShift", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.Jack_Deck.Deck,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B],
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "HeavyShift", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = ThisArt.Sprite,
        flippable = true,
        cost = 1,
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new AStatus{status = ModEntry.Instance.MidrowHaltStatus.Status, statusAmount = 1, targetPlayer = true},
        new AStatus{status = Status.droneShift, statusAmount = 1, targetPlayer = true},
        new ADroneMove{dir = -2}
      ],
      Upgrade.B => [
        new AStatus{status = ModEntry.Instance.MidrowHaltStatus.Status, statusAmount = 2, targetPlayer = true},
        new ADroneMove{dir = -2}
      ],
      _ => [
        new AStatus{status = ModEntry.Instance.MidrowHaltStatus.Status, statusAmount = 1, targetPlayer = true},
        new ADroneMove{dir = -2}
      ],
    };
  }
}
