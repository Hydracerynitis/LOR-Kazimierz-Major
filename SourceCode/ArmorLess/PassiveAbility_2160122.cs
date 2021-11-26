using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class PassiveAbility_2160122 :PassiveAbilityBase
    {
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            if (behavior.card.target.hp > behavior.card.target.MaxHp * 0.9)
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmg = behavior.DiceResultValue });
        }
    }
}
