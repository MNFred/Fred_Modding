using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using HarmonyLib;

namespace FredAndRadience.Radiant_Shipyard.features;

internal sealed class ElecChargeManager : IStatusLogicHook
{
    public ElecChargeManager()
    {
        ModEntry.Instance.KokoroApi.RegisterStatusLogicHook(this, 2);
    }
}