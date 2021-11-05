using UnityEngine;
using System.Collections;
using System;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_powerUp1thisRoundenergy2 : DiceCardSelfAbilityBase
    {
        public override void OnStartBattle()
        {
            this.owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.AllPowerUp, 1);
        }
        public override void OnUseCard()
        {
            this.owner.cardSlotDetail.RecoverPlayPointByCard(2);
        }
    }
}
