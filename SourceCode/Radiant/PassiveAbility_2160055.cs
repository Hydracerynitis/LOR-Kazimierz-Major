using UnityEngine;
using System.Collections.Generic;
using LOR_DiceSystem;

namespace KazimierzMajor
{
    public class PassiveAbility_2160055 : PassiveAbilityBase
    {
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            if (behavior.card.card.GetSpec().Ranged != CardRange.Near)
                return;
            behavior._damageReductionByGuard = 0;
            BattleUnitModel target = behavior.card.target;
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmg = target.GetDamageReduction(behavior), breakDmg = target.GetBreakDamageReduction(behavior) });
        }
    }
}
