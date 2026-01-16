using Dougie;
using Dougie.Actions;
using Dougie.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Dougie.cards
{
  internal sealed class Apoptosis : Card, IDougieCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/Apoptosis.png"));
      helper.Content.Cards.RegisterCard("Apoptosis", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.DougieDeck.Deck,
          rarity = Rarity.rare,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Apoptosis", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = ThisArt.Sprite,
        exhaust = true,
        cost = upgrade == Upgrade.A ? 1 : 2
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new ApoptosisExtraTooltips{status = ModEntry.Instance.ApoptosisStatus.Status, statusAmount = 1, targetPlayer = true, timer = 0, tatusAmount = 1}
        ],
      Upgrade.B => [
        new ApoptosisExtraTooltips{status = ModEntry.Instance.ApoptosisStatus.Status, statusAmount = 2, targetPlayer = true, timer = 0, tatusAmount = 2}
      ],
      _ => [
        new ApoptosisExtraTooltips{status = ModEntry.Instance.ApoptosisStatus.Status, statusAmount = 1, targetPlayer = true, timer = 0, tatusAmount = 1}
      ],
    };
  }
}
public class ApoptosisExtraTooltips : AStatus
{
    public required int tatusAmount;
    public override List<Tooltip> GetTooltips(State s)
    {
        return [..StatusMeta.GetTooltips(ModEntry.Instance.ApoptosisStatus.Status,1),
        new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::Midrow::Cell"){Icon = ModEntry.Instance.CellColonyIcon.Sprite, TitleColor = Colors.midrow, Title = ModEntry.Instance.Localizations.Localize(["midrow", "Cell", "name"]), Description = string.Format("Will block 1 shot before being destroyed.")},
        new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::action::CellHarvest"){	Icon = ModEntry.Instance.CostUnsatisfiedIcon.Sprite,	TitleColor = Colors.action,	Title = "CELL HARVEST",	Description = "Choose <c=keyword>#</c> <c=midrow>cell colonies</c> at most 1 space offset from your ship to destroy. If there are not enough, this action does not happen."},];
    }
    public override void Begin(G g, State s, Combat c)
    {
        c.QueueImmediate(new AStatus{status = ModEntry.Instance.ApoptosisStatus.Status, statusAmount = statusAmount, targetPlayer = true});
    }
}