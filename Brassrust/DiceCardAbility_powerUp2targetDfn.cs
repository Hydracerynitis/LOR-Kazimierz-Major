using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class DiceCardAbility_powerUp2targetDfn : DiceCardAbilityBase
    {
        public override void BeforeRollDice()
        {
            if (this.behavior.TargetDice == null)
                return;
            BattleDiceBehavior currentBehavior = this.behavior.TargetDice;
            if (!this.IsDefenseDice(currentBehavior.Detail))
                return;
            this.behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 2 });
        }
    }
}
