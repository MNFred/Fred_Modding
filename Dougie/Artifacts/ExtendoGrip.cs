using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Reflection;

namespace Dougie.Artifacts;
public class ExtendoGrip : Artifact, IDougieArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("ExtendoGrip", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.DougieDeck.Deck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Artifact/ExtendoGrip.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ExtendoGrip", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ExtendoGrip", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
      return [
        new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::action::CellHarvest"){	Icon = ModEntry.Instance.CostUnsatisfiedIcon.Sprite,	TitleColor = Colors.action,	Title = "CELL HARVEST",	Description = "Choose <c=keyword>#</c> <c=midrow>cell colonies</c> at most 1 space offset from your ship to destroy. If there are not enough, this action does not happen."}
      ];
    }
}