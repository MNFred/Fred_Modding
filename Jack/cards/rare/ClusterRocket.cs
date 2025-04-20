using Fred.Jack.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fred.Jack.cards
{
  internal sealed class ClusterMissileCard : Card, IJackCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/background_ClusterMissile.png"));
      helper.Content.Cards.RegisterCard("ClusterMissileCard", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.Jack_Deck.Deck,
          rarity = Rarity.rare,
          upgradesTo = [Upgrade.A, Upgrade.B],
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "ClusterMissileCard", "name"]).Localize,
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
        new ASpawn{
          thing = new ClusterMissile{targetPlayer = false}
        }
      ],
      Upgrade.B => [
        new ASpawn{
          thing = new ClusterMissile{targetPlayer = false}
        },
        new ASpawn{
          thing = new Asteroid()
        }
      ],
      _ => [
        new ASpawn{
          thing = new ClusterMissile{targetPlayer = false}
        }
      ],
    };
  }
}
