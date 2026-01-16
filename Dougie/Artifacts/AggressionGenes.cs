using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Reflection;
using Dougie.Midrow;

namespace Dougie.Artifacts;
public class AggressionGenes : Artifact, IDougieArtifact
{
  public int spawnCounter = 3;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("AggressionGenes", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.DougieDeck.Deck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Artifact/AggressionGenes.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "AggressionGenes", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "AggressionGenes", "description"]).Localize,
        });
    }
    public override int? GetDisplayNumber(State s)
     => spawnCounter;
    public override List<Tooltip>? GetExtraTooltips()
    {
      return [
        new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::Midrow::Cell")
        {
            Icon = ModEntry.Instance.CellColonyIcon.Sprite,
            TitleColor = Colors.midrow,
            Title = ModEntry.Instance.Localizations.Localize(["midrow", "Cell", "name"]),
            Description = string.Format("Will block 1 shot before being destroyed.")
        },
        new GlossaryTooltip($"action.{GetType().Namespace!}::MutateAllA")
		{
			Icon = ModEntry.Instance.AMutationIcon.Sprite,
			TitleColor = Colors.action,
			Title = ModEntry.Instance.Localizations.Localize(["action", "MutateAllA", "name"]),
			Description = ModEntry.Instance.Localizations.Localize(["action", "MutateAllA", "description"]),
		},
      ];
    }
    public override StuffBase ReplaceSpawnedThing(State state, Combat combat, StuffBase thing, bool spawnedByPlayer)
    {
        CellColony mutatedCellA = new CellColony{MutationA = true, targetPlayer = false};
        if(thing is CellColony)
        {
            spawnCounter--;
            if(spawnCounter <= 0)
            {
                Pulse();
                spawnCounter = 3;
                return mutatedCellA;
            }
            else
            {
                return thing;
            }
        }
        else
        {
            return thing;
        }
    }
    public override void OnCombatStart(State state, Combat combat)
    {
        spawnCounter = 3;
    }
}