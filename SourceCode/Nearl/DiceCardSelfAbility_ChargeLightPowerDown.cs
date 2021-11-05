using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_ChargeLightPowerDown : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            if(BattleUnitBuf_ChargeLight.GetBuff(this.owner,out BattleUnitBuf_ChargeLight buf) && buf.stack > 6)
            {
                buf.UseStack(2);
                if (this.card.target.currentDiceAction == null)
                    return;
                this.card.target.currentDiceAction.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { max=-2 });
            }
        }
    }
}
