using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace KazimierzMajor
{
    public class DiceCardAbility_SleepingLib : DiceCardAbilityBase
    {
        public override void OnWinParrying()
        {
            BattleUnitModel target = card.target;
            BattleUnitBuf_Sleep.AddBuf(target,owner, 1);
            target.breakDetail.breakGauge = 0;
            target.breakDetail.breakLife = 0;
            target.breakDetail.DestroyBreakPoint();
        }
    }
}
