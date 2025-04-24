using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Reflection;
using Fred.Jack.cards;

namespace Fred.Jack.Artifacts;
public class TripleTag : Artifact, IJackArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("TripleTag", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.Jack_Deck.Deck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/DeadlyPrec.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "TripleTag", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "TripleTag", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [
          new TTCard{
            card = new PrecisionCard()
          }];
    }
    public override void OnCombatStart(State state, Combat combat)
    {
      combat.QueueImmediate(new AAddCard{card = new PrecisionCard(), amount = 1, destination = CardDestination.Deck, timer = 0.1});
      combat.QueueImmediate(new AAddCard{card = new PrecisionCard(), amount = 1, destination = CardDestination.Hand, timer = 0.1});
      combat.QueueImmediate(new AAddCard{card = new PrecisionCard(), amount = 1, destination = CardDestination.Discard, timer = 0.1});
    }
}