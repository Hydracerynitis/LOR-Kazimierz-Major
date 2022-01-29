using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace KazimierzMajor 
{
    public class BattleUnitBuf_Corrosion : BattleUnitBuf
    {
        protected override string keywordId => "Corrosion";
        protected override string keywordIconId => "Decay_Yan";
        public override int paramInBufDesc => _owner==null? 4: 4+_owner.bufListDetail.GetKewordBufStack(KeywordBuf.Vulnerable)*2;
        public static void AddBuf(BattleUnitModel model, int value)
        {
            if (!(model.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Corrosion) is BattleUnitBuf_Corrosion Corrosion))
            {
                Corrosion = new BattleUnitBuf_Corrosion{stack = value};
                model.bufListDetail.AddBuf(Corrosion);
            }
            else
                Corrosion.stack+=value;
            if (Corrosion.stack >= 4)
            {
                Corrosion.stack -= 4;
                int vul = Corrosion._owner.bufListDetail.GetKewordBufStack(KeywordBuf.Vulnerable);
                Corrosion._owner.TakeDamage(4 + 2 * vul);
                Corrosion._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Vulnerable, 2);
                Corrosion._owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Vulnerable, 2);
            }
        }
        public static bool GetBuf(BattleUnitModel model, out BattleUnitBuf_Corrosion buf)
        {
            buf = null;
            if(model.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_Corrosion) is BattleUnitBuf_Corrosion Corrosion)
            {
                buf = Corrosion;
                return true;
            }
            return false;
        }
        public override void OnRoundEnd()
        {
            if (stack <= 0)
                Destroy();
        }
    }
}
