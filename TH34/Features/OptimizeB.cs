using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using HarmonyLib;

namespace Fred.TH34.features;

internal sealed class AOptimizeBManager : IStatusLogicHook
{
    public AOptimizeBManager()
    {
        ModEntry.Instance.KokoroApi.RegisterStatusLogicHook(this, 4);
    }
    public void OnStatusTurnTrigger(State state, Combat combat, StatusTurnTriggerTiming timing, Ship ship, Status status, int oldAmount, int newAmount)
    {
        if (timing == StatusTurnTriggerTiming.TurnStart && status == ModEntry.Instance.OptimizeBStatus.Status && ship.Get(ModEntry.Instance.PlusChargeStatus.Status) > 0)
        {
            combat.Queue(new AEnergy
            {
                changeAmount = ship.Get(ModEntry.Instance.OptimizeBStatus.Status),
            });
        }
    }
}