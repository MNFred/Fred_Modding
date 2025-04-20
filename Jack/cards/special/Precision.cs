using Fred.Jack.Midrow;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fred.Jack.cards
{
  internal sealed class PrecisionCard : Card, IJackCard
  {
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      helper.Content.Cards.RegisterCard("PrecisionCard", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.Jack_Deck.Deck,
          rarity = Rarity.common,
          upgradesTo = [Upgrade.A, Upgrade.B],
          unreleased = true,
          dontOffer = true
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "PrecisionCard", "name"]).Localize,
      });
    }
    public override CardData GetData(State state)
    {
      return new CardData()
      {
        temporary = true,
        exhaust = upgrade == Upgrade.B ? true : false,
        cost = upgrade == Upgrade.B ? 1 : 0
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new AStatus{
          status = ModEntry.Instance.LockOnStatus.Status,
          statusAmount = 2,
          targetPlayer = false
        }
      ],
      Upgrade.B => [
        new AStatus{
          status = ModEntry.Instance.ALockOnStatus.Status,
          statusAmount = 1,
          targetPlayer = false
        }
      ],
      _ => [
        new AStatus{
          status = ModEntry.Instance.LockOnStatus.Status,
          statusAmount = 1,
          targetPlayer = false
        }
      ],
    };
  }
}
