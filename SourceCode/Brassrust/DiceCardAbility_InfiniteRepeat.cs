using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class DiceCardAbility_InfiniteRepeat : DiceCardAbilityBase
    {
        private int count = 0;
        public override void BeforeRollDice()
        {
            base.BeforeRollDice();
            if (count > 5)
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 5 });
        }
        public override void OnSucceedAttack()
        {
            if (owner.IsBreakLifeZero())
                return;
            if (!behavior.IsParrying())
                return;
            this.ActivateBonusAttackDice();
            count += 1;
        }
        public override void OnLoseParrying()
        {
            if (owner.IsBreakLifeZero())
                return;
            this.ActivateBonusAttackDice();
            count += 1;
        }
        public override void OnDrawParrying()
        {
            if (owner.IsBreakLifeZero())
                return;
            this.ActivateBonusAttackDice();
            count += 1;
        }
    }
}
