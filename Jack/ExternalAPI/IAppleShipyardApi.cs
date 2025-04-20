
using System;

namespace Fred.Jack;
public interface IAppleShipyardApi
{
  void RegisterActionLooksForPartType(Type actionType, PType partType);
}

