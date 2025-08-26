using FMOD;
using FSPRO;

namespace FredAndRadience.Radiant_Shipyard.actions;
public class MercuryPartReveal : CardAction
{
    public string ?randomKey = null;
    public override void Begin(G g, State s, Combat c)
    {
        foreach(Part part in s.ship.parts)
        {
            if(part.key == randomKey)
            {
                Audio.Play(new GUID?(Event.TogglePart));
                part.active = true;
            }
        }
    }
}