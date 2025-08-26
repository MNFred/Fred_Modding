using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Xna.Framework.Graphics;
using HarmonyLib;
using FredAndRadience.Radiant_Shipyard.Patches;

namespace FredAndRadience.Radiant_Shipyard;
public class ArtifactUranusCore2 : Artifact, IRadiantArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("UranusCore2", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.Boss],
                unremovable = true
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Uranus/artifact_Uranus_Core_V2.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "UranusCore2", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "UranusCore2", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [
            new TTGlossary("midrow.bubbleShield")
        ];
    }
    public override void OnReceiveArtifact(State state)
    {
        foreach(Part part in state.ship.parts)
        {
            if(part.key == "LeftComp" || part.key == "RightComp")
            {
                state.ship.hullMax += 1;
                state.ship.Heal(1);
            }
        }
        state.GetCurrentQueue().Queue(new RefillNukesAction());
    }
}