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
  internal sealed class MeatGrinder : Card, IDougieCard, IHasCustomCardTraits
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/MeatGrinder.png"));
      helper.Content.Cards.RegisterCard("MeatGrinder", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.DougieDeck.Deck,
          rarity = Rarity.rare,
          unreleased = true,
          dontOffer = true,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "MeatGrinder", "name"]).Localize,
      });
    }
    public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state)
	{
        HashSet<ICardTraitEntry> cardTraitEntries = new HashSet<ICardTraitEntry>();
        if(upgrade != Upgrade.B)
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
        cost = 0,
        temporary = true,
        retain = true,
        infinite = upgrade == Upgrade.B ? false : true,
        recycle = upgrade == Upgrade.B ? true : false
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        ModEntry.Instance.KokoroApi.ActionCosts.MakeCostAction(
          ModEntry.Instance.KokoroApi.ActionCosts.MakeResourceCost(
            new CellResource(), 1
          ),
				new AFakeMeatGrinder { changeAmount = 2, amountOfEnergy = 2}
			).AsCardAction
        ],
      Upgrade.B => [
        ModEntry.Instance.KokoroApi.ActionCosts.MakeCostAction(
          ModEntry.Instance.KokoroApi.ActionCosts.MakeResourceCost(
            new CellResource(), 1
          ),
				new AFakeMeatGrinder { changeAmount = 3, amountOfEnergy = 3}
			).AsCardAction
      ],
      _ => [
        ModEntry.Instance.KokoroApi.ActionCosts.MakeCostAction(
          ModEntry.Instance.KokoroApi.ActionCosts.MakeResourceCost(
            new CellResource(), 1
          ),
				new AFakeMeatGrinder { changeAmount = 1, amountOfEnergy = 1}
			).AsCardAction
      ],
    };
  }
}
public class AFakeMeatGrinder : AEnergy
{
    public required int amountOfEnergy;
    public override void Begin(G g, State s, Combat c)
    {
        c.QueueImmediate(new AEnergy{ changeAmount = amountOfEnergy });
        c.QueueImmediate(new HarvestMarkedCells{timer = 0.4});
        c.QueueImmediate(new PickCellColony{amountCells = 1, timer = 0});
    }
}