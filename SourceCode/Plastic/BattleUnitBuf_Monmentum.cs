using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace KazimierzMajor
{
    public class BattleUnitBuf_Monmentum : BattleUnitBuf
    {
        public override string keywordId => "Monmentum";
        public static void AddBuf(BattleUnitModel model, int value)
        {
            if (!(model.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Monmentum) is BattleUnitBuf_Monmentum battleUnitBufMonmentum))
            {
                battleUnitBufMonmentum = new BattleUnitBuf_Monmentum { stack = value };
                model.bufListDetail.AddBuf(battleUnitBufMonmentum);
            }
            else
                battleUnitBufMonmentum.stack += value;
        }
        public static bool GetBuf(BattleUnitModel model,out BattleUnitBuf_Monmentum buf)
        {
            buf = null;
            if (model.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Monmentum) is BattleUnitBuf_Monmentum battleUnitBufMonmentum)
            {
                buf = battleUnitBufMonmentum;
                return true;
            }
            return false;
        }
        public void UseStack(int stack)
        {
            if (this.stack < stack)
                return;
            this.stack -= stack;
            if (this.stack == 0 && !this._owner.passiveDetail.HasPassive<PassiveAbility_2060013>())
                this.Destroy();
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (this.IsAttackDice(behavior.Detail))
                return;
            this.UseStack(1);
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 1 });
        }
    }
}
