using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Xna.Framework.Graphics;
using HarmonyLib;

namespace Fred.TH34.Artifacts;
public class ArtifactElectronDonor : Artifact, ITH34Artifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("ElectronDonor", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.TH34_Deck.Deck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/ElectronDonor.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ElectronDonor", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ElectronDonor", "description"]).Localize,
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [..StatusMeta.GetTooltips(ModEntry.Instance.MinusChargeStatus.Status,1)];
    }
    public override int ModifyBaseDamage(int baseDamage, Card? card, State state, Combat? combat, bool fromPlayer)
    {
        if(state.ship.Get(ModEntry.Instance.MinusChargeStatus.Status)>0)
        {
            if(fromPlayer == true)
            {
                if(state.ship.Get(Status.heat)<-2)
                    return state.ship.Get(ModEntry.Instance.MinusChargeStatus.Status);
                if(state.ship.Get(Status.heat)>=-2)
                    return 0;
                else return 0;
            }else return 0;
        }else return 0;
    }
}