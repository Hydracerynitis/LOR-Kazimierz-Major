using UnityEngine;
using System.Collections;
using HarmonyLib;
using System;
using LOR_DiceSystem;
using static UnityEngine.EventSystems.EventTrigger;
using System.Collections.Generic;

namespace KazimierzMajor
{
    public class DiceCardAbility_DoubleHit : DiceCardAbilityBase
    {
        private List<BattleUnitModel> targets = new List<BattleUnitModel>();
        public override void OnSucceedAreaAttack(BattleUnitModel target)
        {
            base.OnSucceedAreaAttack(target);
            if (targets.Exists(x => x == target) || !target.bufListDetail.HasBuf<DiceCardAbility_DoubleHitIndicator.Indicator>())
                return;
            targets.Add(target);
            BattleVoidBehaviour.ExtraHit(target, behavior, 0);
        }
    }
    public class DiceCardAbility_DoubleHitIndicator : DiceCardAbilityBase
    {
        private List<BattleUnitModel> targets = new List<BattleUnitModel>();
        public override void OnSucceedAreaAttack(BattleUnitModel target)
        {
            target.bufListDetail.AddBuf(new Indicator());
        }
        public class Indicator : BattleUnitBuf
        {
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
    }
}
