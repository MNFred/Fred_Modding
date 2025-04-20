using Fred.Jack.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Fred.Jack.cards
{
  internal sealed class BlankMissileCard : Card, IJackCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/background_RocketDisposal.png"));
      helper.Content.Cards.RegisterCard("BlankMissile", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.Jack_Deck.Deck,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B],
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "BlankMissile", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = ThisArt.Sprite,
        cost = 1,
        exhaust = upgrade == Upgrade.B ? true : false
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new ASpawn{
          thing = new BlankMissile{targetPlayer = false}
        },
        new AStatus{
          status = Status.droneShift, statusAmount = 1, targetPlayer = true
        }
      ],
      Upgrade.B => [
        new ASpawn{
          thing = new BlankMissile{targetPlayer = false},
          offset = -1
        },
        new ASpawn{
          thing = new BlankMissile{targetPlayer = false}
        },
        new ASpawn{
          thing = new BlankMissile{targetPlayer = false},
          offset = 1
        },
      ],
      _ => [
        new ASpawn{
          thing = new BlankMissile{targetPlayer = false}
        },
      ],
    };
  }
}
