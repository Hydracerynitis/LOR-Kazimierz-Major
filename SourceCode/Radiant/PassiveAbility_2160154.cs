using UnityEngine;
using System.Collections.Generic;
using LOR_DiceSystem;

namespace KazimierzMajor
{
    public class PassiveAbility_2160154 : PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            owner.bufListDetail.AddBuf(new Shield() { stack = 10 });
        }
        public override void OnReleaseBreak()
        {
            base.OnReleaseBreak();
            owner.bufListDetail.AddBuf(new Shield() { stack = 5 });
        }
    }
}
