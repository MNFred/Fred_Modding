using Nickel;
using Nanoray.PluginManager;
using System.Reflection;

namespace Fred.AbandonedShipyard;
public class HeartOfFoundry : Artifact, IAbandonedArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("FoundryHeart", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.EventOnly],
                unremovable = true
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Foundry/FoundryHeart.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "FoundryHeart", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "FoundryHeart", "description"]).Localize,
        });
    }
    public override void OnTurnStart(State state, Combat combat)
    {
        if(combat.turn != 1 && combat.turn % 3 == 0)
        {
            Pulse();
            combat.QueueImmediate(new AHullMax{amount = -1, targetPlayer = true});
        }
    }
}