using System.Collections.Generic;
using System.Linq;
using Dougie.Midrow;
using Microsoft.Xna.Framework.Graphics;
using Nickel;

namespace Dougie.Actions;

public class MutateAllF : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
		=> [
			new GlossaryTooltip($"action.{GetType().Namespace!}::MutateAllF")
			{
				Icon = ModEntry.Instance.FMutationIcon.Sprite,
				TitleColor = Colors.action,
				Title = ModEntry.Instance.Localizations.Localize(["action", "MutateAllF", "name"]),
				Description = ModEntry.Instance.Localizations.Localize(["action", "MutateAllF", "description"]),
			},
            new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::Midrow::Cell")
                {
                    Icon = ModEntry.Instance.CellColonyIcon.Sprite,
                    TitleColor = Colors.midrow,
                    Title = ModEntry.Instance.Localizations.Localize(["midrow", "Cell", "name"]),
                    Description = string.Format("Will block 1 shot before being destroyed.")
                },
            new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::action::CellHarvest"){	Icon = ModEntry.Instance.CostUnsatisfiedIcon.Sprite,	TitleColor = Colors.action,	Title = "CELL HARVEST",	Description = "Choose <c=keyword>#</c> <c=midrow>cell colonies</c> at most 1 space offset from your ship to destroy. If there are not enough, this action does not happen."}
		];
    public override Icon? GetIcon(State s)
        => new(ModEntry.Instance.FMutationIcon.Sprite,null, Colors.action, false);
    public override void Begin(G g, State s, Combat c)
    {
        c.QueueImmediate(new ActuallyMutateAllofThemFBecauseIWasLyingAFewLinesUp());
        c.QueueImmediate(new VisualTransformationF2{timer = 0.8});
        c.QueueImmediate(new VisualTransformationF1{timer = 0.2});
    }
}
public class VisualTransformationF1 : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        foreach(StuffBase stuff in c.stuff.Values.ToArray())
        {
            if(stuff is CellColony cellColony)
                if(!cellColony.MutationF)
                {
                    c.fx.Add(new AnimationTransformF_1{currentCellColony = cellColony});
                }
        }
    }
}
public class VisualTransformationF2 : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        foreach(StuffBase stuff in c.stuff.Values.ToArray())
        {
            if(stuff is CellColony cellColony)
                if(!cellColony.MutationF)
                    c.fx.Add(new AnimationTransformF_2{currentCellColony = cellColony});
        }
    }
}
public class AnimationTransformF_1 : FX
{
    private static ISpriteEntry BtF = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/BtF_0.png"));
    private static ISpriteEntry AtF = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/AtF_0.png"));
    private static ISpriteEntry DtF = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/DtF_0.png"));
    private static ISpriteEntry XtF = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/XtF_0.png"));
    private static ISpriteEntry ADtF = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/ADtF_0.png"));
    private static ISpriteEntry AXtF = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/AXtF_0.png"));
    private static ISpriteEntry DXtF = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/DXtF_0.png"));
    private static ISpriteEntry DAXtF = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/DAXtF_0.png"));
    public required CellColony currentCellColony;
    public bool ShouldDrawHilight(G g)
    {
        if (g.state.route is Combat combat)
        {
            if (currentCellColony.hilight <= 0)
            {
                if (combat.isHoveringDroneMove > 0)
                {
                    return !currentCellColony.Immovable();
                }

                return false;
            }

            return true;
        }
        return false;
    }
    public void DrawWithHilight(G g, Spr id, Vec v, bool flipX = false, bool flipY = false, Rect? pixelRect = null)
    {
        if (ShouldDrawHilight(g))
        {
            Texture2D? outlined = SpriteLoader.GetOutlined(id);
            double num = v.x - 2.0;
            double y = v.y - 2.0;
            BlendState screen = BlendMode.Screen;
            Color? color = Colors.droneOutline;
            Draw.Sprite(outlined, num, y, flipX, flipY, 0.0, null, null, null, pixelRect, color, screen);
        }
        Draw.Sprite(id, v.x - 1.0 + currentCellColony.xLerped * 16, v.y + 49.0, flipX, flipY, pixelRect: pixelRect);
    }
    public override void Render(G g, Vec v)
    {
        if(!currentCellColony.MutationD && !currentCellColony.MutationX && !currentCellColony.MutationA)
        {
            DrawWithHilight(g, BtF.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(!currentCellColony.MutationD && !currentCellColony.MutationX && currentCellColony.MutationA)
        {
            DrawWithHilight(g, AtF.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(!currentCellColony.MutationA && !currentCellColony.MutationX && currentCellColony.MutationD)
        {
            DrawWithHilight(g, DtF.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(!currentCellColony.MutationA && currentCellColony.MutationX && !currentCellColony.MutationD)
        {
            DrawWithHilight(g, XtF.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationA && !currentCellColony.MutationX && currentCellColony.MutationD)
        {
            DrawWithHilight(g, ADtF.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationA && currentCellColony.MutationX && !currentCellColony.MutationD)
        {
            DrawWithHilight(g, AXtF.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(!currentCellColony.MutationA && currentCellColony.MutationX && currentCellColony.MutationD)
        {
            DrawWithHilight(g, DXtF.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationA && currentCellColony.MutationX && currentCellColony.MutationD)
        {
            DrawWithHilight(g, DAXtF.Sprite, v + currentCellColony.GetOffset(g));
        }
    }
}
public class AnimationTransformF_2 : FX
{
    private static ISpriteEntry BtF = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/BtF_1.png"));
    private static ISpriteEntry AtF = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/AtF_1.png"));
    private static ISpriteEntry DtF = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/DtF_1.png"));
    private static ISpriteEntry XtF = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/XtF_1.png"));
    private static ISpriteEntry ADtF = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/ADtF_1.png"));
    private static ISpriteEntry AXtF = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/AXtF_1.png"));
    private static ISpriteEntry DXtF = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/DXtF_1.png"));
    private static ISpriteEntry DAXtF = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/DAXtF_1.png"));
    public required CellColony currentCellColony;
    public bool ShouldDrawHilight(G g)
    {
        if (g.state.route is Combat combat)
        {
            if (currentCellColony.hilight <= 0)
            {
                if (combat.isHoveringDroneMove > 0)
                {
                    return !currentCellColony.Immovable();
                }

                return false;
            }

            return true;
        }
        return false;
    }
    public void DrawWithHilight(G g, Spr id, Vec v, bool flipX = false, bool flipY = false, Rect? pixelRect = null)
    {
        if (ShouldDrawHilight(g))
        {
            Texture2D? outlined = SpriteLoader.GetOutlined(id);
            double num = v.x - 2.0;
            double y = v.y - 2.0;
            BlendState screen = BlendMode.Screen;
            Color? color = Colors.droneOutline;
            Draw.Sprite(outlined, num, y, flipX, flipY, 0.0, null, null, null, pixelRect, color, screen);
        }
        Draw.Sprite(id, v.x - 1.0 + currentCellColony.xLerped * 16, v.y + 49.0, flipX, flipY, pixelRect: pixelRect);
    }
    public override void Render(G g, Vec v)
    {
        if(!currentCellColony.MutationA && !currentCellColony.MutationD && !currentCellColony.MutationX)
        {
            DrawWithHilight(g, BtF.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationA && !currentCellColony.MutationD && !currentCellColony.MutationX)
        {
            DrawWithHilight(g, AtF.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(!currentCellColony.MutationA && currentCellColony.MutationD && !currentCellColony.MutationX)
        {
            DrawWithHilight(g, DtF.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(!currentCellColony.MutationA && !currentCellColony.MutationD && currentCellColony.MutationX)
        {
            DrawWithHilight(g, XtF.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationA && currentCellColony.MutationD && !currentCellColony.MutationX)
        {
            DrawWithHilight(g, ADtF.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationA && currentCellColony.MutationX && !currentCellColony.MutationD)
        {
            DrawWithHilight(g, AXtF.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(!currentCellColony.MutationA && currentCellColony.MutationX && currentCellColony.MutationD)
        {
            DrawWithHilight(g, DXtF.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationA && currentCellColony.MutationX && currentCellColony.MutationD)
        {
            DrawWithHilight(g, DAXtF.Sprite, v + currentCellColony.GetOffset(g));
        }
    }
}
public class ActuallyMutateAllofThemFBecauseIWasLyingAFewLinesUp : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        foreach(StuffBase stuff in c.stuff.Values)
        {
            if(stuff is CellColony cellColony)
            {
                if(!cellColony.MutationF)
                {
                    cellColony.MutationF = true;
                }
            }
        }
    }
}