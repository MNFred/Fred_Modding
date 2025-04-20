using Fred.Jack.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fred.Jack.cards
{
  internal sealed class MissileBarrage : Card, IJackCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/background_MissileBarrage2.png"));
      helper.Content.Cards.RegisterCard("MissileBarrage", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.Jack_Deck.Deck,
          rarity = Rarity.uncommon,
          upgradesTo = [Upgrade.A, Upgrade.B],
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "MissileBarrage", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = ThisArt.Sprite,
        flippable = upgrade == Upgrade.A ? true : false,
        cost = upgrade == Upgrade.B ? 2 : 1,
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new ASpawn{
          thing = new MiniMissile{targetPlayer = false}
        },
        new AMove{
          dir = 1, targetPlayer = true
        },
        new ASpawn{
          thing = new BlankMissile{targetPlayer = false}
        },
        new AMove{
          dir = 1, targetPlayer = true
        },
        new ASpawn{
          thing = new Missile{targetPlayer = false, missileType = MissileType.normal}
        },
      ],
      Upgrade.B => [
        new ASpawn{
          thing = new BlankMissile{targetPlayer = false}
        },
        new AMove{
          dir = 1, targetPlayer = true
        },
        new ASpawn{
          thing = new Missile{targetPlayer = false, missileType = MissileType.normal}
        },
        new AMove{
          dir = 1, targetPlayer = true
        },
        new ASpawn{
          thing = new Missile{targetPlayer = false, missileType = MissileType.heavy}
        },
      ],
      _ => [
        new ASpawn{
          thing = new MiniMissile{targetPlayer = false}
        },
        new AMove{
          dir = 1, targetPlayer = true
        },
        new ASpawn{
          thing = new BlankMissile{targetPlayer = false}
        },
        new AMove{
          dir = 1, targetPlayer = true
        },
        new ASpawn{
          thing = new Missile{targetPlayer = false, missileType = MissileType.normal}
        },
      ],
    };
  }
}
