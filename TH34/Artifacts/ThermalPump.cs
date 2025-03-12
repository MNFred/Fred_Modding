using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Xna.Framework.Graphics;
using HarmonyLib;

namespace Fred.TH34.Artifacts;
public class ArtifactThermalPump : Artifact, ITH34Artifact
{
    private int turnCount;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("ThermalPump", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.TH34_Deck.Deck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/ThermalPump.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ThermalPump", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ThermalPump", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [..StatusMeta.GetTooltips(ModEntry.Instance.RefractoryStatus.Status,1)];
    }
    public override int? GetDisplayNumber(State s)
    {
        return turnCount;
    }
    public override void OnCombatStart(State state, Combat combat)
    {
        turnCount = 0;
    }
    public override void OnTurnStart(State state, Combat combat)
    {
        turnCount++;
        if(turnCount == 4)
        {
            combat.QueueImmediate(new AStatus{status = ModEntry.Instance.RefractoryStatus.Status, statusAmount = 1, targetPlayer = true});
            Pulse();
            turnCount = 0;
        }
    }
}