using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fred.Andromeda.cards
{
  internal sealed class AndromedaEXE : Card, IAndromedaCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/Card/sameartbutIneedthisfortheEXEcarddontmindthis.png"));
      helper.Content.Cards.RegisterCard("AndromedaEXE", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = Deck.colorless,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "AndromedaEXE", "name"]).Localize,
      });
    }
    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = ThisArt.Sprite,
        exhaust = true,
        artTint = "000999",
        cost = upgrade == Upgrade.A ? 0 : 1,
        description = ModEntry.Instance.Localizations.Localize(["card", "AndromedaEXE", "description", upgrade.ToString()]),
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new ACardOffering
				{
					amount = 2,
					limitDeck = ModEntry.Instance.AndromedaDeck.Deck,
					makeAllCardsTemporary = true,
					overrideUpgradeChances = false,
					canSkip = false,
					inCombat = true,
					discount = -1,
				}
      ],
      Upgrade.B => [
        new ACardOffering
				{
					amount = 3,
					limitDeck = ModEntry.Instance.AndromedaDeck.Deck,
					makeAllCardsTemporary = true,
					overrideUpgradeChances = false,
					canSkip = false,
					inCombat = true,
					discount = -1,
				}
      ],
      _ => [
        new ACardOffering
				{
					amount = 2,
					limitDeck = ModEntry.Instance.AndromedaDeck.Deck,
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