using UnityEngine;
using System.Collections.Generic;
using LOR_DiceSystem;

namespace KazimierzMajor
{
    public class PassiveAbility_2160056 : PassiveAbilityBase
    {
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (owner.IsBreakLifeZero())
                return;
            owner.bufListDetail.AddBuf(new Shield() { stack = 20 });
        }
    }
    public class Shield : BattleUnitBuf
    {
        public override string keywordId => "RadiantShield";
        public override string keywordIconId => "Resistance_simple";
        public override bool IsImmuneDmg()
        {
            return true;
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (behavior.card.card.GetSpec().Ranged != CardRange.Near || _owner.faction==Faction.Player)
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = 50, breakRate = 50 });
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            if(_owner.faction==Faction.Enemy)
                Destroy();
        }
        public void Reduce()
        {
            if (--stack <= 0)
                Destroy();
        }
    }
}
