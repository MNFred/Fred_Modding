using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Dougie.Midrow
{
  public class CellColony : AttackDrone
  {
    public static readonly string MIDROW_OBJECT_NAME = "cellColonyName";
    public bool MutationX = false;
    public bool MutationD = false;
    public bool MutationF = false;
    public bool MutationA = false;

    public override void Render(G g, Vec v)
    {
        //No Mutations
        if (!this.MutationA && !this.MutationD && !this.MutationF && !this.MutationX)
            DrawWithHilight(g, ModEntry.Instance.Cell_Blank_Sprite.Sprite, v + GetOffset(g));
        //single mutations
        else if (this.MutationA && !this.MutationD && !this.MutationF && !this.MutationX)
            DrawWithHilight(g, ModEntry.Instance.Cell_A_Sprite.Sprite, v + GetOffset(g));
        else if (!this.MutationA && this.MutationD && !this.MutationF && !this.MutationX)
            DrawWithHilight(g, ModEntry.Instance.Cell_D_Sprite.Sprite, v + GetOffset(g));
        else if (!this.MutationA && !this.MutationD && this.MutationF && !this.MutationX)
            DrawWithHilight(g, ModEntry.Instance.Cell_F_Sprite.Sprite, v + GetOffset(g));
        else if (!this.MutationA && !this.MutationD && !this.MutationF && this.MutationX)
            DrawWithHilight(g, ModEntry.Instance.Cell_X_Sprite.Sprite, v + GetOffset(g));
        //double mutations
        else if (this.MutationA && this.MutationD && !this.MutationF && !this.MutationX)
            DrawWithHilight(g, ModEntry.Instance.Cell_DA_Sprite.Sprite, v + GetOffset(g));
        else if (!this.MutationA && this.MutationD && this.MutationF && !this.MutationX)
            DrawWithHilight(g, ModEntry.Instance.Cell_DF_Sprite.Sprite, v + GetOffset(g));
        else if (this.MutationA && !this.MutationD && this.MutationF && !this.MutationX)
            DrawWithHilight(g, ModEntry.Instance.Cell_FA_Sprite.Sprite, v + GetOffset(g));
        else if (this.MutationA && !this.MutationD && !this.MutationF && this.MutationX)
            DrawWithHilight(g, ModEntry.Instance.Cell_XA_Sprite.Sprite, v + GetOffset(g));
        else if (!this.MutationA && this.MutationD && !this.MutationF && this.MutationX)
            DrawWithHilight(g, ModEntry.Instance.Cell_XD_Sprite.Sprite, v + GetOffset(g));
        else if (!this.MutationA && !this.MutationD && this.MutationF && this.MutationX)
            DrawWithHilight(g, ModEntry.Instance.Cell_XF_Sprite.Sprite, v + GetOffset(g));
        //triple mutations
        else if (this.MutationA && this.MutationD && this.MutationF && !this.MutationX)
            DrawWithHilight(g, ModEntry.Instance.Cell_DFA_Sprite.Sprite, v + GetOffset(g));
        else if (this.MutationA && this.MutationD && !this.MutationF && this.MutationX)
            DrawWithHilight(g, ModEntry.Instance.Cell_XDA_Sprite.Sprite, v + GetOffset(g));
        else if (!this.MutationA && this.MutationD && this.MutationF && this.MutationX)
            DrawWithHilight(g, ModEntry.Instance.Cell_XDF_Sprite.Sprite, v + GetOffset(g));
        else if (this.MutationA && !this.MutationD && this.MutationF && this.MutationX)
            DrawWithHilight(g, ModEntry.Instance.Cell_XFA_Sprite.Sprite, v + GetOffset(g));
        //all mutations
        else if (this.MutationA && this.MutationD && this.MutationF && this.MutationX)
            DrawWithHilight(g, ModEntry.Instance.Cell_XDFA_Sprite.Sprite, v + GetOffset(g));
    }
    public override double GetWiggleAmount() => 0.8;

    public override double GetWiggleRate() => 1.0;

    public override Spr? GetIcon() => new Spr?(ModEntry.Instance.CellColonyIcon.Sprite);

    public override List<Tooltip> GetTooltips()
    {
        List<Tooltip> tooltip =
        [
            new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::Midrow::Cell")
                {
                    Icon = ModEntry.Instance.CellColonyIcon.Sprite,
                    TitleColor = Colors.midrow,
                    Title = ModEntry.Instance.Localizations.Localize(["midrow", "Cell", "name"]),
                    Description = string.Format("Will block 1 shot before being destroyed. \n{0} {1} {2} {3}", !this.MutationA ? "" : "<c=keyword>A</c>: Shoots a 1 damage shot each turn.", !this.MutationF ? "" : "\n<c=keyword>F</c>: Can be harvested 1 more time before being destroyed.", !this.MutationD ? "" : "\n<c=keyword>D</c>: Beams 1 <c=status>SHIELD</c> at it's target once per turn.", !this.MutationX ? "" : "\n<c=keyword>X</c>: Shoots a 2 damage shot each turn.")
                },
        ];
        return tooltip;
    }
    public override List<CardAction>? GetActions(State s, Combat c)
    {
        List<CardAction> actions = new List<CardAction>();
        if(this.MutationA)
        {
            actions.Add(new AAttack{damage = 1, targetPlayer = this.targetPlayer, fromDroneX = this.x});
        }
        if(this.MutationX)
        {
            actions.Add(new AAttack{damage = 2, targetPlayer = this.targetPlayer, fromDroneX = this.x});
        }
        if(this.MutationD)
        {
            actions.Add(new AShiftTargetTemporary{x = this.x, toWhat = true});
            actions.Add(new AAttack{damage = 0, targetPlayer = !this.targetPlayer, fromDroneX = this.x, isBeam = true, status = Status.shield, statusAmount = 1});
            actions.Add(new AShiftTargetTemporary{x = this.x, toWhat = false});
        }
      return actions;
    }
  }
}
public class AShiftTargetTemporary : CardAction
{
    public int x;
    public bool toWhat;

    public override void Begin(G g, State s, Combat c)
    {
        timer = 0.0;
        if (c.stuff.TryGetValue(x, out StuffBase? value))
        {
            value.targetPlayer = toWhat;
        }
    }
}