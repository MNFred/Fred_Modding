using System;

namespace FredAndRadience.Radiant_Shipyard;

public interface IEssentialsApi
{
	Type? GetExeCardTypeForDeck(Deck deck);
	bool IsExeCardType(Type type);
}