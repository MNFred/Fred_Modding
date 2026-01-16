using System.Collections.Generic;
using System.Linq;
using Dougie.Midrow;
using Microsoft.Xna.Framework.Graphics;
using Nickel;

namespace Dougie.Actions;

public class MutateAllX : CardAction
{
    public override List<Tooltip> GetTooltips(State s)
		=> [
			new GlossaryTooltip($"action.{GetType().Namespace!}::MutateAllX")
			{
				Icon = ModEntry.Instance.XMutationIcon.Sprite,
				TitleColor = Colors.action,
				Title = ModEntry.Instance.Localizations.Localize(["action", "MutateAllX", "name"]),
				Description = ModEntry.Instance.Localizations.Localize(["action", "MutateAllX", "description"]),
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
        => new(ModEntry.Instance.XMutationIcon.Sprite,null, Colors.action, false);
    public override void Begin(G g, State s, Combat c)
    {
        c.QueueImmediate(new ActuallyMutateAllofThemXBecauseIWasLyingAFewLinesUp());
        c.QueueImmediate(new VisualTransformationX2{timer = 0.8});
        c.QueueImmediate(new VisualTransformationX1{timer = 0.2});
    }
}
public class VisualTransformationX1 : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        foreach(StuffBase stuff in c.stuff.Values.ToArray())
        {
            if(stuff is CellColony cellColony)
                if(!cellColony.MutationX)
                {
                    c.fx.Add(new AnimationTransformX_1{currentCellColony = cellColony});
                }
        }
    }
}
public class VisualTransformationX2 : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        foreach(StuffBase stuff in c.stuff.Values.ToArray())
        {
            if(stuff is CellColony cellColony)
                if(!cellColony.MutationX)
                    c.fx.Add(new AnimationTransformX_2{currentCellColony = cellColony});
        }
    }
}
public class AnimationTransformX_1 : FX
{
    private static ISpriteEntry BtX = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/BtX_0.png"));
    private static ISpriteEntry AtX = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/AtX_0.png"));
    private static ISpriteEntry DtX = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/DtX_0.png"));
    private static ISpriteEntry FtX = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/FtX_0.png"));
    private static ISpriteEntry ADtX = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/ADtX_0.png"));
    private static ISpriteEntry AFtX = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/AFtX_0.png"));
    private static ISpriteEntry DFtX = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/DFtX_0.png"));
    private static ISpriteEntry DAFtX = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/DAFtX_0.png"));
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
        if(!currentCellColony.MutationD && !currentCellColony.MutationF && !currentCellColony.MutationA)
        {
            DrawWithHilight(g, BtX.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(!currentCellColony.MutationD && !currentCellColony.MutationF && currentCellColony.MutationA)
        {
            DrawWithHilight(g, AtX.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationD && !currentCellColony.MutationF && !currentCellColony.MutationA)
        {
            DrawWithHilight(g, DtX.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(!currentCellColony.MutationD && currentCellColony.MutationF && !currentCellColony.MutationA)
        {
            DrawWithHilight(g, FtX.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationD && !currentCellColony.MutationF && currentCellColony.MutationA)
        {
            DrawWithHilight(g, ADtX.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(!currentCellColony.MutationD && currentCellColony.MutationF && currentCellColony.MutationA)
        {
            DrawWithHilight(g, AFtX.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationD && currentCellColony.MutationF && !currentCellColony.MutationA)
        {
            DrawWithHilight(g, DFtX.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationD && currentCellColony.MutationF && currentCellColony.MutationA)
        {
            DrawWithHilight(g, DAFtX.Sprite, v + currentCellColony.GetOffset(g));
        }
    }
}
public class AnimationTransformX_2 : FX
{
    private static ISpriteEntry BtX = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/BtX_1.png"));
    private static ISpriteEntry AtX = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/AtX_1.png"));
    private static ISpriteEntry DtX = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/DtX_1.png"));
    private static ISpriteEntry FtX = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/FtX_1.png"));
    private static ISpriteEntry ADtX = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/ADtX_1.png"));
    private static ISpriteEntry AFtX = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/AFtX_1.png"));
    private static ISpriteEntry DFtX = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/DFtX_1.png"));
    private static ISpriteEntry DAFtX = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/Cells/DAFtX_1.png"));
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
        if(!currentCellColony.MutationA && !currentCellColony.MutationD && !currentCellColony.MutationF)
        {
            DrawWithHilight(g, BtX.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationA && !currentCellColony.MutationD && !currentCellColony.MutationF)
        {
            DrawWithHilight(g, AtX.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(!currentCellColony.MutationA && currentCellColony.MutationD && !currentCellColony.MutationF)
        {
            DrawWithHilight(g, DtX.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(!currentCellColony.MutationA && !currentCellColony.MutationD && currentCellColony.MutationF)
        {
            DrawWithHilight(g, FtX.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationA && currentCellColony.MutationD && !currentCellColony.MutationF)
        {
            DrawWithHilight(g, ADtX.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationA && !currentCellColony.MutationD && currentCellColony.MutationF)
        {
            DrawWithHilight(g, AFtX.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationD && currentCellColony.MutationF && !currentCellColony.MutationA)
        {
            DrawWithHilight(g, DFtX.Sprite, v + currentCellColony.GetOffset(g));
        }
        if(currentCellColony.MutationD && currentCellColony.MutationF && currentCellColony.MutationA)
        {
            DrawWithHilight(g, DAFtX.Sprite, v + currentCellColony.GetOffset(g));
        }
    }
}
public class ActuallyMutateAllofThemXBecauseIWasLyingAFewLinesUp : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        foreach(StuffBase stuff in c.stuff.Values)
        {
            if(stuff is CellColony cellColony)
            {
                if(!cellColony.MutationX)
                {
                    cellColony.MutationX = true;
                }
            }
        }
    }
}