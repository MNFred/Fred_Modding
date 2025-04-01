using Nickel;
using Nanoray.PluginManager;
using System.Reflection;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System;
using Nanoray.Shrike;
using Nanoray.Shrike.Harmony;
using System.Reflection.Emit;
using Microsoft.Extensions.Logging;

namespace Fred.AbandonedShipyard;
public class AquaticModule : Artifact, IAbandonedArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("AquaticModule", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.Unreleased],
                unremovable = true,
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Chrysalis/Modules/FishModule.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "modules", "AquaticModule", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "modules", "AquaticModule", "description"]).Localize,
        });
    }
    public override void OnReceiveArtifact(State state)
    {
        var artifact = state.EnumerateAllArtifacts().OfType<ModuleStealer>().FirstOrDefault();
        if (artifact != null)
        {
            artifact.moduleTooltip.Add(new AquaticModule().GetTooltips().First());
            artifact.TFishModule = true;
            int num1;
            int num2 = num1 = (int) Math.Ceiling((double) state.ship.parts.Count / 2.0);
            Part part = new Part()
            {
                type = PType.empty,
                skin = "scaffolding"
            };
            state.ship.parts.Insert(num2, Mutil.DeepCopy<Part>(part));
            state.GetCurrentQueue().QueueImmediate(new ALoseArtifact { artifactType = new AquaticModule().Key() });
        }
    }
}