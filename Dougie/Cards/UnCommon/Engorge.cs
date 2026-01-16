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
  internal sealed class Engorge : Card, IDougieCard
  {
    private static ISpriteEntry ThisTopArt = null!;
    private static ISpriteEntry ThisBottomArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisTopArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/Engorge_Top.png"));
      ThisBottomArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/Engorge_Bottom.png"));
      helper.Content.Cards.RegisterCard("Engorge", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.DougieDeck.Deck,
          rarity = Rarity.uncommon,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Engorge", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = flipped ? ThisTopArt.Sprite : ThisBottomArt.Sprite,
        floppable = true,
        cost = 1
      };
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        List<CardAction> actions = new List<CardAction>();
        CardAction action1 = new ASpawn{thing = new CellColony{targetPlayer = false, bubbleShield = upgrade == Upgrade.B ? true : false}};
        CardAction action2 = ModEntry.Instance.KokoroApi.ActionCosts.MakeCostAction(
            ModEntry.Instance.KokoroApi.ActionCosts.MakeResourceCost(
                new CellResource(), 1
            ),
                    new AFakeEngorge { status = Status.shield, statusAmount = upgrade == Upgrade.A ? 3 : 2, shieldAmount = upgrade == Upgrade.A ? 3 : 2, targetPlayer = true}
                ).AsCardAction;
        CardAction action3 = new AStatus{ status = Status.tempShield, statusAmount = 1, targetPlayer = true};
        CardAction noAction = new ADummyAction{timer = 0};
        action1.disabled = !flipped;
        action2.disabled = flipped;
        action3.disabled = flipped;
        actions.Add(action1);
        actions.Add(noAction);
        actions.Add(action2);
        actions.Add(action3);
        return actions;
    }
  }
}
public class AFakeEngorge : AStatus
{
    public required int shieldAmount;
    public override void Begin(G g, State s, Combat c)
    {
        c.QueueImmediate(new AStatus{ status = Status.shield, statusAmount = shieldAmount, targetPlayer = true});
        c.QueueImmediate(new HarvestMarkedCells{timer = 0.4});
        c.QueueImmediate(new PickCellColony{amountCells = 1, timer = 0});
    }
}