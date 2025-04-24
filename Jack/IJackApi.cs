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
    MiniMissile MiniMissile { get; }
    APRocket APRocket { get; }
    BalisticDormant balisticMissile { get; }
    ClusterMissile clusterMissile { get; }
    BlankMissile blankMissile { get; }
}