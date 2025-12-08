using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Reflection;
using Fred.Andromeda.cards;

namespace Fred.Andromeda.Artifacts;
public class ReversedPolarity : Artifact, IAndromedaArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("ReversedPolarity", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.AndromedaDeck.Deck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Artifact/ReversePolarity.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ReversedPolarity", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ReversedPolarity", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
      return [
        new TTCard{card = new Polarity()}
      ];
    }
    public override void OnCombatStart(State state, Combat combat)
    {
        combat.QueueImmediate(new AAddCard{card = new Polarity(), amount = 1, destination = CardDestination.Hand});
    }
}