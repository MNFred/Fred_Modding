using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Xna.Framework.Graphics;
using HarmonyLib;
using FredAndRadience.Radiant_Shipyard.actions;
using FMOD;
using FSPRO;

namespace FredAndRadience.Radiant_Shipyard;
public class ArtifactVenusCore2 : Artifact, IRadiantArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("VenusCore2", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.Boss],
                unremovable = true
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Venus/artifact_Wing_Armor.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "VenusCore2", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "VenusCore2", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [
            new TTGlossary("parttrait.armor"),
        ];
    }
    public override void OnReceiveArtifact(State state)
    {
        for(int i=0; i<state.ship.parts.Count;i++)
        {
            if(state.ship.parts[i].key == "VenusLeftWing")
            {
                state.ship.parts[i].damageModifier = PDamMod.armor;
            }
            if(state.ship.parts[i].key == "VenusRightWing")
            {
                state.ship.parts[i].damageModifier = PDamMod.armor;
            }
        }
    }
}
