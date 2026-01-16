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
  internal sealed class LethalInjection : Card, IDougieCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/LethalInjection.png"));
      helper.Content.Cards.RegisterCard("LethalInjection", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.DougieDeck.Deck,
          rarity = Rarity.uncommon,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "LethalInjection", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = ThisArt.Sprite,
        cost = 2
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        ModEntry.Instance.KokoroApi.ActionCosts.MakeCostAction(
          ModEntry.Instance.KokoroApi.ActionCosts.MakeResourceCost(
            new CellResource(), 2
          ),
				new AFakeInjection { dmg = GetDmg(s,9) , damage = GetDmg(s,9), piercing = true}
			).AsCardAction
        ],
      Upgrade.B => [
        ModEntry.Instance.KokoroApi.ActionCosts.MakeCostAction(
          ModEntry.Instance.KokoroApi.ActionCosts.MakeResourceCost(
            new CellResource(), 1
          ),
				new AFakeInjectionB { dmg = GetDmg(s,4) , damage = GetDmg(s,4), piercing = true}
			).AsCardAction,
        ModEntry.Instance.KokoroApi.ActionCosts.MakeCostAction(
          ModEntry.Instance.KokoroApi.ActionCosts.MakeResourceCost(
            new CellResource(), 1
          ),
				new AFakeInjectionB { dmg = GetDmg(s,3) , damage = GetDmg(s,3), piercing = true}
			).AsCardAction
      ],
      _ => [
        ModEntry.Instance.KokoroApi.ActionCosts.MakeCostAction(
          ModEntry.Instance.KokoroApi.ActionCosts.MakeResourceCost(
            new CellResource(), 2
          ),
				new AFakeInjection { dmg = GetDmg(s,7) , damage = GetDmg(s,7), piercing = true}
			).AsCardAction
      ],
    };
  }
}
public class AFakeInjection : AAttack
{
  public required int dmg;
    public override void Begin(G g, State s, Combat c)
    {
        c.QueueImmediate(new AAttack{damage = dmg, piercing = true});
        c.QueueImmediate(new HarvestMarkedCells{timer = 0.4});
        c.QueueImmediate(new PickCellColony{amountCells = 2, timer = 0});
    }
}
public class AFakeInjectionB : AAttack
{
  public required int dmg;
    public override void Begin(G g, State s, Combat c)
    {
        c.QueueImmediate(new AAttack{damage = dmg, piercing = true});
        c.QueueImmediate(new HarvestMarkedCells{timer = 0.4});
        c.QueueImmediate(new PickCellColony{amountCells = 1, timer = 0});
    }
}