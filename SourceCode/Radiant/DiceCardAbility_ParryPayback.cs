using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class DiceCardAbility_ParryPayback : DiceCardAbilityBase, ParryAbility
    {
        public void TriggerParry()
        {
            if (behavior.TargetDice != null)
            {
                behavior.TargetDice.owner.TakeDamage(behavior.TargetDice.DiceResultValue);
                behavior.TargetDice.owner.TakeBreakDamage(behavior.TargetDice.DiceResultValue);
            }
        }
    }
}
