using UnityEngine;
using System.Collections;

namespace KazimierzMajor
{
    public class PassiveAbility_2060031 : PassiveAbilityBase
    {
        public override void OnStartTargetedOneSide(BattlePlayingCardDataInUnitModel attackerCard)
        {
            base.OnStartTargetedOneSide(attackerCard);
            attackerCard.ApplyDiceStatBonus(DiceMatch.AllAttackDice,new DiceStatBonus() { power = -1 });
        }
        public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
        {
            base.OnStartParrying(card);
            card.target?.currentDiceAction?.ApplyDiceStatBonus(DiceMatch.AllAttackDice, new DiceStatBonus() { power = -1 });
        }
    }
}
