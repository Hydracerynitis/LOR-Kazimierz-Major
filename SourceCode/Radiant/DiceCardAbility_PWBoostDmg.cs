using UnityEngine;
using System.Collections;
using System;

namespace KazimierzMajor
{
    public class DiceCardAbility_PWBoostDmg : DiceCardAbilityBase
    {
        public override void OnWinParrying()
        {
            base.OnWinParrying();
            if (behavior.TargetDice != null)
                card.ApplyDiceStatBonus(DiceMatch.NextAttackDice, new DiceStatBonus() { dmg = 4 * Math.Abs(behavior.DiceResultValue - behavior.TargetDice.DiceResultValue) });
        }
    }
}
