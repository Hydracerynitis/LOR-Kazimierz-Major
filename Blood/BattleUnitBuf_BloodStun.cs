using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace KazimierzMajor 
{
    public class BattleUnitBuf_BloodStun : BattleUnitBuf
    {
        private BattleUnitModel Giver;
        protected override string keywordId => "BloodStun";
        protected override string keywordIconId => "Stun";
        public static void AddBuf(BattleUnitModel model, BattleUnitModel giver)
        {
            if (!(model.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_BloodStun) is BattleUnitBuf_BloodStun battleUnitBufBloodStun))
            {
                battleUnitBufBloodStun = new BattleUnitBuf_BloodStun { stack = 2,Giver=giver};
                model.bufListDetail.AddReadyBuf(battleUnitBufBloodStun);
            }
            else
                battleUnitBufBloodStun.stack+=2;
        }
        public static bool GetBuf(BattleUnitModel model, out BattleUnitBuf_BloodStun buf)
        {
            buf = null;
            if(model.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_BloodStun) is BattleUnitBuf_BloodStun battleUnitBufBloodStun)
            {
                buf = battleUnitBufBloodStun;
                return true;
            }
            return false;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            _owner.breakDetail.breakGauge = 0;
            _owner.breakDetail.breakLife = 0;
            _owner.breakDetail.DestroyBreakPoint();
            _owner.TakeDamage(10);
            BattleUnitBuf_Blood.AddBuf(Giver,2);
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            stack--;
            if (stack <= 0)
            {
                this._owner.breakDetail.RecoverBreakLife(this._owner.MaxBreakLife);
                this._owner.breakDetail.nextTurnBreak = false;
                this._owner.breakDetail.RecoverBreak(this._owner.breakDetail.GetDefaultBreakGauge());
                this.Destroy();
            }
        }
    }
}
