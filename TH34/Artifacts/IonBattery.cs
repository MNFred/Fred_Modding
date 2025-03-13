using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Xna.Framework.Graphics;
using HarmonyLib;

namespace Fred.TH34.Artifacts;
public class ArtifactIonBattery : Artifact, ITH34Artifact
{
    private bool triggeredThisTurn;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("IonBattery", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.TH34_Deck.Deck,
                pools = [ArtifactPool.Boss],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/IonBattery.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "IonBattery", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "IonBattery", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [
            ..StatusMeta.GetTooltips(ModEntry.Instance.PlusChargeStatus.Status,1),
            ..StatusMeta.GetTooltips(ModEntry.Instance.MinusChargeStatus.Status,1),
        ];
    }
    public override void OnTurnStart(State state, Combat combat)
    {
        triggeredThisTurn = false;
    }
    public override void AfterPlayerStatusAction(State state, Combat combat, Status status, AStatusMode mode, int statusAmount)
    {
        if(status == ModEntry.Instance.PlusChargeStatus.Status && statusAmount != 0)
        {
            if(triggeredThisTurn == false)
            {
                Pulse();
                combat.QueueImmediate(new AEnergy{changeAmount = 1});
                triggeredThisTurn = true;
            }
        }
        if(status == ModEntry.Instance.MinusChargeStatus.Status && statusAmount != 0)
        {
            if(triggeredThisTurn == false)
            {
                Pulse();
                combat.QueueImmediate(new AEnergy{changeAmount = 1});
                triggeredThisTurn = true;
            }
        }
    }
}