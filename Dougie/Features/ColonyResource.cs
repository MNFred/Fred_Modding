using System;
using System.Collections.Generic;
using System.Linq;
using Dougie.Artifacts;
using Dougie.Midrow;
using Nickel;
using static Dougie.Actions.CellHarvest;
using static Dougie.IKokoroApi.IV2.IActionCostsApi;
using static Dougie.IKokoroApi.IV2.IActionCostsApi.IResourceProvider;

namespace Dougie.features;

public class CellResource : IResource
{
    public static bool IsMarkedForDeath(StuffBase thing)
    {
        return ModEntry.Instance.Helper.ModData.TryGetModData<bool>(thing, "MarkedForDeath", out var data) && data;
    }
    public string ResourceKey => "CellResource";
    public int GetCurrentResourceAmount(State state, Combat combat)
    {
        int rangeExtension = 0;
        int amountOfCellsNearby = 0;
        if (state.EnumerateAllArtifacts().FirstOrDefault(a => a is ExtendoGrip) is { } artifact)
        {
            rangeExtension = 1;
        }
        else
        {
            rangeExtension = 0;
        }
        foreach(StuffBase stuffBase in combat.stuff.Values)
        {
            if(stuffBase is CellColony cellColony)
            {
                if(cellColony.x >= state.ship.x - 1 - rangeExtension && cellColony.x <= state.ship.x + state.ship.parts.Count + rangeExtension)
                {
                    amountOfCellsNearby += 1;
                }
            }
        }
        return amountOfCellsNearby;
    }
    public void Pay(State state, Combat combat, int amount)
    {
        return;
    }
    public IReadOnlyList<Tooltip> GetTooltips(State state, Combat combat, int amount)
    {
        return [new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::action::CellHarvest")
		{
			Icon = ModEntry.Instance.CostUnsatisfiedIcon.Sprite,
			TitleColor = Colors.action,
			Title = "CELL HARVEST",
			Description = "Choose <c=keyword>"+ amount +"</c> <c=midrow>cell colonies</c> at most 1 space offset from your ship to destroy. If there are not enough, this action does not happen."
		},
        new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::Midrow::Cell")
        {
            Icon = ModEntry.Instance.CellColonyIcon.Sprite,
            TitleColor = Colors.midrow,
            Title = ModEntry.Instance.Localizations.Localize(["midrow", "Cell", "name"]),
            Description = string.Format("Will block 1 shot before being destroyed.")
        }];
    }
}