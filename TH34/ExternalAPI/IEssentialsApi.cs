using System;

namespace Fred.TH34;

public interface IEssentialsApi
{
	Type? GetExeCardTypeForDeck(Deck deck);
	bool IsExeCardType(Type type);
}