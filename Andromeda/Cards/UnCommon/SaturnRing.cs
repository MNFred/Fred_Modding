using Fred.Andromeda;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Fred.Andromeda.cards
{
  internal sealed class SaturnRing : Card, IAndromedaCard
  {
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
      helper.Content.Cards.RegisterCard("SaturnRing", new()
      {
        CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
        Meta = new()
        {
          deck = ModEntry.Instance.AndromedaDeck.Deck,
          rarity = Rarity.uncommon,
          upgradesTo = [Upgrade.A, Upgrade.B]
        },
        Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "SaturnRing", "name"]).Localize,
      });
    }
    public override CardData GetData(State state)
    {
      return new CardData()
      {
        description = ModEntry.Instance.Localizations.Localize(["card", "SaturnRing", "desc", upgrade.ToString()]),
        cost = upgrade == Upgrade.B ? 3 : 2,
        exhaust = true,
        retain = upgrade == Upgrade.A ? true : false
      };
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
      Upgrade.A => [
        new SaturnRingAction{upgradeB = false}
      ],
      Upgrade.B => [
        new SaturnRingAction{upgradeB = true}
      ],
      _ => [
        new SaturnRingAction{upgradeB = false}
      ],
    };
  }
}
public class SaturnRingAction : CardAction
{
    public bool upgradeB = false;
    public override List<Tooltip> GetTooltips(State s)
    {
        switch(upgradeB)
        {
            case true:
                return [new TTGlossary("midrow.asteroid"), new TTGlossary("midrow.bubbleShield")];
            case false:
                return [new TTGlossary("midrow.asteroid")];
        }
    }
    public override void Begin(G g, State s, Combat c)
    {
        foreach(Part part in s.ship.parts)
        {
            if(part!= null)
                c.QueueImmediate(new ASpawn{thing = new Asteroid{bubbleShield = upgradeB}, fromX = s.ship.GetLocalXOfPart(part.key!)});
        }
    }
}