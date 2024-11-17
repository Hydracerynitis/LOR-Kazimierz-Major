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
            if (card.card.GetCost() > 0)
                return;
            BattleObjectManager.instance.GetAliveList(owner.faction).ForEach((x) =>
            {
                x.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Protection, 2);
                x.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.BreakProtection, 2);
            });
        }
    }
}
