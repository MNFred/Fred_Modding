using Nickel;
using Nanoray.PluginManager;
using System.Reflection;
using Fred.AbandonedShipyard.cards;
using System.Collections.Generic;
using System.Linq;

namespace Fred.AbandonedShipyard;
public class StingerModule : Artifact, IAbandonedArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("StingerModule", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.Unreleased],
                unremovable = true,
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Chrysalis/Modules/StingerModule.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "modules", "StingerModule", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "modules", "StingerModule", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [new TTCard{card = new BasicMissileCard()}];
    }
    public override void OnReceiveArtifact(State state)
    {
        var artifact = state.EnumerateAllArtifacts().OfType<ModuleStealer>().FirstOrDefault();
        if (artifact != null)
        {
            artifact.moduleTooltip.Add(new StingerModule().GetTooltips().First());
            artifact.TStingerModule = true;
            state.deck.Add(new BasicMissileCard());
            state.GetCurrentQueue().QueueImmediate(new ALoseArtifact { artifactType = new StingerModule().Key() });
        }
    }
}