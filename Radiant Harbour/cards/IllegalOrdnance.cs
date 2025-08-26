using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using FMOD;
using FredAndRadience.Radiant_Shipyard.Midrow;
using FredAndRadience.Radiant_Shipyard.Uranus;
using FSPRO;
using Nanoray.PluginManager;
using Nickel;

namespace FredAndRadience.Radiant_Shipyard;

internal sealed class CardIllegalOrdnance : Card, IRadiantCard
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("IllegalOrdnance", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.Uranus_Deck.Deck,
                rarity = Rarity.common,
                dontOffer = true
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "IllegalOrdnance", "name"]).Localize,
        });
    }
    public override CardData GetData(State state) => new()
    {
        cost = 1,
        retain = true,
        flippable = true,
        temporary = true,
        singleUse = true,
        description = IODesc(state)
    };
    public string IODesc(State s)
    {
        bool LP = false;
        bool RP = false;
        foreach (Part part in s.ship.parts)
        {
            if (part.key == "LeftComp")
            {
                LP = true;
            }
            if (part.key == "RightComp")
            {
                RP = true;
            }
        }
        if (LP == true && RP == true)
        {
            return this.flipped ? "Launch a <c=midrow>nuke</c> from your <c=keyword>right</c> wing. Make the wing a scaffold." : "Launch a <c=midrow>nuke</c> from your <c=keyword>left</c> wing. Make the wing a scaffold.";
        }
        else
        {
            return "Launch a <c=midrow>nuke</c> out of your only wing. Make that wing a scaffold.";
        }
    }
    public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
    {
        Upgrade.A => [

        ],
        Upgrade.B => [

        ],
        _ => [
            new DisposeNukes{side = !flipped, timer = 0},
            new NukeTooltip{timer = 0}
        ],
    };
}
public class DisposeNukes : CardAction
{
    public bool side;
    public override List<Tooltip> GetTooltips(State s)
    {
        List<Tooltip> list = new List<Tooltip>();
        switch (side)
        {
            case false:
                foreach (Part part in s.ship.parts)
                {
                    if (part.key == "RightComp")
                    {
                        part.hilight = true;
                        return list;
                    }
                }
                foreach (Part part in s.ship.parts)
                {
                    if (part.key == "LeftComp")
                    {
                        part.hilight = true;
                    }
                }
                return list;
            case true:
                foreach (Part part in s.ship.parts)
                {
                    if (part.key == "LeftComp")
                    {
                        part.hilight = true;
                        return list;
                    }
                }
                foreach (Part part in s.ship.parts)
                {
                    if (part.key == "RightComp")
                    {
                        part.hilight = true;
                    }
                }
                return list;
        }
    }
    public override void Begin(G g, State s, Combat c)
    {
        ArtifactUranusCore2? artifact = s.artifacts.Find((x) => x is ArtifactUranusCore2) as ArtifactUranusCore2;
        switch (side)
        {
            case true:
                foreach (Part part in s.ship.parts)
                {
                    if (part.key == "LeftComp")
                    {
                        c.QueueImmediate(
                        from p in s.ship.parts.Select((Part part, int x) => new { part, x })
                        where p.part.key == "LeftComp"
                        select new ASpawn()
                        {
                            fromX = p.x,
                            thing = new NukeLeftInActive { targetPlayer = false, bubbleShield = artifact != null ? true : false },
                            timer = 0
                        }
                        );
                        Audio.Play(new GUID?(Event.Hits_HitHurt));
                        c.QueueImmediate(new NukeEjection()
                        {
                            compKey = "LeftComp",
                            timer = 0
                        }
                        );
                        s.ship.shieldMaxBase -= 2;
                        return;
                    }
                }
                foreach (Part part in s.ship.parts)
                {
                    if (part.key == "RightComp")
                    {
                        c.QueueImmediate(
                        from p in s.ship.parts.Select((Part part, int x) => new { part, x })
                        where p.part.key == "RightComp"
                        select new ASpawn()
                        {
                            fromX = p.x,
                            thing = new NukeRightInActive { targetPlayer = false, bubbleShield = artifact != null ? true : false },
                            timer = 0
                        }
                        );
                        Audio.Play(new GUID?(Event.Hits_HitHurt));
                        c.QueueImmediate(new NukeEjection()
                        {
                            compKey = "RightComp",
                            timer = 0
                        }
                        );
                        s.ship.shieldMaxBase -= 2;
                    }
                }
                break;
            case false:
                foreach (Part part in s.ship.parts)
                {
                    if (part.key == "RightComp")
                    {
                        c.QueueImmediate(
                        from p in s.ship.parts.Select((Part part, int x) => new { part, x })
                        where p.part.key == "RightComp"
                        select new ASpawn()
                        {
                            fromX = p.x,
                            thing = new NukeRightInActive { targetPlayer = false, bubbleShield = artifact != null ? true : false },
                            timer = 0
                        }
                        );
                        Audio.Play(new GUID?(Event.Hits_HitHurt));
                        c.QueueImmediate(new NukeEjection()
                        {
                            compKey = "RightComp",
                            timer = 0
                        }
                        );
                        s.ship.shieldMaxBase -= 2;
                        return;
                    }
                }
                foreach (Part part in s.ship.parts)
                {
                    if (part.key == "LeftComp")
                    {
                        c.QueueImmediate(
                        from p in s.ship.parts.Select((Part part, int x) => new { part, x })
                        where p.part.key == "LeftComp"
                        select new ASpawn()
                        {
                            fromX = p.x,
                            thing = new NukeLeftInActive { targetPlayer = false, bubbleShield = artifact != null ? true : false },
                            timer = 0
                        }
                        );
                        Audio.Play(new GUID?(Event.Hits_HitHurt));
                        c.QueueImmediate(new NukeEjection()
                        {
                            compKey = "LeftComp",
                            timer = 0
                        }
                        );
                        s.ship.shieldMaxBase -= 2;
                        return;
                    }
                }
                break;
            default:
        }
    }
}
public class NukeEjection : CardAction
{
    public string compKey = "";
    public override void Begin(G g, State s, Combat c)
    {
        ArtifactUranusCore? artifact = s.artifacts.Find((x) => x is ArtifactUranusCore) as ArtifactUranusCore;
        if(artifact == null)
            return;
        for(int i = 0; i < s.ship.parts.Count; i++ )
        {
            if(s.ship.parts[i].key == compKey)
            {
                if(compKey == "RightComp")
                {
                    c.fx.Add(new RemnantEjectR{worldX = (s.ship.x + i) * 15 + 2});
                    artifact.originalPartsPlace.Add("UranusEmptyR", i);
                    s.ship.parts[i] = new Part()
                    {
                        type = PType.empty,
                        flip = true,
                        skin = ModEntry.Instance.Uranus_Scaffold.UniqueName,
                        key = "UranusEmptyR"
                    };
                    c.fx.Add(new DroneExplosion(){pos = new Vec(((s.ship.x + i) * 16) +6,110)});
                }
                if(compKey == "LeftComp")
                {
                    c.fx.Add(new RemnantEjectL{worldX = (s.ship.x + i) * 15});
                    artifact.originalPartsPlace.Add("UranusEmptyL", i);
                    s.ship.parts[i] = new Part()
                    {
                        type = PType.empty,
                        skin = ModEntry.Instance.Uranus_Scaffold.UniqueName,
                        flip = false,
                        key = "UranusEmptyL"
                    };
                    c.fx.Add(new DroneExplosion(){pos = new Vec(((s.ship.x + i) * 16) +6,110)});
                }
                break;
            }
        }
    }
}
public class NukeTooltip : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
        {
            ArtifactUranusCore2? artifact = s.artifacts.Find((x) => x is ArtifactUranusCore2) as ArtifactUranusCore2;
            return [
                new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::Midrow::Nuke")
                {
                    Icon = ModEntry.Instance.Uranus_Nuke_Icon.Sprite,
                    TitleColor = Colors.midrow,
                    Title = ModEntry.Instance.Localizations.Localize(["midrow", "nuke", "name"]),
                    Description = ModEntry.Instance.Localizations.Localize(["midrow", "nuke","description"])
                },
            ];
        }
}