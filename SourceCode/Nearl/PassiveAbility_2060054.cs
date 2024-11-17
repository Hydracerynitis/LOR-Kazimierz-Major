using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LOR_DiceSystem;
using System;

namespace KazimierzMajor
{
    public class PassiveAbility_2060054 : PassiveAbilityBase, SpendCostAbility
    {
        int count= 0;
        public override void OnWaveStart()
        {
            BattleUnitBuf_ChargeLight.AddBuf(this.owner, 0);
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            this.owner.cardSlotDetail.RecoverPlayPoint(1);
        }

        public void OnSpendCost(int cost)
        {
            count+=cost;
            if (count >= 8)
            {
                count-=8;
                BattleUnitBuf_ChargeLight.AddBuf(this.owner, 1);
            }
        }
    }
    public interface SpendCostAbility
    {
        public void OnSpendCost(int cost);
    }
}

