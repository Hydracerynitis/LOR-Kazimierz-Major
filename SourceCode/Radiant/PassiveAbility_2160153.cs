using UnityEngine;
using System.Collections.Generic;

namespace KazimierzMajor
{
    public class PassiveAbility_2160153 : PassiveAbilityBase
    {
        public static BattleCardBehaviourResult GetParried;
        private int count = 0;
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (count == 0)
            {
                owner.bufListDetail.AddBuf(new CounterReady());
                count = 3;
            }
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            if (owner.bufListDetail.HasBuf<CounterReady>())
                return;
            count--;
        }
        public class CounterReady : BattleUnitBuf
        {
            public override string keywordIconId => "KeterFinal_SilenceGirl_Hand_Target";
            public override string keywordId => "CounterReady";
        }
    }
}
