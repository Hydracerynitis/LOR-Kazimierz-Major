using UnityEngine;
using System.Collections;
using System;
using BaseMod;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_AmpDmg : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "AmpDmg_Desc"};
        public override void OnSucceedAreaAttack(BattleUnitModel target)
        {
            base.OnSucceedAreaAttack(target);
            owner.bufListDetail.AddBufByCard<AmpDmg>(1);
        }
    }
    public class AmpDmg : BattleUnitBuf
    {
        public override string keywordId => "AmpDmg";
        public override string keywordIconId => "Philip_Strong";
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (IsDefenseDice(behavior.Detail))
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmgRate = 50, breakRate = 50 });
            stack--;
            if (stack <= 0)
                Destroy();
        }
    }
}
