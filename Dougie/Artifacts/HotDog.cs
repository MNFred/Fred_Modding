using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Reflection;
using Dougie.cards;

namespace Dougie.Artifacts;
public class HotDog : Artifact, IDougieArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("HotDog", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.DougieDeck.Deck,
                pools = [ArtifactPool.Boss],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Artifact/HotDog.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "HotDog", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "HotDog", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
      return [
        new TTCard{card = new MeatGrinder()}
      ];
    }
    public override void OnCombatStart(State state, Combat combat)
    {
        Pulse();
        combat.QueueImmediate(new AAddCard{card = new MeatGrinder(), amount = 1, destination = CardDestination.Hand});
    }
}