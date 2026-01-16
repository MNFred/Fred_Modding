using Dougie.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Dougie.cards
{
  internal sealed class DougieEXE : Card, IDougieCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/DougieEXE.png"));
      helper.Content.Cards.RegisterCard("DougieEXE", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = Deck.colorless,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "DougieEXE", "name"]).Localize,
      });
    }
    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = ThisArt.Sprite,
        exhaust = true,
        artTint = "CC6641",
        cost = upgrade == Upgrade.A ? 0 : 1,
        description = ModEntry.Instance.Localizations.Localize(["card", "DougieEXE", "description", upgrade.ToString()]),
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new ASpawn{thing = new CellColony{targetPlayer = false}},
        new ACardOffering
				{
					amount = 3,
					limitDeck = ModEntry.Instance.DougieDeck.Deck,
					makeAllCardsTemporary = true,
					overrideUpgradeChances = false,
					canSkip = false,
					inCombat = true,
					discount = -1,
				}
      ],
      Upgrade.B => [
        new ASpawn{thing = new CellColony{targetPlayer = false}},
        new ACardOffering
				{
					amount = 4,
					limitDeck = ModEntry.Instance.DougieDeck.Deck,
					makeAllCardsTemporary = true,
					overrideUpgradeChances = false,
					canSkip = false,
					inCombat = true,
					discount = -1,
				}
      ],
      _ => [
        new ASpawn{thing = new CellColony{targetPlayer = false}},
        new ACardOffering
				{
					amount = 3,
					limitDeck = ModEntry.Instance.DougieDeck.Deck,
					makeAllCardsTemporary = true,
					overrideUpgradeChances = false,
					canSkip = false,
					inCombat = true,
					discount = -1,
				}
      ],
    };
  }
}