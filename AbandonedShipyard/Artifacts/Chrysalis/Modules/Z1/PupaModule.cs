using Nickel;
using Nanoray.PluginManager;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Fred.AbandonedShipyard;
public class PupaModule : Artifact, IAbandonedArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("PupaModule", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.Unreleased],
                unremovable = true,
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Chrysalis/Modules/PupaModule.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "modules", "PupaModule", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "modules", "PupaModule", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [..StatusMeta.GetTooltips(Status.tempShield,1)];
    }
    public override void OnReceiveArtifact(State state)
    {
        var artifact = state.EnumerateAllArtifacts().OfType<ModuleStealer>().FirstOrDefault();
        if (artifact != null)
        {
            artifact.moduleTooltip.Add(new PupaModule().GetTooltips().First());
            artifact.TPupaModule = true;
            state.GetCurrentQueue().QueueImmediate(new ALoseArtifact { artifactType = new PupaModule().Key() });
        }
    }
}