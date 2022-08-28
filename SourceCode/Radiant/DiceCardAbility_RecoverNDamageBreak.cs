using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class DiceCardAbility_RecoverNDamageBreak : DiceCardAbilityBase
    {
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            base.OnSucceedAttack(target);
            owner.breakDetail.RecoverBreak(5);
            target.TakeBreakDamage(5);
        }
    }
}
