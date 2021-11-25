using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class PassiveAbility_2060123 : PassiveAbilityBase
    {
        private bool _activated;
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            _activated = false;
        }
        public override void OnLoseParrying(BattleDiceBehavior behavior)
        {
            if (!IsAttackDice(behavior.Detail) || _activated == true || owner.hp > owner.MaxHp / 2 || owner.IsBreakLifeZero())
                return;
            behavior.isBonusAttack = true;
            _activated = true;
        }
    }
}
