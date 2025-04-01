using System;

namespace Fred.AbandonedShipyard;

public interface IEssentialsApi
{
	Type? GetExeCardTypeForDeck(Deck deck);
	bool IsExeCardType(Type type);
}