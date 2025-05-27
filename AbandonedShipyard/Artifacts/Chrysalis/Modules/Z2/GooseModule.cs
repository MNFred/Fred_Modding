using Nickel;
using Nanoray.PluginManager;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace Fred.AbandonedShipyard;

public class GooseModule : Artifact, IAbandonedArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("GooseModule", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.Unreleased],
                unremovable = true,
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Chrysalis/Modules/AnnoyingModule.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "modules", "GooseModule", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "modules", "GooseModule", "description"]).Localize,
        });
    }
    public override void OnReceiveArtifact(State state)
    {
        var artifact = state.EnumerateAllArtifacts().OfType<ModuleStealer>().FirstOrDefault();
        if (artifact != null)
        {
            artifact.moduleTooltip.Add(new GooseModule().GetTooltips().First());
            artifact.TGooseModule = true;
            state.GetCurrentQueue().QueueImmediate(new ACardSelect{browseAction = new GooseIncrease()});
            state.GetCurrentQueue().QueueImmediate(new ALoseArtifact { artifactType = new GooseModule().Key() });
        }
    }
}
public class GooseIncrease : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        if (selectedCard != null)
        {
            s.deck.Remove(selectedCard);
            s.deck.Add(new TrashAnnoyance { upgrade = Upgrade.A });
        }
    }
    public override string? GetCardSelectText(State s)
    {
        return "Select a card, it turns into an <c=card>Annoyance A</c>. <c=artifact>:3</c>";
    }
}