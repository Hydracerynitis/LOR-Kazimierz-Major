using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_ChargeLightPowerUp : DiceCardSelfAbilityBase
    {
        private bool isCost0 = false;
        public override void OnStartBattle()
        {
            if (card.card.GetCost() <= 0)
                isCost0 = true;
        }
        public override void OnUseCard()
        {
            if (!isCost0)
                return;
            this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { max = 3 });
        }
    }
}
