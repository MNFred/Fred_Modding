using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;

namespace Fred.Jack.features;
internal sealed class MidrowHaltManager : IStatusLogicHook
{
  public MidrowHaltManager()
  {
    ModEntry.Instance.KokoroApi.RegisterStatusLogicHook(this, 4);
    ModEntry.Instance.Harmony.Patch(
		  original: AccessTools.DeclaredMethod(typeof(Combat), nameof(Combat.DrainCardActions)),
		  prefix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Combat_DrainCardActions_Prefix))
	  );
  }
  private static void Combat_DrainCardActions_Prefix(G g, Combat __instance)
    {
      State state = g.state;
      if(__instance.cardActions.Any(a => a is ADroneTurn) && !__instance.isPlayerTurn)
      {
        if(state.ship.Get(ModEntry.Instance.MidrowHaltStatus.Status) > 0)
        {
          __instance.cardActions.Remove(__instance.cardActions.Find(a => a is ADroneTurn)!);
          __instance.Queue(new AEnemyTurn());
          return;
        }
      }
      if(state.EnumerateAllArtifacts().FirstOrDefault(a => a is RadioRepeater) is { } artifact)
      {
        if(__instance.cardActions.Any(a => a is AStartPlayerTurn) && !__instance.isPlayerTurn)
        {
          if(state.ship.Get(ModEntry.Instance.MidrowHaltStatus.Status) > 0)
          {
            __instance.cardActions.Remove(__instance.cardActions.Find(a => a is AStartPlayerTurn)!);
            return;
          }
        }
      }
    }
  public bool HandleStatusTurnAutoStep(State state,Combat combat,StatusTurnTriggerTiming timing,Ship ship,Status status,ref int amount,ref StatusTurnAutoStepSetStrategy setStrategy)
  {
    if (status != ModEntry.Instance.MidrowHaltStatus.Status || timing != StatusTurnTriggerTiming.TurnStart || amount <= 0)
        return false;
    amount--;
    return false;
  }
}