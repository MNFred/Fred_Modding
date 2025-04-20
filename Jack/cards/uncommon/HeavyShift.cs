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
          rarity = Rarity.uncommon,
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
        infinite = upgrade == Upgrade.B ? true : false,
        cost = 1,
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new ADroneMove{
          dir = 2, playerDidIt = true, 
        },
        new AMove{
          dir = -1, targetPlayer = true
        }
      ],
      Upgrade.B => [
        new ADroneMove{
          dir = 1, playerDidIt = true, 
        },
      ],
      _ => [
        new ADroneMove{
          dir = 2, playerDidIt = true, 
        }
      ],
    };
  }
}
