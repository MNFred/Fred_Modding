using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Xna.Framework.Graphics;
using HarmonyLib;

namespace FredAndRadience.Radiant_Shipyard;
public class ArtifactRenovations : Artifact, IRadiantArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("Renovations", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.Boss],
                unremovable = true
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Changeling/artifact_Renovations.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Renovations", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Renovations", "description"]).Localize,
        });
    }
    public override void OnReceiveArtifact(State state)
    {
        ArmoredBay? artifact = state.artifacts.Find((x) => x is ArmoredBay) as ArmoredBay;
        GlassCannon? artifact2 = state.artifacts.Find((x) => x is GlassCannon) as GlassCannon;
        for(int i=0; i<state.ship.parts.Count; i++)
        {
            if(state.ship.parts[i].key == "ChangelingLeftWing")
            {
                state.ship.parts[i] = new Part
                {
                    skin = ModEntry.Instance.Changeling_Cannon.UniqueName,
                    type = PType.cannon,
                    damageModifier = artifact2 == null ? PDamMod.none : PDamMod.weak,
                    key = "ChangelingCannon2"
                };
            }
            if(state.ship.parts[i].key == "ChangelingRightWing")
            {
                state.ship.parts[i] = new Part
                {
                    skin = ModEntry.Instance.ChangelingMissiles.UniqueName,
                    type = PType.missiles,
                    damageModifier = artifact == null ? PDamMod.none : PDamMod.armor,
                    key = "ChangelingMissiles2"
                };
            }
        }
        state.GetCurrentQueue().Queue(new AInsertPart{part = new Part{skin = ModEntry.Instance.Changeling_Comms.UniqueName, damageModifier = PDamMod.weak, type = PType.comms}});
    }
}