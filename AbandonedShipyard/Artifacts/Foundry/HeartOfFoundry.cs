using Nickel;
using Nanoray.PluginManager;
using System.Reflection;

namespace Fred.AbandonedShipyard;
public class HeartOfFoundry : Artifact, IAbandonedArtifact
{
    static readonly Spr heartstage1 = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Foundry/FoundryHeart_1.png")).Sprite;
    static readonly Spr heartstage2 = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Foundry/FoundryHeart_2.png")).Sprite;
    static readonly Spr heartstage3 = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Foundry/FoundryHeart_3.png")).Sprite;
    static readonly Spr heartstage4 = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Foundry/FoundryHeart_4.png")).Sprite;
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
            Sprite = heartstage4,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "FoundryHeart", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "FoundryHeart", "description"]).Localize,
        });
    }
    public override Spr GetSprite()
    {
        if (MG.inst.g?.state is not { } state)
			return heartstage4;
		if (state.ship.hull == 1)
			return heartstage4;
		return (1.0 * state.ship.hull / state.ship.hullMax) switch
		{
			<= 0.25 => heartstage4,
			<= 0.5 => heartstage3,
			<= 0.75 => heartstage2,
			_ => heartstage1
		};
    }
    public override void OnTurnStart(State state, Combat combat)
    {
        if (combat.turn != 1 && combat.turn % 3 == 0)
        {
            Pulse();
            combat.QueueImmediate(new AHullMax { amount = -1, targetPlayer = true });
        }
    }
}