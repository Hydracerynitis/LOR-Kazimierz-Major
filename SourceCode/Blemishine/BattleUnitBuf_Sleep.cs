using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace KazimierzMajor
{
    public class BattleUnitBuf_Sleep : BattleUnitBuf
    {
        private BattleUnitModel giver;
        protected override string keywordId => "Sleep";
        protected override string keywordIconId => "BigBird_Sleep";
        public static void AddBuf(BattleUnitModel model,BattleUnitModel giver, int value)
        {
            if (!(model.bufListDetail.GetReadyBufList().Find(x => x is BattleUnitBuf_Sleep) is BattleUnitBuf_Sleep battleUnitBufSleep))
            {
                battleUnitBufSleep = new BattleUnitBuf_Sleep{ stack = value, giver = giver};
                model.bufListDetail.AddBuf(new Immune() { Giver = giver });
                model.bufListDetail.AddReadyBuf(battleUnitBufSleep);
            }
            else
                battleUnitBufSleep.stack += (value);
        }
        public static bool GetBuf(BattleUnitModel model, out BattleUnitBuf_Sleep buf)
        {
            buf = null;
            if(model.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Sleep) is BattleUnitBuf_Sleep battleUnitBufSleep)
            {
                buf = battleUnitBufSleep;
                return true;
            }
            return false;
        }
        public override bool IsTargetable(BattleUnitModel attacker) => attacker==null || attacker ==giver;
        public override bool IsActionable() => false;
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            this.Destroy();
        }
        public class Immune : BattleUnitBuf
        {
            public BattleUnitModel Giver;
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                _owner.breakDetail.RecoverBreakLife(_owner.MaxBreakLife);
                _owner.breakDetail.nextTurnBreak = false;
                _owner.breakDetail.RecoverBreak(_owner.breakDetail.GetDefaultBreakGauge());
                this.Destroy();
            }
            public override double ChangeDamage(BattleUnitModel attacker, double dmg)
            {
                if (attacker != Giver)
                    return 0;
                return base.ChangeDamage(attacker, dmg);
            }
        }
    }
}
