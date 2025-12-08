using System;
using System.Linq;
using System.Reflection;
using Andromeda.External;
using Fred.Andromeda;
using HarmonyLib;

namespace Andromeda.features;

internal sealed class HermesDashManager : IKokoroApi.IV2.IStatusLogicApi.IHook
{
	public HermesDashManager()
	{
		ModEntry.Instance.KokoroApi.StatusLogic.RegisterHook(this);
		ModEntry.Instance.Harmony.Patch(
		  original: AccessTools.DeclaredMethod(typeof(Combat), nameof(Combat.DrainCardActions)),
		  prefix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Combat_DrainCardActions_Prefix))
	  );
	}
	private static void Combat_DrainCardActions_Prefix(G g, Combat __instance)
    {
      State state = g.state;
	  int change = 1;
      if(__instance.cardActions.Any(a => a is AMove))
      {
		AMove curMove = (AMove)__instance.cardActions.Find(a => a is AMove)!;
		if (curMove.dir < 0)
            {
                change = -1;
            }
            else
            {
                change = 1;
            }
        if(state.ship.Get(ModEntry.Instance.HermesDash.Status) > 0)
        {
			if(curMove.targetPlayer)
			{
				__instance.QueueImmediate(new AMove{targetPlayer = true, dir = state.ship.Get(ModEntry.Instance.HermesDash.Status) * change, timer = 0.1});
				state.ship.Set(ModEntry.Instance.HermesDash.Status,0);
				return;
			}
        }
		if(__instance.otherShip.Get(ModEntry.Instance.HermesDash.Status)>0)
        {
			if(!curMove.targetPlayer)
			{
				__instance.QueueImmediate(new AMove{targetPlayer = false, dir = __instance.otherShip.Get(ModEntry.Instance.HermesDash.Status) * change, timer = 0.1});
				__instance.otherShip.Set(ModEntry.Instance.HermesDash.Status,0);
				return;
			}
        }
      }
    }
}