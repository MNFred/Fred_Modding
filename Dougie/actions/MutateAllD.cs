using System.Collections.Generic;
using System.Linq;
using Dougie.Midrow;
using Microsoft.Xna.Framework.Graphics;
using Nickel;

namespace Dougie.Actions;

public class MutateAllD : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
		=> [
			new GlossaryTooltip($"action.{GetType().Namespace!}::MutateAllD")
			{
				Icon = ModEntry.Instance.DMutationIcon.Sprite,
				TitleColor = Colors.action,
				Title = ModEntry.Instance.Localizations.Localize(["action", "MutateAllD", "name"]),
				Description = ModEntry.Instance.Localizations.Localize(["action", "MutateAllD", "description"]),
			},
            new TTGlossary("status.shield"),
            new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::Midrow::Cell")
                {
                    Icon = ModEntry.Instance.CellColonyIcon.Sprite,
                    TitleColor = Colors.midrow,
                    Title = ModEntry.Instance.Localizations.Localize(["midrow", "Cell", "name"]),
                    Description = string.Format("Will block 1 shot before being destroyed.")
                },
		];
    public override Icon? GetIcon(State s)
        => new(ModEntry.Instance.DMutationIcon.Sprite,null, Colors.action, false);
    public override void Begin(G g, State s, Combat c)
    {
        c.QueueImmediate(new ActuallyMutateAllofThemDBecauseIWasLyingAFewLinesUp());
        c.QueueImmediate(new VisualTransformationD2{timer = 0.8});
        c.QueueImmediate(new VisualTransformationD1{timer = 0.2});
    }
}
public class VisualTransformationD1 : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        foreach(StuffBase stuff in c.stuff.Values.ToArray())
        {
            if(stuff is CellColony cellColony)
                if(!cellColony.MutationD)
                {
                    c.fx.Add(new AnimationTransformD_1{currentCellColony = cellColony});
                }
        }
    }
}
public class VisualTransformationD2 : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        foreach(StuffBase stuff in c.stuff.Values.ToArray())
        {
            if(stuff is CellColony cellColony)
                if(!cellColony.MutationD)
                    c.fx.Add(new AnimationTransformD_2{currentCellColony = cellColony});
        }
    }
}
public class AnimationTransformD_1 : FX
{
    private static ISpriteEntry BtD = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/BtD_0.png"));
    private static ISpriteEntry AtD = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/AtD_0.png"));
    private static ISpriteEntry FtD = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/FtD_0.png"));
    private static ISpriteEntry XtD = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/XtD_0.png"));
    private static ISpriteEntry AFtD = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/AFtD_0.png"));
    private static ISpriteEntry AXtD = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/AXtD_0.png"));
    private static ISpriteEntry XFtD = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/XFtD_0.png"));
    private static ISpriteEntry AXFtD = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/AXFtD_0.png"));
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
        if(!currentCellColony.MutationF && !currentCellColony.MutationX && !currentCellColony.MutationA)
        {
            DrawWithHilight(g, BtD.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(!currentCellColony.MutationF && !currentCellColony.MutationX && currentCellColony.MutationA)
        {
            DrawWithHilight(g, AtD.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationF && !currentCellColony.MutationX && !currentCellColony.MutationA)
        {
            DrawWithHilight(g, FtD.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(!currentCellColony.MutationF && currentCellColony.MutationX && !currentCellColony.MutationA)
        {
            DrawWithHilight(g, XtD.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationF && !currentCellColony.MutationX && currentCellColony.MutationA)
        {
            DrawWithHilight(g, AFtD.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(!currentCellColony.MutationF && currentCellColony.MutationX && currentCellColony.MutationA)
        {
            DrawWithHilight(g, AXtD.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationF && currentCellColony.MutationX && !currentCellColony.MutationA)
        {
            DrawWithHilight(g, XFtD.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationF && currentCellColony.MutationX && currentCellColony.MutationA)
        {
            DrawWithHilight(g, AXFtD.Sprite, v + currentCellColony.GetOffset(g));
        }
    }
}
public class AnimationTransformD_2 : FX
{
    private static ISpriteEntry BtD = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/BtD_1.png"));
    private static ISpriteEntry AtD = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/AtD_1.png"));
    private static ISpriteEntry FtD = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/FtD_1.png"));
    private static ISpriteEntry XtD = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/XtD_1.png"));
    private static ISpriteEntry AFtD = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/AFtD_1.png"));
    private static ISpriteEntry AXtD = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/AXtD_1.png"));
    private static ISpriteEntry XFtD = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/XFtD_1.png"));
    private static ISpriteEntry AXFtD = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/AXFtD_1.png"));
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
        if(!currentCellColony.MutationA && !currentCellColony.MutationF && !currentCellColony.MutationX)
        {
            DrawWithHilight(g, BtD.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationA && !currentCellColony.MutationF && !currentCellColony.MutationX)
        {
            DrawWithHilight(g, AtD.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(!currentCellColony.MutationA && currentCellColony.MutationF && !currentCellColony.MutationX)
        {
            DrawWithHilight(g, FtD.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(!currentCellColony.MutationA && !currentCellColony.MutationF && currentCellColony.MutationX)
        {
            DrawWithHilight(g, XtD.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationA && currentCellColony.MutationF && !currentCellColony.MutationX)
        {
            DrawWithHilight(g, AFtD.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(!currentCellColony.MutationF && currentCellColony.MutationX && currentCellColony.MutationA)
        {
            DrawWithHilight(g, AXtD.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationF && currentCellColony.MutationX && !currentCellColony.MutationA)
        {
            DrawWithHilight(g, XFtD.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationF && currentCellColony.MutationX && currentCellColony.MutationA)
        {
            DrawWithHilight(g, AXFtD.Sprite, v + currentCellColony.GetOffset(g));
        }
    }
}
public class ActuallyMutateAllofThemDBecauseIWasLyingAFewLinesUp : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        foreach(StuffBase stuff in c.stuff.Values)
        {
            if(stuff is CellColony cellColony)
            {
                if(!cellColony.MutationD)
                {
                    cellColony.MutationD = true;
                }
            }
        }
    }
}