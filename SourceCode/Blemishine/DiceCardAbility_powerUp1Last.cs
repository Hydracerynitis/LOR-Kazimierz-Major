using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace KazimierzMajor
{
    public class DiceCardAbility_powerUp1Last : DiceCardAbilityBase
    {
        public override void OnWinParrying()
        {
            this.card.ApplyDiceStatBonus(DiceMatch.LastDice, new DiceStatBonus() { power = 1 });
        }
    }
}
