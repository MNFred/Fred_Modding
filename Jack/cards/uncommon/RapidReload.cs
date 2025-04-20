using Fred.Jack.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fred.Jack.cards
{
  internal sealed class RapidReload : Card, IJackCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/background_ExpandStorage.png"));
      helper.Content.Cards.RegisterCard("RapidReload", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.Jack_Deck.Deck,
          rarity = Rarity.uncommon,
          upgradesTo = [Upgrade.A, Upgrade.B],
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "RapidReload", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = ThisArt.Sprite,
        description = ModEntry.Instance.Localizations.Localize(["card", "RapidReload", "description", upgrade.ToString()]),
        retain = true,
        cost = 1,
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new ASelectAllSpawn{
          cardAmount = 3,
          discard = false
        }
      ],
      Upgrade.B => [
        new ASelectAllSpawn{
          cardAmount = 2,
          discard = true
        }
      ],
      _ => [
        new ASelectAllSpawn{
          cardAmount = 2,
          discard = false
        }
      ],
    };
  }
  public class ASelectAllSpawn : CardAction
  {
    public int cardAmount;
    public bool discard;
    private List<Card> spawnCard = new List<Card>();

    public override void Begin(G g, State s, Combat c)
    {
      spawnCard.Clear();
      if (!discard)
      {
        foreach (Card card in c.discard.Shuffle(s.rngActions))
        {
          foreach (CardAction action in card.GetActions(s, c))
          {
            if (action is ASpawn)
              spawnCard.Add(card);
          }
        }
      }
      if (discard)
      {
        foreach (Card card in s.deck.Shuffle(s.rngActions))
        {
          foreach (CardAction action in card.GetActions(s, c))
          {
            if (action is ASpawn)
              spawnCard.Add(card);
          }
        }
      }
      if (spawnCard.Count == 0)
        return;
      foreach (Card card in spawnCard)
      {
        s.RemoveCardFromWhereverItIs(card.uuid);
        c.SendCardToHand(s, card);
      }
    }
  }
}
