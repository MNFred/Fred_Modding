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
  internal sealed class Cryptobiosis : Card, IDougieCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/Cryptobiosis.png"));
      helper.Content.Cards.RegisterCard("Cryptobiosis", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.DougieDeck.Deck,
          rarity = Rarity.rare,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Cryptobiosis", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = ThisArt.Sprite,
        exhaust = true,
        cost = upgrade == Upgrade.A ? 2 : 3
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new ASpawn{thing = new CellColony{targetPlayer = false}},
        new CryptobiosisExtraTooltips{status = ModEntry.Instance.CryptobiosisStatus.Status, statusAmount = 1, targetPlayer = true, timer = 0}
        ],
      Upgrade.B => [
        new ASpawn{thing = new CellColony{targetPlayer = false}},
        new AMove{dir = 2, targetPlayer = true},
        new ASpawn{thing = new CellColony{targetPlayer = false}},
        new CryptobiosisExtraTooltips{status = ModEntry.Instance.CryptobiosisStatus.Status, statusAmount = 1, targetPlayer = true, timer = 0}
      ],
      _ => [
        new ASpawn{thing = new CellColony{targetPlayer = false}},
        new CryptobiosisExtraTooltips{status = ModEntry.Instance.CryptobiosisStatus.Status, statusAmount = 1, targetPlayer = true, timer = 0}
      ],
    };
  }
}
public class CryptobiosisExtraTooltips : AStatus
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [..StatusMeta.GetTooltips(ModEntry.Instance.CryptobiosisStatus.Status,1),
        new TTGlossary("midrow.asteroid"), 
        new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::Midrow::Cell"){Icon = ModEntry.Instance.CellColonyIcon.Sprite, TitleColor = Colors.midrow, Title = ModEntry.Instance.Localizations.Localize(["midrow", "Cell", "name"]), Description = string.Format("Will block 1 shot before being destroyed.")},
        new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::action::CellHarvest"){	Icon = ModEntry.Instance.CostUnsatisfiedIcon.Sprite,	TitleColor = Colors.action,	Title = "CELL HARVEST",	Description = "Choose <c=keyword>#</c> <c=midrow>cell colonies</c> at most 1 space offset from your ship to destroy. If there are not enough, this action does not happen."},];
    }
    public override void Begin(G g, State s, Combat c)
    {
        c.QueueImmediate(new AStatus{status = ModEntry.Instance.CryptobiosisStatus.Status, statusAmount = 1, targetPlayer = true});
    }
}