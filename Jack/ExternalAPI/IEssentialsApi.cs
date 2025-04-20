
using System;

namespace Fred.Jack;
public interface IEssentialsApi
{
  Type? GetExeCardTypeForDeck(Deck deck);
  bool IsExeCardType(Type type);
}

