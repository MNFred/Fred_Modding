using Dougie.features;
using Dougie.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Dougie.cards
{
  internal sealed class Clone : Card, IDougieCard, IHasCustomCardTraits
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/Clone.png"));
      helper.Content.Cards.RegisterCard("Clone", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.DougieDeck.Deck,
          rarity = Rarity.uncommon,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Clone", "name"]).Localize,
      });
    }
    public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state)
	{
        HashSet<ICardTraitEntry> cardTraitEntries = new HashSet<ICardTraitEntry>();
        if(upgrade != Upgrade.A)
        {
		    this.SetSymbiotic(true);
            cardTraitEntries.Add(ModEntry.Instance.SymbioticTrait);
      }
      else
      {
        this.SetSymbiotic(false);
      }
		return cardTraitEntries;
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
        new ASpawn{thing = new CellColony{targetPlayer = false}},
        new AMove{dir = 1, targetPlayer = true},
        new ASpawn{thing = new CellColony{targetPlayer = false}},
        ],
      Upgrade.B => [
        new ASpawn{thing = new CellColony{targetPlayer = false, bubbleShield = true}},
        new AMove{dir = 1, targetPlayer = true},
        new ASpawn{thing = new CellColony{targetPlayer = false}},
      ],
      _ => [
        new ASpawn{thing = new CellColony{targetPlayer = false}},
        new AMove{dir = 1, targetPlayer = true},
        new ASpawn{thing = new CellColony{targetPlayer = false}},
      ],
    };
  }
}