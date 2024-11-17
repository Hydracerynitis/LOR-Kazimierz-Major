using UnityEngine;
using System.Collections;
using JetBrains.Annotations;
using System;
using BaseMod;

namespace KazimierzMajor
{
    public class PassiveAbility_2060032 : PassiveAbilityBase
    {
        private bool _getannoyed=true;
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
        public override void OnWaveStart()
        {
            if (owner.faction == Faction.Enemy)
                return;
            owner.allyCardDetail.AddNewCard(Tools.MakeLorId(2060301));
        }
        public override void OnRoundStart()
        {
            if (owner.faction == Faction.Player || !_getannoyed)
                return;
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList_opponent(this.owner.faction))
            {
                if (BattleUnitBuf_Depressed.GetBuf(battleUnitModel, out BattleUnitBuf_Depressed buf) && buf.stack >= 3)
                {
                    this.owner.allyCardDetail.AddNewCard(Tools.MakeLorId(2060301));
                    _getannoyed = false;
                    return;
                }
            }
        }
    }
}
