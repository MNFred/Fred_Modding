using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FMOD;
using FSPRO;
using HarmonyLib;
using Nanoray.PluginManager;
using Nickel;

namespace FredAndRadience.Radiant_Shipyard.Midrow
{
    public class NukeLeftInActive : StuffBase
    {
        public static readonly string Midrow_Object_Name = "NukeLeftInActive";
        static NukeLeftInActive()
        {
            DB.drones[Midrow_Object_Name] = ModEntry.Instance.Uranus_NukeActive.Sprite;
        }
        public override List<Tooltip> GetTooltips()
        {
            List<Tooltip> tooltips = new List<Tooltip>()
            {
                new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::Midrow::Nuke")
                {
                    Icon = ModEntry.Instance.Uranus_Nuke_Icon.Sprite,
                    TitleColor = Colors.midrow,
                    Title = ModEntry.Instance.Localizations.Localize(["midrow", "nuke", "name"]),
                    Description = ModEntry.Instance.Localizations.Localize(["midrow", "nuke", "description"])
                },
            };
            return tooltips;
        }
        public override double GetWiggleAmount()
        {
            return 1.0;
        }
        public override double GetWiggleRate()
        {
            return 1.0;
        }
        public override void Render(G g, Vec v)
        {
            DrawWithHilight(g, ModEntry.Instance.Uranus_NukeActive.Sprite, v + GetOffset(g));
        }
        public override List<CardAction>? GetActionsOnDestroyed(State s, Combat c, bool wasPlayer, int worldX)
        {
            PFX.combatExplosion.Add(new Particle
            {
                pos = new Vec((worldX*16),70)+ new Vec(7.5, 4.0),
                lifetime = 0.8,
                size = 75,
                dragCoef = 1.5,
                dragVel = new Vec(y: -20.0)
            });
            Audio.Play(Event.Hits_ShipExplosion);
            foreach(StuffBase stuffBase in c.stuff.Values.ToList())
            {
                if(stuffBase.x != worldX && stuffBase != null)
                    c.DestroyDroneAt(s,stuffBase.x,false);
                if(stuffBase is NukeLeftInActive or NukeRightInActive or ArmedNuke)
                {
                    c.stuff.Remove(stuffBase.x);
                }
            }
            if(wasPlayer == false)
            {
                c.QueueImmediate(new AHurt{hurtAmount = 3, hurtShieldsFirst = true, targetPlayer = false});
                c.QueueImmediate(new AStunShip{targetPlayer = false});
            }
            if(wasPlayer == true)
            {
                c.QueueImmediate(new AHurt{hurtAmount = 3, hurtShieldsFirst = true, targetPlayer = true});
                c.QueueImmediate(new AStunShip{targetPlayer = false});
            }
            return null;
        }
        public override List<CardAction>? GetActions(State s, Combat c)
        {
            return new List<CardAction>()
            {
                new ArmNuke{side = true}
            };
        }
    }
    public class NukeRightInActive : StuffBase
    {
        public static readonly string Midrow_Object_Name = "NukeRightInActive";
        static NukeRightInActive()
        {
            DB.drones[Midrow_Object_Name] = ModEntry.Instance.Uranus_NukeActive.Sprite;
        }
        public override List<Tooltip> GetTooltips()
        {
            List<Tooltip> tooltips = new List<Tooltip>()
            {
                new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::Midrow::Nuke")
                {
                    Icon = ModEntry.Instance.Uranus_Nuke_Icon.Sprite,
                    TitleColor = Colors.midrow,
                    Title = ModEntry.Instance.Localizations.Localize(["midrow", "nuke", "name"]),
                    Description = ModEntry.Instance.Localizations.Localize(["midrow", "nuke","description"])
                },
            };
            return tooltips;
        }
        public override double GetWiggleAmount()
        {
            return 1.0;
        }
        public override double GetWiggleRate()
        {
            return 1.0;
        }
        public override void Render(G g, Vec v)
        {
            DrawWithHilight(g, ModEntry.Instance.Uranus_NukeActive.Sprite, v + GetOffset(g));
        }
        public override List<CardAction>? GetActionsOnDestroyed(State s, Combat c, bool wasPlayer, int worldX)
        {
            PFX.combatExplosion.Add(new Particle
            {
                pos = new Vec((worldX*16),70)+ new Vec(7.5, 4.0),
                lifetime = 0.8,
                size = 75,
                dragCoef = 1.5,
                dragVel = new Vec(y: -20.0)
            });
            Audio.Play(Event.Hits_ShipExplosion);
            foreach(StuffBase stuffBase in c.stuff.Values.ToList())
            {
                if(stuffBase.x != worldX && stuffBase != null)
                    c.DestroyDroneAt(s,stuffBase.x,false);
                if(stuffBase is NukeLeftInActive or NukeRightInActive or ArmedNuke)
                {
                    c.stuff.Remove(stuffBase.x);
                }
            }
            if(wasPlayer == false)
            {
                c.QueueImmediate(new AHurt{hurtAmount = 3, hurtShieldsFirst = true, targetPlayer = false});
                c.QueueImmediate(new AStunShip{targetPlayer = false});
            }
            if(wasPlayer == true)
            {
                c.QueueImmediate(new AHurt{hurtAmount = 3, hurtShieldsFirst = true, targetPlayer = true});
                c.QueueImmediate(new AStunShip{targetPlayer = false});
            }
            return null;
        }
        public override List<CardAction>? GetActions(State s, Combat c)
        {
            return new List<CardAction>()
            {
                new ArmNuke{side = false}
            };
        }
    }
    public class ArmedNuke : Missile
    {
        public static readonly string Midrow_Object_Name = "NukeActive";
        public static readonly int BASE_DAMAGE = 9;
        public static readonly Color exhaustColor = new Color("919191");
        static ArmedNuke()
        {
            DB.drones[Midrow_Object_Name] = ModEntry.Instance.Uranus_NukeActive.Sprite;
        }
        public ArmedNuke()
        {
            base.skin = Midrow_Object_Name;
        }
        public override double GetWiggleAmount()
        {
            return 2.0;
        }
        public override double GetWiggleRate()
        {
            return 5.0;
        }
        public override void Render(G g, Vec v)
        {
            Vec offset = GetOffset(g, doRound: true);
            Vec vec2 = v + offset;
            Vec vec3 = default;
            if (!targetPlayer)
            {
                vec3 += new Vec(0.0, 21.0);
            }
            Vec vec4 = vec2 + vec3 + new Vec(7.0, 8.0);
            bool flag4;
            bool flag2 = false;
            Spr? id = exhaustSprites.GetModulo((int)(g.state.time * 36.0 + x * 10));
            double num2 = vec4.x - 5.0;
            double y = vec4.y + (!targetPlayer ? 14 : 0);
            Vec? originRel = new Vec(0.0, 1.0);
            flag4 = targetPlayer;
            bool flipX = flag2;
            bool flipY = !flag4;
            Color? color = exhaustColor;
            Spr spr = ModEntry.Instance.Uranus_NukeActive.Sprite;
            DrawWithHilight(g, spr, vec2, flag2, flag4);
            Draw.Sprite(id, num2-1, y+3, flipX, flipY, 0.0, null, originRel, new Vec(1.2,1.2), null, color);
            Glow.Draw(vec4 + new Vec(0.5, -2.5), 35.0, exhaustColor * new Color(1.0, 0.5, 0.5).gain(0.2 + 0.1 * Math.Sin(g.state.time * 30.0 + x) * 0.5));
        }
        public override List<Tooltip> GetTooltips()
        {
            List<Tooltip> tooltips = new List<Tooltip>()
            {
                new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::Midrow::Nuke")
                {
                    Icon = ModEntry.Instance.Uranus_Nuke_Icon.Sprite,
                    TitleColor = Colors.midrow,
                    Title = ModEntry.Instance.Localizations.Localize(["midrow", "nuke", "name"]),
                    Description = ModEntry.Instance.Localizations.Localize(["midrow", "nuke","description"])
                },
            };
            return tooltips;
        }
        public override List<CardAction>? GetActionsOnDestroyed(State s, Combat c, bool wasPlayer, int worldX)
        {
            PFX.combatExplosion.Add(new Particle
            {
                pos = new Vec((worldX*16),70)+ new Vec(7.5, 4.0),
                lifetime = 0.8,
                size = 75,
                dragCoef = 1.5,
                dragVel = new Vec(y: -20.0)
            });
            Audio.Play(Event.Hits_ShipExplosion);
            foreach(StuffBase stuffBase in c.stuff.Values)
            {
                if(stuffBase.x != worldX && stuffBase != null)
                    c.DestroyDroneAt(s,stuffBase.x,false);
                if(stuffBase is NukeLeftInActive or NukeRightInActive or ArmedNuke)
                {
                    c.stuff.Remove(stuffBase.x);
                }
            }
            if(wasPlayer == false)
            {
                c.QueueImmediate(new AHurt{hurtAmount = 3, hurtShieldsFirst = true, targetPlayer = false});
                c.QueueImmediate(new AStunShip{targetPlayer = false});
            }
            if(wasPlayer == true)
            {
                c.QueueImmediate(new AHurt{hurtAmount = 3, hurtShieldsFirst = true, targetPlayer = true});
                c.QueueImmediate(new AStunShip{targetPlayer = false});
            }
            return null;
        }
        public override List<CardAction>? GetActions(State s, Combat c)
        {
            ArtifactUranusCore2? artifact = s.artifacts.Find((x) => x is ArtifactUranusCore2) as ArtifactUranusCore2;
            return new List<CardAction>()
            {
                artifact == null
                ?new ANukeHit
                {
                    worldX = x,
                    outgoingDamage = BASE_DAMAGE,
                    targetPlayer = targetPlayer,
                }
                :new ANukeHit
                {
                    worldX = x,
                    outgoingDamage = BASE_DAMAGE + 1,
                    targetPlayer = targetPlayer,
                }
            };
        }
    }
    public class ArmNuke : CardAction
    {
        public bool side;
        public override void Begin(G g, State s, Combat c)
        {
            foreach(StuffBase stuffBase in c.stuff.Values.ToList())
            {
                switch(side)
                {
                    case true:
                        if(stuffBase is NukeLeftInActive)
                        {
                            c.stuff.Remove(stuffBase.x);
                            Audio.Play(new GUID?(Event.Status_PowerUp));
                            Missile missile = new ArmedNuke
                            {
                                x = stuffBase.x,
                                age = stuffBase.age,
                                xLerped = stuffBase.xLerped,
                                bubbleShield = stuffBase.bubbleShield,
                                targetPlayer = stuffBase.targetPlayer,
                            };
                            c.stuff[stuffBase.x] = missile;
                        }break;
                    case false:
                    if(stuffBase is NukeRightInActive)
                        {
                            c.stuff.Remove(stuffBase.x);
                            Audio.Play(new GUID?(Event.Status_PowerUp));
                            Missile missile = new ArmedNuke
                            {
                                x = stuffBase.x,
                                age = stuffBase.age,
                                xLerped = stuffBase.xLerped,
                                bubbleShield = stuffBase.bubbleShield,
                                targetPlayer = stuffBase.targetPlayer,
                            };
                            c.stuff[stuffBase.x] = missile;
                        }break;
                }
            }
        }
    }
}
public class ANukeHit : AMissileHit
    {
        public override void Update(G g, State s, Combat c)
        {
            c.stuff.TryGetValue(worldX, out var value);
            Missile? missile = value as Missile;
            if (missile == null)
            {
                timer -= g.dt;
                return;
            }

            Ship ship = (targetPlayer ? s.ship : c.otherShip);
            if (ship == null)
            {
                return;
            }

            RaycastResult raycastResult = CombatUtils.RaycastGlobal(c, ship, fromDrone: true, worldX);
            bool flag = false;
            if (raycastResult.hitShip)
            {
                Part? partAtWorldX = ship.GetPartAtWorldX(raycastResult.worldX);
                if (partAtWorldX == null || partAtWorldX!.type != PType.empty)
                {
                    flag = true;
                }
            }else{
                c.QueueImmediate(new ADummyAction{dialogueSelector = ".Uranus_NukeMissed"});
            }


            if (missile.missileType == MissileType.seeker)
            {
                flag = true;
                raycastResult.worldX = missile.GetSeekerImpact(s, c);
            }

            if (!missile.isHitting)
            {
                Audio.Play(flag ? Event.Drones_MissileIncoming : Event.Drones_MissileMiss);
                missile.isHitting = true;
            }

            if (!(missile.yAnimation >= 3.5))
            {
                return;
            }

            if (flag)
            {
                c.otherShip.shake = 2.4;
                Vec vec = new Vec(missile.x * 16, missile.yAnimation * 6);
                PFX.combatExplosion.Add(new Particle
                {
                    pos = vec,
                    lifetime = 1.2,
                    size = 140,
                    dragCoef = 1.5,
                    dragVel = new Vec(y: -15.0)
                });
                int num = outgoingDamage;
                foreach (Artifact item in s.EnumerateAllArtifacts())
                {
                    num += item.ModifyBaseMissileDamage(s, s.route as Combat, targetPlayer);
                }

                if (num < 0)
                {
                    num = 0;
                }

                DamageDone dmg = ship.NormalDamage(s, c, num, raycastResult.worldX, piercing:false);
                EffectSpawner.NonCannonHit(g, targetPlayer, raycastResult, dmg);
                Part? partAtWorldX2 = ship.GetPartAtWorldX(raycastResult.worldX);
                if (partAtWorldX2 != null && partAtWorldX2!.stunModifier == PStunMod.stunnable)
                {
                    c.QueueImmediate(new AStunPart
                    {
                        worldX = raycastResult.worldX
                    });
                }

                if (status.HasValue && flag)
                {
                    c.QueueImmediate(new AStatus
                    {
                        status = status.Value,
                        statusAmount = statusAmount,
                        targetPlayer = targetPlayer
                    });
                }

                if (weaken && flag)
                {
                    c.QueueImmediate(new AWeaken
                    {
                        worldX = worldX,
                        targetPlayer = targetPlayer
                    });
                }

                if (ship.Get(Status.payback) > 0 || ship.Get(Status.tempPayback) > 0)
                {
                    c.QueueImmediate(new AAttack
                    {
                        damage = Card.GetActualDamage(s, ship.Get(Status.payback) + ship.Get(Status.tempPayback), !targetPlayer),
                        targetPlayer = !targetPlayer,
                        fast = true
                    });
                }
                c.QueueImmediate(new AStunShip{targetPlayer = false});
            }

            c.stuff.Remove(worldX);
            if (!(raycastResult.hitDrone || flag))
            {
                c.stuffOutro.Add(missile);
            }
        }
    }