using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class BattleUnitBuf_Shield : BattleUnitBuf
    {
        public override string keywordId => _owner?.faction == Faction.Player ? "RadiantShieldLib" : "RadiantShield";
        public override string keywordIconId => "Resistance_simple";
        public override bool IsImmuneDmg()
        {
            return true;
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (behavior.card.card.GetSpec().Ranged != CardRange.Near || _owner.faction == Faction.Player)
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = 50, breakRate = 50 });
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            if (_owner.faction == Faction.Enemy)
                Destroy();
        }
        public void Reduce()
        {
            if (--stack <= 0)
                Destroy();
        }
    }
}
