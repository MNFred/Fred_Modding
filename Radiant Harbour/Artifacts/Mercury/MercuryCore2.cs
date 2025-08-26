using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Xna.Framework.Graphics;
using HarmonyLib;
using FredAndRadience.Radiant_Shipyard.actions;
using System.Threading;
using FMOD;
using FSPRO;

namespace FredAndRadience.Radiant_Shipyard;
public class ArtifactMercuryCore2 : Artifact, IRadiantArtifact
{
    private List<string> partsMercury = new List<string>() {"LeftMissile", "LeftCannon", "RightCannon", "RightMissile"};
    private string ?chosenKey = null;
    private string ?turnPart = null;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("MercuryCore2", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.Boss],
                unremovable = true
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Mercury/artifact_Mercury_Core_V2.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "MercuryCore2", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "MercuryCore2", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [new TTCard{card = new CardMercuryDriver()}];
    }
    public override void OnReceiveArtifact(State state)
    {
        state.artifacts.RemoveAll((Artifact r) => r is ArtifactMercuryCore);
    }
    public override void OnTurnStart(State state, Combat combat)
    {
        combat.QueueImmediate(new ADummyAction{dialogueSelector = ".Mercury_StartRun", timer = 0});
        Audio.Play(new GUID?(Event.TogglePart));
        partsMercury.Clear();
        foreach(Part part in state.ship.parts)
        {
            if(part.active == true && part.type != PType.cockpit)
                part.active = false;
            if(part.key != null && part.type != PType.cockpit)
                partsMercury.Add(part.key);
        }
        turnPart = partsMercury.Random(state.rngActions);
        combat.Queue(new MercuryPartReveal{randomKey = turnPart});
        partsMercury.Remove(turnPart);
    }
    public override void OnCombatStart(State state, Combat combat)
    {
        combat.QueueImmediate(new AAddCard{card = new CardMercuryDriver(), amount = 1, destination = CardDestination.Hand});
    }
    public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount)
    {
        if(partsMercury.Count > 0)
        {
            chosenKey = partsMercury.Random(state.rngActions);
            combat.Queue(new MercuryPartReveal{randomKey = chosenKey});
            partsMercury.Remove(chosenKey);
        }
    }
}