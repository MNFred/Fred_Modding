using Fred.Jack.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fred.Jack.cards
{
  internal sealed class JackEXE : Card, IJackCard
  {
    private static ISpriteEntry ThisArt = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      ThisArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cards/Jack_BG.png"));
      helper.Content.Cards.RegisterCard("JackEXE", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = Deck.colorless,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "JackEXE", "name"]).Localize,
      });
    }
    public override CardData GetData(State state)
    {
      return new CardData()
      {
        art = ThisArt.Sprite,
        exhaust = true,
        artTint = "0cd85d",
        cost = upgrade == Upgrade.A ? 0 : 1,
        description = ModEntry.Instance.Localizations.Localize(["card", "JackEXE", "description", upgrade.ToString()]),
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new ACardOffering
				{
					amount = 2,
					limitDeck = ModEntry.Instance.Jack_Deck.Deck,
					makeAllCardsTemporary = true,
					overrideUpgradeChances = false,
					canSkip = false,
					inCombat = true,
					discount = -1,
					dialogueSelector = ".jack_EXE"
				}
      ],
      Upgrade.B => [
        new ACardOffering
				{
					amount = 3,
					limitDeck = ModEntry.Instance.Jack_Deck.Deck,
					makeAllCardsTemporary = true,
					overrideUpgradeChances = false,
					canSkip = false,
					inCombat = true,
					discount = -1,
					dialogueSelector = ".jack_EXE"
				}
      ],
      _ => [
        new ACardOffering
				{
					amount = 2,
					limitDeck = ModEntry.Instance.Jack_Deck.Deck,
					makeAllCardsTemporary = true,
					overrideUpgradeChances = false,
					canSkip = false,
					inCombat = true,
					discount = -1,
					dialogueSelector = ".jack_EXE"
				}
      ],
    };
  }
}
