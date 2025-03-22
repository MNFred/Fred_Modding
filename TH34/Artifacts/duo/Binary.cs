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
public class ArtifactBinary : Artifact, ITH34Artifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        if (ModEntry.Instance.DuoArtifactsApi is not { } api)
			return;
        if (ModEntry.Instance.JohnsonApi is not { } JohnApi)
            return;
        helper.Content.Artifacts.RegisterArtifact("Binary", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/Binary.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Binary", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Binary", "description"]).Localize,
        });
        api.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.TH34_Deck.Deck, JohnApi.JohnsonDeck.Deck]);
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [..StatusMeta.GetTooltips(ModEntry.Instance.PlusChargeStatus.Status,1), ..StatusMeta.GetTooltips(ModEntry.Instance.MinusChargeStatus.Status,1)];
    }
    public override int ModifyBaseDamage(int baseDamage, Card? card, State state, Combat? combat, bool fromPlayer)
    {
        if(card == null)
            return 0;
        if(state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)>0)
        {
            if(card.upgrade == Upgrade.B)
            {
                return 1;
            }
        }
        if(state.ship.Get(ModEntry.Instance.MinusChargeStatus.Status)>0)
        {
            if(card.upgrade == Upgrade.A)
            {
                return 1;
            }
        }
        return 0;
    }
}