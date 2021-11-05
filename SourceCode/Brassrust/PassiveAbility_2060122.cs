using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class PassiveAbility_2060122 : PassiveAbilityBase
    {
        private bool _activated;
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            _activated = false;
        }
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            if (!this.IsAttackDice(behavior.Detail) || _activated == true || this.owner.hp < (double)this.owner.MaxHp / 2)
                return;
            behavior.card?.target?.TakeDamage((int)((double)behavior.DiceResultValue / 2));
            _activated = true;
        }
    }
}
