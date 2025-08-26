using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Xna.Framework.Graphics;
using HarmonyLib;

namespace FredAndRadience.Radiant_Shipyard;
public class ArtifactUranusCore : Artifact, IRadiantArtifact
{
    public Dictionary<string, int> originalPartsPlace = new();
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("UranusCore", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.EventOnly],
                unremovable = true
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Uranus/artifact_Uranus_Core.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "UranusCore", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "UranusCore", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [
            new TTCard
            {
                card = new CardIllegalOrdnance()
            }
        ];
    }
    public override void OnCombatStart(State state, Combat combat)
    {
        foreach(Part part in state.ship.parts)
        {
            if(part.type == PType.wing)
            {
                combat.QueueImmediate(new UranusCardCheck());
                return;
            }
        }
    }
    public override void OnTurnStart(State state, Combat combat)
    {
        combat.QueueImmediate(new ADummyAction{dialogueSelector = ".Uranus_StartRun"});
        foreach(Part part in state.ship.parts)
        {
            if(part.type == PType.wing)
            {
                combat.QueueImmediate(new UranusCardCheck());
                return;
            }
        }
    }
}
public class UranusCardCheck : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        foreach(Card card in c.hand)
        {
            if(card is CardIllegalOrdnance)
            {
                return;
            }
        }
        c.QueueImmediate(new AAddCard{
            card = new CardIllegalOrdnance(),
            destination = CardDestination.Hand,
            amount = 1
        });
    }
}