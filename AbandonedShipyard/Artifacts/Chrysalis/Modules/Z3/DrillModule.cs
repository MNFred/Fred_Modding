using Nickel;
using Nanoray.PluginManager;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace Fred.AbandonedShipyard;
public class DrillModule : Artifact, IAbandonedArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("DrillModule", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.Unreleased],
                unremovable = true,
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Chrysalis/Modules/DrillModule.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "modules", "DrillModule", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "modules", "DrillModule", "description"]).Localize,
        });
    }
    public override void OnReceiveArtifact(State state)
    {
        var artifact = state.EnumerateAllArtifacts().OfType<ModuleStealer>().FirstOrDefault();
        if (artifact != null)
        {
            artifact.moduleTooltip.Add(new DrillModule().GetTooltips().First());
            artifact.TDrillModule = true;
            state.GetCurrentQueue().QueueImmediate(new ALoseArtifact { artifactType = new DrillModule().Key() });
        }
    }
}