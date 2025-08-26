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
public class ArtifactVenusCore : Artifact, IRadiantArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("VenusCore", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.EventOnly],
                unremovable = true
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Venus/artifact_Venus_Core.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "VenusCore", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "VenusCore", "description"]).Localize,
        });
    }
    public override void OnTurnStart(State state, Combat combat)
    {
        combat.QueueImmediate(new ADummyAction{dialogueSelector = ".Venus_StartRun"});
    }
    public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount)
    {
        combat.Queue(new VenusSwap());
    }
}