using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace KazimierzMajor
{
    public class BattleUnitBuf_Blessing : BattleUnitBuf
    {
        public BattleUnitModel Giver;
        protected override string keywordId => _owner.faction==Faction.Enemy? "Blessing": "BlessingLib";
        protected override string keywordIconId => "Blessing";
        public static void AddBuf(BattleUnitModel model, BattleUnitModel giver, int value)
        {
            if (!(model.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Blessing) is BattleUnitBuf_Blessing battleUnitBufBlessing))
            {
                battleUnitBufBlessing = new BattleUnitBuf_Blessing{stack = value,Giver = giver};
                model.bufListDetail.AddBuf(battleUnitBufBlessing);
            }
            else
                battleUnitBufBlessing.stack += value;
        }
        public bool GetBuff(BattleUnitModel model, out BattleUnitBuf_Blessing buf)
        {
            if (model.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Blessing) is BattleUnitBuf_Blessing battleUnitBufBlessing)
            {
                buf = battleUnitBufBlessing;
                return true;
            }
            buf = null;
            return false;
        }

        public override void OnRoundEnd()
        {
            int hp = (int)((double)this._owner.MaxHp);
            if (_owner.faction == Faction.Enemy)
                this._owner.RecoverHP(hp);
            else
                _owner.RecoverHP(hp / 2);
            this._owner.breakDetail.RecoverBreakLife(this._owner.MaxBreakLife);
            this._owner.breakDetail.RecoverBreak(this._owner.breakDetail.GetDefaultBreakGauge());
            BattleUnitBuf_Stunning.AddBuf(Giver, 1);
            this.Destroy();
        }
    }
}
