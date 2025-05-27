using Nickel;
using Nanoray.PluginManager;
using System.Reflection;
using System.Linq;

namespace Fred.AbandonedShipyard;
public class CallOfTheVoid : Artifact, IAbandonedArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("CallOfTheVoid", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.Boss],
                unremovable = true
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Echo/CallOfTheVoid.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "CallOfTheVoid", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "CallOfTheVoid", "description"]).Localize,
        });
    }
    public override void OnReceiveArtifact(State state)
    {
        state.GetCurrentQueue().QueueImmediate(new ALoseArtifact { artifactType = new EchoChamber().Key() });
    }
    public override void OnTurnStart(State state, Combat combat)
    {
        if (combat.turn == 1)
        {
            state.ship.baseDraw -= 1;
        }
        if (combat.discard.Count > 0)
        {
            var cardReturn = combat.discard.Last();
            if (cardReturn.GetCurrentCost(state) >= 3)
                cardReturn.discount = -1;
            state.RemoveCardFromWhereverItIs(cardReturn.uuid);
            combat.SendCardToHand(state, cardReturn);
        }
        if (combat.discard.Count == 0)
        {
            combat.QueueImmediate(new ADrawCard { count = 1, timer = 0 });
        }
    }
    public override void OnCombatEnd(State state)
    {
        state.ship.baseDraw += 1;
    }
}