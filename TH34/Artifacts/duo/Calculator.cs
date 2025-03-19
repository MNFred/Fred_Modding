using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Xna.Framework.Graphics;
using HarmonyLib;

namespace Fred.TH34.Artifacts;
public class ArtifactCalculator : Artifact, ITH34Artifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        if (ModEntry.Instance.DuoArtifactsApi is not { } api)
			return;
        helper.Content.Artifacts.RegisterArtifact("Calculator", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/Calculator.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Calculator", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Calculator", "description"]).Localize,
        });
        api.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.TH34_Deck.Deck, Deck.riggs]);
        ModEntry.Instance.Harmony.Patch(
			original: AccessTools.DeclaredMethod(typeof(Card), nameof(Card.GetActionsOverridden)),
			postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Card_GetActionsOverridden_Postfix))
		);
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return StatusMeta.GetTooltips(ModEntry.Instance.MinusChargeStatus.Status,1);
    }
    private static void Card_GetActionsOverridden_Postfix(Card __instance, State s, ref List<CardAction> __result)
	{
		if (s.ship.Get(ModEntry.Instance.MinusChargeStatus.Status)<=0)
			return;

		if (s.EnumerateAllArtifacts().FirstOrDefault(a => a is ArtifactCalculator) is not { } artifact)
			return;

		foreach (var baseAction in __result)
		{
			if (baseAction is ADrawCard draw)
			{
                if(__instance.GetMeta().deck == Deck.riggs)
				    draw.count += 1;
			}
		}
	}
}