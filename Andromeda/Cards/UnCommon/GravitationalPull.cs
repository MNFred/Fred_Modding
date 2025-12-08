using Fred.Andromeda;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fred.Andromeda.cards
{
  internal sealed class GravitationalPull : Card, IAndromedaCard
  {
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      helper.Content.Cards.RegisterCard("GravitationalPull", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.AndromedaDeck.Deck,
          rarity = Rarity.uncommon,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "GravitationalPull", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        description = ModEntry.Instance.Localizations.Localize(["card", "GravitationalPull", "desc", upgrade.ToString()]),
        exhaust = true,
        cost = upgrade == Upgrade.A ? 0 : 1
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new BringGravitate{upgradeB = false}
      ],
      Upgrade.B => [
        new BringGravitate{upgradeB = true}
      ],
      _ => [
        new BringGravitate{upgradeB = false}
      ],
    };
  }
}
public class BringGravitate : CardAction
{
    public bool upgradeB = false;
    private List<Card> gravityCard = new List<Card>();
    public override List<Tooltip> GetTooltips(State s)
    {
        return [..StatusMeta.GetTooltips(ModEntry.Instance.PassiveGravitateStatus.Status,1), ..StatusMeta.GetTooltips(ModEntry.Instance.ForcefullGravitate.Status,1)];
    }
    public override void Begin(G g, State s, Combat c)
    {
        gravityCard.Clear();
        switch(upgradeB)
        {
            case false:
                foreach(Card card in s.deck)
                {
                    foreach(CardAction action in card.GetActions(s,c))
                    {
                        if(action is AStatus aStatus && aStatus.status == ModEntry.Instance.PassiveGravitateStatus.Status || action is AStatus aStatus2 && aStatus2.status == ModEntry.Instance.ForcefullGravitate.Status)
                        {
                            gravityCard.Add(card);
                        }
                    }
                }
            break;
            case true:
                foreach(Card card in c.discard)
                {
                    foreach(CardAction action in card.GetActions(s,c))
                    {
                        if(action is AStatus aStatus && aStatus.status == ModEntry.Instance.PassiveGravitateStatus.Status || action is AStatus aStatus2 && aStatus2.status == ModEntry.Instance.ForcefullGravitate.Status)
                        {
                            gravityCard.Add(card);
                        }
                    }
                }
            break;
        }
        if(gravityCard.Count == 0)
            return;
        foreach(Card card1 in gravityCard)
        {
            s.RemoveCardFromWhereverItIs(card1.uuid);
            c.SendCardToHand(s,card1);
        }
    }
}