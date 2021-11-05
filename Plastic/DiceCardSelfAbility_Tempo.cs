using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_Tempo : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            BattleUnitBuf_Monmentum.AddBuf(this.owner, 3);
            BattleUnitBuf_Force.AddBuf(this.owner, 3);
            this.owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Stun, 1, this.owner);
        }
    }
}
