using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace KazimierzMajor
{
    public class DiceCardAbility_BackStab : DiceCardAbilityBase
    {
        public override void BeforeRollDice()
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 2 * owner.cardSlotDetail.keepCard.GetDiceBehaviorList().Count });
        }
    }
}
