using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Fred.Jack.cards
{
  internal sealed class BackUpMissileCard : Card, IJackCard
  {
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      helper.Content.Cards.RegisterCard("BackUpMissileCard", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.Jack_Deck.Deck,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B],
          unreleased = true,
          dontOffer = true
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "BackUpMissileCard", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        cost = 0,
        temporary = true
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new ASpawn{
          thing = new Missile{targetPlayer = false, missileType = MissileType.heavy}
        },
      ],
      Upgrade.B => [
        new ASpawn{
          thing = new Missile{targetPlayer = false, missileType = MissileType.seeker}
        },
      ],
      _ => [
        new ASpawn{
          thing = new Missile{targetPlayer = false}
        },
      ],
    };
  }
}
