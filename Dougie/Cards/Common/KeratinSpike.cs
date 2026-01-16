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
  internal sealed class KeratinSpike : Card, IDougieCard, IHasCustomCardTraits
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/KeratinSpike.png"));
      helper.Content.Cards.RegisterCard("KeratinSpike", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.DougieDeck.Deck,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "KeratinSpike", "name"]).Localize,
      });
    }
    public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state)
	{
		this.SetSymbiotic(true);
		HashSet<ICardTraitEntry> cardTraitEntries = new HashSet<ICardTraitEntry>()
		{
			ModEntry.Instance.SymbioticTrait
		};
		return cardTraitEntries;
	}
    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = ThisArt.Sprite,
        infinite = true,
        cost = upgrade == Upgrade.A ? 0 : 1
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new AAttack{damage = GetDmg(s,1), piercing = true},
        ModEntry.Instance.KokoroApi.ActionCosts.MakeCostAction(
          ModEntry.Instance.KokoroApi.ActionCosts.MakeResourceCost(
            new CellResource(), 1
          ),
				new AFakeKeratinAttack { dmg = GetDmg(s,1) , damage = GetDmg(s,1), piercing = true}
			).AsCardAction
        ],
      Upgrade.B => [
        new AAttack{damage = GetDmg(s,1), piercing = true},
        new AAttack{damage = GetDmg(s,1), piercing = true},
        ModEntry.Instance.KokoroApi.ActionCosts.MakeCostAction(
          ModEntry.Instance.KokoroApi.ActionCosts.MakeResourceCost(
            new CellResource(), 1
          ),
				new AFakeKeratinAttack { dmg = GetDmg(s,1) , damage = GetDmg(s,1), piercing = true}
			).AsCardAction
      ],
      _ => [
        new AAttack{damage = GetDmg(s,1), piercing = true},
        ModEntry.Instance.KokoroApi.ActionCosts.MakeCostAction(
          ModEntry.Instance.KokoroApi.ActionCosts.MakeResourceCost(
            new CellResource(), 1
          ),
				new AFakeKeratinAttack { dmg = GetDmg(s,1) , damage = GetDmg(s,1), piercing = true}
			).AsCardAction
      ],
    };
  }
}
public class AFakeKeratinAttack : AAttack
{
  public required int dmg;
    public override void Begin(G g, State s, Combat c)
    {
        c.QueueImmediate(new AAttack{damage = dmg, piercing = true});
        c.QueueImmediate(new HarvestMarkedCells{timer = 0.4});
        c.QueueImmediate(new PickCellColony{amountCells = 1, timer = 0});
    }
}