using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class DiceCardAbility_ParryCritical : DiceCardAbilityBase, ParryAbility
    {
        bool triggered = false;
        public override void BeforeRollDice()
        {
            base.BeforeRollDice();
            triggered = behavior.TargetDice == null;
        }
        public void TriggerParry()
        {
            triggered = true;
        }
        public override void BeforeGiveDamage()
        {
            if(triggered)
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = 100, breakRate = 100 });
            }
        }
    }
}
