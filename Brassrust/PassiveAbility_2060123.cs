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
            if (!this.IsAttackDice(behavior.Detail) || _activated == true || this.owner.hp > (double)this.owner.MaxHp / 2)
                return;
            behavior.isBonusAttack = true;
            _activated = true;
        }
    }
}
