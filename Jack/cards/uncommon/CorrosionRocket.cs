using Fred.Jack.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fred.Jack.cards
{
  internal sealed class CorrosonMissileCard : Card, IJackCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/background_CorrosiveMissile.png"));
      helper.Content.Cards.RegisterCard("CorrosonMissileCard", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.Jack_Deck.Deck,
          rarity = Rarity.uncommon,
          upgradesTo = [Upgrade.A, Upgrade.B],
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "CorrosonMissileCard", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = ThisArt.Sprite,
        exhaust = true,
        cost = upgrade == Upgrade.B ? 1 : 2
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new ASpawn{
          thing = new Missile{targetPlayer = false, missileType = MissileType.corrode}
        },
        new AStatus{
          status = Status.droneShift, statusAmount = 1, targetPlayer = true
        }
      ],
      Upgrade.B => [
        new ASpawn{
          thing = new Missile{targetPlayer = true, missileType = MissileType.corrode},
          offset = -1
        },
        new ASpawn{
          thing = new Missile{targetPlayer = false, missileType = MissileType.corrode},
        },
      ],
      _ => [
        new ASpawn{
          thing = new Missile{targetPlayer = false, missileType = MissileType.corrode}
        },
      ],
    };
  }
}
