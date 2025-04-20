
using Fred.Jack.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Fred.Jack.cards
{
  internal sealed class MissileThrow : Card, IJackCard
  {
    private static ISpriteEntry MainArt = null!;

    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      MainArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/background_MissileThrow.png"));
      helper.Content.Cards.RegisterCard("MissileThrow", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.Jack_Deck.Deck,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "MissileThrow", "name"]).Localize,
      });
    }
    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = MainArt.Sprite,
        cost = upgrade == Upgrade.B ? 2 : 1
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new ASpawn{
          thing = new Missile{targetPlayer = false, missileType = MissileType.seeker},
          offset = -1
        },
        new ASpawn{
          thing = new Missile{targetPlayer = false},
        }
      ],
      Upgrade.B => [
        new ASpawn{
          thing = new Missile{targetPlayer = false, missileType = MissileType.heavy},
          offset = -1
        },
        new ASpawn{
          thing = new Missile{targetPlayer = false, missileType = MissileType.heavy},
        }
      ],
      _ => [
        new ASpawn{
          thing = new MiniMissile{targetPlayer = false},
          offset = -1
        },
        new ASpawn{
          thing = new Missile{targetPlayer = false},
        }
      ],
    };
  }
}
