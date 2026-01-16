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
	public StuffBase MiniMissile
		=> new MiniMissile();
	public StuffBase APRocket
		=> new APRocket();
	public StuffBase BalisticMissile
		=> new BalisticDormant();
	public StuffBase ClusterMissile
		=> new ClusterMissile();
	public StuffBase BlankMissile
		=> new BlankMissile();
}