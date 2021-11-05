using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_ChargeLightPowerUp : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            if(BattleUnitBuf_ChargeLight.GetBuff(this.owner, out BattleUnitBuf_ChargeLight buf) && buf.stack > 6)
            {
                buf.UseStack(2);
                this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { power = 1 });
            }
        }
    }
}
