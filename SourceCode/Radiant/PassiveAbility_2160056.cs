using UnityEngine;
using System.Collections.Generic;
using LOR_DiceSystem;

namespace KazimierzMajor
{
    public class PassiveAbility_2160056 : PassiveAbilityBase
    {
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (owner.IsBreakLifeZero())
                return;
            owner.bufListDetail.AddBuf(new BattleUnitBuf_Shield() { stack = 20 });
        }
    }
}
