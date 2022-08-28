using UnityEngine;
using System.Collections.Generic;

namespace KazimierzMajor
{
    public class PassiveAbility_2160051 : PassiveAbilityBase
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            owner.breakDetail.RecoverBreak(20); //15
        }
        public override void OnTakeBreakDamageByAttack(BattleDiceBehavior atkDice, int breakdmg)
        {
            base.OnTakeBreakDamageByAttack(atkDice, breakdmg);
            //owner.breakDetail.RecoverBreak(10);
        }
        public override bool DontChangeResistByBreak()
        {
            return true;
        }
    }
}
