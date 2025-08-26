using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Xna.Framework.Graphics;
using HarmonyLib;
using FMOD;
using FSPRO;

namespace FredAndRadience.Radiant_Shipyard;
public class ArtifactCerberusHead : Artifact, IRadiantArtifact
{
    public int combo = 0;
    public int counterTool;
    public int streak = 0;
    public bool attacking = false;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("CerberusHead", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = Deck.colorless,
                pools = [ArtifactPool.EventOnly],
                unremovable = true
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cerberus/artifact_Cerberus's_Heads.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "CerberusHead", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "CerberusHead", "description"]).Localize,
        });
    }
    public override int? GetDisplayNumber(State s)
    {
        return combo;
    }
    public override void OnTurnStart(State state, Combat combat)
    {
        combat.QueueImmediate(new ADummyAction{timer = 0, dialogueSelector = ".Cerberus_StartRun"});
    }
    public override void OnCombatStart(State state, Combat combat)
    {
        counterTool = 2;
        int i = 1;
        foreach(Part part in state.ship.parts)
        {
            if(part.type == PType.cannon)
            {
                part.uuid = i;
                i++;
            }
        }
        combat.QueueImmediate(new CerberusCycleReset{timer = 0.4});
    }
    public override void OnCombatEnd(State state)
    {
        combo = 0;
        streak = 0;
    }
    public override void OnPlayerAttack(State state, Combat combat)
    {
        if(attacking == true)
        {
            combo += 1;
            if(combo < 4)
                streak = 0;
            if(combo >= 4 && combo < 6)
                streak = 1;
            if(combo == 6)
                streak = 2;
            if(combo > 6)
            {
                combo = 0;
                streak = 0;
            }
            attacking = false;
            combat.Queue(new CerberusHeadCycle{counter = counterTool, comboTool = combo, streakTool = streak, timer = 0.4});
        }
    }
    public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount)
    {
        attacking = true;
        counterTool++;
        if(counterTool > 3)
            counterTool = 1;
        combat.Queue(new CerberusHeadCycle{counter = counterTool, comboTool = combo, streakTool = streak, timer = 0.4});
    }
    public override void OnEnemyDodgePlayerAttack(State state, Combat combat)
    {
        combo = 0;
        streak = 0;
        combat.Queue(new CerberusHeadCycle{comboTool = combo, counter = counterTool, streakTool = streak, timer = 0.4});
    }
}
public class CerberusHeadCycle : CardAction
{
    public int counter;
    public int comboTool;
    private string ?comboSkin = null;
    public int streakTool = 0;
    private int counter2;
    public override void Begin(G g, State s, Combat c)
    {
        ArtifactCerberusArmor? artifact = s.artifacts.Find((x) => x is ArtifactCerberusArmor) as ArtifactCerberusArmor;
        ArtifactCerberusHead? artifact2 = s.artifacts.Find((x) => x is ArtifactCerberusHead) as ArtifactCerberusHead;
        if(artifact2 == null)
            return;
        comboSkin = comboTool switch
        {
            0 => ModEntry.Instance.CerberusCannon0.UniqueName,
            1 => ModEntry.Instance.CerberusCannon1.UniqueName,
            2 => ModEntry.Instance.CerberusCannon2.UniqueName,
            3 => ModEntry.Instance.CerberusCannon3.UniqueName,
            4 => ModEntry.Instance.CerberusCannon4.UniqueName,
            5 => ModEntry.Instance.CerberusCannon5.UniqueName,
            6 => ModEntry.Instance.CerberusCannon6.UniqueName,
            _ => ModEntry.Instance.CerberusCannon6.UniqueName
        };
        counter2 = counter + 1;
        if(counter2 > 3)
            counter2 = 1;
        switch(streakTool)
        {
            case 0:
                if(comboTool != 0)
                    Audio.Play(new GUID?(Event.TogglePart));
                for(int i = 0; i<s.ship.parts.Count; i++)
                {
                    if(s.ship.parts[i].uuid == counter && s.ship.parts[i].type == PType.cannon)
                    {
                        int uid1 = s.ship.parts[i].uuid;
                        string ?key1 = s.ship.parts[i].key;
                        s.ship.parts[i] = new Part()
                        {
                            active = true,
                            type = PType.cannon,
                            uuid = uid1,
                            key = key1,
                            skin = comboSkin
                        };
                    }
                    if(s.ship.parts[i].uuid != counter && s.ship.parts[i].type == PType.cannon)
                    {
                        int uid1 = s.ship.parts[i].uuid;
                        string ?key1 = s.ship.parts[i].key;
                        s.ship.parts[i] = new Part()
                        {
                            active = false,
                            damageModifier = artifact == null ? PDamMod.none : PDamMod.armor,
                            type = PType.cannon,
                            uuid = uid1,
                            key = key1,
                            skin = ModEntry.Instance.CerberusCannonInActive.UniqueName
                        };
                    }
                }
                if(c.currentCardAction == null)
                    artifact2.attacking = false;
            break;
            case 1:
                if(comboTool != 0)
                    Audio.Play(new GUID?(Event.TogglePart));
                for(int i = 0; i<s.ship.parts.Count; i++)
                {
                    if(s.ship.parts[i].uuid == counter || s.ship.parts[i].uuid == counter2 && s.ship.parts[i].type == PType.cannon)
                    {
                        int uid1 = s.ship.parts[i].uuid;
                        string ?key1 = s.ship.parts[i].key;
                        s.ship.parts[i] = new Part()
                        {
                            active = true,
                            type = PType.cannon,
                            uuid = uid1,
                            key = key1,
                            skin = comboSkin
                        };
                    }
                    if(s.ship.parts[i].uuid != counter && s.ship.parts[i].uuid != counter2 && s.ship.parts[i].type == PType.cannon)
                    {
                        int uid1 = s.ship.parts[i].uuid;
                        string ?key1 = s.ship.parts[i].key;
                        s.ship.parts[i] = new Part()
                        {
                            active = false,
                            damageModifier = artifact == null ? PDamMod.none : PDamMod.armor,
                            type = PType.cannon,
                            uuid = uid1,
                            key = key1,
                            skin = ModEntry.Instance.CerberusCannonInActive.UniqueName
                        };
                    }
                }
                if(c.currentCardAction == null)
                    artifact2.attacking = false;
            break;
            case 2:
                Audio.Play(new GUID?(Event.TogglePart));
                for(int i = 0; i<s.ship.parts.Count; i++)
                {
                    if(s.ship.parts[i].uuid == 1 || s.ship.parts[i].uuid == 2 || s.ship.parts[i].uuid == 3 && s.ship.parts[i].type == PType.cannon)
                    {
                        int uid1 = s.ship.parts[i].uuid;
                        string ?key1 = s.ship.parts[i].key;
                        s.ship.parts[i] = new Part()
                        {
                            active = true,
                            type = PType.cannon,
                            uuid = uid1,
                            key = key1,
                            skin = comboSkin
                        };
                    }
                }
                if(c.currentCardAction == null)
                    artifact2.attacking = false;
            break;
        }
    }
}
public class CerberusCycleReset : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        ArtifactCerberusHead? artifact = s.artifacts.Find((x) => x is ArtifactCerberusHead) as ArtifactCerberusHead;
        if(artifact == null)
            return;
        artifact.counterTool = 2;
        ArtifactCerberusArmor? artifact2 = s.artifacts.Find((x) => x is ArtifactCerberusArmor) as ArtifactCerberusArmor;
        for(int i2 = 0; i2 < s.ship.parts.Count; i2++)
        {
            if(s.ship.parts[i2].uuid == 1)
            {
                s.ship.parts[i2] = new Part()
                {
                    damageModifier = artifact2 == null ? PDamMod.none : PDamMod.armor,
                    type = PType.cannon,
                    uuid = 1,
                    skin = ModEntry.Instance.CerberusCannonInActive.UniqueName,
                    active = false,
                    key = "CerbLeft"
                };
            }
            if(s.ship.parts[i2].uuid == 2)
            {
                s.ship.parts[i2] = new Part()
                {
                    damageModifier = PDamMod.none,
                    type = PType.cannon,
                    uuid = 2,
                    active = true,
                    skin = ModEntry.Instance.CerberusCannon0.UniqueName,
                    key = "CerbMiddle"
                };
            }
            if(s.ship.parts[i2].uuid == 3)
            {
                s.ship.parts[i2] = new Part()
                {
                    damageModifier = artifact2 == null ? PDamMod.none : PDamMod.armor,
                    type = PType.cannon,
                    skin = ModEntry.Instance.CerberusCannonInActive.UniqueName,
                    active = false,
                    uuid = 3,
                    key = "CerbRight"
                };
            }
        }
    }
}