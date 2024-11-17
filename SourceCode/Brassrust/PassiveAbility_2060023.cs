using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class PassiveAbility_2060023 : PassiveAbilityBase
    {
        private bool _activated;
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            _activated = false;
        }
        public override void OnLoseParrying(BattleDiceBehavior behavior)
        {
            if (!this.IsAttackDice(behavior.Detail) || _activated == true || owner.IsBreakLifeZero() || behavior.isBonusAttack)
                return;
            behavior.isBonusAttack = true;
            int CurrentBonus = behavior._statBonus.dmgRate;
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = 50 + CurrentBonus / 2 });
            _activated = true;
        }
    }
}
