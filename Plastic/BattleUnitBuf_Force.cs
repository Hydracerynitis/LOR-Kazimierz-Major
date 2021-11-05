using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace KazimierzMajor
{
    public class BattleUnitBuf_Force : BattleUnitBuf
    {
        protected override string keywordId => "Force";
        public void Add(int add) => this.stack += add;

        public static void AddBuf(BattleUnitModel model, int value)
        {
            if (!(model.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Force) is BattleUnitBuf_Force battleUnitBufForce))
            {
                battleUnitBufForce = new BattleUnitBuf_Force{ stack = value };
                model.bufListDetail.AddBuf(battleUnitBufForce);
            }
            else
                battleUnitBufForce.stack += value;
        }
        public static bool GetBuf(BattleUnitModel model, out BattleUnitBuf_Force buf)
        {
            buf = null;
            if(model.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Force) is BattleUnitBuf_Force battleUnitBufForce)
            {
                buf = battleUnitBufForce;
                return true;
            }
            return  false;
        }
        public void UseStack(int stack)
        {
            if (this.stack < stack)
                return;
            this.stack -= stack;
            if (this.stack <= 0 && !this._owner.passiveDetail.HasPassive<PassiveAbility_2060014>())
                this.Destroy();
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (IsDefenseDice(behavior.Detail))
                return;
            this.UseStack(1);
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 1 });
        }
    }
}
