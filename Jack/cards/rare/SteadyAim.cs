using Fred.Jack.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fred.Jack.cards
{
  internal sealed class SteadyAim : Card, IJackCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/background_Improvise.png"));
      helper.Content.Cards.RegisterCard("SteadyAim", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.Jack_Deck.Deck,
          rarity = Rarity.rare,
          upgradesTo = [Upgrade.A, Upgrade.B],
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "SteadyAim", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = ThisArt.Sprite,
        exhaust = true,
        description = ModEntry.Instance.Localizations.Localize(["card", "SteadyAim", "description", upgrade.ToString()]),
        cost = 1,
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new AAddCard{
          card = new TargetPractice{temporaryOverride = true},
          amount = 3,
          destination = CardDestination.Deck
        }
      ],
      Upgrade.B => [
        new AAddCard{
          card = new TargetPractice{temporaryOverride = true, upgrade = Upgrade.B},
          amount = 2,
          destination = CardDestination.Deck
        }
      ],
      _ => [
        new AAddCard{
          card = new TargetPractice{temporaryOverride = true},
          amount = 2,
          destination = CardDestination.Deck
        }
      ],
    };
  }
}
