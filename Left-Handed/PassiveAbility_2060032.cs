using UnityEngine;
using System.Collections;
using JetBrains.Annotations;
using System;

namespace KazimierzMajor
{
    public class PassiveAbility_2060032 : PassiveAbilityBase
    {
        public override void OnMakeBreakState(BattleUnitModel target)
        {
            if (target == owner)
                return;
            base.OnMakeBreakState(target);
            target.bufListDetail.AddBuf(new Crushing());
        }
        public class Crushing : BattleUnitBuf
        {
            public override void OnRoundEnd()
            {
                BattleUnitBuf_Depressed.AddBuf(this._owner, 1);
                this._owner.RecoverBreakLife(this._owner.MaxBreakLife);
                this._owner.breakDetail.nextTurnBreak = false;
                _owner.ResetBreakGauge();
                this.Destroy();
            }
        }
    }
}
