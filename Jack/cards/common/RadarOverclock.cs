using Fred.Jack.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Fred.Jack.cards
{
  internal sealed class RadarOverclock : Card, IJackCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/background_SignalBoost.png"));
      helper.Content.Cards.RegisterCard("RadarOverclock", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.Jack_Deck.Deck,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B],
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "RadarOverclock", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = ThisArt.Sprite,
        retain = upgrade == Upgrade.A ? true : false,
        cost = 1,
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new AStatus{
          status = Status.droneShift, statusAmount = 4, targetPlayer = true
        },
        new AStatus{
          status = ModEntry.Instance.LoseDroneshiftNextStatus.Status, statusAmount = 1, targetPlayer = true
        },
      ],
      Upgrade.B => [
        new AStatus{
          status = Status.droneShift, statusAmount = 6, targetPlayer = true
        },
        new AStatus{
          status = ModEntry.Instance.LoseDroneshiftNextStatus.Status, statusAmount = 1, targetPlayer = true
        },
      ],
      _ => [
        new AStatus{
          status = Status.droneShift, statusAmount = 4, targetPlayer = true
        },
        new AStatus{
          status = ModEntry.Instance.LoseDroneshiftNextStatus.Status, statusAmount = 1, targetPlayer = true
        },
      ],
    };
  }
}
