using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Fred.Jack.Artifacts;
public class PreloadedBays : Artifact, IJackArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("PreloadedBays", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.Jack_Deck.Deck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/nowastesprite.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "PreloadedBays", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "PreloadedBays", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
      return [
        new TTGlossary("action.spawn")
      ];
    }
    public override void OnCombatStart(State state, Combat combat)
    {
      foreach(Card card in state.deck)
      {
        if(card.GetActions(state, combat).Any(a => a is ASpawn))
        {
          card.discount = -1;
        }
      }
      foreach(Card card in combat.hand)
      {
        if(card.GetActions(state, combat).Any(a => a is ASpawn))
        {
          card.discount = -1;
        }
      }
    }
}