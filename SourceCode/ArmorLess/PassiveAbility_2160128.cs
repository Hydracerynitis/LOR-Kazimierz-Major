using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class PassiveAbility_2160128 :PassiveAbilityBase
    {
        private bool oneSide = false;
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            oneSide = false;
            if (behavior.IsParrying())
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = -9999, breakRate = -9999 });
            oneSide = true;
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if(oneSide)
                owner.battleCardResultLog.SetSucceedAtkEvent(() => GiveDamage(behavior));
        }
        private void GiveDamage(BattleDiceBehavior behavior)
        {
            behavior.card.target.TakeDamage(behavior.DiceResultValue, attacker: owner);
            behavior.card.target.TakeBreakDamage(behavior.DiceResultValue, attacker: owner);
        }
    }
}
