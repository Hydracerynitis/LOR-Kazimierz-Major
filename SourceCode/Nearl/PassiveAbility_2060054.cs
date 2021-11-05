using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LOR_DiceSystem;
using System;

namespace KazimierzMajor
{
    public class PassiveAbility_2060054 : PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            BattleUnitBuf_ChargeLight.AddBuf(this.owner, 0);
        }
        public override void OnRoundStart()
        {
            if (this.owner.cardSlotDetail.PlayPoint > 3)
            {
                int num = this.owner.cardSlotDetail.PlayPoint - 3;
                this.owner.cardSlotDetail.LosePlayPoint(num);
                BattleUnitBuf_ChargeLight.AddBuf(this.owner, num);
            }
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            this.owner.cardSlotDetail.RecoverPlayPoint(1);
        }
    }
}

