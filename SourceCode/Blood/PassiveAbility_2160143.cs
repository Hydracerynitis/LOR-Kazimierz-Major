using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class PassiveAbility_2160143 : PassiveAbilityBase
    {
        public override void OnRollDice(BattleDiceBehavior behavior)
        {
            base.OnRollDice(behavior);
            if (behavior.DiceVanillaValue >= 10)
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = 20 });
                owner.cardSlotDetail.RecoverPlayPoint(1);
            }
        }
    }
}
