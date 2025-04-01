using Nickel;
using Nanoray.PluginManager;
using System.Reflection;
using System.Collections.Generic;
using Fred.AbandonedShipyard.cards;
using System.Linq;

namespace Fred.AbandonedShipyard;
public class MonarchModule : Artifact, IAbandonedArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("MonarchModule", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.Unreleased],
                unremovable = true,
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Chrysalis/Modules/MonarchModule.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "modules", "MonarchModule", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "modules", "MonarchModule", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [new TTCard{card = new BasicDroneCard()}];
    }
    public override void OnReceiveArtifact(State state)
    {
        var artifact = state.EnumerateAllArtifacts().OfType<ModuleStealer>().FirstOrDefault();
        if (artifact != null)
        {
            artifact.moduleTooltip.Add(new MonarchModule().GetTooltips().First());
            artifact.TMonarchModule = true;
            state.deck.Add(new BasicDroneCard());
            state.GetCurrentQueue().QueueImmediate(new ALoseArtifact { artifactType = new MonarchModule().Key() });
        }
    }
}