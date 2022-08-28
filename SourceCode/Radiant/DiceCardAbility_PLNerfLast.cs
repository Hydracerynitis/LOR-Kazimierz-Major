using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class DiceCardAbility_PLNerfLast : DiceCardAbilityBase
    {
        public override void OnLoseParrying()
        {
            base.OnLoseParrying();
            card.ApplyDiceStatBonus(DiceMatch.LastDice, new DiceStatBonus() { min = -9, max = -9 });
        }
    }
}
