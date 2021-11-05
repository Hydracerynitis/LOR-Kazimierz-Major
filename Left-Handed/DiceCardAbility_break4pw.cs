using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace KazimierzMajor
{
    public class DiceCardAbility_break4pw : DiceCardAbilityBase
    {
        public override void OnWinParrying()
        {
            BattleUnitModel target = base.card.target;
            target?.TakeBreakDamage(4, DamageType.Attack, base.owner, AtkResist.Normal);
        }
    }
}
