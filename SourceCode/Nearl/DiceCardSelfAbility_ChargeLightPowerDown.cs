using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_ChargeLightPowerDown : DiceCardSelfAbilityBase
    {
        private bool isCost0 = false;
        public override void OnStartBattle()
        {
            if(card.card.GetCost() <=0)
                isCost0 = true;
        }
        public override void OnUseCard()
        {
            if (this.card.target.currentDiceAction == null || !isCost0)
                return;
            this.card.target.currentDiceAction?.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { min=-2, max = -2 });
        }
    }
}
