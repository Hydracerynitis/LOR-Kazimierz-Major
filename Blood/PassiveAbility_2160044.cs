using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class PassiveAbility_2160044 :PassiveAbilityBase
    {
        private bool oneSide = false;
        public override AtkResist GetResistBP(AtkResist origin, BehaviourDetail detail)
        {
            if (oneSide)
                return AtkResist.Resist;
            return base.GetResistBP(origin, detail);
        }
        public override AtkResist GetResistHP(AtkResist origin, BehaviourDetail detail)
        {
            if (oneSide)
                return AtkResist.Resist;
            return base.GetResistHP(origin, detail);
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
    }
}
