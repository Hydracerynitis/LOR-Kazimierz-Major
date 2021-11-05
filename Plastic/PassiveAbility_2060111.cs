﻿using UnityEngine;
using System.Collections;
using System;

namespace KazimierzMajor
{
    public class PassiveAbility_2060111 : PassiveAbilityBase
    {
        private int count = 0;
        public override void OnRoundStart()
        {
            if (this.owner.breakDetail.breakLife == 0 || this.owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_stun) is BattleUnitBuf_stun || this.owner.bufListDetail.GetReadyBufList().Find(x => x is BattleUnitBuf_stun) is BattleUnitBuf_stun)
                return;
            this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, 2);
            this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, 2);
            this.owner.allyCardDetail.DrawCards(1);
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            count += 1;
            if (count >= 6)
            {
                owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Stun, 1);
                count = 0;
            }
        }
    }
}
