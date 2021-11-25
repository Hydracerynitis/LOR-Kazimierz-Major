using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class PassiveAbility_2160025 :PassiveAbilityBase
    {
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            if (curCard.earlyTarget != curCard.target)
                curCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { power = 2 });
        }
    }
}
