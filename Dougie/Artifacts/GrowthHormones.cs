using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Reflection;
using Dougie.Midrow;

namespace Dougie.Artifacts;
public class GrowthHormones : Artifact, IDougieArtifact
{
  public bool firstSpawn = true;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("GrowthHormones", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.DougieDeck.Deck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Artifact/GrowthHormones.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "GrowthHormones", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "GrowthHormones", "description"]).Localize,
        });
    }
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
        new GlossaryTooltip($"action.{GetType().Namespace!}::MutateAllF")
		{
			Icon = ModEntry.Instance.FMutationIcon.Sprite,
			TitleColor = Colors.action,
			Title = ModEntry.Instance.Localizations.Localize(["action", "MutateAllF", "name"]),
			Description = ModEntry.Instance.Localizations.Localize(["action", "MutateAllF", "description"]),
		},
        new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::action::CellHarvest"){	Icon = ModEntry.Instance.CostUnsatisfiedIcon.Sprite,	TitleColor = Colors.action,	Title = "CELL HARVEST",	Description = "Choose <c=keyword>#</c> <c=midrow>cell colonies</c> at most 1 space offset from your ship to destroy. If there are not enough, this action does not happen."}
      ];
    }
    public override StuffBase ReplaceSpawnedThing(State state, Combat combat, StuffBase thing, bool spawnedByPlayer)
    {
        CellColony mutatedCellF = new CellColony{MutationF = true, targetPlayer = false};
        if(thing is CellColony)
        {
            if(firstSpawn)
            {
                firstSpawn = false;
                Pulse();
                return mutatedCellF;
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
        firstSpawn = true;
    }
}