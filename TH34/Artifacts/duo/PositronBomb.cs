using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nanoray.Shrike;
using Nanoray.Shrike.Harmony;
using Nickel;

namespace Fred.TH34.Artifacts;
public class ArtifactPositronBomb : Artifact, ITH34Artifact
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        if (ModEntry.Instance.DuoArtifactsApi is not { } api)
			return;
        if (ModEntry.Instance.DynaApi is not { } DynaApi)
            return;
        helper.Content.Artifacts.RegisterArtifact("PositronBomb", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
                pools = [ArtifactPool.Common],
            },
            Sprite = helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/artifacts/PositronBomb.png")).Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "PositronBomb", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "PositronBomb", "description"]).Localize,
        });
        api.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.TH34_Deck.Deck, DynaApi.DynaDeck.Deck]);
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [..StatusMeta.GetTooltips(ModEntry.Instance.PlusChargeStatus.Status,1), ..StatusMeta.GetTooltips(Status.heat,3)];
    }
    public int ModifyBlastwaveDamage(Card? card, State state, bool targetPlayer, int blastwaveIndex)
    {
        if(state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)>0)
        {
            return 1;
        }else return 0;
    }
    public void OnBlastwaveTrigger(State state, Combat combat, Ship ship, int worldX, bool hitMidrow)
    {
        if(state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)==0)
            return;
        combat.QueueImmediate(new AStatus{status = Status.heat, targetPlayer = true, timer = 0, statusAmount = 1});
    }
}