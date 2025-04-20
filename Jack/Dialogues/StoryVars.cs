using Microsoft.Extensions.Logging;

namespace Fred.Jack.DialogueAdditions;

public static class StoryVarsAdditions
{
	internal static ModEntry Instance => ModEntry.Instance;

	private static IKokoroApi KokoroApi => Instance.KokoroApi;

	internal static void DrawLoadingScreen_Prefix(MG __instance, ref int __state)
		=> __state = __instance.loadingQueue?.Count ?? 0;

	internal static void DrawLoadingScreen_Postfix(MG __instance, ref int __state)
	{
		if (__state <= 0)
			return;
		if ((__instance.loadingQueue?.Count ?? 0) > 0)
			return;
      CombatDialogue.Inject();
      EventDialogue.Inject();
	}
}