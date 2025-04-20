using FSPRO;
using Microsoft.Xna.Framework.Graphics;
using Nickel;
using System;
using System.Collections.Generic;

namespace Fred.Jack.Midrow
{
  public class APRocket : Missile
  {
    public static readonly string MIDROW_OBJECT_NAME = "aprocket";
    public static readonly int BASE_DAMAGE = 2;
    public static readonly Color exhaustColor = new Color("919191");

    static APRocket()
    {
      DB.drones[MIDROW_OBJECT_NAME] = ModEntry.Instance.APRocket_Sprite.Sprite;
    }

    public APRocket() => skin = MIDROW_OBJECT_NAME;

    public override double GetWiggleAmount() => 2.0;

    public override double GetWiggleRate() => 5.0;

    public override string GetDialogueTag() => MIDROW_OBJECT_NAME;

    public override Spr? GetIcon() => new Spr?(ModEntry.Instance.APRocket_Icon.Sprite);

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
      Spr sprite = ModEntry.Instance.APRocket_Sprite.Sprite;
      DrawWithHilight(g, sprite, v1, flipX, targetPlayer);
      Draw.Sprite(nullable1, num1, num2, flag1, flag2, 0.0, new Vec?(), nullable2, new Vec?(), new Rect?(), nullable3);
      Glow.Draw(vec2 + new Vec(0.5, -2.5), 25.0, exhaustColor * new Color(1.0, 0.5, 0.5).gain(0.2 + 0.1 * Math.Sin(g.state.time * 30.0 + x) * 0.5));
    }

    public override List<Tooltip> GetTooltips()
    {
      return [new GlossaryTooltip($"{ModEntry.Instance.Package.Manifest.UniqueName}::Midrow::APMissile")
			{
				Icon = ModEntry.Instance.APRocket_Icon.Sprite,
				TitleColor = Colors.midrow,
				Title = ModEntry.Instance.Localizations.Localize(["midrow", "APMissile", "name"]),
				Description = ModEntry.Instance.Localizations.Localize(["midrow", "APMissile", "description"])
			},new TTGlossary("parttrait.armor")];
    }

    public override List<CardAction>? GetActions(State s, Combat c)
    {
      return new List<CardAction>
      {
        new APiercingMissileHit{
          worldX = x,
          outgoingDamage = BASE_DAMAGE,
          targetPlayer = targetPlayer
        }
      };
    }
  }
}
public class APiercingMissileHit : AMissileHit
  {
    public override void Update(G g, State s, Combat c)
    {
        c.stuff.TryGetValue(worldX, out StuffBase? value);
        if (!(value is Missile missile))
        {
            timer -= g.dt;
            return;
        }

        Ship ship = targetPlayer ? s.ship : c.otherShip;
        if (ship == null)
        {
          return;
        }

        RaycastResult raycastResult = CombatUtils.RaycastGlobal(c, ship, fromDrone: true, worldX);
        bool flag = false;
        if (raycastResult.hitShip)
        {
            Part? partAtWorldX = ship.GetPartAtWorldX(raycastResult.worldX);
            if (partAtWorldX == null || partAtWorldX.type != PType.empty)
            {
                flag = true;
            }
        }

        if (Missile.missileData[missile.missileType].seeking)
        {
            raycastResult.worldX = missile.GetSeekerImpact(s, c);
            Part partAtWorldX2 = ship.GetPartAtWorldX(raycastResult.worldX)!;
            flag = partAtWorldX2 != null && partAtWorldX2.type != PType.empty;
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
            int num = outgoingDamage;
            foreach (Artifact item in s.EnumerateAllArtifacts())
            {
                num += item.ModifyBaseMissileDamage(s, s.route as Combat, targetPlayer);
            }

            if (num < 0)
            {
                num = 0;
            }

            DamageDone dmg = ship.NormalDamage(s, c, num, raycastResult.worldX, true);
            EffectSpawner.NonCannonHit(g, targetPlayer, raycastResult, dmg);
            if (xPush != 0)
            {
                c.QueueImmediate(new AMove
                {
                    targetPlayer = targetPlayer,
                    dir = xPush
                });
            }

            Part? partAtWorldX3 = ship.GetPartAtWorldX(raycastResult.worldX);
            if (partAtWorldX3 != null && partAtWorldX3.stunModifier == PStunMod.stunnable)
            {
                c.QueueImmediate(new AStunPart
                {
                    worldX = raycastResult.worldX
                });
            }
            if(partAtWorldX3 != null && partAtWorldX3.damageModifier == PDamMod.armor)
            {
              partAtWorldX3.damageModifier = PDamMod.none;
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
        }

        c.stuff.Remove(worldX);
        if (!(raycastResult.hitDrone || flag))
        {
            c.stuffOutro.Add(missile);
        }
    }
  }