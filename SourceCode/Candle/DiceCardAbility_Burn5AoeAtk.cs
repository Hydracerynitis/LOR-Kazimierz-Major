using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class DiceCardAbility_Burn5AoeAtk : DiceCardAbilityBase
    {
        public override void OnSucceedAreaAttack(BattleUnitModel target)
        {
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Burn, 5, owner);
        }
    }
}
