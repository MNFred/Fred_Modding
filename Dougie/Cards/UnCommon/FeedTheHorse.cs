using Dougie.Actions;
using Dougie.features;
using Dougie.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using static Dougie.Actions.CellHarvest;

namespace Dougie.cards
{
  internal sealed class FeedTheHorde : Card, IDougieCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/FeedTheHorde.png"));
      helper.Content.Cards.RegisterCard("FeedTheHorde", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.DougieDeck.Deck,
          rarity = Rarity.uncommon,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "FeedTheHorde", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = ThisArt.Sprite,
        cost = upgrade == Upgrade.A ? 0 : 1
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        ModEntry.Instance.KokoroApi.ActionCosts.MakeCostAction(
          ModEntry.Instance.KokoroApi.ActionCosts.MakeResourceCost(
            new CellResource(), 1
          ),
				new AFakeHordeFeed()
			).AsCardAction
        ],
      Upgrade.B => [
        new MutateAllX{timer = 0}
      ],
      _ => [
        ModEntry.Instance.KokoroApi.ActionCosts.MakeCostAction(
          ModEntry.Instance.KokoroApi.ActionCosts.MakeResourceCost(
            new CellResource(), 1
          ),
				new AFakeHordeFeed()
			).AsCardAction
      ],
    };
  }
}
public class AFakeHordeFeed : MutateAllX
{
    public override void Begin(G g, State s, Combat c)
    {
        c.QueueImmediate(new MutateAllX{timer = 0});
        c.QueueImmediate(new HarvestMarkedCells{timer = 0.4});
        c.QueueImmediate(new PickCellColony{amountCells = 1, timer = 0});
    }
}