
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Fred.Jack.Midrow
{
  public class DOME : StuffBase
  {
    public static readonly string MIDROW_OBJECT_NAME = "domeMid";
    public int counter = 1;

    public override void Render(G g, Vec v)
    {
      if (this.counter == 1)
        DrawWithHilight(g, ModEntry.Instance.DOME_Sprite1.Sprite, v + GetOffset(g));
      else if (this.counter == 2)
        DrawWithHilight(g, ModEntry.Instance.DOME_Sprite2.Sprite, v + GetOffset(g));
      else
          DrawWithHilight(g, ModEntry.Instance.DOME_Sprite3.Sprite, v + GetOffset(g));
    }

    public override Spr? GetIcon() => new Spr?(ModEntry.Instance.DOME_Icon.Sprite);

    public override List<Tooltip> GetTooltips()
    {
      return [new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::Midrow::DOME")
			{
				Icon = ModEntry.Instance.DOME_Icon.Sprite,
				TitleColor = Colors.midrow,
				Title = ModEntry.Instance.Localizations.Localize(["midrow", "Dome", "name"]),
				Description = ModEntry.Instance.Localizations.Localize(["midrow", "Dome", "description"])
			}];
    }

    public override List<CardAction>? GetActionsOnDestroyed(State s, Combat c, bool wasPlayer, int worldX)
    {
      if (this.counter >= 3)
      {
        foreach (StuffBase stuffBase in c.stuff.Values.ToList())
        {
          if (stuffBase.x != worldX && stuffBase != null && stuffBase is not DOME)
            c.DestroyDroneAt(s, stuffBase.x, false);
        }
        return null;
      }
      if (this.counter < 3)
        foreach (StuffBase stuffBase in c.stuff.Values.ToList())
        {
          if (stuffBase.x != worldX && stuffBase != null && stuffBase is not DOME)
            stuffBase.bubbleShield = true;
        }
      return null;
    }

    public override List<CardAction>? GetActions(State s, Combat c)
    {
      this.counter++;
      return null;
    }
  }
}
