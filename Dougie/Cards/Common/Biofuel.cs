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
  internal sealed class Biofuel : Card, IDougieCard, IHasCustomCardTraits
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/Biofuel.png"));
      helper.Content.Cards.RegisterCard("Biofuel", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.DougieDeck.Deck,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Biofuel", "name"]).Localize,
      });
    }
    public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state)
	{
        HashSet<ICardTraitEntry> cardTraitEntries = new HashSet<ICardTraitEntry>();
        if(upgrade == Upgrade.B)
        {
		    this.SetSymbiotic(true);
            cardTraitEntries.Add(ModEntry.Instance.SymbioticTrait);
        }
		return cardTraitEntries;
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
				new AFakeBiofuel { status = Status.evade, statusAmount = 2, targetPlayer = true}
			).AsCardAction
        ],
      Upgrade.B => [
        new AStatus{status = Status.evade, targetPlayer = true, statusAmount = 2}
      ],
      _ => [
        ModEntry.Instance.KokoroApi.ActionCosts.MakeCostAction(
          ModEntry.Instance.KokoroApi.ActionCosts.MakeResourceCost(
            new CellResource(), 1
          ),
				new AFakeBiofuel { status = Status.evade, statusAmount = 2, targetPlayer = true}
			).AsCardAction
      ],
    };
  }
}
public class AFakeBiofuel : AStatus
{
    public override void Begin(G g, State s, Combat c)
    {
        c.QueueImmediate(new AStatus{ status = Status.evade, statusAmount = 2, targetPlayer = true});
        c.QueueImmediate(new HarvestMarkedCells{timer = 0.4});
        c.QueueImmediate(new PickCellColony{amountCells = 1, timer = 0});
    }
}