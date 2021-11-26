using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace KazimierzMajor
{
    public class PassiveAbility_2160121 :PassiveAbilityBase
    {
        public override void OnDrawParrying(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = -9999, breakRate = -9999 });
            if (behavior.TargetDice?.Detail == BehaviourDetail.Guard)
                behavior.SetDamageRedution(behavior.TargetDice.DiceResultValue);
            behavior.GiveDamage(behavior.card.target);
        }
        public override void OnLoseParrying(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = -9999, breakRate = -9999 });
            if (behavior.TargetDice?.Detail == BehaviourDetail.Guard)
                behavior.SetDamageRedution(behavior.TargetDice.DiceResultValue);
            behavior.GiveDamage(behavior.card.target);
        }
    }
}
