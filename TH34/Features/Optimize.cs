using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using HarmonyLib;

namespace Fred.TH34.features;

internal sealed class AOptimizeManager : IStatusLogicHook
{
    public AOptimizeManager()
    {
        ModEntry.Instance.KokoroApi.RegisterStatusLogicHook(this, 3);
    }
    public void OnStatusTurnTrigger(State state, Combat combat, StatusTurnTriggerTiming timing, Ship ship, Status status, int oldAmount, int newAmount)
    {
        if (timing == StatusTurnTriggerTiming.TurnStart && status == ModEntry.Instance.OptimizeStatus.Status && ship.Get(ModEntry.Instance.MinusChargeStatus.Status) > 0)
        {
            combat.Queue(new AEnergy
            {
                changeAmount = ship.Get(ModEntry.Instance.OptimizeStatus.Status),
            });
        }
    }
}