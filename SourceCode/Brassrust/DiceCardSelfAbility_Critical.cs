using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_Critical:DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            if (owner.hp > owner.MaxHp / 4)
            {
                int lost_hp = (int)(owner.hp - owner.MaxHp / 4);
                owner.LoseHp(lost_hp);
                int dmgRate = (int) (90 * (lost_hp / 0.75 * owner.MaxHp));
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { dmgRate = dmgRate });
            }
            
        }
    }
}
