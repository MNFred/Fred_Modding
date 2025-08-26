using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FredAndRadience.Radiant_Shipyard.Uranus
{
    public class RemnantEjectL : FX
    {
        public int worldX;

        public override void Render(G g, Vec v)
        {
            Draw.Sprite(ModEntry.Instance.Uranus_Remnant_Left.Sprite,(v.x + worldX + 32) + age * 25,(v.y + 110) + age * 160,rotation: age * 15,originPx: new Vec(17, 32.5));
            Draw.Sprite(ModEntry.Instance.Uranus_Remnant_Middle_L.Sprite,v.x + worldX + 16,(v.y + 110) + age * 160,rotation: age * -15 ,originPx: new Vec(8.5, 32.5));
            Draw.Sprite(ModEntry.Instance.Uranus_Remnant_Right.Sprite,(v.x + worldX + 2) - age * 25,(v.y + 110) + age * 160,rotation: age * -15,originPx: new Vec(0, 32.5));
        }
    }
    public class RemnantEjectR : FX
    {
        public int worldX;

        public override void Render(G g, Vec v)
        {
            Draw.Sprite(ModEntry.Instance.Uranus_Remnant_Left.Sprite,(v.x + worldX + 32) + age * 25,(v.y + 110) + age * 160,rotation: age * 15,originPx: new Vec(17, 32.5));
            Draw.Sprite(ModEntry.Instance.Uranus_Remnant_Middle_R.Sprite,v.x + worldX + 16,(v.y + 110) + age * 160,rotation: age * -15 ,originPx: new Vec(8.5, 32.5));
            Draw.Sprite(ModEntry.Instance.Uranus_Remnant_Right.Sprite,(v.x + worldX + 2) - age * 25,(v.y + 110) + age * 160,rotation: age * -15,originPx: new Vec(0, 32.5));
        }
    }
}