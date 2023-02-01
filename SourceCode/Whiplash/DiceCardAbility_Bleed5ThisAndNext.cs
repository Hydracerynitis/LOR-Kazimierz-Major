using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class DiceCardAbility_Bleed5ThisAndNext : DiceCardAbilityBase
    {
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 5,owner);
            target.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Bleeding, 5,owner);
        }
    }
}
