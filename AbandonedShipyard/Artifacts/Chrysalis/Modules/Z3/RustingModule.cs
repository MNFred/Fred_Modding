using Nickel;
using Nanoray.PluginManager;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace Fred.AbandonedShipyard;
public class RustingModule : Artifact, IAbandonedArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("RustingModule", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.Unreleased],
                unremovable = true,
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Chrysalis/Modules/RustingModule.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "modules", "RustingModule", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "modules", "RustingModule", "description"]).Localize,
        });
    }
    public override void OnReceiveArtifact(State state)
    {
        var artifact = state.EnumerateAllArtifacts().OfType<ModuleStealer>().FirstOrDefault();
        if (artifact != null)
        {
            artifact.moduleTooltip.Add(new RustingModule().GetTooltips().First());
            artifact.TRustingModule = true;
            state.ship.hullMax += 3;
            state.ship.Heal(3);
            state.GetCurrentQueue().QueueImmediate(new ALoseArtifact { artifactType = new RustingModule().Key() });
        }
    }
}