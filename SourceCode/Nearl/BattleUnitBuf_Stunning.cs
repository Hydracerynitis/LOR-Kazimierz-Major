using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace KazimierzMajor
{
    public class BattleUnitBuf_Stunning : BattleUnitBuf
    {
        public override string keywordId => "Stunning";
        public override string keywordIconId => "Stun";
        public static void AddBuf(BattleUnitModel model, int value)
        {
            if (!(model.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Stunning) is BattleUnitBuf_Stunning battleUnitBufStunning))
            {
                battleUnitBufStunning = new BattleUnitBuf_Stunning{ stack = value };
                model.bufListDetail.AddBuf(battleUnitBufStunning);
            }
            else
                battleUnitBufStunning.stack+=value;
        }
        public static bool GetBuff(BattleUnitModel model, out BattleUnitBuf_Stunning buf)
        {
            buf = null;
            if(model.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Stunning) is BattleUnitBuf_Stunning battleUnitBufStunning)
            {
                buf = battleUnitBufStunning;
                return true;
            }
            return false;
        }
        public override bool IsActionable() => false;
        public override void OnRoundEnd()
        {
            stack -= 1;
            if (stack > 0)
                return;
            this.Destroy();
        }
    }
}
