using System.Collections.Generic;
using System.Linq;
using Dougie.Midrow;
using Microsoft.Xna.Framework.Graphics;
using Nickel;

namespace Dougie.Actions;

public class MutateAllA : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
		=> [
			new GlossaryTooltip($"action.{GetType().Namespace!}::MutateAllA")
			{
				Icon = ModEntry.Instance.AMutationIcon.Sprite,
				TitleColor = Colors.action,
				Title = ModEntry.Instance.Localizations.Localize(["action", "MutateAllA", "name"]),
				Description = ModEntry.Instance.Localizations.Localize(["action", "MutateAllA", "description"]),
			},
            new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::Midrow::Cell")
                {
                    Icon = ModEntry.Instance.CellColonyIcon.Sprite,
                    TitleColor = Colors.midrow,
                    Title = ModEntry.Instance.Localizations.Localize(["midrow", "Cell", "name"]),
                    Description = string.Format("Will block 1 shot before being destroyed.")
                },
		];
    public override Icon? GetIcon(State s)
        => new(ModEntry.Instance.AMutationIcon.Sprite,null, Colors.action, false);
    public override void Begin(G g, State s, Combat c)
    {
        c.QueueImmediate(new ActuallyMutateAllofThemABecauseIWasLyingAFewLinesUp());
        c.QueueImmediate(new VisualTransformationA2{timer = 0.8});
        c.QueueImmediate(new VisualTransformationA1{timer = 0.2});
    }
}
public class VisualTransformationA1 : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        foreach(StuffBase stuff in c.stuff.Values.ToArray())
        {
            if(stuff is CellColony cellColony)
                if(!cellColony.MutationA)
                {
                    c.fx.Add(new AnimationTransformA_1{currentCellColony = cellColony});
                }
        }
    }
}
public class VisualTransformationA2 : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        foreach(StuffBase stuff in c.stuff.Values.ToArray())
        {
            if(stuff is CellColony cellColony)
                if(!cellColony.MutationA)
                    c.fx.Add(new AnimationTransformA_2{currentCellColony = cellColony});
        }
    }
}
public class AnimationTransformA_1 : FX
{
    private static ISpriteEntry BtA = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/BtA_0.png"));
    private static ISpriteEntry DtA = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/DtA_0.png"));
    private static ISpriteEntry FtA = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/FtA_0.png"));
    private static ISpriteEntry XtA = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/XtA_0.png"));
    private static ISpriteEntry DFtA = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/DFtA_0.png"));
    private static ISpriteEntry DXtA = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/DXtA_0.png"));
    private static ISpriteEntry XFtA = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/XFtA_0.png"));
    private static ISpriteEntry DXFtA = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/DXFtA_0.png"));
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
        if(!currentCellColony.MutationD && !currentCellColony.MutationF && !currentCellColony.MutationX)
        {
            DrawWithHilight(g, BtA.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationD && !currentCellColony.MutationF && !currentCellColony.MutationX)
        {
            DrawWithHilight(g, DtA.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(!currentCellColony.MutationD && currentCellColony.MutationF && !currentCellColony.MutationX)
        {
            DrawWithHilight(g, FtA.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(!currentCellColony.MutationD && !currentCellColony.MutationF && currentCellColony.MutationX)
        {
            DrawWithHilight(g, XtA.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationD && currentCellColony.MutationF && !currentCellColony.MutationX)
        {
            DrawWithHilight(g, DFtA.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationD && !currentCellColony.MutationF && currentCellColony.MutationX)
        {
            DrawWithHilight(g, DXtA.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(!currentCellColony.MutationD && currentCellColony.MutationF && currentCellColony.MutationX)
        {
            DrawWithHilight(g, XFtA.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationD && currentCellColony.MutationF && currentCellColony.MutationX)
        {
            DrawWithHilight(g, DXFtA.Sprite, v + currentCellColony.GetOffset(g));
        }
    }
}
public class AnimationTransformA_2 : FX
{
    private static ISpriteEntry BtA = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/BtA_1.png"));
    private static ISpriteEntry DtA = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/DtA_1.png"));
    private static ISpriteEntry FtA = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/FtA_1.png"));
    private static ISpriteEntry XtA = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/XtA_1.png"));
    private static ISpriteEntry DFtA = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/DFtA_1.png"));
    private static ISpriteEntry DXtA = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/DXtA_1.png"));
    private static ISpriteEntry XFtA = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/XFtA_1.png"));
    private static ISpriteEntry DXFtA = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/DXFtA_1.png"));
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
        if(!currentCellColony.MutationD && !currentCellColony.MutationF && !currentCellColony.MutationX)
        {
            DrawWithHilight(g, BtA.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationD && !currentCellColony.MutationF && !currentCellColony.MutationX)
        {
            DrawWithHilight(g, DtA.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(!currentCellColony.MutationD && currentCellColony.MutationF && !currentCellColony.MutationX)
        {
            DrawWithHilight(g, FtA.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(!currentCellColony.MutationD && !currentCellColony.MutationF && currentCellColony.MutationX)
        {
            DrawWithHilight(g, XtA.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationD && currentCellColony.MutationF && !currentCellColony.MutationX)
        {
            DrawWithHilight(g, DFtA.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationD && !currentCellColony.MutationF && currentCellColony.MutationX)
        {
            DrawWithHilight(g, DXtA.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(!currentCellColony.MutationD && currentCellColony.MutationF && currentCellColony.MutationX)
        {
            DrawWithHilight(g, XFtA.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationD && currentCellColony.MutationF && currentCellColony.MutationX)
        {
            DrawWithHilight(g, DXFtA.Sprite, v + currentCellColony.GetOffset(g));
        }
    }
}
public class ActuallyMutateAllofThemABecauseIWasLyingAFewLinesUp : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        foreach(StuffBase stuff in c.stuff.Values)
        {
            if(stuff is CellColony cellColony)
            {
                if(!cellColony.MutationA)
                {
                    cellColony.MutationA = true;
                }
            }
        }
    }
}