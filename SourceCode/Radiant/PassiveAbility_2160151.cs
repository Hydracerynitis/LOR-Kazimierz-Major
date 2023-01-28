using UnityEngine;
using System.Collections.Generic;

namespace KazimierzMajor
{
    public class PassiveAbility_2160151 : PassiveAbilityBase
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            owner.breakDetail.RecoverBreak(5); //15
        }
    }
}
