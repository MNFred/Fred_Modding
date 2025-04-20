using Fred.Jack.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Fred.Jack.cards
{
  internal sealed class MissileSalvo : Card, IJackCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/background_MissileBarrage.png"));
      helper.Content.Cards.RegisterCard("MissileSalvo", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.Jack_Deck.Deck,
          rarity = Rarity.uncommon,
          upgradesTo = [Upgrade.A, Upgrade.B],
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "MissileSalvo", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = ThisArt.Sprite,
        cost = upgrade == Upgrade.B ? 3 : 2,
        exhaust = upgrade == Upgrade.A ? false : true
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new ASpawn{
          thing = new APRocket{targetPlayer = false},
          offset = -1
        },
        new ASpawn{
          thing = new Missile{targetPlayer = false, missileType = MissileType.normal},
        },
        new ASpawn{
          thing = new Missile{targetPlayer = false, missileType = MissileType.seeker},
          offset = 1
        },
        new ASpawn{
          thing = new MiniMissile{targetPlayer = false},
          offset = 2
        },
      ],
      Upgrade.B => [
        new ASpawn{
          thing = new Missile{targetPlayer = false, missileType = MissileType.heavy},
          offset = -1
        },
        new ASpawn{
          thing = new BalisticDormant{targetPlayer = false},
        },
        new ASpawn{
          thing = new Missile{targetPlayer = false, missileType = MissileType.normal},
          offset = 1
        },
        new ASpawn{
          thing = new BlankMissile{targetPlayer = false},
          offset = 2
        },
      ],
      _ => [
        new ASpawn{
          thing = new APRocket{targetPlayer = false},
          offset = -1
        },
        new ASpawn{
          thing = new Missile{targetPlayer = false, missileType = MissileType.normal},
        },
        new ASpawn{
          thing = new Missile{targetPlayer = false, missileType = MissileType.seeker},
          offset = 1
        },
        new ASpawn{
          thing = new MiniMissile{targetPlayer = false},
          offset = 2
        },
      ],
    };
  }
}
