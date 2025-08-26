using FMOD;
using FSPRO;
using HarmonyLib;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FredAndRadience.Radiant_Shipyard.Patches
{
    public class RefillNukesAction : CardAction
    {
        public override void Begin(G g, State s, Combat c)
        {
            ArtifactUranusCore? artifact = s.artifacts.Find((x) => x is ArtifactUranusCore) as ArtifactUranusCore;
            if(artifact == null)
                return;
            for(int i = 0; i < s.ship.parts.Count; i++ )
            {
                if(s.ship.parts[i].key == "UranusEmptyL")
                {
                    s.ship.parts[i] = new Part()
                    {
                        type = PType.wing,
                        skin = ModEntry.Instance.UranusWingL.UniqueName,
                        key = "LeftComp"
                    };
                    s.ship.shieldMaxBase += 2;
                }
                if(s.ship.parts[i].key == "UranusEmptyR")
                {
                    s.ship.parts[i] = new Part()
                    {
                        type = PType.wing,
                        skin = ModEntry.Instance.UranusWingR.UniqueName,
                        key = "RightComp"
                    };
                    s.ship.shieldMaxBase += 2;
                }
            }
            Audio.Play(new GUID?(Event.Status_PowerUp));
            artifact.originalPartsPlace.Clear();
        }
    }
    [HarmonyPatch]
    public static class PatchRefillNukesAtShop
    {
        [HarmonyPatch(typeof(Events), nameof(Events.NewShop)), HarmonyPostfix]
        public static void AddRepairChoice(State s, ref List<Choice> __result)
        {
            if (s.EnumerateAllArtifacts().Any((a) => a is ArtifactUranusCore g))
            {
                foreach(Part part in s.ship.parts)
                {
                    if(part.key == "UranusEmptyL" || part.key == "UranusEmptyR")
                    {
                        __result.Insert(0,new Choice() {    
                        label = ModEntry.Instance.Localizations.Localize(["choice", "Shop", "RefillNukes"]),
                        key = ".shopRefillNukes",
                        actions = new List<CardAction>()
                        {
                            new RefillNukesAction()
                        }
                        });
                        return;
                    }
                }
                
            }
        }
    }
}