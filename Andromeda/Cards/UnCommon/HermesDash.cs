using Fred.Andromeda;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fred.Andromeda.cards
{
  internal sealed class HermesDash : Card, IAndromedaCard
  {
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      helper.Content.Cards.RegisterCard("HermesDash", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.AndromedaDeck.Deck,
          rarity = Rarity.uncommon,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "HermesDash", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        exhaust = true,
        retain = upgrade == Upgrade.B ? true : false,
        cost = 2
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new AStatus{status = ModEntry.Instance.HermesDash.Status, statusAmount = 3, targetPlayer = true}
      ],
      Upgrade.B => [
        new AStatus{status = ModEntry.Instance.HermesDash.Status, statusAmount = 2, targetPlayer = true}
      ],
      _ => [
        new AStatus{status = ModEntry.Instance.HermesDash.Status, statusAmount = 2, targetPlayer = true}
      ],
    };
  }
}