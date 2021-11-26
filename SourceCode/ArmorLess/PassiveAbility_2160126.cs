using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class PassiveAbility_2160126 :PassiveAbilityBase
    {
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (!IsAttackDice(behavior.Detail) || owner.cardSlotDetail.keepCard.GetDiceBehaviorList().Count<=0)
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmg=2 });
        }
    }
}
