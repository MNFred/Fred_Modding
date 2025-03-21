using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Xna.Framework.Graphics;
using HarmonyLib;

namespace Fred.TH34.Artifacts;
public class ArtifactSilverCoils : Artifact, ITH34Artifact
{
    public bool activeThisFight = false;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("SilverCoils", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.TH34_Deck.Deck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/SilverCoils.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "SilverCoils", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "SilverCoils", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [
            ..StatusMeta.GetTooltips(ModEntry.Instance.RefractoryStatus.Status,1),
            new TTGlossary("action.overheat")
        ];
    }
    public override void OnCombatStart(State state, Combat combat)
    {
        activeThisFight = false;
    }
    public override void AfterPlayerOverheat(State state, Combat combat)
    {
        if(activeThisFight == false)
        {
            Pulse();
            combat.QueueImmediate(new AStatus{status = ModEntry.Instance.RefractoryStatus.Status, targetPlayer = true, statusAmount = 1});
            combat.QueueImmediate(new AHeal
            {
                targetPlayer = true,
                healAmount = 1,
            });
            activeThisFight = true;
        }
    }
}