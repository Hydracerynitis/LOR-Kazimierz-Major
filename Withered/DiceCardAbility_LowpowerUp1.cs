using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class DiceCardAbility_LowpowerUp1 : DiceCardAbilityBase
    {
        public override void BeforeRollDice()
        {
            if (this.card.target.breakDetail.breakGauge > (int)((double)this.card.target.breakDetail.GetDefaultBreakGauge() * 0.5))
                return;
            this.behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 1 });
        }
    }
}
