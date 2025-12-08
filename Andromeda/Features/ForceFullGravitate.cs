using System;
using System.Linq;
using Andromeda.External;
using Fred.Andromeda;
using Fred.Andromeda.Artifacts;
using Fred.Andromeda.cards;

namespace Andromeda.features;

internal sealed class ForcefullGravitateManager : IKokoroApi.IV2.IStatusLogicApi.IHook
{
	public ForcefullGravitateManager()
	{
		ModEntry.Instance.KokoroApi.StatusLogic.RegisterHook(this);
        ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook("OnPlayerAttack", (State state, Combat combat) =>
        {
            if (state.ship.Get(ModEntry.Instance.ForcefullGravitate.Status) > 0)
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
                state.ship.Add(ModEntry.Instance.ForcefullGravitate.Status, -1);
            }
            if (combat.otherShip.Get(ModEntry.Instance.ForcefullGravitate.Status)>0)
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
                combat.otherShip.Add(ModEntry.Instance.ForcefullGravitate.Status, -1);
            }
        },0);
	}
}