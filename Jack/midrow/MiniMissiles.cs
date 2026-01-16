using Microsoft.Xna.Framework.Graphics;
using Nickel;
using System;
using System.Collections.Generic;

namespace Fred.Jack.Midrow
{
  public class MiniMissile : Missile
  {
    public static readonly string MIDROW_OBJECT_NAME = "minimissile";
    public static readonly int BASE_DAMAGE = 1;
    public static readonly Color exhaustColor = new Color("919191");

    static MiniMissile()
    {
      DB.drones[MIDROW_OBJECT_NAME] = ModEntry.Instance.MiniMissile.Sprite;
    }

    public MiniMissile() => this.skin = MIDROW_OBJECT_NAME;

    public override double GetWiggleAmount() => 2.0;

    public override double GetWiggleRate() => 5.0;

    public override string GetDialogueTag() => MIDROW_OBJECT_NAME;

    public override Spr? GetIcon() => new Spr?(ModEntry.Instance.MiniMissile_Icon.Sprite);

    public override void Render(G g, Vec v)
    {
      Vec offset = GetOffset(g, true);
      Vec v1 = v + offset;
      Vec vec1 = new Vec();
      if (!this.targetPlayer)
        vec1 += new Vec(y: 21.0);
      Vec vec2 = v1 + vec1 + new Vec(7.0, 8.0);
      bool flipX = false;
      Spr? nullable1 = new Spr?(exhaustSprites.GetModulo((int) (g.state.time * 36.0 + x * 10)));
      double num1 = vec2.x - 5.0;
      double num2 = vec2.y + (!this.targetPlayer ? 14.0 : 0.0);
      Vec? nullable2 = new Vec?(new Vec(y: 1.0));
      bool targetPlayer = this.targetPlayer;
      bool flag1 = flipX;
      bool flag2 = !targetPlayer;
      Color? nullable3 = new Color?(exhaustColor);
      Spr sprite = ModEntry.Instance.MiniMissile.Sprite;
      DrawWithHilight(g, sprite, v1, flipX, targetPlayer);
      Draw.Sprite(nullable1, num1, num2, flag1, flag2, 0.0, new Vec?(), nullable2, new Vec?(), new Rect?(), nullable3);
      Glow.Draw(vec2 + new Vec(0.5, -2.5), 25.0, exhaustColor * new Color(1.0, 0.5, 0.5).gain(0.2 + 0.1 * Math.Sin(g.state.time * 30.0 + x) * 0.5));
    }

    public override List<Tooltip> GetTooltips()
    {
      return [new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::Midrow::MiniMissile")
			{
				Icon = ModEntry.Instance.MiniMissile_Icon.Sprite,
				TitleColor = Colors.midrow,
				Title = ModEntry.Instance.Localizations.Localize(["midrow", "MiniMissile", "name"]),
				Description = ModEntry.Instance.Localizations.Localize(["midrow", "MiniMissile", "description"])
			}];
    }

    public override List<CardAction>? GetActions(State s, Combat c)
    {
      return new List<CardAction>()
      {
        new AMissileHit()
        {
          worldX = x,
          outgoingDamage = BASE_DAMAGE,
        }
      };
    }
  }
}
