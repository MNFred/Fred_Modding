using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Xna.Framework.Graphics;
using HarmonyLib;

namespace FredAndRadience.Radiant_Shipyard;
public class ArtifactChangelingCore : Artifact, IRadiantArtifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("ChangelingCore", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.EventOnly],
                unremovable = true
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Changeling/artifact_Changeling_Core.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ChangelingCore", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ChangelingCore", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [new TTCard{card = new CardShipPartShuffle()}];
    }
    public override void OnCombatStart(State state, Combat combat)
    {
        combat.QueueImmediate(new AAddCard{card = new CardShipPartShuffle(), amount = 1, destination = CardDestination.Hand});
    }
    public override void OnTurnStart(State state, Combat combat)
    {
        combat.Queue(new AShuffleShip{targetPlayer = true, timer = 0.5});
        combat.Queue(new ADummyAction{dialogueSelector = ".Change_StartRun", timer = 0});
    }
}