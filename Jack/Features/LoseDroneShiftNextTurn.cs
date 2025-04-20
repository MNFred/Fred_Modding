
namespace Fred.Jack.features;
internal sealed class LoseDroneShiftManager : IStatusLogicHook
{
  public LoseDroneShiftManager()
  {
    ModEntry.Instance.KokoroApi.RegisterStatusLogicHook(this, 2);
  }
  void IStatusLogicHook.OnStatusTurnTrigger(State state,Combat combat,StatusTurnTriggerTiming timing,Ship ship,Status status,int oldAmount,int newAmount)
  {
    if (status != ModEntry.Instance.LoseDroneshiftNextStatus.Status || timing != StatusTurnTriggerTiming.TurnStart || oldAmount <= 0)
      return;
    combat.QueueImmediate(new AStatus{targetPlayer = true,status = Status.droneShift,statusAmount = 0,mode = AStatusMode.Set});
  }
  public bool HandleStatusTurnAutoStep(State state,Combat combat,StatusTurnTriggerTiming timing,Ship ship,Status status,ref int amount,ref StatusTurnAutoStepSetStrategy setStrategy)
  {
    if (status != ModEntry.Instance.LoseDroneshiftNextStatus.Status || timing != StatusTurnTriggerTiming.TurnStart || amount <= 0)
      return false;
    amount = 0;
    return false;
  }
}
