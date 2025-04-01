using Nickel;
using Nanoray.PluginManager;
using System.Reflection;
using System.Linq;

namespace Fred.AbandonedShipyard;
public class EmptyModule : Artifact, IAbandonedArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("EmptyModule", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.Unreleased],
                unremovable = true,
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Chrysalis/Modules/EmptyModule.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "modules", "EmptyModule", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "modules", "EmptyModule", "description"]).Localize,
        });
    }
}