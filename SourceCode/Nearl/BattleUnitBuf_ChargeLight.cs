using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace KazimierzMajor
{
    public class BattleUnitBuf_ChargeLight : BattleUnitBuf
    {
        public override string keywordId => "ChargeLight";
        public static void AddBuf(BattleUnitModel model, int value)
        {
            if (!(model.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_ChargeLight) is BattleUnitBuf_ChargeLight battleUnitBufChargeLight))
            {
                battleUnitBufChargeLight = new BattleUnitBuf_ChargeLight{stack = value};
                model.bufListDetail.AddBuf(battleUnitBufChargeLight);
            }
            else
                battleUnitBufChargeLight.stack += value;
        }
        public static bool GetBuff(BattleUnitModel model, out BattleUnitBuf_ChargeLight buf)
        {
            buf = null;
            if(model.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_ChargeLight) is BattleUnitBuf_ChargeLight battleUnitBufChargeLight)
            {
                buf = battleUnitBufChargeLight;
                return true;
            }
            return false;
        }
        public void UseStack(int stack)
        {
            if (this.stack < stack)
                return;
            this.stack -= stack;
            if (this.stack <= 0 && !this._owner.passiveDetail.HasPassive<PassiveAbility_2060054>())
                this.Destroy();
        }
        public override void OnRoundStartAfter()
        {
            foreach (BattleDiceCardModel card in this._owner.allyCardDetail.GetAllDeck())
                card.SetCurrentCost(card.GetOriginCost() - stack);
        }
    }
}
