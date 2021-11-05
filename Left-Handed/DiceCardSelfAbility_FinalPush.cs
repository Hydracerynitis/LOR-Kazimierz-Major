using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_FinalPush : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            if (BattleUnitBuf_Depressed.GetBuf(card.target, out BattleUnitBuf_Depressed buf))
                this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { power = buf.stack * 10 });
        }
        public override bool IsValidTarget(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            if (BattleUnitBuf_Depressed.GetBuf(targetUnit, out BattleUnitBuf_Depressed buf))
                return buf.stack >= 3;
            return false;
        }
    }
}
