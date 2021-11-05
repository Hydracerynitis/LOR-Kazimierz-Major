using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace KazimierzMajor
{
    public class DiceCardAbility_Sleeping : DiceCardAbilityBase
    {
        public override void OnWinParrying()
        {
            BattleUnitModel target = card.target;
            BattleUnitBuf_Sleep.AddBuf(target,owner, 1);
            target.breakDetail.breakGauge = 0;
            target.breakDetail.breakLife = 0;
            target.breakDetail.DestroyBreakPoint();
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(this.owner.faction))
            {
                battleUnitModel.bufListDetail.AddBuf(new Regenerate());
            }
            if (owner.passiveDetail.PassiveList.Find(x => x is PassiveAbility_2060056) is PassiveAbility_2060056 passive)
                passive._count = 0;
        }
        public class Regenerate : BattleUnitBuf
        {
            public override void OnRoundEnd()
            {
                this._owner.RecoverHP((int)(this._owner.MaxHp * 0.15));
                this.Destroy();
            }
        }
    }
}
