using System;
using System.Collections.Generic;
using System.Linq;
using Dougie.Artifacts;
using Dougie.ExternalAPIs;
using Dougie.Midrow;
using Nickel;
namespace Dougie.Actions;

public class LateHarvest : CardAction
{
    public required StuffBase thingy;
    public override void Begin(G g, State s, Combat c)
    {
        c.DestroyDroneAt(s,thingy.x,true);
    }
}
public class LateAddAsteroid : CardAction
{
    public required int thingyPosition;
    public override void Begin(G g, State s, Combat c)
    {
        c.stuff.Add(thingyPosition, new Asteroid{x = thingyPosition, xLerped = thingyPosition});
    }
}
public class HarvestMarkedCells : CardAction
{
    public static bool IsMarkedForDeath(StuffBase thing)
    {
        return ModEntry.Instance.Helper.ModData.TryGetModData<bool>(thing, "MarkedForDeath", out var data) && data;
    }
    public override void Begin(G g, State s, Combat c)
    {
        foreach(StuffBase stuff in c.stuff.Values.ToList())
        {
            if(stuff is not null)
            {
                if(stuff is CellColony cellColony)
                {
                    if(IsMarkedForDeath(cellColony))
                    {
                        if(cellColony.MutationF)
                        {
                            if(s.ship.Get(ModEntry.Instance.ApoptosisStatus.Status)>0)
                            {
                                c.QueueImmediate(new AAttack{damage = s.ship.Get(ModEntry.Instance.ApoptosisStatus.Status), fromDroneX = cellColony.x, timer = 0.2});
                            }
                            cellColony.MutationF = false;
                            ModEntry.Instance.Helper.ModData.SetModData(cellColony, "MarkedForDeath", false);
                        }
                        else
                        {
                            int objectPosition = cellColony.x;
                            Console.Write(objectPosition);
                            if(s.ship.Get(ModEntry.Instance.ApoptosisStatus.Status)>0)
                            {
                                c.QueueImmediate(new AAttack{damage = s.ship.Get(ModEntry.Instance.ApoptosisStatus.Status), fromDroneX = objectPosition, timer = 0.2});
                            }
                            if(s.ship.Get(ModEntry.Instance.CryptobiosisStatus.Status)>0)
                            {
                                c.QueueImmediate(new LateAddAsteroid{timer = 0, thingyPosition = objectPosition});   
                            }
                            c.QueueImmediate(new LateHarvest{thingy = cellColony, timer = 0});
                        }
                    }
                }
            }
        }
    }
}
public sealed class CellHarvest
{
    private static readonly UK MidrowExecutionUK = ModEntry.Instance.Helper.Utilities.ObtainEnumCase<UK>();
        private static readonly UK CancelExecutionUK = ModEntry.Instance.Helper.Utilities.ObtainEnumCase<UK>();
        public sealed class PickCellColony : CardAction
        {
            public required int amountCells;
            public override Route? BeginWithRoute(G g, State s, Combat c)
                {
                    return new ActionRoute{amount = amountCells};
                }
        }
        public sealed class ActionRoute : Route
        {
            public int rangeExtension = 0;
            public required int amount;
            public override bool GetShowOverworldPanels() => true;
            public override bool CanBePeeked() => false;
            public override void Render(G g)
            {
                State state = g.state;
                base.Render(g);
                if (g.state.route is not Combat combat)
                {
                    g.CloseRoute(this);
                    return;
                }
                if (state.EnumerateAllArtifacts().FirstOrDefault(a => a is ExtendoGrip) is { } artifact)
                {
                    rangeExtension = 1;
                }
                else
                {
                    rangeExtension = 0;
                }

                Draw.Rect(0, 0, MG.inst.PIX_W, MG.inst.PIX_H, Colors.black.fadeAlpha(0.4));

                var centerX = g.state.ship.x + g.state.ship.parts.Count / 2.0;
                foreach (var (worldX, @object) in combat.stuff)
                {
                    if(@object is CellColony)
                    {
                    if(@object.x < g.state.ship.x - 1 - rangeExtension || @object.x > g.state.ship.x + g.state.ship.parts.Count + rangeExtension) continue;
                    if (Math.Abs(worldX - centerX) > 10) continue;
                    if (g.boxes.FirstOrDefault(b => b.key is { } key && key.k == StableUK.midrow && key.v == worldX) is not { } realBox) continue;
                    var box = g.Push(new global::UIKey(MidrowExecutionUK, worldX), realBox.rect, onMouseDown: new MouseDownHandler(() => OnMidrowSelected(g, @object)));
                    if(@object is CellColony)
                        @object.Render(g, box.rect.xy);
                    if(IsMarkedForDeath(@object))
                    {
                        Rect rect = Combat.marginRect + Combat.arenaPos + combat.GetCamOffset();
                        Vec loc = @object.GetGetRect().xy + rect.xy;
                        Draw.Sprite(ModEntry.Instance.MarkedIcon.Sprite, loc.x + 2, loc.y + 10, color: Colors.white.fadeAlpha(Math.Abs(Math.Sin(g.time * 2.5))));
                    }
                    if (box.rect.x is > 60 and < 464.0 && box.IsHover() && @object is CellColony cellColony)
                    {
                        if(cellColony.x >= g.state.ship.x-1-rangeExtension && cellColony.x <= g.state.ship.x + g.state.ship.parts.Count + rangeExtension)
                        {
                            if (!Input.gamepadIsActiveInput)
                            {
                                MouseUtil.DrawGamepadCursor(box);
                            }
                            g.tooltips.Add(box.rect.xy + new Vec(16.0, 24.0), @object.GetTooltips());
                            @object.hilight = 2;
                        }
                    }
                    g.Pop();
                }
                else
                {
                    continue;
                }
                }
                var marginRect = new Rect(5.0, 26.0, G.screenRect.w - 10.0, G.screenRect.h - 26.0 - 8.0);
                SharedArt.WarningPopup(g, CancelExecutionUK, "Cells left to mark: " + amount, new Vec(marginRect.w / 2.0, marginRect.h - 79.0) + new Vec(0.0, -5.0));
            }
            private void OnMidrowSelected(G g, StuffBase @object)
            {
                if (g.state.route is not Combat combat)
                {
                    g.CloseRoute(this);
                    return;
                }
                Console.Write("Entering the if with a number " + amount);
                if(amount > 0)
                {
                    if(@object is CellColony cellColony)
                    {
                        if(cellColony.x >= g.state.ship.x-1-rangeExtension && cellColony.x <= g.state.ship.x + g.state.ship.parts.Count + rangeExtension)
                        {
                            if(!IsMarkedForDeath(cellColony))
                            {
                                ModEntry.Instance.Helper.ModData.SetModData(cellColony, "MarkedForDeath", true);
                                Console.Write("Succefully marked a cell for death");
                                amount--;
                                Console.Write("cells left to mark: " + amount);
                            }
                            else
                            {
                                ModEntry.Instance.Helper.ModData.SetModData(cellColony, "MarkedForDeath", false);
                                Console.Write("Succefully unmarked a cell for death");
                                amount++;
                                Console.Write("cells left to mark: " + amount);
                            }
                        }
                        if(amount > 0)
                        {
                            if (g.state.route is not Combat curCombat)
                                return;
                            curCombat.QueueImmediate(new PickCellColony{amountCells = amount});
                        }
                        else
                        {
                            g.CloseRoute(this);
                            return;
                        }
                    }
                    g.CloseRoute(this);
                    return;
            }
        }
    }
    public static bool IsMarkedForDeath(StuffBase thing)
    {
        return ModEntry.Instance.Helper.ModData.TryGetModData<bool>(thing, "MarkedForDeath", out var data) && data;
    }
}