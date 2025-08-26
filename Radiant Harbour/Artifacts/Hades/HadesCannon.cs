using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Xna.Framework.Graphics;
using HarmonyLib;
using FredAndRadience.Radiant_Shipyard.actions;
using FSPRO;
using FMOD;

namespace FredAndRadience.Radiant_Shipyard;
public class ArtifactHadesCannon : Artifact, IRadiantArtifact
{
    public bool RailgunActive;
    public int shieldAmount;
    public int counter = 0;
    public bool attackFromCard;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("HadesCannon", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.EventOnly],
                unremovable = true
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Hades/artifact_Hades_Cannon.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "HadesCannon", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "HadesCannon", "description"]).Localize,
        });
    }
    public override void OnTurnStart(State state, Combat combat)
    {
        attackFromCard = false;
        combat.QueueImmediate(new ADummyAction { dialogueSelector = ".Hades_StartRun", timer = 0 });
        combat.Queue(new RailgunActivation());
        combat.Queue(new AHadesCardcheck());
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [
            new TTCard{
                card = new CardChargeCannon()
            }
        ];
    }
    public override int ModifyBaseDamage(int baseDamage, Card? card, State state, Combat? combat, bool fromPlayer)
    {
        if (!fromPlayer) return 0;
        if(card is TrashAutoShoot)
            return 0;
        return RailgunActive ? state.ship.Get(ModEntry.Instance.Elec_Charge.Status) : 0;
    }
    public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount)
    {
        attackFromCard = true;
        combat.Queue(new RailgunCheck{timer = 0});
    }
    public override void OnPlayerAttack(State state, Combat combat)
    {
        if(RailgunActive == true)
        {
            if(attackFromCard == true)
            {
                if(state.ship.Get(ModEntry.Instance.Elec_Charge.Status)>0)
                    Audio.Play(Event.Hits_ShipExplosion);
                RailgunActive = false;
                combat.Queue(new AStatus{status = ModEntry.Instance.Elec_Charge.Status, statusAmount = 0, targetPlayer = true, mode = AStatusMode.Set, timer = 0});
                combat.Queue(new AModifyPartType{
                originalPartType = PType.cannon,
                newPartType = PType.cannon,
                skin = ModEntry.Instance.Hades_UnChargedCannon.UniqueName,
                timer = 0
                });
                Audio.Play(new GUID?(Event.TogglePart));
                attackFromCard = false;
            }
        }
        return;
    }
}
public class RailgunActivation : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        Audio.Play(new GUID?(Event.TogglePart));
        if(s.ship.Get(ModEntry.Instance.Elec_Charge.Status)>0)
        {
            foreach (Part part in s.ship.parts)
            {
                if (part.type == PType.cannon)
                {
                    part.skin = ModEntry.Instance.Hades_ChargedCannon.UniqueName;
                }
            }
        }
        if(s.ship.Get(ModEntry.Instance.Elec_Charge.Status)==0)
        {
            foreach (Part part in s.ship.parts)
            {
                if (part.type == PType.cannon)
                {
                    part.skin = ModEntry.Instance.Hades_UnChargedCannon.UniqueName;
                }
            }
        }
    }
}
public class RailgunCheck : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        ArtifactHadesCannon? artifact = s.artifacts.Find((x) => x is ArtifactHadesCannon) as ArtifactHadesCannon;
        if (artifact == null)
            return;
        if (c.currentCardAction == null)
            artifact.attackFromCard = false;
        if (s.ship.Get(ModEntry.Instance.Elec_Charge.Status) > 0)
        {
            Audio.Play(new GUID?(Event.TogglePart));
            foreach (Part part in s.ship.parts)
            {
                if (part.type == PType.cannon)
                {
                    part.skin = ModEntry.Instance.Hades_ChargedCannon.UniqueName;
                }
            }
            artifact.RailgunActive = true;
        }
        else
        {
            Audio.Play(new GUID?(Event.TogglePart));
            foreach (Part part in s.ship.parts)
            {
                if (part.type == PType.cannon)
                {
                    part.skin = ModEntry.Instance.Hades_UnChargedCannon.UniqueName;
                }
            }
        }
    }
}
public class AHadesCardcheck : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        foreach (Card card in c.hand)
        {
            if (card is CardChargeCannon)
            {
                return;
            }
        }
        c.QueueImmediate(new AAddCard { card = new CardChargeCannon(), destination = CardDestination.Hand, amount = 1, timer = 0 });
    }
}