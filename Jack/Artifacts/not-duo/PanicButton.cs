using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Fred.Jack.cards;

namespace Fred.Jack.Artifacts;
public class PanicButton : Artifact, IJackArtifact
{
    private static Spr SpriteOn = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/panicbutton.png")).Sprite;
    private static Spr SpriteOff = ModEntry.Instance.Helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/panicbuttonOff.png")).Sprite;
    public int objectCount;
    public bool activated;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("PanicButton", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.Jack_Deck.Deck,
                pools = [ArtifactPool.Common],
            },
            Sprite = SpriteOn,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "PanicButton", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "PanicButton", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [
            new TTCard{card = new PanicButtonCard()}
        ];
    }
    public override Spr GetSprite()
    {
        if(activated)
            return SpriteOff;
        return SpriteOn;
    }
    public override void OnTurnEnd(State state, Combat combat)
    {
        combat.QueueImmediate(new SeekTarget{timer = 0.1});
    }
    public override void OnTurnStart(State state, Combat combat)
    {
        if(activated)
        {
            activated = false;
            return;
        }
        objectCount = 0;
        foreach(StuffBase stuff in combat.stuff.Values.ToList())
        {
            if(stuff is DualDrone)
            {
                objectCount++;
            }
            if(stuff is AttackDrone attackDrone)
            {
                if(attackDrone.targetPlayer)
                {
                    objectCount++;
                }
            }
            if(stuff is Missile missile)
            {
                if(missile.targetPlayer)
                {
                    objectCount++;
                }
            }
        }
        if(objectCount > 0)
        {
            combat.QueueImmediate(new AAddCard{card = new PanicButtonCard(), destination = CardDestination.Hand, amount = 1});
            Pulse();
            activated = true;
        }
    }
}