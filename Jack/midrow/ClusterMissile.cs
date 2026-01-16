using Fred.Jack.Midrow;
using HarmonyLib;
using Microsoft.Xna.Framework.Graphics;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fred.Jack.Midrow
{
  public class ClusterMissile : Missile
  {
    public static readonly string MIDROW_OBJECT_NAME = "clusterrocket";
    public static readonly int BASE_DAMAGE = 3;
    public static readonly Color exhaustColor = new Color("919191");

    static ClusterMissile()
    {
      DB.drones[MIDROW_OBJECT_NAME] = ModEntry.Instance.ClusterRocket_Sprite.Sprite;
      ModEntry.Instance.Harmony.Patch(
			  original: AccessTools.DeclaredMethod(typeof(Combat), nameof(Combat.DestroyDroneAt)),
			  prefix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Combat_DestroyDroneAt_Prefix))
		  );
    }

    public ClusterMissile() => skin = MIDROW_OBJECT_NAME;

    public override double GetWiggleAmount() => 2.0;

    public override double GetWiggleRate() => 5.0;

    public override string GetDialogueTag() => MIDROW_OBJECT_NAME;

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
      Spr sprite = ModEntry.Instance.ClusterRocket_Sprite.Sprite;
      DrawWithHilight(g, sprite, v1, flipX, targetPlayer);
      Draw.Sprite(nullable1, num1, num2, flag1, flag2, 0.0, new Vec?(), nullable2, new Vec?(), new Rect?(), nullable3);
      Glow.Draw(vec2 + new Vec(0.5, -2.5), 25.0, exhaustColor * new Color(1.0, 0.5, 0.5).gain(0.2 + 0.1 * Math.Sin(g.state.time * 30.0 + x) * 0.5));
    }

    public override Spr? GetIcon() => new Spr?(ModEntry.Instance.ClusterRocket_Icon.Sprite);

    public override List<Tooltip> GetTooltips()
    {
      return [new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::Midrow::ClusterMissile")
			{
				Icon = ModEntry.Instance.ClusterRocket_Icon.Sprite,
				TitleColor = Colors.midrow,
				Title = ModEntry.Instance.Localizations.Localize(["midrow", "ClusterMissile", "name"]),
				Description = ModEntry.Instance.Localizations.Localize(["midrow", "ClusterMissile", "description"])
			},
      new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::Midrow::MiniMissile")
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
    private static void Combat_DestroyDroneAt_Prefix(State s, int x, bool playerDidIt)
    {
      Route route = s.route;
      Combat? combat = route as Combat;
      if (combat != null)
      {
        StuffBase? stuff = combat.stuff.GetValueOrDefault(x);
        if(stuff != null && stuff is ClusterMissile)
        {
          combat.QueueImmediate(new APlaceMissile{timer = 0.0, X = stuff.x, targettingUs = stuff.targetPlayer, XL = stuff.xLerped});
          combat.QueueImmediate(new MoveObstructions{X = stuff.x, timer = 0.0});
        }
      }
    }
  }
}
public class APlaceMissile : CardAction
  {
    public int X;
    public double XL;
    public bool targettingUs;
    public override void Begin(G g, State s, Combat c)
    {
      StuffBase? somethingHereM = c.stuff.GetValueOrDefault(X);
      MiniMissile mini = new MiniMissile{x = X-1, xLerped = XL - 1.0, targetPlayer = targettingUs};
      MiniMissile mini2 = new MiniMissile{x = X, xLerped = XL, targetPlayer = targettingUs};
      MiniMissile mini3 = new MiniMissile{x = X+1, xLerped = XL + 1.0, targetPlayer = targettingUs};
      c.stuff[X-1] = mini;
      if(somethingHereM == null)
      {
        c.stuff[X] = mini2;
      }
      c.stuff[X+1] = mini3;
    }
  }
public class MoveObstructions : CardAction
{
  public int X;
    public override void Begin(G g, State s, Combat c)
    {
      StuffBase? somethingHereL = c.stuff.GetValueOrDefault(X-1);
      StuffBase? somethingHereR = c.stuff.GetValueOrDefault(X+1);
      if(somethingHereL != null)
      {
        ADroneMove.DoMoveSingleDrone(s,c,X-1,-1,true);
      }
      if(somethingHereR != null)
      {
        ADroneMove.DoMoveSingleDrone(s,c,X+1,1,true);
      }
      return;
    }
}
