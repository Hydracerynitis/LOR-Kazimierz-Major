using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_Allbreak4pw : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            this.card.ApplyDiceAbility(DiceMatch.AllDice, new DiceCardAbility_break4pw());
        }
    }
}
