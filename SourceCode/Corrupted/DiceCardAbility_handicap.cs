using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class DiceCardAbility_handicap: DiceCardAbilityBase
    {
        public override void OnSucceedAttack()
        {
            card.ApplyDiceAbility(DiceMatch.NextAttackDice,new handicapAbility());
        }
    }
    public class handicapAbility : DiceCardAbilityBase
    {
        public override void OnSucceedAttack()
        {
            this.card.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Weak, 2, this.owner);
            this.card.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Disarm, 2, this.owner);
            this.card.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Binding, 2, this.owner);
        }
    }
}
