using UnityEngine;
using System.Collections;
using HarmonyLib;
using System;
using EmotionalFix;
using LOR_DiceSystem;
using static UnityEngine.EventSystems.EventTrigger;

namespace KazimierzMajor
{
    public class DiceCardAbility_TripleHit : DiceCardAbilityBase
    {
        private bool ExtraHit = false;
        private int DamageReductionByGuard = 0;
        public override void BeforeGiveDamage()
        {
            base.BeforeGiveDamage();
            if (ExtraHit)
                return;
            DamageReductionByGuard = behavior._damageReductionByGuard;
        }
        public override void OnSucceedAttack()
        {
            base.OnSucceedAttack();
            if (ExtraHit)
                return;
            BattleUnitModel target = behavior.card.target;
            ExtraHit = true;
            BattleVoidBehaviour.ExtraHit(target, behavior, DamageReductionByGuard);
            BattleVoidBehaviour.ExtraHit(target, behavior, DamageReductionByGuard);
        }
    }
}
