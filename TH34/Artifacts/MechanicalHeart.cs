using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Xna.Framework.Graphics;
using HarmonyLib;

namespace Fred.TH34.Artifacts;
public class ArtifactMechanicalHeart : Artifact, ITH34Artifact
{
    private bool activated;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("MechanicalHeart", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.TH34_Deck.Deck,
                pools = [ArtifactPool.Boss],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/MechanicalHeart.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "MechanicalHeart", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "MechanicalHeart", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [
            ..StatusMeta.GetTooltips(ModEntry.Instance.PlusChargeStatus.Status,1),
            ..StatusMeta.GetTooltips(ModEntry.Instance.MinusChargeStatus.Status,1),
        ];
    }
    public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount)
    {
        activated = false;
    }
    public override void AfterPlayerStatusAction(State state, Combat combat, Status status, AStatusMode mode, int statusAmount)
    {
        if(status == ModEntry.Instance.PlusChargeStatus.Status && statusAmount != 0 && activated == false)
        {
            Pulse();
            combat.QueueImmediate(new AStatus{status = ModEntry.Instance.PlusChargeStatus.Status, statusAmount = 1, targetPlayer = true});
            activated = true;
            return;
        }
        if(status == ModEntry.Instance.MinusChargeStatus.Status && statusAmount != 0 && activated == false)
        {
            Pulse();
            combat.QueueImmediate(new AStatus{status = ModEntry.Instance.MinusChargeStatus.Status, statusAmount = 1, targetPlayer = true});
            activated = true;
            return;
        }
    }
}