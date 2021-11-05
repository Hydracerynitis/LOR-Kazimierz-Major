using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace KazimierzMajor
{
    public class DiceCardAbility_break2pl : DiceCardAbilityBase
    {
        public override void OnLoseParrying()
        {
            BattleUnitModel Owner = this.card.owner;
            Owner.TakeBreakDamage(2, DamageType.Attack, base.owner, AtkResist.Normal);
        }
    }
}
