using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_ApplySuckTarget : DiceCardSelfAbilityBase
    {
        public override bool IsOnlyAllyUnit()
        {
            return true ;
        }
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            base.OnUseInstance(unit, self, targetUnit);
            targetUnit.bufListDetail.AddBuf(new SuckTarget());
        }
    }
    public class SuckTarget : BattleUnitBuf
    {
        public override string keywordId => "SuckTarget";
        public override string keywordIconId => "KeterFinal_Light";
        public override void Init(BattleUnitModel owner)
        {
            base.Init(owner);
            stack = 0;
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            Destroy();
        }
    }
}
