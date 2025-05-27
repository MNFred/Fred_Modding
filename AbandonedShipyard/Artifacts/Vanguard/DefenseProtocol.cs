using Nickel;
using Nanoray.PluginManager;
using System.Reflection;
using System.Collections.Generic;

namespace Fred.AbandonedShipyard;

public class DefenseProtocol : Artifact, IAbandonedArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("DefenseProtocol", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.Boss],
                unremovable = true
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Vanguard/DefenseProtocol.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "DefenseProtocol", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "DefenseProtocol", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [new TTGlossary("parttrait.armor"), .. StatusMeta.GetTooltips(Status.payback, 1)];
    }
    public override void OnReceiveArtifact(State state)
    {
        foreach (Part part in state.ship.parts)
        {
            if (part.type == PType.wing)
            {
                part.damageModifier = PDamMod.armor;
            }
        }
    }
    public override void OnCombatStart(State state, Combat combat)
    {
        combat.QueueImmediate(new AStatus { status = Status.payback, statusAmount = 1, targetPlayer = true });
    }
}