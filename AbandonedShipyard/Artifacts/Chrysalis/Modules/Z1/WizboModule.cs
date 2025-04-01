using Nickel;
using Nanoray.PluginManager;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace Fred.AbandonedShipyard;
public class WizboModule : Artifact, IAbandonedArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("WizboModule", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.Unreleased],
                unremovable = true,
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Chrysalis/Modules/WizboModule.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "modules", "WizboModule", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "modules", "WizboModule", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [..StatusMeta.GetTooltips(Status.heat,3)];
    }
    public override void OnReceiveArtifact(State state)
    {
        var artifact = state.EnumerateAllArtifacts().OfType<ModuleStealer>().FirstOrDefault();
        if (artifact != null)
        {
            artifact.moduleTooltip.Add(new WizboModule().GetTooltips().First());
            artifact.TWizboModule = true;
            state.ship.heatTrigger += 1;
            state.GetCurrentQueue().QueueImmediate(new ALoseArtifact { artifactType = new WizboModule().Key() });
        }
    }
}