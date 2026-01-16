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
  internal sealed class Bioweapon : Card, IDougieCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/Bioweapon.png"));
      helper.Content.Cards.RegisterCard("Bioweapon", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.DougieDeck.Deck,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Bioweapon", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = ThisArt.Sprite,
        cost = 1
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new AStatus{status = Status.evade, statusAmount = 1, targetPlayer = true},
        ModEntry.Instance.KokoroApi.ActionCosts.MakeCostAction(
          ModEntry.Instance.KokoroApi.ActionCosts.MakeResourceCost(
            new CellResource(), 1
          ),
				new AFakeBioweapon { dmg = GetDmg(s,3) , damage = GetDmg(s,3), piercing = true}
			).AsCardAction
        ],
      Upgrade.B => [
        new AStatus{status = Status.evade, statusAmount = 1, targetPlayer = true},
        new AStatus{status = Status.droneShift, statusAmount = 1, targetPlayer = true},
        ModEntry.Instance.KokoroApi.ActionCosts.MakeCostAction(
          ModEntry.Instance.KokoroApi.ActionCosts.MakeResourceCost(
            new CellResource(), 1
          ),
				new AFakeBioweapon { dmg = GetDmg(s,2) , damage = GetDmg(s,2), piercing = true}
			).AsCardAction
      ],
      _ => [
        new AStatus{status = Status.evade, statusAmount = 1, targetPlayer = true},
        ModEntry.Instance.KokoroApi.ActionCosts.MakeCostAction(
          ModEntry.Instance.KokoroApi.ActionCosts.MakeResourceCost(
            new CellResource(), 1
          ),
				new AFakeBioweapon { dmg = GetDmg(s,2) , damage = GetDmg(s,2), piercing = true}
			).AsCardAction
      ],
    };
  }
}
public class AFakeBioweapon : AAttack
{
  public required int dmg;
    public override void Begin(G g, State s, Combat c)
    {
        c.QueueImmediate(new AAttack{damage = dmg, piercing = true});
        c.QueueImmediate(new HarvestMarkedCells{timer = 0.4});
        c.QueueImmediate(new PickCellColony{amountCells = 1, timer = 0});
    }
}