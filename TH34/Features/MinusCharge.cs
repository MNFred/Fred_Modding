using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Fred.TH34.Artifacts;
using FSPRO;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.Shrike;
using Nanoray.Shrike.Harmony;

namespace Fred.TH34.features;

internal sealed class AMinusChargeManager : IStatusLogicHook
{
    public AMinusChargeManager()
    {
        ModEntry.Instance.KokoroApi.RegisterStatusLogicHook(this, 0);
        ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.ModifyBaseDamage), (int baseDamage, Card? card, State state, Combat? combat, bool fromPlayer) =>
        {
            if(state.ship.Get(ModEntry.Instance.MinusChargeStatus.Status)>0)
            {
                if(fromPlayer == true)
                {
                    return -state.ship.Get(ModEntry.Instance.MinusChargeStatus.Status);
                }else return 0;
            }else return 0;
        }, 0);
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Ship), nameof(Ship.Set)),
            transpiler: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Ship_SetHeat_Transpiler))
        );
        ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnPlayerPlayCard), (int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount) =>
        {
            if(state.ship.Get(ModEntry.Instance.MinusChargeStatus.Status)>0)
            {
                int heatAmount = state.ship.Get(Status.heat);
                int chargeAmount = state.ship.Get(ModEntry.Instance.MinusChargeStatus.Status);
                int heatLeft = 0;
                if(state.artifacts.Any((x) => x is ArtifactGolemancy))
                {
                    state.ship.heatMin = -99;
                    state.ship.Add(Status.heat, -state.ship.Get(ModEntry.Instance.MinusChargeStatus.Status));
                    if(state.ship.Get(Status.heat)<=-3)
                    {
                        heatLeft = state.ship.Get(Status.heat)+3;
                        state.ship.Set(Status.heat,-3);
                        if(state.EnumerateAllArtifacts().FirstOrDefault(x => x is SubzeroHeatsinks) is { } artifact)
                            state.ship.heatMin = -3;
                        else state.ship.heatMin = -0;
                    }
                    if(heatLeft < 0)
                        combat.QueueImmediate(new AStatus{status = Status.shard, targetPlayer = true, statusAmount = Math.Abs(heatLeft),timer = 0});
                }else{
                    SubzeroHeatsinks? artifact = state.artifacts.Find((x) => x is SubzeroHeatsinks) as SubzeroHeatsinks;
                    state.ship.heatMin = -3;
                    state.ship.Add(Status.heat, -state.ship.Get(ModEntry.Instance.MinusChargeStatus.Status));
                    if(artifact == null)
                    {
                        state.ship.heatMin = 0;
                    }else{
                        return;
                    }
                }
            }
        }, 0);
    }
    private static IEnumerable<CodeInstruction> Ship_SetHeat_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        try
        {
            return new SequenceBlockMatcher<CodeInstruction>(instructions)
                .Find(
                    ILMatches.Ldfld("heatMin")
                )
                .Insert(
                    SequenceMatcherPastBoundsDirection.After, SequenceMatcherInsertionResultingBounds.JustInsertion,
                    [
                        new(OpCodes.Ldarg_0),
                        new(OpCodes.Ldc_I4, (int) Status.heat),
                        new(OpCodes.Call, AccessTools.DeclaredMethod(typeof(Ship), "Get")),
                        new(OpCodes.Call, AccessTools.DeclaredMethod(typeof(Math), "Min", [typeof(int), typeof(int)]))
                    ]
                )
                .AllElements();
        }
        catch (Exception e)
        {
            Console.WriteLine("Minus Charge Ship.Set patch failed!");
            Console.WriteLine(e);
            return instructions;
        }
    }
}