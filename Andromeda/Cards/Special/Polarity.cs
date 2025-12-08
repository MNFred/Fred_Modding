using Fred.Andromeda;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fred.Andromeda.cards
{
  internal sealed class Polarity : Card, IAndromedaCard
  {
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      helper.Content.Cards.RegisterCard("Polarity", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.AndromedaDeck.Deck,
          rarity = Rarity.common,
          dontOffer = true,
          unreleased = true,
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Polarity", "name"]).Localize,
      });
    }

    public override CardData GetData(State state)
    {
      return new CardData()
      {
        description = flipped ? "<c=status>GRAVITY</c> statuses now move things to the <c=keyword>right</c>. <c=keyword>Flip to toggle.</c>" : "<c=status>GRAVITY</c> statuses now move things to the <c=keyword>left</c>. <c=keyword>Flip to toggle.</c>",
        cost = 1,
        flippable = true,
        unplayable = true,
        temporary = true,
        retain = true
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      _ => [
        new PolarityDirection{whywouldyouplaythiscardWhatsthepointdude = flipped},
        new PolarityDirection{whywouldyouplaythiscardWhatsthepointdude = !flipped}
      ],
    };
  }
}
public class PolarityDirection : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
    {
        return [..StatusMeta.GetTooltips(ModEntry.Instance.PassiveGravitateStatus.Status,1), ..StatusMeta.GetTooltips(ModEntry.Instance.ForcefullGravitate.Status,1)];
    }
    public bool whywouldyouplaythiscardWhatsthepointdude = false;
    public override void Begin(G g, State s, Combat c)
    {
      c.QueueImmediate(new AStatus{status = ModEntry.Instance.PassiveGravitateStatus.Status, statusAmount = 10, targetPlayer = true, disabled = whywouldyouplaythiscardWhatsthepointdude});
      c.QueueImmediate(new AStatus{status = ModEntry.Instance.ForcefullGravitate.Status, statusAmount = 10, targetPlayer = true, disabled = !whywouldyouplaythiscardWhatsthepointdude});
    }
}