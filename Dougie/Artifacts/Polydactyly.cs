using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Reflection;
using Dougie.cards;
using System.Linq;

namespace Dougie.Artifacts;
public class Polydactyly : Artifact, IDougieArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("Polydactyly", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.DougieDeck.Deck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Artifact/Polydactyly.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Polydactyly", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Polydactyly", "description"]).Localize,
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
      ];
    }
    public override void OnTurnStart(State state, Combat combat)
    {
        foreach(StuffBase stuff in combat.stuff.Values.ToList())
        {
            Part? curPart = state.ship.GetPartAtWorldX(stuff.x);
            if(curPart != null)
            {
                if(curPart.type == PType.cockpit || curPart.type == PType.missiles)
                {
                    Pulse();
                    combat.QueueImmediate(new ADrawCard{count = 1, timer = 0});
                    return;
                }   
            }
        }
    }
}