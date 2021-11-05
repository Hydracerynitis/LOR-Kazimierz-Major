using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class PassiveAbility_2060022 : PassiveAbilityBase
    {
        private bool _activated;
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            _activated = false;
        }
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            if (!this.IsAttackDice(behavior.Detail) || _activated == true)
                return;
            behavior.card?.target?.TakeDamage((int)((double)behavior.DiceResultValue / 2));
            _activated = true;
        }
    }
}
