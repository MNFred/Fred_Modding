using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Xna.Framework.Graphics;
using HarmonyLib;

namespace Fred.TH34.Artifacts;
public class ArtifactGolemancy : Artifact, ITH34Artifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        if (ModEntry.Instance.DuoArtifactsApi is not { } api)
			return;
        helper.Content.Artifacts.RegisterArtifact("Golemancy", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/Golemancy.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Golemancy", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Golemancy", "description"]).Localize,
        });
        api.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.TH34_Deck.Deck, Deck.shard]);
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [..StatusMeta.GetTooltips(ModEntry.Instance.MinusChargeStatus.Status,1),
        ..StatusMeta.GetTooltips(ModEntry.Instance.PlusChargeStatus.Status,1),
        ..StatusMeta.GetTooltips(Status.shard,3),
        ..StatusMeta.GetTooltips(Status.heat,3)];
    }
}