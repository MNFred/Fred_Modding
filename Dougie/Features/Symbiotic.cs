using System;
using System.Linq;
using System.Reflection;
using Dougie.Artifacts;
using Dougie.Midrow;
using HarmonyLib;
using Nickel;

namespace Dougie.features;

internal static class SymbioticExt
{
	public static bool IsSymbiotic(this Card self)
		=> ModEntry.Instance.Helper.ModData.GetModDataOrDefault<bool>(self, "Symbiotic");

	public static void SetSymbiotic(this Card self, bool value)
		=> ModEntry.Instance.Helper.ModData.SetModData(self, "Symbiotic", value);

}
public class SymbioticManager
{
    static ModEntry Instance => ModEntry.Instance;
    static IModHelper Helper => Instance.Helper;
    public SymbioticManager()
    {
        ModEntry.Instance.Harmony.Patch(
			  original: AccessTools.DeclaredMethod(typeof(Card), nameof(Card.GetDataWithOverrides)),
			  postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Card_GetDataWithOverrides_Postfix))
		  );
    }
    private static void Card_GetDataWithOverrides_Postfix(Card __instance, ref CardData __result, State state)
    {
        int rangeExtension = 0;
        Combat? currCombat = state.route as Combat;
        if(currCombat == null)
            return;
        if (state.EnumerateAllArtifacts().FirstOrDefault(a => a is ExtendoGrip) is { } artifact)
        {
            rangeExtension = 1;
        }
        else
        {
            rangeExtension = 0;
        }
        foreach(StuffBase stuffBase in currCombat.stuff.Values.ToList())
        {
            if(stuffBase is CellColony cellColony)
            {
                if(cellColony.x >= state.ship.x-1-rangeExtension && cellColony.x <= state.ship.x + state.ship.parts.Count + rangeExtension)
                {
                    return;
                }
            }
        }
        if(__instance.IsSymbiotic())
        {
            __result.cost += 1;
        }
    }
}