using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class DiceCardAbility_pwNextPower3 : DiceCardAbilityBase
    {
        public override void OnWinParrying()
        {
            base.OnWinParrying();
            card.ApplyDiceStatBonus(DiceMatch.NextDice,new DiceStatBonus() { power = 3 });
        }
    }
}
