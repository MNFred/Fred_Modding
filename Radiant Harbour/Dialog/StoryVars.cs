using Microsoft.Extensions.Logging;

namespace FredAndRadience.Radiant_Shipyard.DialogueAdditions;

public static class StoryVarsAdditions
{
	internal static void DrawLoadingScreen_Prefix(MG __instance, ref int __state)
		=> __state = __instance.loadingQueue?.Count ?? 0;

	internal static void DrawLoadingScreen_Postfix(MG __instance, ref int __state)
	{
		if (__state <= 0)
			return;
		if ((__instance.loadingQueue?.Count ?? 0) > 0)
			return;
		UranusCombat.Inject();
		UranusEvent.Inject();
		CerberusCombat.Inject();
		VenusCombat.Inject();
		MercuryCombat.Inject();
		HadesCombat.Inject();
		ChangelingCombat.Inject();
	}
}