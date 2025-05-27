using Nickel;
using Nanoray.PluginManager;
using System.Reflection;

namespace Fred.AbandonedShipyard;
public class AutoAssembly : Artifact, IAbandonedArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("AutoAssembly", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.EventOnly],
                unremovable = true
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Foundry/AutoAssembly.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "AutoAssembly", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "AutoAssembly", "description"]).Localize,
        });
    }
    public override void OnCombatEnd(State state)
    {
        state.rewardsQueue.Queue(new ACardSelect{allowCancel = true, browseAction = new AssembleHull(), filterUnremovableAtShops = true, filterTemporary = false});
    }
}
public class AssembleHull : CardAction
{
    public override Route? BeginWithRoute(G g, State s, Combat c)
    {
        if(selectedCard != null)
        {
            s.ship.hullMax += 2;
            s.ship.Heal(selectedCard.GetCurrentCost(s)*3);
            s.deck.Remove(selectedCard);
            return null;
        }
        return null;
    }
    public override string? GetCardSelectText(State s)
	{
		return "Choose a card to destroy, gain <c=hull>2 max hull</c>. Then <c=heal>heal</c> for triple its cost.";
	}
}