using Dougie;
using Dougie.Actions;
using Dougie.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Dougie.cards
{
  internal sealed class RSelection : Card, IDougieCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/RSelection.png"));
      helper.Content.Cards.RegisterCard("RSelection", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.DougieDeck.Deck,
          rarity = Rarity.rare,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "RSelection", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = ThisArt.Sprite,
        description = ModEntry.Instance.Localizations.Localize(["card", "RSelection", "description", upgrade.ToString()]),
        retain = upgrade == Upgrade.A ? true : false,
        cost = upgrade == Upgrade.B ? 2 : 1
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new ARSelection{destroyWeaklings = true}
        ],
      Upgrade.B => [
        new ARSelection{destroyWeaklings = false}
      ],
      _ => [
        new ARSelection{destroyWeaklings = true}
      ],
    };
  }
}
public class ARSelection : CardAction
{
    public bool destroyWeaklings = true;
    public override List<Tooltip> GetTooltips(State s)
    {
        return [new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::Midrow::Cell")
                {
                    Icon = ModEntry.Instance.CellColonyIcon.Sprite,
                    TitleColor = Colors.midrow,
                    Title = ModEntry.Instance.Localizations.Localize(["midrow", "Cell", "name"]),
                    Description = string.Format("Will block 1 shot before being destroyed.")
                }];
    }
    public override void Begin(G g, State s, Combat c)
    {
        if(destroyWeaklings)
        {
            foreach(StuffBase stuff in c.stuff.Values.ToList())
            {
                if(stuff is CellColony cellColony)
                {
                    if(!cellColony.MutationA && !cellColony.MutationD && !cellColony.MutationF && !cellColony.MutationX)
                    {
                        c.DestroyDroneAt(s,cellColony.x,true);
                    }
                }
            }
        }
        foreach(StuffBase stuff in c.stuff.Values.ToList())
        {
            if(stuff is CellColony cellColony)
            {
                c.Queue(new ASpawnCellFromCell
                {
                    worldX = cellColony.x,
                    thing = new CellColony(),
                    offset = -1,
                    byPlayer = true,
                });
                c.Queue(new ASpawnCellFromCell
                {
                    worldX = cellColony.x,
                    thing = new CellColony(),
                    offset = 1,
                    byPlayer = true
                });
            }
        }
    }
}