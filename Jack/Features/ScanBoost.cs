
namespace Fred.Jack.features;
internal sealed class ScanBoostManager : IStatusLogicHook
{
  public ScanBoostManager()
  {
    ModEntry.Instance.KokoroApi.RegisterStatusLogicHook(this, 3);
  }
  void IStatusLogicHook.OnStatusTurnTrigger(State state,Combat combat,StatusTurnTriggerTiming timing,Ship ship,Status status,int oldAmount,int newAmount)
  {
    if (status != ModEntry.Instance.ScanBoostStatus.Status || timing != StatusTurnTriggerTiming.TurnStart || oldAmount <= 0)
      return;
    combat.QueueImmediate(new AStatus{targetPlayer = true,status = Status.droneShift,statusAmount = oldAmount});
  }
}

