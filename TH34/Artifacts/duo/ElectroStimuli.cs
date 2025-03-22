using Nickel;
using Nanoray.PluginManager;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Xna.Framework.Graphics;
using HarmonyLib;

namespace Fred.TH34.Artifacts;
public class ArtifactElectroStimuli : Artifact, ITH34Artifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        if (ModEntry.Instance.DuoArtifactsApi is not { } api)
			return;
        if (ModEntry.Instance.TySashaApi is not { } TyApi)
			return;
        helper.Content.Artifacts.RegisterArtifact("ElectroStimuli", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/ElectroStimuli.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ElectroStimuli", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "ElectroStimuli", "description"]).Localize,
        });
        api.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.TH34_Deck.Deck, TyApi.TyDeck]);
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [..StatusMeta.GetTooltips(ModEntry.Instance.PlusChargeStatus.Status,1), ..StatusMeta.GetTooltips(Status.heat,3)];
    }
    public int AffectX(ITyAndSashaApi.IHook.IAffectXArgs args)
    {
        if(args.State.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)>0)
        {
            return args.State.ship.Get(ModEntry.Instance.PlusChargeStatus.Status);
        }else{
            return 0;
        }
    }
}