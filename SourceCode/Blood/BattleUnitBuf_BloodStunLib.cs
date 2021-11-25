using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace KazimierzMajor 
{
    public class BattleUnitBuf_BloodStunLib : BattleUnitBuf
    {
        private BattleUnitModel Giver;
        protected override string keywordId => "BloodStunLib";
        protected override string keywordIconId => "Stun";
        public override int SpeedDiceBreakedAdder()
        {
            return 1;
        }
        public static void AddBuf(BattleUnitModel model, BattleUnitModel giver)
        {
            if (!(model.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_BloodStunLib) is BattleUnitBuf_BloodStunLib battleUnitBufBloodStun))
            {
                battleUnitBufBloodStun = new BattleUnitBuf_BloodStunLib { stack = 2,Giver=giver};
                model.bufListDetail.AddReadyBuf(battleUnitBufBloodStun);
            }
            else
                battleUnitBufBloodStun.stack+=2;
        }
        public static bool GetBuf(BattleUnitModel model, out BattleUnitBuf_BloodStunLib buf)
        {
            buf = null;
            if(model.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_BloodStunLib) is BattleUnitBuf_BloodStunLib battleUnitBufBloodStun)
            {
                buf = battleUnitBufBloodStun;
                return true;
            }
            return false;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            BattleUnitBuf_Blood.AddBuf(Giver, 2);
            _owner.TakeDamage(10);
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            stack--;
            if (stack <= 0)
            {
                this.Destroy();
            }
        }
    }
}
