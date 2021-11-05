using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor_Mod.Corrupted
{
    public class DiceCardAbility_handicap: DiceCardAbilityBase
    {
        public override void OnSucceedAttack()
        {
            card.ApplyDiceAbility(DiceMatch.NextAttackDice,new DiceCardAbility_weakDisarmBinding1atk());
        }
    }
}
