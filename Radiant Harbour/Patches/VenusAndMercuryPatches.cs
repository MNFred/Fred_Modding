using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace FredAndRadience.Radiant_Shipyard.MercuryAndVenus
{
    [HarmonyPatch]
    internal static class VenusAndMercuryPatches
    {
        [HarmonyPatch(typeof(Card), nameof(Card.RenderAction)), HarmonyPrefix]
        public static void DisableActionsPostfix(State state, CardAction action)
        {
            if (action is ASpawn a && a.fromPlayer == true)
            {
                if(CanVenusSpawn(state) == false)
                    a.disabled = true;
                if(CanMercurySpawn(state) == false)
                    a.disabled = true;
            }
            if (action is AAttack a2)
            {
                if(CanMercuryAttack(state) == false)
                    a2.disabled = true;
            }
        }
        public static bool CanVenusSpawn(State s)
        {
            if (s.artifacts.Any((x) => x is ArtifactVenusCore or ArtifactVenusCore2))
            {
                return s.ship.parts.Any((x) => x.type == PType.missiles);
            }
            return true;
        }
        public static bool CanMercurySpawn(State s)
        {
            if (s.artifacts.Any((x) => x is ArtifactMercuryCore or ArtifactMercuryCore2))
            {
                return s.ship.parts.Any((x) => x.type == PType.missiles && x.active == true);
            }
            return true;
        }
        public static bool CanMercuryAttack(State s)
        {
            if(s.artifacts.Any((x) => x is ArtifactMercuryCore or ArtifactMercuryCore2))
            {
                return s.ship.parts.Any((x) => x.type == PType.cannon && x.active == true);
            }
            return true;
        }
    }
}