using System;
using System.Linq;
using Andromeda.External;
using Fred.Andromeda;
using Fred.Andromeda.Artifacts;
using Fred.Andromeda.cards;

namespace Andromeda.features;

internal sealed class PassiveGravitateManager : IKokoroApi.IV2.IStatusLogicApi.IHook
{
	public PassiveGravitateManager()
	{
		ModEntry.Instance.KokoroApi.StatusLogic.RegisterHook(this);
        ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook("OnPlayerPlayCard", (int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount) =>
        {
            if (state.ship.Get(ModEntry.Instance.PassiveGravitateStatus.Status) > 0)
            {
                if (state.EnumerateAllArtifacts().FirstOrDefault(a => a is ShatteredStar) is { } artifact)
                {
                    if(card.GetMeta().deck != ModEntry.Instance.AndromedaDeck.Deck)
                    {
                        if(combat.hand.Any(c => c is Polarity polarity && polarity.flipped == false))
                        {
                            combat.Queue(new AMove { dir = -2, targetPlayer = true, timer = 0.1 });
                        }
                        else if(combat.hand.Any(c => c is Polarity polarity && polarity.flipped == true))
                        {
                            combat.Queue(new AMove { dir = 2, targetPlayer = true, timer = 0.1 });
                        }
                        else
                        {
                            combat.Queue(new AMove { dir = 2, targetPlayer = true, timer = 0.1 });
                        }
                        state.ship.Add(ModEntry.Instance.PassiveGravitateStatus.Status, -1);
                    }
                }
                else
                {
                    if(combat.hand.Any(c => c is Polarity polarity && polarity.flipped == false))
                    {
                        combat.Queue(new AMove { dir = -1, targetPlayer = true, timer = 0.1 });
                    }
                    else if(combat.hand.Any(c => c is Polarity polarity && polarity.flipped == true))
                    {
                        combat.Queue(new AMove { dir = 1, targetPlayer = true, timer = 0.1 });
                    }
                    else
                    {
                        combat.Queue(new AMove { dir = 1, targetPlayer = true, timer = 0.1 });
                    }
                    state.ship.Add(ModEntry.Instance.PassiveGravitateStatus.Status, -1);
                }
            }
            if (combat.otherShip.Get(ModEntry.Instance.PassiveGravitateStatus.Status)>0)
            {
                if (state.EnumerateAllArtifacts().FirstOrDefault(a => a is ShatteredStar) is { } artifact)
                {
                    if(card.GetMeta().deck != ModEntry.Instance.AndromedaDeck.Deck)
                    {
                        if(combat.hand.Any(c => c is Polarity polarity && polarity.flipped == false))
                        {
                            combat.Queue(new AMove { dir = -2, targetPlayer = false, timer = 0.1 });
                        }
                        else if(combat.hand.Any(c => c is Polarity polarity && polarity.flipped == true))
                        {
                            combat.Queue(new AMove { dir = 2, targetPlayer = false, timer = 0.1 });
                        }
                        else
                        {
                            combat.Queue(new AMove { dir = 2, targetPlayer = false, timer = 0.1 });
                        }
                        combat.otherShip.Add(ModEntry.Instance.PassiveGravitateStatus.Status, -1);
                    }
                }
                else
                {
                    if(combat.hand.Any(c => c is Polarity polarity && polarity.flipped == false))
                    {
                        combat.Queue(new AMove { dir = -1, targetPlayer = false, timer = 0.1 });
                    }
                    else if(combat.hand.Any(c => c is Polarity polarity && polarity.flipped == true))
                    {
                        combat.Queue(new AMove { dir = 1, targetPlayer = false, timer = 0.1 });
                    }
                    else
                    {
                        combat.Queue(new AMove { dir = 1, targetPlayer = false, timer = 0.1 });
                    }
                    combat.otherShip.Add(ModEntry.Instance.PassiveGravitateStatus.Status, -1);
                }
            }
        },0);
	}
}