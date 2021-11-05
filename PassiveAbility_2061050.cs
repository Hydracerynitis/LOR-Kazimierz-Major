using UnityEngine;
using System.Collections;
using System;

namespace KazimierzMajor
{
    public class PassiveAbility_2061050 : PassiveAbilityBase
    {
        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            if (this.owner.IsBreakLifeZero())
                return false;
            this.owner.breakDetail.RecoverBreak(2);
            return false;
        }
    }
}
