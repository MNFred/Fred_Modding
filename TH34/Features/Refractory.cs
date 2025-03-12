using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using HarmonyLib;

namespace Fred.TH34.features;

internal sealed class ARefractoryManager : IStatusLogicHook
{
    public ARefractoryManager()
    {
        ModEntry.Instance.KokoroApi.RegisterStatusLogicHook(this, 2);
        ModEntry.Instance.Harmony.Patch(
			original: AccessTools.DeclaredMethod(typeof(AStatus), nameof(AStatus.Begin)),
			postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(AStatus_Begin_Postfix))
		);
        ModEntry.Instance.Harmony.Patch(
			original: AccessTools.DeclaredMethod(typeof(AStatus), nameof(AStatus.Begin)),
			prefix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(AStatus_Begin_Prefix))
		);
        ModEntry.Instance.Harmony.Patch(
			original: AccessTools.DeclaredMethod(typeof(Ship), nameof(Ship.ResetAfterCombat)),
			prefix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Ship_ResetAfterCombat_Prefix))
		);
    }
    private static void AStatus_Begin_Prefix(AStatus __instance, State s, out int __state)
    {
        __state = s.ship.Get(ModEntry.Instance.RefractoryStatus.Status);
    }
    private static void AStatus_Begin_Postfix(AStatus __instance, State s, int __state)
    {
        var diff = s.ship.Get(ModEntry.Instance.RefractoryStatus.Status) - __state;
        s.ship.heatTrigger += diff;
    }
    private static void Ship_ResetAfterCombat_Prefix(Ship __instance)
    {
        __instance.heatTrigger -= __instance.Get(ModEntry.Instance.RefractoryStatus.Status);
    }
}