using Dougie;
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
  internal sealed class Exoskeleton : Card, IDougieCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/Exoskeleton.png"));
      helper.Content.Cards.RegisterCard("Exoskeleton", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.DougieDeck.Deck,
          rarity = Rarity.rare,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Exoskeleton", "name"]).Localize,
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
        new AExoskeleton(),
        new AStatus{status = Status.droneShift, statusAmount = 1, targetPlayer = true}
        ],
      Upgrade.B => [
        new ABubbleField()
      ],
      _ => [
        new AExoskeleton()
      ],
    };
  }
}
public class AExoskeleton : CardAction
{
    public override Icon? GetIcon(State s)
     => new(ModEntry.Instance.BMutationIcon.Sprite,null, Colors.action, false);
    public override List<Tooltip> GetTooltips(State s)
    {
        return [new GlossaryTooltip($"action.{GetType().Namespace!}::MutateAllB")
			{
				Icon = ModEntry.Instance.BMutationIcon.Sprite,
				TitleColor = Colors.action,
				Title = ModEntry.Instance.Localizations.Localize(["action", "MutateAllB", "name"]),
				Description = ModEntry.Instance.Localizations.Localize(["action", "MutateAllB", "description"]),
			},
            new TTGlossary("midrow.bubbleShield"),
            new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::Midrow::Cell")
            {
                Icon = ModEntry.Instance.CellColonyIcon.Sprite,
                TitleColor = Colors.midrow,
                Title = ModEntry.Instance.Localizations.Localize(["midrow", "Cell", "name"]),
                Description = string.Format("Will block 1 shot before being destroyed.")
            },];
    }
    public override void Begin(G g, State s, Combat c)
    {
        foreach(StuffBase stuff in c.stuff.Values.ToList())
        {
            if(stuff is CellColony cellColony)
            {
                cellColony.bubbleShield = true;
            }
        }
    }
}