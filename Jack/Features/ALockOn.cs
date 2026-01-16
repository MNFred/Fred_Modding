using System;

namespace Fred.Jack.features
{
  internal sealed class ALockOnManager : IStatusLogicHook
  {
    public ALockOnManager()
    {
      ModEntry.Instance.KokoroApi.RegisterStatusLogicHook(this, 0);
      ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook("ModifyBaseMissileDamage", (Func<State, Combat, bool, int>)((state, combat, targetPlayer) => 
      {
        if(combat.otherShip.Get(ModEntry.Instance.ALockOnStatus.Status) > 0)
        {
          if(!targetPlayer)
            return combat.otherShip.Get(ModEntry.Instance.ALockOnStatus.Status);
          else return 0;
        }
        if(state.ship.Get(ModEntry.Instance.ALockOnStatus.Status)>0)
        {
          if(targetPlayer)
            return state.ship.Get(ModEntry.Instance.ALockOnStatus.Status);
          else return 0;
        } else return 0;
      }),0);
    }
    public bool HandleStatusTurnAutoStep(State state, Combat combat, StatusTurnTriggerTiming timing, Ship ship, Status status, ref int amount, ref StatusTurnAutoStepSetStrategy setStrategy)
    {
      if(status != ModEntry.Instance.ALockOnStatus.Status)
        return false;
      if(timing != StatusTurnTriggerTiming.TurnEnd)
        return false;
      if(state.ship.Get(Status.timeStop) > 0)
        return false;
      if(amount>0){
        amount--;
      }
      return false;
    }
  }
}
