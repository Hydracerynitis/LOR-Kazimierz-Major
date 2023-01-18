using UnityEngine;
using System.Collections;
using System;
using BaseMod;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_InfiniteLight : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            BattleObjectManager.instance.GetAliveList().ForEach(x => x.bufListDetail.AddBufByCard<InfiniteLight>(3,readyType:BufReadyType.NextRound));
        }
    }
    public class InfiniteLight : BattleUnitBuf
    {
        public override string keywordId => "InifiniteLight";
        public override string keywordIconId => "KeterFinal_Light";
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            _owner.cardSlotDetail.RecoverPlayPoint(_owner.cardSlotDetail.GetMaxPlayPoint());
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            stack--;
            if (stack <= 0)
                Destroy();
        }
    }
}
