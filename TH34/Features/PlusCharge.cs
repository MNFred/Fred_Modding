using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Fred.TH34.Artifacts;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nickel;

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
                if(state.artifacts.Any((x) => x is ArtifactGolemancy))
                {
                    int shardAmount = state.ship.Get(Status.shard);
                    int chargeAmount = state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status);
                    int shardsLeft = 0;
                    if(state.artifacts.Any((x) => x is ArtifactSilverLining))
                    {
                        if(state.ship.hull <= 3)
                        {
                            if(shardAmount>0)
                            {
                            chargeAmount-=1;
                            shardsLeft = shardAmount-chargeAmount;
                            combat.QueueImmediate(new AStatus{status = Status.shard, targetPlayer = true, statusAmount = -state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)+1});
                            if(shardsLeft < 0)
                            {
                                combat.QueueImmediate(new AStatus{status = Status.heat, targetPlayer = true, statusAmount = shardsLeft*-1});
                            }
                            }else{
                                combat.QueueImmediate(new AStatus{status = Status.heat, targetPlayer = true, statusAmount = state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)-1});
                            }
                        }else{
                            if(shardAmount>0)
                            {
                            shardsLeft = shardAmount-chargeAmount;
                            combat.QueueImmediate(new AStatus{status = Status.shard, targetPlayer = true, statusAmount = -state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)});
                            if(shardsLeft < 0)
                            {
                                combat.QueueImmediate(new AStatus{status = Status.heat, targetPlayer = true, statusAmount = shardsLeft*-1});
                            }
                            }else{
                                combat.QueueImmediate(new AStatus{status = Status.heat, targetPlayer = true, statusAmount = state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)});
                            }
                        }
                    }else{
                        if(shardAmount>0)
                        {
                            shardsLeft = shardAmount-chargeAmount;
                            combat.QueueImmediate(new AStatus{status = Status.shard, targetPlayer = true, statusAmount = -state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)});
                            if(shardsLeft < 0)
                            {
                                combat.QueueImmediate(new AStatus{status = Status.heat, targetPlayer = true, statusAmount = shardsLeft*-1});
                            }
                        }else{
                            combat.QueueImmediate(new AStatus{status = Status.heat, targetPlayer = true, statusAmount = state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)});
                        }
                    }
                }else{
                    if(state.artifacts.Any((x) => x is ArtifactSilverLining))
                    {
                        if(state.ship.hull <= 3)
                        {
                            combat.QueueImmediate(new AStatus{status = Status.heat, targetPlayer = true, statusAmount = state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)-1});
                        }else{
                            combat.QueueImmediate(new AStatus{status = Status.heat, targetPlayer = true, statusAmount = state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)});
                        }
                    }else{
                        combat.QueueImmediate(new AStatus{status = Status.heat, targetPlayer = true, statusAmount = state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)});
                    }
                }
            }
        }, 0);
        ModEntry.Instance.Helper.Events.RegisterBeforeArtifactsHook(nameof(Artifact.OnPlayerPlayCard), (int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount) =>
        {
            bool stopFromHeating = false;
            bool hasXValue = false;
            foreach(CardAction action in card.GetActions(state, combat))
            {
                if(action is AAttack)
                {
                    stopFromHeating = true;
                }
                if(action is AVariableHint)
                {
                    hasXValue = true;
                }
            }
            if(hasXValue == true && stopFromHeating == false)
            {
                if(state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)>0)
                {
                if(state.artifacts.Any((x) => x is ArtifactGolemancy))
                {
                    int shardAmount = state.ship.Get(Status.shard);
                    int chargeAmount = state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status);
                    int shardsLeft = 0;
                    if(state.artifacts.Any((x) => x is ArtifactSilverLining))
                    {
                        if(state.ship.hull <= 3)
                        {
                            if(shardAmount>0)
                            {
                            chargeAmount-=1;
                            shardsLeft = shardAmount-chargeAmount;
                            combat.QueueImmediate(new AStatus{status = Status.shard, targetPlayer = true, statusAmount = -state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)+1});
                            if(shardsLeft < 0)
                            {
                                combat.QueueImmediate(new AStatus{status = Status.heat, targetPlayer = true, statusAmount = shardsLeft*-1});
                            }
                            }else{
                                combat.QueueImmediate(new AStatus{status = Status.heat, targetPlayer = true, statusAmount = state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)-1});
                            }
                        }else{
                            if(shardAmount>0)
                            {
                            shardsLeft = shardAmount-chargeAmount;
                            combat.QueueImmediate(new AStatus{status = Status.shard, targetPlayer = true, statusAmount = -state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)});
                            if(shardsLeft < 0)
                            {
                                combat.QueueImmediate(new AStatus{status = Status.heat, targetPlayer = true, statusAmount = shardsLeft*-1});
                            }
                            }else{
                                combat.QueueImmediate(new AStatus{status = Status.heat, targetPlayer = true, statusAmount = state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)});
                            }
                        }
                    }else{
                        if(shardAmount>0)
                        {
                            shardsLeft = shardAmount-chargeAmount;
                            combat.QueueImmediate(new AStatus{status = Status.shard, targetPlayer = true, statusAmount = -state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)});
                            if(shardsLeft < 0)
                            {
                                combat.QueueImmediate(new AStatus{status = Status.heat, targetPlayer = true, statusAmount = shardsLeft*-1});
                            }
                        }else{
                            combat.QueueImmediate(new AStatus{status = Status.heat, targetPlayer = true, statusAmount = state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)});
                        }
                    }
                }else{
                    if(state.artifacts.Any((x) => x is ArtifactSilverLining))
                    {
                        if(state.ship.hull <= 3)
                        {
                            combat.QueueImmediate(new AStatus{status = Status.heat, targetPlayer = true, statusAmount = state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)-1});
                        }else{
                            combat.QueueImmediate(new AStatus{status = Status.heat, targetPlayer = true, statusAmount = state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)});
                        }
                    }else{
                        combat.QueueImmediate(new AStatus{status = Status.heat, targetPlayer = true, statusAmount = state.ship.Get(ModEntry.Instance.PlusChargeStatus.Status)});
                    }
                }
                }
            }
        }, 0);
    }
}