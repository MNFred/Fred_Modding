using System;
using System.Linq;
using Fred.Jack.Artifacts;

namespace Fred.Jack.features
{
  internal sealed class LockOnManager : IStatusLogicHook
  {
    public LockOnManager()
    {
      ModEntry.Instance.KokoroApi.RegisterStatusLogicHook(this, 1);
      ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook("ModifyBaseMissileDamage", (Func<State, Combat, bool, int>)((state, combat, targetPlayer) => 
      {
        if(combat.otherShip.Get(ModEntry.Instance.LockOnStatus.Status) > 0)
        {
          if(!targetPlayer)
            return combat.otherShip.Get(ModEntry.Instance.LockOnStatus.Status);
          else return 0;
        }
        if(state.ship.Get(ModEntry.Instance.LockOnStatus.Status)>0)
        {
          if(targetPlayer)
            return state.ship.Get(ModEntry.Instance.LockOnStatus.Status);
          else return 0;
        }else return 0;
      }),0);
      ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook("ModifyBaseDamage", (int baseDamage, Card? card, State state, Combat? combat, bool fromPlayer) =>
      {
        if (state.EnumerateAllArtifacts().FirstOrDefault(a => a is SecondaryReticle) is { } artifact)
        {
          if(fromPlayer)
          {
            return combat!.otherShip.Get(ModEntry.Instance.LockOnStatus.Status)/2;
          }else return 0;
        }else return 0;
      },0);
    }

    public bool HandleStatusTurnAutoStep(State state, Combat combat, StatusTurnTriggerTiming timing, Ship ship, Status status, ref int amount, ref StatusTurnAutoStepSetStrategy setStrategy)
    {
      if(status != ModEntry.Instance.LockOnStatus.Status)
        return false;
      if(timing != StatusTurnTriggerTiming.TurnEnd)
        return false;
      if(state.ship.Get(Status.timeStop) > 0)
        return false;
      if (state.EnumerateAllArtifacts().FirstOrDefault(a => a is CrystalReticle) is { } artifact)
      {
        amount = 0;
      }else{
        --amount;
      }
      return false;
    }
  }
}
