using Fred.Jack.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fred.Jack.cards
{
  internal sealed class BalisticMissileCard : Card, IJackCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/background_BalisticMissile.png"));
      helper.Content.Cards.RegisterCard("BalisticMissileCard", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.Jack_Deck.Deck,
          rarity = Rarity.uncommon,
          upgradesTo = [Upgrade.A, Upgrade.B],
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "BalisticMissileCard", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = ThisArt.Sprite,
        cost = upgrade == Upgrade.B ? 3 : 2
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new ASpawn{
          thing = new BalisticDormant{targetPlayer = false}
        },
        new ADroneTurn{ignoreEnemyTurn = true}
      ],
      Upgrade.B => [
        new ASpawn{
          thing = new BalisticDormant{targetPlayer = false},
          offset = -1
        },
        new ASpawn{
          thing = new BalisticDormant{targetPlayer = false},
          offset = 1
        }
      ],
      _ => [
        new ASpawn{
          thing = new BalisticDormant{targetPlayer = false}
        }
      ],
    };
  }
}
