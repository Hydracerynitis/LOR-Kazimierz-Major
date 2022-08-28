using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class DiceCardAbility_RepeatBelow50 : DiceCardAbilityBase
    {
        bool triggered = false;
        public override void AfterAction()
        {
            base.AfterAction();
            if (owner.hp < owner.MaxHp / 2 && !triggered)
            {
                ActivateBonusAttackDice();
                triggered = true;
            }
        }
    }
}
