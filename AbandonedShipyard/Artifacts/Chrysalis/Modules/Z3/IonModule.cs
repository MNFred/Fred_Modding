using Nickel;
using Nanoray.PluginManager;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace Fred.AbandonedShipyard;
public class IonRangerModule : Artifact, IAbandonedArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("IonRangerModule", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.Unreleased],
                unremovable = true,
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Chrysalis/Modules/IonModule.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "modules", "IonRangerModule", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "modules", "IonRangerModule", "description"]).Localize,
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
            artifact.moduleTooltip.Add(new IonRangerModule().GetTooltips().First());
            artifact.moduleTooltip.Remove(new FireballModule().GetTooltips().First());
            artifact.TIonRangerModule = true;
            artifact.TFireballModule = false;
            state.GetCurrentQueue().QueueImmediate(new ALoseArtifact { artifactType = new IonRangerModule().Key() });
        }
    }
}