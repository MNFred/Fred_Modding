using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Fred.Jack.cards
{
  internal sealed class PanicButtonCard : Card, IJackCard, IHasCustomCardTraits
  {
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      helper.Content.Cards.RegisterCard("PanicButtonCard", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.Jack_Deck.Deck,
          rarity = Rarity.uncommon,
          upgradesTo = [Upgrade.A, Upgrade.B],
          unreleased = true,
          dontOffer = true
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "PanicButtonCard", "name"]).Localize,
      });
    }
    public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state)
		=> new HashSet<ICardTraitEntry> { ModEntry.Instance.kokoroV2.Fleeting.Trait };
    public override CardData GetData(State state)
    {
      return new CardData()
      {
        cost = 1,
        temporary = true,
        exhaust = true
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new AStatus{status = ModEntry.Instance.MidrowHaltStatus.Status, statusAmount = 2, targetPlayer = true}
      ],
      Upgrade.B => [
        new AStatus{status = ModEntry.Instance.MidrowHaltStatus.Status, statusAmount = 1, targetPlayer = true},
        new AStatus{status = Status.droneShift, statusAmount = 2, targetPlayer = true}
      ],
      _ => [
        new AStatus{status = ModEntry.Instance.MidrowHaltStatus.Status, statusAmount = 1, targetPlayer = true}
      ],
    };
  }
}
