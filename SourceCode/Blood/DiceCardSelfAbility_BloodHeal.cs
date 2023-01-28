using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_BloodHeal : DiceCardSelfAbilityBase
    {
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            base.OnUseInstance(unit, self, targetUnit);
            unit.RecoverHP((int)(unit.MaxHp *0.15));
            unit.allyCardDetail.DrawCards(1);
        }
    }
}
