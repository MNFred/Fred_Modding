using Nickel;

namespace Fred.Jack;

public sealed class ApiImplementation : IJackApi
{
	public IDeckEntry Jack_Deck
		=> ModEntry.Instance.Jack_Deck;
	public IStatusEntry ScanBoost_Status
		=> ModEntry.Instance.ScanBoostStatus;
}