using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class PassiveAbility_2060021 : PassiveAbilityBase
    {
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (this.owner.hp < (int)(this.owner.MaxHp * 0.5))
                return;
            this.owner.TakeDamage((int)(this.owner.MaxHp * 0.05));
            behavior.ApplyDiceStatBonus(new DiceStatBonus { power = 1 });
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if (this.owner.hp > (int)(this.owner.MaxHp * 0.5))
                return;
            this.owner.RecoverHP(behavior.DiceResultValue);
        }
    }
}
