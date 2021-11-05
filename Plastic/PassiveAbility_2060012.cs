using UnityEngine;
using System.Collections;
using System;
using BaseMod;
using LOR_DiceSystem;

namespace KazimierzMajor
{
    public class PassiveAbility_2060012 : PassiveAbilityBase
    {
        private int _count=1;
        public override void OnRoundStart()
        {
            _count += 1;
            if (_count == 5)
            {
                this.owner.allyCardDetail.AddNewCard(Tools.MakeLorId(2060101)).XmlData.optionList.Add(CardOption.ExhaustOnUse);
                _count = 0;
            }
            if (this.owner.breakDetail.breakLife == 0 || this.owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_stun) !=null || this.owner.bufListDetail.GetReadyBufList().Find(x => x is BattleUnitBuf_stun)!=null)
            {
                this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Vulnerable, 2);
                if (BattleUnitBuf_Monmentum.GetBuf(owner, out BattleUnitBuf_Monmentum monmentum))
                    monmentum.stack = 0;
                if (BattleUnitBuf_Force.GetBuf(owner, out BattleUnitBuf_Force force))
                    force.stack = 0;
            }
        }
    }
}
