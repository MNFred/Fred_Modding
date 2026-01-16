using Fred.Jack.Midrow;
using Nickel;

namespace Fred.Jack;

public interface IJackApi
{
	IDeckEntry Jack_Deck { get; }
    IStatusEntry ScanBoost_Status { get; }
    IStatusEntry LockOnStatus { get; }
    IStatusEntry ALockOnStatus { get; }
    IStatusEntry MidrowHaltStatus { get; }
    IStatusEntry LoseDroneshiftNextStatus { get; }
    StuffBase MiniMissile { get; }
    StuffBase APRocket { get; }
    StuffBase BalisticMissile { get; }
    StuffBase ClusterMissile { get; }
    StuffBase BlankMissile { get; }
}