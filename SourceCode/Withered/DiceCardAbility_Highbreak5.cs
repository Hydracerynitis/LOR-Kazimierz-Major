using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class DiceCardAbility_Highbreak5 : DiceCardAbilityBase
    {
        public override void BeforeRollDice()
        {
            if (this.card.target.breakDetail.breakGauge < (int)(this.card.target.breakDetail.GetDefaultBreakGauge() * 0.5))
                return;
            this.behavior.ApplyDiceStatBonus(new DiceStatBonus() { breakDmg = 5 });
        }
    }
}
