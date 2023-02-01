using UnityEngine;
using System.Collections;
using System;
using LOR_DiceSystem;
using BaseMod;

namespace KazimierzMajor
{
    public class PassiveAbility_2260004 : PassiveAbilityBase
    {
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 2);
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, 2);
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, 3);
            owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.BreakProtection, 3);
        }
    }
}
