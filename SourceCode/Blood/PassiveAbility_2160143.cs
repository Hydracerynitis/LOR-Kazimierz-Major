using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class PassiveAbility_2160143 : PassiveAbilityBase
    {
        private bool oneSide = false;
        public override int GetDamageReduction(BattleDiceBehavior behavior)
        {
            if (oneSide)
                return RandomUtil.Range(2, 3);
            return base.GetDamageReduction(behavior);
        }
        public override int GetBreakDamageReduction(BattleDiceBehavior behavior)
        {
            if (oneSide)
                return RandomUtil.Range(2, 3);
            return base.GetBreakDamageReduction(behavior);
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            oneSide = false;
        }
        public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
        {
            base.OnStartParrying(card);
            oneSide = false;
        }
        public override void OnStartTargetedOneSide(BattlePlayingCardDataInUnitModel attackerCard)
        {
            base.OnStartTargetedOneSide(attackerCard);
            oneSide = true;
        }
        public override void OnStartTargetedByAreaAtk(BattlePlayingCardDataInUnitModel attackerCard)
        {
            base.OnStartTargetedByAreaAtk(attackerCard);
            oneSide = false;
        }
    }
}
