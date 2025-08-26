using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Xna.Framework.Graphics;
using HarmonyLib;

namespace FredAndRadience.Radiant_Shipyard;
public class ArtifactTartarusRage : Artifact, IRadiantArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("TartarusRage", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.Boss],
                unremovable = true
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Hades/artifact_Wrath_of_Tartarus.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "TartarusRage", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "TartarusRage", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [
            ..StatusMeta.GetTooltips(ModEntry.Instance.Elec_Charge.Status,1)
        ];
    }
    public override void OnPlayerLoseHull(State state, Combat combat, int amount)
    {
        combat.Queue(new AStatus{status = ModEntry.Instance.Elec_Charge.Status, statusAmount = amount, targetPlayer = true});
    }
    public override void OnReceiveArtifact(State state)
    {
        state.ship.hullMax += 3;
        state.ship.Heal(3);
    }
}