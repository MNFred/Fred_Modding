using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nanoray.Shrike;
using Nanoray.Shrike.Harmony;
using Nickel;

namespace Fred.TH34.Artifacts;
public class ArtifactBitFlip : Artifact, ITH34Artifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        if (ModEntry.Instance.DuoArtifactsApi is not { } api)
			return;
        helper.Content.Artifacts.RegisterArtifact("BitFlip", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/BitFlip.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "BitFlip", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "BitFlip", "description"]).Localize,
        });
        api.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.TH34_Deck.Deck, Deck.colorless]);
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [..StatusMeta.GetTooltips(ModEntry.Instance.PlusChargeStatus.Status,1), ..StatusMeta.GetTooltips(ModEntry.Instance.MinusChargeStatus.Status,1)];
    }
    public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount)
    {
        int plusAmount = state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status);
        int minusAmount = state.ship.Get(ModEntry.Instance.MinusChargeStatus.Status);
        Pulse();
        if (card.Name().Contains("basic", StringComparison.OrdinalIgnoreCase))
                {
                    combat.Queue(new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, mode = AStatusMode.Set, statusAmount = plusAmount, targetPlayer = true, timer = 0});
                    combat.Queue(new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, mode = AStatusMode.Set, statusAmount = minusAmount, targetPlayer = true, timer = 0});
                }
            if (card.GetFullDisplayName().Contains("basic", StringComparison.OrdinalIgnoreCase))
            {
                combat.Queue(new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, mode = AStatusMode.Set, statusAmount = plusAmount, targetPlayer = true, timer = 0});
                combat.Queue(new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, mode = AStatusMode.Set, statusAmount = minusAmount, targetPlayer = true, timer = 0});
            }
    }
}