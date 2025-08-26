
using FMOD;
using FSPRO;

namespace FredAndRadience.Radiant_Shipyard.actions;
public class VenusSwap : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        ArmoredBay? artifact = s.artifacts.Find((x) => x is ArmoredBay) as ArmoredBay;
        GlassCannon? artifact2 = s.artifacts.Find((x) => x is GlassCannon) as GlassCannon;
            for(int i = 0; i < s.ship.parts.Count; i++ )
            {
                if(s.ship.parts[i].key == "LeftPart")
                {
                    Audio.Play(new GUID?(Event.TogglePart));
                    if(s.ship.parts[i].type == PType.cannon)
                    {
                        s.ship.parts[i] = new Part()
                        {
                            type = PType.missiles,
                            skin = ModEntry.Instance.VenusMissiles.UniqueName,
                            damageModifier = artifact == null ? PDamMod.none : PDamMod.armor,
                            key = "LeftPart"
                        };
                        continue;
                    }
                    if(s.ship.parts[i].type == PType.missiles)
                    {
                        s.ship.parts[i] = new Part()
                        {
                            type = PType.cannon,
                            skin = ModEntry.Instance.VenusCannon.UniqueName,
                            damageModifier = artifact2 == null ? PDamMod.none : PDamMod.weak,
                            key = "LeftPart"
                        };
                        continue;
                    }
                }
                if(s.ship.parts[i].key == "RightPart")
                {
                    Audio.Play(new GUID?(Event.TogglePart));
                    if(s.ship.parts[i].type == PType.cannon)
                    {
                        s.ship.parts[i] = new Part()
                        {
                            type = PType.missiles,
                            skin = ModEntry.Instance.VenusMissiles.UniqueName,
                            damageModifier = artifact == null ? PDamMod.none : PDamMod.armor,
                            key = "RightPart"
                        };
                        continue;
                    }
                    if(s.ship.parts[i].type == PType.missiles)
                    {
                        s.ship.parts[i] = new Part()
                        {
                            type = PType.cannon,
                            skin = ModEntry.Instance.VenusCannon.UniqueName,
                            damageModifier = artifact2 == null ? PDamMod.none : PDamMod.weak,
                            key = "RightPart"
                        };
                        continue;
                    }
                }
            }
    }
}