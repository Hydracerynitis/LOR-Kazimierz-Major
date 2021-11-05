using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_ChargeLightProtection : DiceCardSelfAbilityBase
    {
        public override void OnStartBattle()
        {
            if(BattleUnitBuf_ChargeLight.GetBuff(this.owner,out BattleUnitBuf_ChargeLight buf) && buf.stack > 6)
            {
                buf.UseStack(2);
                List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(this.owner.faction);
                if (aliveList.Count <= 0)
                    return;
                aliveList.Sort((x, y) => (int)((double)x.hp - (double)y.hp));
                aliveList[0].bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Protection, 2);
                aliveList[0].bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.BreakProtection, 2);
            }
        }
    }
}
