using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace Fred.AbandonedShipyard.Patches
{
    [HarmonyPatch]
    public static class PatchRefillNukesAtShop
    {
        [HarmonyPatch(typeof(Events), nameof(Events.NewShop)), HarmonyPostfix]
        public static void AddRepairChoice(State s, ref List<Choice> __result)
        {
            if (s.EnumerateAllArtifacts().Any((a) => a is HeartOfFoundry g))
            {
                __result.Insert(0,new Choice() {
                label = ModEntry.Instance.Localizations.Localize(["choice", "Shop", "RestoreHull"]),
                key = ".shopRestoreHull",
                actions = new List<CardAction>()
                {
                    new AHullMax{amount = 3, targetPlayer = true}
                }
                });
                return;
            }
        }
    }
}