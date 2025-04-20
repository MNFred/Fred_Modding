using Nickel;

namespace Fred.Jack;

public interface IJackApi
{
	IDeckEntry Jack_Deck { get; }
    IStatusEntry ScanBoost_Status { get; }
}