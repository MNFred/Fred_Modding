using Fred.Jack.Midrow;
using Nickel;

namespace Fred.Jack;

public sealed class ApiImplementation : IJackApi
{
	public IDeckEntry Jack_Deck
		=> ModEntry.Instance.Jack_Deck;
	public IStatusEntry ScanBoost_Status
		=> ModEntry.Instance.ScanBoostStatus;
	public IStatusEntry LockOnStatus
		=> ModEntry.Instance.LockOnStatus;
	public IStatusEntry ALockOnStatus
		=> ModEntry.Instance.ALockOnStatus;
	public IStatusEntry MidrowHaltStatus
		=> ModEntry.Instance.MidrowHaltStatus;
	public IStatusEntry LoseDroneshiftNextStatus
		=> ModEntry.Instance.LoseDroneshiftNextStatus;
	public MiniMissile MiniMissile
		=> new MiniMissile();
	public APRocket APRocket
		=> new APRocket();
	public BalisticDormant balisticMissile
		=> new BalisticDormant();
	public ClusterMissile clusterMissile
		=> new ClusterMissile();
	public BlankMissile blankMissile
		=> new BlankMissile();
}