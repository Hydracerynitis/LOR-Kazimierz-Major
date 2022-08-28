using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class DiceCardAbility_ParryGuaran : DiceCardAbilityBase, ParryAbility
    {
        bool triggered = false;
        public override void AfterAction()
        {
            base.AfterAction();
            if (triggered)
            {
                ActivateBonusAttackDice();
                triggered = false;
            }
        }
        public void TriggerParry()
        {
            triggered = true;
        }
    }
}
