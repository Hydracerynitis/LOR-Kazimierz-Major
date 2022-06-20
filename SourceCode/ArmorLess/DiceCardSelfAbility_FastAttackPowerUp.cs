using UnityEngine;
using System.Collections;
using System;

namespace KazimierzMajor
{
    public class DiceCardSelfAbility_FastAttackPowerUp : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "FastAttack" };
        public override void OnUseCard()
        {
            if (FastLateAttack.IsFastAttack(card))
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { power = 2 });
        }
    }
}
