using Nickel;
using System;
using System.Collections.Generic;

namespace Fred.TH34;

public sealed class ApiImplementation : ITH34Api
{
	public IDeckEntry TH34_Deck
		=> ModEntry.Instance.TH34_Deck;
	public IStatusEntry PlusChargeStatus
		=> ModEntry.Instance.PlusChargeStatus;
	public IStatusEntry MinusChargeStatus
		=> ModEntry.Instance.MinusChargeStatus;
	public IStatusEntry RefractoryStatus
		=> ModEntry.Instance.RefractoryStatus;
	public IStatusEntry OptimizeStatus
		=> ModEntry.Instance.OptimizeStatus;
	public IStatusEntry OptimizeBStatus
		=> ModEntry.Instance.OptimizeBStatus;
}