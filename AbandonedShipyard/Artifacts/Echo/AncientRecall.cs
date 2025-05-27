using Nickel;
using Nanoray.PluginManager;
using System.Reflection;
using System.Linq;

namespace Fred.AbandonedShipyard;

public class AncientRecal : Artifact, IAbandonedArtifact
{
    public bool active = false;
    static readonly Spr artifactSpriteOn = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Echo/AncientRecall.png")).Sprite;
    static readonly Spr artifactSpriteOff = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Echo/AncientRecallOff.png")).Sprite;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("AncientRecal", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.Common],
            },
            Sprite = artifactSpriteOn,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "AncientRecall", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "AncientRecall", "description"]).Localize,
        });
    }
    public override Spr GetSprite()
    {
        switch (active)
        {
            case true:
                return artifactSpriteOff;
            case false:
                return artifactSpriteOn;
        }
    }
    public override void OnPlayerTakeNormalDamage(State state, Combat combat, int rawAmount, Part? part)
    {
        if (!active && part != null && part.type == PType.comms)
        {
            foreach (Card card in combat.exhausted.Shuffle(state.rngActions))
            {
                state.RemoveCardFromWhereverItIs(card.uuid);
                combat.SendCardToHand(state, card);
                Pulse();
                active = true;
                return;
            }
        }
    }
    public override void OnCombatEnd(State state)
    {
        active = false;
    }
    public override void OnCombatStart(State state, Combat combat)
    {
        active = false;
    }
}