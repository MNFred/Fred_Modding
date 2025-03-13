using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using HarmonyLib;

namespace Fred.TH34.features;

internal sealed class APlusChargeManager : IStatusLogicHook
{
    public APlusChargeManager()
    {
        ModEntry.Instance.KokoroApi.RegisterStatusLogicHook(this, 1);
        ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.ModifyBaseDamage), (int baseDamage, Card? card, State state, Combat? combat, bool fromPlayer) =>
        {
            if(state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)>0)
            {
                if(fromPlayer == true)
                {
                    return state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status);
                }else return 0;
            }else return 0;
        }, 0);
        ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnPlayerAttack), (State state, Combat combat) =>
        {
            if(state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)>0)
            {
                combat.QueueImmediate(new AStatus{status = Status.heat, targetPlayer = true, statusAmount = state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)});
            }
        }, 0);
    }
}